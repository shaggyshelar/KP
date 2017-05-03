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

namespace ESPL.KP.Controllers.Status
{
    [Route("api/statuscollections")]
    [Authorize]
    public class StatusCollectionsController : Controller
    {
        private IAppRepository _appRepository;

        public StatusCollectionsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.StatusCreate)]
        public IActionResult CreateStatusCollection(
            [FromBody] IEnumerable<StatusForCreationDto> statusCollection)
        {
            if (statusCollection == null)
            {
                return BadRequest();
            }

            var statusEntities = Mapper.Map<IEnumerable<MstStatus>>(statusCollection);

            foreach (var status in statusEntities)
            {
                _appRepository.AddStatus(status);
            }

            if (!_appRepository.Save())
            {
                throw new Exception("Creating an status collection failed on save.");
            }

            var statusCollectionToReturn = Mapper.Map<IEnumerable<StatusDto>>(statusEntities);
            var idsAsString = string.Join(",",
                statusCollectionToReturn.Select(a => a.StatusID));

            return CreatedAtRoute("GetStatusCollection",
                new { ids = idsAsString },
                statusCollectionToReturn);
            //return Ok();
        }

        // (key1,key2, ...)

        [HttpGet("({ids})", Name = "GetStatusCollection")]
        [Authorize(Policy = Permissions.StatusRead)]
        public IActionResult GetStatusCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var statusEntities = _appRepository.GetStatuses(ids);

            if (ids.Count() != statusEntities.Count())
            {
                return NotFound();
            }

            var statusesToReturn = Mapper.Map<IEnumerable<StatusDto>>(statusEntities);
            return Ok(statusesToReturn);
        }
    }
}