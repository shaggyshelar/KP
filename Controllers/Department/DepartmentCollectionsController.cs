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

namespace ESPL.KP.Controllers.Department
{
    [Route("api/departmentcollections")]
    [Authorize]
    public class DepartmentCollectionsController : Controller
    {
        private ILibraryRepository _libraryRepository;

        public DepartmentCollectionsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
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
                _libraryRepository.AddDepartment(department);
            }

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an department collection failed on save.");
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

            var departmentEntities = _libraryRepository.GetDepartments(ids);

            if (ids.Count() != departmentEntities.Count())
            {
                return NotFound();
            }

            var departmentsToReturn = Mapper.Map<IEnumerable<DepartmentDto>>(departmentEntities);
            return Ok(departmentsToReturn);
        }
    }
}