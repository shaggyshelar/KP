using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESPL.KP.Helpers;
using AutoMapper;
using ESPL.KP.Entities;
using Microsoft.AspNetCore.Http;
using ESPL.KP.Models.Core;
using ESPL.KP.Helpers.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ESPL.KP.Controllers.Core
{
    [Route("api/roles/{roleId}/permissions")]
    [Authorize(Policy = "SystemAdmin")]
    public class RolePermissionsController : Controller
    {
        private ILibraryRepository _libraryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<ESPLUser> _userMgr;

        public RolePermissionsController(ILibraryRepository libraryRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            UserManager<ESPLUser> userMgr,
            RoleManager<IdentityRole> roleMgr)
        {
            _libraryRepository = libraryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        [HttpGet(Name = "GetRolePermissions")]
        [HttpHead]
        public async Task<IActionResult> GetRolePermissions(Guid roleId)
        {
            var roleFromDB = _libraryRepository.GetESPLRole(roleId);
            if (roleFromDB == null)
            {
                return NotFound("Role Not Found");
            }

            var permissionsList = new List<string>();
            var rolesFromDB = await _roleMgr.GetClaimsAsync(roleFromDB);
            rolesFromDB.ToList().ForEach(permission =>
            {
                permissionsList.Add(permission.Type.ToString());
            });

            return Ok(permissionsList);
        }


        [HttpPost(Name = "AddPermissionRole")]
        public async Task<IActionResult> AddPermissionRole(Guid roleId,
            [FromBody] AddPermissionToRoleDto model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var roleFromDB = _libraryRepository.GetESPLRole(roleId);
            if (roleFromDB == null)
            {
                return NotFound("Role Not Found");
            }

            var appModule = _libraryRepository.AppModuleExists(model.AppModuleName);
            if (!appModule)
            {
                return NotFound("App Module Not Found");
            }
            var claimName = string.Format("{0}.{1}", model.AppModuleName, model.PermissionType.ToString());
            var claimToAdd = new Claim(claimName, "True");
            await _roleMgr.AddClaimAsync(roleFromDB, claimToAdd);

            return Ok();
        }

        [HttpDelete(Name = "RemovePermissionFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(Guid roleId,
            [FromBody] AddPermissionToRoleDto model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var roleFromDB = _libraryRepository.GetESPLRole(roleId);
            if (roleFromDB == null)
            {
                return NotFound("Role Not Found");
            }

            var appModule = _libraryRepository.AppModuleExists(model.AppModuleName);
            if (!appModule)
            {
                return NotFound("App Module Not Found");
            }

            var claimName = string.Format("{0}.{1}", model.AppModuleName, model.PermissionType.ToString());
            var claimToAdd = new Claim(claimName, "True");
            await _roleMgr.RemoveClaimAsync(roleFromDB, claimToAdd);
            return Ok();
        }
    }
}