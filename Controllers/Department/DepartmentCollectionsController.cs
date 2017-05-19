using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ESPL.KP.Helpers.Core;
using ESPL.KP.DapperRepositoryInterfaces;

namespace ESPL.KP.Controllers.Department
{
    [Route("api/departmentcollections")]
    [Authorize]
    public class DepartmentCollectionsController : Controller
    {
        private IDapperRepository _appRepository;

        public DepartmentCollectionsController(IDapperRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.DepartmentCreate)]
        public IActionResult CreateDepartmentCollection(
            [FromBody] IEnumerable<DepartmentForCreationDto> departmentCollection)
        {
            if (departmentCollection == null)
            {
                return BadRequest();
            }

            var departmentEntities = Mapper.Map<IEnumerable<MstDepartment>>(departmentCollection);

            foreach (var department in departmentEntities)
            {
                _appRepository.AddDepartment(department);
            }

            var departmentCollectionToReturn = Mapper.Map<IEnumerable<DepartmentDto>>(departmentEntities);
            var idsAsString = string.Join(",",
                departmentCollectionToReturn.Select(a => a.DepartmentID));

            return CreatedAtRoute("GetDepartmentCollection",
                new { ids = idsAsString },
                departmentCollectionToReturn);
            //return Ok();
        }

        // (key1,key2, ...)

        [HttpGet("({ids})", Name = "GetDepartmentCollection")]
        [Authorize(Policy = Permissions.DepartmentRead)]
        public IActionResult GetDepartmentCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var departmentEntities = _appRepository.GetDepartments(ids);

            if (ids.Count() != departmentEntities.Count())
            {
                return NotFound();
            }

            var departmentsToReturn = Mapper.Map<IEnumerable<DepartmentDto>>(departmentEntities);
            return Ok(departmentsToReturn);
        }
    }
}