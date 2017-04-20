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

                var shapedShiftsWithLinks = shapedShifts.Select(department =>
                {
                    var departmentAsDictionary = department as IDictionary<string, object>;
                    var departmentLinks = CreateLinksForShift(
                        (Guid)departmentAsDictionary["Id"], shiftsResourceParameters.Fields);

                    departmentAsDictionary.Add("links", departmentLinks);

                    return departmentAsDictionary;
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

            var departmentFromRepo = _libraryRepository.GetShift(id);

            if (departmentFromRepo == null)
            {
                return NotFound();
            }

            var department = Mapper.Map<ShiftDto>(departmentFromRepo);

            var links = CreateLinksForShift(id, fields);

            var linkedResourceToReturn = department.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateShift")]
        public IActionResult CreateShift([FromBody] ShiftForCreationDto department)
        {
            if (department == null)
            {
                return BadRequest();
            }

            var departmentEntity = Mapper.Map<MstShift>(department);

            _libraryRepository.AddShift(departmentEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an department failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var departmentToReturn = Mapper.Map<ShiftDto>(departmentEntity);

            var links = CreateLinksForShift(departmentToReturn.ShiftID, null);

            var linkedResourceToReturn = departmentToReturn.ShapeData(null)
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
            var departmentFromRepo = _libraryRepository.GetShift(id);
            if (departmentFromRepo == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteShift(departmentFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Deleting department {id} failed on save.");
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
              "delete_department",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForShift", new { departmentId = id }),
              "create_book_for_department",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link("GetBooksForShift", new { departmentId = id }),
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