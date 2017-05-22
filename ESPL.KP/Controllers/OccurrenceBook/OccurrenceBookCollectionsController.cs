using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace KP.Controllers.OccurrenceBook
{
     [Route("api/OccurrenceBookCollection")]
     [Authorize]
    public class OccurrenceBookCollectionsController : Controller
    {
        private IAppRepository _appRepository;

        public OccurrenceBookCollectionsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.OccurrenceBookCreate)]
        public IActionResult CreateOccurrenceBookCollection(
            [FromBody] IEnumerable<OccurrenceBookForCreationDto> occurrenceBookCollection)
        {
            if (occurrenceBookCollection == null)
            {
                return BadRequest();
            }

            var occurrenceBookEntities = Mapper.Map<IEnumerable<MstOccurrenceBook>>(occurrenceBookCollection);

            foreach (var occurrenceBook in occurrenceBookEntities)
            {
                _appRepository.AddOccurrenceBook(occurrenceBook);
            }

            if (!_appRepository.Save())
            {
                throw new Exception("Creating an occurrenceBook collection failed on save.");
            }

            var occurrenceBookCollectionToReturn = Mapper.Map<IEnumerable<OccurrenceBookDto>>(occurrenceBookEntities);
            var idsAsString = string.Join(",",
                occurrenceBookCollectionToReturn.Select(a => a.OBID));

            return CreatedAtRoute("GetOccurrenceBookCollection",
                new { ids = idsAsString },
                occurrenceBookCollectionToReturn);
            //return Ok();
        }

        // (key1,key2, ...)

        [HttpGet("({ids})", Name = "GetOccurrenceBookCollection")]
        [Authorize(Policy = Permissions.OccurrenceBookRead)]
        public IActionResult GetOccurrenceBookCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var occurrenceBookEntities = _appRepository.GetOccurrenceBooks(ids);

            if (ids.Count() != occurrenceBookEntities.Count())
            {
                return NotFound();
            }

            var occurrenceBooksToReturn = Mapper.Map<IEnumerable<OccurrenceBookDto>>(occurrenceBookEntities);
            return Ok(occurrenceBooksToReturn);
        }
    }
}