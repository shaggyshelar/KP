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

namespace ESPL.KP.Controllers.Shift
{
    [Route("api/shiftcollections")]
    public class ShiftCollectionsController : Controller
    {
        private ILibraryRepository _libraryRepository;

        public ShiftCollectionsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpPost]
        public IActionResult CreateShiftCollection(
            [FromBody] IEnumerable<ShiftForCreationDto> shiftCollection)
        {
            if (shiftCollection == null)
            {
                return BadRequest();
            }

            var shiftEntities = Mapper.Map<IEnumerable<MstShift>>(shiftCollection);

            foreach (var shift in shiftEntities)
            {
                _libraryRepository.AddShift(shift);
            }

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an shift collection failed on save.");
            }

            var shiftCollectionToReturn = Mapper.Map<IEnumerable<ShiftDto>>(shiftEntities);
            var idsAsString = string.Join(",",
                shiftCollectionToReturn.Select(a => a.ShiftID));

            return CreatedAtRoute("GetShiftCollection",
                new { ids = idsAsString },
                shiftCollectionToReturn);
            //return Ok();
        }

        // (key1,key2, ...)

        [HttpGet("({ids})", Name = "GetShiftCollection")]
        public IActionResult GetShiftCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var shiftEntities = _libraryRepository.GetShifts(ids);

            if (ids.Count() != shiftEntities.Count())
            {
                return NotFound();
            }

            var shiftsToReturn = Mapper.Map<IEnumerable<ShiftDto>>(shiftEntities);
            return Ok(shiftsToReturn);
        }
    }
}