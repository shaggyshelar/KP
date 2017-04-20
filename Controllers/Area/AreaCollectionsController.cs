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

namespace ESPL.KP.Controllers.Area
{
    [Route("api/areacollections")]
    public class AreaCollectionsController : Controller
    {
        //Global Declaration
        private ILibraryRepository _libraryRepository;
        //Constructor
        public AreaCollectionsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        //action methods
        [HttpPost]
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
                _libraryRepository.AddArea(Area);
            }

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an Area collection failed on save.");
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
        public IActionResult GetAreaCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var AreaEntities = _libraryRepository.GetAreas(ids);

            if (ids.Count() != AreaEntities.Count())
            {
                return NotFound();
            }

            var AreasToReturn = Mapper.Map<IEnumerable<AreaDto>>(AreaEntities);
            return Ok(AreasToReturn);
        }
    }
}