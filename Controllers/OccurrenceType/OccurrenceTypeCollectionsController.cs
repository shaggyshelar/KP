using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace KP.Controllers.OccurrenceType
{
    [Route("api/occurrenceTypecollections")]
    public class OccurrenceTypeCollectionsController : Controller
    {
        private ILibraryRepository _libraryRepository;
        public OccurrenceTypeCollectionsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpPost]
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
                _libraryRepository.AddOccurrenceType(occurrenceType);
            }

            if (!_libraryRepository.Save())
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
        public IActionResult GetOccurrenceTypeCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var occurrenceTypeEntities = _libraryRepository.GetOccurrenceType(ids);

            if (ids.Count() != occurrenceTypeEntities.Count())
            {
                return NotFound();
            }

            var occurrenceTypesToReturn = Mapper.Map<IEnumerable<MstOccurrenceType>>(occurrenceTypeEntities);
            return Ok(occurrenceTypesToReturn);
        }
    }
}