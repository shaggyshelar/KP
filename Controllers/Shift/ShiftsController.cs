using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ESPL.KP.Helpers;
using AutoMapper;
using ESPL.KP.Entities;
using Microsoft.AspNetCore.Http;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Helpers.Shift;
using Microsoft.AspNetCore.JsonPatch;

namespace ESPL.KP.Controllers.Shift
{
    [Route("api/shifts")]
    public class ShiftsController : Controller
    {
        private ILibraryRepository _libraryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public ShiftsController(ILibraryRepository libraryRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _libraryRepository = libraryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetShifts")]
        [HttpHead]
        public IActionResult GetShifts(ShiftsResourceParameters shiftsResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<ShiftDto, MstShift>
               (shiftsResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<ShiftDto>
                (shiftsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var shiftsFromRepo = _libraryRepository.GetShifts(shiftsResourceParameters);

            var shifts = Mapper.Map<IEnumerable<ShiftDto>>(shiftsFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = shiftsFromRepo.TotalCount,
                    pageSize = shiftsFromRepo.PageSize,
                    currentPage = shiftsFromRepo.CurrentPage,
                    totalPages = shiftsFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForShifts(shiftsResourceParameters,
                    shiftsFromRepo.HasNext, shiftsFromRepo.HasPrevious);

                var shapedShifts = shifts.ShapeData(shiftsResourceParameters.Fields);

                var shapedShiftsWithLinks = shapedShifts.Select(shift =>
                {
                    var shiftAsDictionary = shift as IDictionary<string, object>;
                    var shiftLinks = CreateLinksForShift(
                        (Guid)shiftAsDictionary["Id"], shiftsResourceParameters.Fields);

                    shiftAsDictionary.Add("links", shiftLinks);

                    return shiftAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedShiftsWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = shiftsFromRepo.HasPrevious ?
                    CreateShiftsResourceUri(shiftsResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = shiftsFromRepo.HasNext ?
                    CreateShiftsResourceUri(shiftsResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = shiftsFromRepo.TotalCount,
                    pageSize = shiftsFromRepo.PageSize,
                    currentPage = shiftsFromRepo.CurrentPage,
                    totalPages = shiftsFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(shifts.ShapeData(shiftsResourceParameters.Fields));
            }
        }

        private string CreateShiftsResourceUri(
            ShiftsResourceParameters shiftsResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetShifts",
                      new
                      {
                          fields = shiftsResourceParameters.Fields,
                          orderBy = shiftsResourceParameters.OrderBy,
                          searchQuery = shiftsResourceParameters.SearchQuery,
                          pageNumber = shiftsResourceParameters.PageNumber - 1,
                          pageSize = shiftsResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetShifts",
                      new
                      {
                          fields = shiftsResourceParameters.Fields,
                          orderBy = shiftsResourceParameters.OrderBy,
                          searchQuery = shiftsResourceParameters.SearchQuery,
                          pageNumber = shiftsResourceParameters.PageNumber + 1,
                          pageSize = shiftsResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetShifts",
                    new
                    {
                        fields = shiftsResourceParameters.Fields,
                        orderBy = shiftsResourceParameters.OrderBy,
                        searchQuery = shiftsResourceParameters.SearchQuery,
                        pageNumber = shiftsResourceParameters.PageNumber,
                        pageSize = shiftsResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{id}", Name = "GetShift")]
        public IActionResult GetShift(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<ShiftDto>
              (fields))
            {
                return BadRequest();
            }

            var shiftFromRepo = _libraryRepository.GetShift(id);

            if (shiftFromRepo == null)
            {
                return NotFound();
            }

            var shift = Mapper.Map<ShiftDto>(shiftFromRepo);

            var links = CreateLinksForShift(id, fields);

            var linkedResourceToReturn = shift.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateShift")]
        public IActionResult CreateShift([FromBody] ShiftForCreationDto shift)
        {
            if (shift == null)
            {
                return BadRequest();
            }

            var shiftEntity = Mapper.Map<MstShift>(shift);

            _libraryRepository.AddShift(shiftEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an shift failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var shiftToReturn = Mapper.Map<ShiftDto>(shiftEntity);

            var links = CreateLinksForShift(shiftToReturn.ShiftID, null);

            var linkedResourceToReturn = shiftToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetShift",
                new { id = linkedResourceToReturn["ShiftID"] },
                linkedResourceToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockShiftCreation(Guid id)
        {
            if (_libraryRepository.ShiftExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteShift")]
        public IActionResult DeleteShift(Guid id)
        {
            var shiftFromRepo = _libraryRepository.GetShift(id);
            if (shiftFromRepo == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteShift(shiftFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Deleting shift {id} failed on save.");
            }

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateShift")]
        public IActionResult UpdateShift(Guid id, [FromBody] ShiftForCreationDto shift)
        {
            if (shift == null)
            {
                return BadRequest();
            }
            // if (!_libraryRepository.OccurrenceBookExists(id))
            // {
            //     return NotFound();
            // }
            //Mapper.Map(source,destination);
            var shiftRepo = _libraryRepository.GetShift(id);

            if (shiftRepo == null)
            {
                var shiftAdd = Mapper.Map<MstShift>(shift);
                shiftAdd.ShiftID = id;

                _libraryRepository.AddShift(shiftAdd);

                if (!_libraryRepository.Save())
                {
                    throw new Exception($"Upserting shift {id} failed on save.");
                }

                var shiftReturnVal = Mapper.Map<ShiftDto>(shiftAdd);

                return CreatedAtRoute("GetShift",
                    new { ShiftID = shiftReturnVal.ShiftID },
                    shiftReturnVal);
            }

            Mapper.Map(shift, shiftRepo);
            _libraryRepository.UpdateShift(shiftRepo);
            if (!_libraryRepository.Save())
            {
                throw new Exception("Updating an shift failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }


            return Ok(shiftRepo);
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateShift")]
        public IActionResult PartiallyUpdateShift(Guid id,
                    [FromBody] JsonPatchDocument<ShiftForCreationDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var shiftFromRepo = _libraryRepository.GetShift(id);

            if (shiftFromRepo == null)
            {
                var shiftDto = new ShiftForCreationDto();
                patchDoc.ApplyTo(shiftDto, ModelState);

                TryValidateModel(shiftDto);

                if (!ModelState.IsValid)
                {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                var shiftToAdd = Mapper.Map<MstShift>(shiftDto);
                shiftToAdd.ShiftID = id;

                _libraryRepository.AddShift(shiftToAdd);

                if (!_libraryRepository.Save())
                {
                    throw new Exception($"Upserting in shift {id} failed on save.");
                }

                var shiftToReturn = Mapper.Map<ShiftDto>(shiftToAdd);
                return CreatedAtRoute("GetShift",
                    new { ShiftID = shiftToReturn.ShiftID },
                    shiftToReturn);
            }

            var shiftToPatch = Mapper.Map<ShiftForCreationDto>(shiftFromRepo);

            patchDoc.ApplyTo(shiftToPatch, ModelState);

            // patchDoc.ApplyTo(shiftToPatch);

            TryValidateModel(shiftToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            Mapper.Map(shiftToPatch, shiftFromRepo);

            _libraryRepository.UpdateShift(shiftFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Patching  shift {id} failed on save.");
            }

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForShift(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetShift", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetShift", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("DeleteShift", new { id = id }),
              "delete_shift",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForShift", new { shiftId = id }),
              "create_book_for_shift",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link("GetBooksForShift", new { shiftId = id }),
               "books",
               "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForShifts(
            ShiftsResourceParameters shiftsResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateShiftsResourceUri(shiftsResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateShiftsResourceUri(shiftsResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateShiftsResourceUri(shiftsResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        [HttpOptions]
        public IActionResult GetShiftsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
    }
}