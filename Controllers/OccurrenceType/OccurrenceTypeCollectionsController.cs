using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace KP.Controllers.OccurrenceType
{
    [Route("api/occurrenceTypecollections")]
    [Authorize]
    public class OccurrenceTypeCollectionsController : Controller
    {
        private IAppRepository _appRepository;
        public OccurrenceTypeCollectionsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.OccurrenceTypeCreate)]
        public IActionResult CreateOccurrenceTypeCollection(
            [FromBody] IEnumerable<MstOccurrenceType> occurrenceTypeCollection)
        {
            if (occurrenceTypeCollection == null)
            {
                return BadRequest();
            }

            var occurrenceTypeEntities = Mapper.Map<IEnumerable<MstOccurrenceType>>(occurrenceTypeCollection);

            foreach (var occurrenceType in occurrenceTypeEntities)
            {
                _appRepository.AddOccurrenceType(occurrenceType);
            }

            if (!_appRepository.Save())
            {
                throw new Exception("Creating an occurrenceType collection failed on save.");
            }

            var occurrenceTypeCollectionToReturn = Mapper.Map<IEnumerable<MstOccurrenceType>>(occurrenceTypeEntities);
            var idsAsString = string.Join(",",
                occurrenceTypeCollectionToReturn.Select(a => a.OBTypeID));

            return CreatedAtRoute("GetOccurrenceTypeCollection",
                new { ids = idsAsString },
                occurrenceTypeCollectionToReturn);
            //return Ok();
        }

        // (key1,key2, ...)

        [HttpGet("({ids})", Name = "GetOccurrenceTypeCollection")]
        [Authorize(Policy = Permissions.OccurrenceTypeRead)]
        public IActionResult GetOccurrenceTypeCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var occurrenceTypeEntities = _appRepository.GetOccurrenceType(ids);

            if (ids.Count() != occurrenceTypeEntities.Count())
            {
                return NotFound();
            }

            var occurrenceTypesToReturn = Mapper.Map<IEnumerable<MstOccurrenceType>>(occurrenceTypeEntities);
            return Ok(occurrenceTypesToReturn);
        }
    }
}