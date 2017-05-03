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
using Microsoft.AspNetCore.Authorization;

namespace ESPL.KP.Controllers.Core
{
    [Authorize]
    public class ProfileController : Controller
    {
        private ILibraryRepository _libraryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<AppUser> _userMgr;

        public ProfileController(ILibraryRepository libraryRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            UserManager<AppUser> userMgr,
            RoleManager<IdentityRole> roleMgr)
        {
            _libraryRepository = libraryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }


        [HttpGet("api/profile")]
        public IActionResult GetUserProfile()
        {
            var userId = User.Claims.FirstOrDefault(cl => cl.Type == "UserId");
            if (userId == null)
            {
                return NotFound("Invalid Token");
            }
            var user = _libraryRepository.GetEmployeeByUserID(new Guid(userId.Value));
            if (user == null)
            {
                return NotFound("User Not Found");
            }
            return Ok(user);
        }

        [HttpGet("api/permissions")]
        public IActionResult GetUserPermissions()
        {
            var permissions = User.Claims.Select(permission =>
            {
                return permission.Type;
            });
            return Ok(permissions);
        }
    }
}