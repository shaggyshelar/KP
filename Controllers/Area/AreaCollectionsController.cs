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
using ESPL.KP.DapperRepositoryInterfaces;

namespace ESPL.KP.Controllers.Area
{
    [Route("api/areacollections")]
    [Authorize]
    public class AreaCollectionsController : Controller
    {
        //Global Declaration
        private IDapperRepository _appRepository;
        //Constructor
        public AreaCollectionsController(IDapperRepository appRepository)
        {
            _appRepository = appRepository;
        }

        //action methods
        [HttpPost]
        [Authorize(Policy = Permissions.AreaCreate)]
        public IActionResult CreateAreaCollection(
            [FromBody] IEnumerable<AreaForCreationDto> AreaCollection)
        {
            if (AreaCollection == null)
            {
                return BadRequest();
            }

            var AreaEntities = Mapper.Map<IEnumerable<MstArea>>(AreaCollection);

            foreach (var Area in AreaEntities)
            {
                _appRepository.AddArea(Area);
            }

            var AreaCollectionToReturn = Mapper.Map<IEnumerable<AreaDto>>(AreaEntities);
            var idsAsString = string.Join(",",
                AreaCollectionToReturn.Select(a => a.AreaID));

            return CreatedAtRoute("GetAreaCollection",
                new { ids = idsAsString },
                AreaCollectionToReturn);
            //return Ok();
        }

        // (key1,key2, ...)

        [HttpGet("({ids})", Name = "GetAreaCollection")]
        [Authorize(Policy = Permissions.AreaRead)]
        public IActionResult GetAreaCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var AreaEntities = _appRepository.GetAreas(ids);

            if (ids.Count() != AreaEntities.Count())
            {
                return NotFound();
            }

            var AreasToReturn = Mapper.Map<IEnumerable<AreaDto>>(AreaEntities);
            return Ok(AreasToReturn);
        }
    }
}