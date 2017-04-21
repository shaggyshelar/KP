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
using ESPL.KP.Helpers.Status;

namespace ESPL.KP.Controllers.Status
{
    [Route("api/statuses")]
    public class StatusesController : Controller
    {
        private ILibraryRepository _libraryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public StatusesController(ILibraryRepository libraryRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _libraryRepository = libraryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetStatuses")]
        [HttpHead]
        public IActionResult GetStatuses(StatusesResourceParameters statusesResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<StatusDto, MstStatus>
               (statusesResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<StatusDto>
                (statusesResourceParameters.Fields))
            {
                return BadRequest();
            }

            var statusesFromRepo = _libraryRepository.GetStatuses(statusesResourceParameters);

            var statuses = Mapper.Map<IEnumerable<StatusDto>>(statusesFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = statusesFromRepo.TotalCount,
                    pageSize = statusesFromRepo.PageSize,
                    currentPage = statusesFromRepo.CurrentPage,
                    totalPages = statusesFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForStatuses(statusesResourceParameters,
                    statusesFromRepo.HasNext, statusesFromRepo.HasPrevious);

                var shapedStatuses = statuses.ShapeData(statusesResourceParameters.Fields);

                var shapedStatusesWithLinks = shapedStatuses.Select(department =>
                {
                    var departmentAsDictionary = department as IDictionary<string, object>;
                    var departmentLinks = CreateLinksForStatus(
                        (Guid)departmentAsDictionary["Id"], statusesResourceParameters.Fields);

                    departmentAsDictionary.Add("links", departmentLinks);

                    return departmentAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedStatusesWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = statusesFromRepo.HasPrevious ?
                    CreateStatusesResourceUri(statusesResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = statusesFromRepo.HasNext ?
                    CreateStatusesResourceUri(statusesResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = statusesFromRepo.TotalCount,
                    pageSize = statusesFromRepo.PageSize,
                    currentPage = statusesFromRepo.CurrentPage,
                    totalPages = statusesFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(statuses.ShapeData(statusesResourceParameters.Fields));
            }
        }

        private string CreateStatusesResourceUri(
            StatusesResourceParameters statusesResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetStatuses",
                      new
                      {
                          fields = statusesResourceParameters.Fields,
                          orderBy = statusesResourceParameters.OrderBy,
                          searchQuery = statusesResourceParameters.SearchQuery,
                          pageNumber = statusesResourceParameters.PageNumber - 1,
                          pageSize = statusesResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetStatuses",
                      new
                      {
                          fields = statusesResourceParameters.Fields,
                          orderBy = statusesResourceParameters.OrderBy,
                          searchQuery = statusesResourceParameters.SearchQuery,
                          pageNumber = statusesResourceParameters.PageNumber + 1,
                          pageSize = statusesResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetStatuses",
                    new
                    {
                        fields = statusesResourceParameters.Fields,
                        orderBy = statusesResourceParameters.OrderBy,
                        searchQuery = statusesResourceParameters.SearchQuery,
                        pageNumber = statusesResourceParameters.PageNumber,
                        pageSize = statusesResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{id}", Name = "GetStatus")]
        public IActionResult GetStatus(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<StatusDto>
              (fields))
            {
                return BadRequest();
            }

            var departmentFromRepo = _libraryRepository.GetStatus(id);

            if (departmentFromRepo == null)
            {
                return NotFound();
            }

            var department = Mapper.Map<StatusDto>(departmentFromRepo);

            var links = CreateLinksForStatus(id, fields);

            var linkedResourceToReturn = department.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateStatus")]
        public IActionResult CreateStatus([FromBody] StatusForCreationDto department)
        {
            if (department == null)
            {
                return BadRequest();
            }

            var departmentEntity = Mapper.Map<MstStatus>(department);

            _libraryRepository.AddStatus(departmentEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an department failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var departmentToReturn = Mapper.Map<StatusDto>(departmentEntity);

            var links = CreateLinksForStatus(departmentToReturn.StatusID, null);

            var linkedResourceToReturn = departmentToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetStatus",
                new { id = linkedResourceToReturn["StatusID"] },
                linkedResourceToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockStatusCreation(Guid id)
        {
            if (_libraryRepository.StatusExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteStatus")]
        public IActionResult DeleteStatus(Guid id)
        {
            var departmentFromRepo = _libraryRepository.GetStatus(id);
            if (departmentFromRepo == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteStatus(departmentFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Deleting department {id} failed on save.");
            }

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForStatus(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetStatus", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetStatus", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("DeleteStatus", new { id = id }),
              "delete_department",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForStatus", new { departmentId = id }),
              "create_book_for_department",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link("GetBooksForStatus", new { departmentId = id }),
               "books",
               "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForStatuses(
            StatusesResourceParameters statusesResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateStatusesResourceUri(statusesResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateStatusesResourceUri(statusesResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateStatusesResourceUri(statusesResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        [HttpOptions]
        public IActionResult GetStatusesOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
    }
}