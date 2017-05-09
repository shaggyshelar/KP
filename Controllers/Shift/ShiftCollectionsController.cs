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

namespace ESPL.KP.Controllers.Shift
{
    [Route("api/shiftcollections")]
    [Authorize]
    public class ShiftCollectionsController : Controller
    {
        private IAppRepository _appRepository;

        public ShiftCollectionsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.ShiftCreate)]
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
                _appRepository.AddShift(shift);
            }

            if (!_appRepository.Save())
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
        [Authorize(Policy = Permissions.ShiftRead)]
        public IActionResult GetShiftCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var shiftEntities = _appRepository.GetShifts(ids);

            if (ids.Count() != shiftEntities.Count())
            {
                return NotFound();
            }

            var shiftsToReturn = Mapper.Map<IEnumerable<ShiftDto>>(shiftEntities);
            return Ok(shiftsToReturn);
        }
    }
}