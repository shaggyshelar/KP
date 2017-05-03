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
using ESPL.KP.Helpers.Core;
using Microsoft.AspNetCore.Authorization;

namespace ESPL.KP.Controllers.Designation
{
    [Route("api/Designationcollections")]
    [Authorize]
    public class DesignationCollectionsController : Controller
    {
        //Global Declaration
        private ILibraryRepository _libraryRepository;
        //Constructor
        public DesignationCollectionsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        //action methods
        [HttpPost]
        [Authorize(Policy = Permissions.DesignationCreate)]
        public IActionResult CreateDesignationCollection(
            [FromBody] IEnumerable<DesignationForCreationDto> DesignationCollection)
        {
            if (DesignationCollection == null)
            {
                return BadRequest();
            }

            var DesignationEntities = Mapper.Map<IEnumerable<MstDesignation>>(DesignationCollection);

            foreach (var Designation in DesignationEntities)
            {
                _libraryRepository.AddDesignation(Designation);
            }

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an Designation collection failed on save.");
            }

            var DesignationCollectionToReturn = Mapper.Map<IEnumerable<DesignationDto>>(DesignationEntities);
            var idsAsString = string.Join(",",
                DesignationCollectionToReturn.Select(a => a.DesignationID));

            return CreatedAtRoute("GetDesignationCollection",
                new { ids = idsAsString },
                DesignationCollectionToReturn);
            //return Ok();
        }

        // (key1,key2, ...)

        [HttpGet("({ids})", Name = "GetDesignationCollection")]
        [Authorize(Policy = Permissions.DesignationRead)]
        public IActionResult GetDesignationCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var DesignationEntities = _libraryRepository.GetDesignations(ids);

            if (ids.Count() != DesignationEntities.Count())
            {
                return NotFound();
            }

            var DesignationsToReturn = Mapper.Map<IEnumerable<DesignationDto>>(DesignationEntities);
            return Ok(DesignationsToReturn);
        }
    }
}