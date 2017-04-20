using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.OccurrenceType;
using ESPL.KP.Models;
using ESPL.KP.Models.OccurrenceType;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESPL.KP.Controllers.OccurrenceType
{
    [Route("api/occurrencetype")]
    public class OccurrenceTypeController : Controller
    {
            private ILibraryRepository _libraryRepository;
            private IUrlHelper _urlHelper;
            private IPropertyMappingService _propertyMappingService;
            private ITypeHelperService _typeHelperService;

            public OccurrenceTypeController(ILibraryRepository libraryRepository,
                IUrlHelper urlHelper,
                IPropertyMappingService propertyMappingService,
                ITypeHelperService typeHelperService)
            {
                _libraryRepository = libraryRepository;
                _urlHelper = urlHelper;
                _propertyMappingService = propertyMappingService;
                _typeHelperService = typeHelperService;
            }

            [HttpGet(Name = "GetoccurrenceType")]
            [HttpHead]
            public IActionResult GetoccurrenceType(OccurrenceTypeResourceParameters occurrenceTypeResourceParameters,
                [FromHeader(Name = "Accept")] string mediaType)
            {
                if (!_propertyMappingService.ValidMappingExistsFor<OccurrenceTypeDto, MstOccurrenceType>
                   (occurrenceTypeResourceParameters.OrderBy))
                {
                    return BadRequest();
                }

                if (!_typeHelperService.TypeHasProperties<OccurrenceTypeDto>
                    (occurrenceTypeResourceParameters.Fields))
                {
                    return BadRequest();
                }

                var occurrenceTypeFromRepo = _libraryRepository.GetOccurrenceTypes(occurrenceTypeResourceParameters);

                var occurrenceType = Mapper.Map<IEnumerable<OccurrenceTypeDto>>(occurrenceTypeFromRepo);

                if (mediaType == "application/vnd.marvin.hateoas+json")
                {
                    var paginationMetadata = new
                    {
                        totalCount = occurrenceTypeFromRepo.TotalCount,
                        pageSize = occurrenceTypeFromRepo.PageSize,
                        currentPage = occurrenceTypeFromRepo.CurrentPage,
                        totalPages = occurrenceTypeFromRepo.TotalPages,
                    };

                    Response.Headers.Add("X-Pagination",
                        Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                    var links = CreateLinksForOccurrenceType(occurrenceTypeResourceParameters,
                        occurrenceTypeFromRepo.HasNext, occurrenceTypeFromRepo.HasPrevious);

                    var shapedoccurrenceType = occurrenceType.ShapeData(occurrenceTypeResourceParameters.Fields);

                    var shapedoccurrenceTypeWithLinks = shapedoccurrenceType.Select(department =>
                    {
                        var departmentAsDictionary = department as IDictionary<string, object>;
                        var departmentLinks = CreateLinksForDepartment(
                            (Guid)departmentAsDictionary["Id"], occurrenceTypeResourceParameters.Fields);

                        departmentAsDictionary.Add("links", departmentLinks);

                        return departmentAsDictionary;
                    });

                    var linkedCollectionResource = new
                    {
                        value = shapedoccurrenceTypeWithLinks,
                        links = links
                    };

                    return Ok(linkedCollectionResource);
                }
                else
                {
                    var previousPageLink = occurrenceTypeFromRepo.HasPrevious ?
                        CreateOccurrenceTypeResourceUri(occurrenceTypeResourceParameters,
                        ResourceUriType.PreviousPage) : null;

                    var nextPageLink = occurrenceTypeFromRepo.HasNext ?
                        CreateOccurrenceTypeResourceUri(occurrenceTypeResourceParameters,
                        ResourceUriType.NextPage) : null;

                    var paginationMetadata = new
                    {
                        previousPageLink = previousPageLink,
                        nextPageLink = nextPageLink,
                        totalCount = occurrenceTypeFromRepo.TotalCount,
                        pageSize = occurrenceTypeFromRepo.PageSize,
                        currentPage = occurrenceTypeFromRepo.CurrentPage,
                        totalPages = occurrenceTypeFromRepo.TotalPages
                    };

                    Response.Headers.Add("X-Pagination",
                        Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                    return Ok(occurrenceType.ShapeData(occurrenceTypeResourceParameters.Fields));
                }
            }

            private string CreateOccurrenceTypeResourceUri(
                OccurrenceTypeResourceParameters occurrenceTypeResourceParameters,
                ResourceUriType type)
            {
                switch (type)
                {
                    case ResourceUriType.PreviousPage:
                        return _urlHelper.Link("GetOccurrenceType",
                          new
                          {
                              fields = occurrenceTypeResourceParameters.Fields,
                              orderBy = occurrenceTypeResourceParameters.OrderBy,
                              searchQuery = occurrenceTypeResourceParameters.SearchQuery,
                              pageNumber = occurrenceTypeResourceParameters.PageNumber - 1,
                              pageSize = occurrenceTypeResourceParameters.PageSize
                          });
                    case ResourceUriType.NextPage:
                        return _urlHelper.Link("GetOccurrenceType",
                          new
                          {
                              fields = occurrenceTypeResourceParameters.Fields,
                              orderBy = occurrenceTypeResourceParameters.OrderBy,
                              searchQuery = occurrenceTypeResourceParameters.SearchQuery,
                              pageNumber = occurrenceTypeResourceParameters.PageNumber + 1,
                              pageSize = occurrenceTypeResourceParameters.PageSize
                          });
                    case ResourceUriType.Current:
                    default:
                        return _urlHelper.Link("GetOccurrenceType",
                        new
                        {
                            fields = occurrenceTypeResourceParameters.Fields,
                            orderBy = occurrenceTypeResourceParameters.OrderBy,
                            searchQuery = occurrenceTypeResourceParameters.SearchQuery,
                            pageNumber = occurrenceTypeResourceParameters.PageNumber,
                            pageSize = occurrenceTypeResourceParameters.PageSize
                        });
                }
            }

            [HttpGet("{id}", Name = "GetOccurrenceType")]
            public IActionResult GetOccurrenceType(Guid id, [FromQuery] string fields)
            {
                if (!_typeHelperService.TypeHasProperties<OccurrenceTypeDto>
                  (fields))
                {
                    return BadRequest();
                }

                var departmentFromRepo = _libraryRepository.GetOccurrenceType(id);

                if (departmentFromRepo == null)
                {
                    return NotFound();
                }

                var department = Mapper.Map<OccurrenceTypeDto>(departmentFromRepo);

                var links = CreateLinksForDepartment(id, fields);

                var linkedResourceToReturn = department.ShapeData(fields)
                    as IDictionary<string, object>;

                linkedResourceToReturn.Add("links", links);

                return Ok(linkedResourceToReturn);
            }

            [HttpPost(Name = "CreateDepartment")]
            [RequestHeaderMatchesMediaType("Content-Type",
                new[] { "application/vnd.marvin.department.full+json" })]
            public IActionResult CreateDepartment([FromBody] OccurrenceTypeDto department)
            {
                if (department == null)
                {
                    return BadRequest();
                }

                var departmentEntity = Mapper.Map<MstOccurrenceType>(department);

                _libraryRepository.AddOccurrenceType(departmentEntity);

                if (!_libraryRepository.Save())
                {
                    throw new Exception("Creating an department failed on save.");
                    // return StatusCode(500, "A problem happened with handling your request.");
                }

                var departmentToReturn = Mapper.Map<OccurrenceTypeDto>(departmentEntity);

                var links = CreateLinksForDepartment(departmentToReturn.OccurrenceTypeID, null);

                var linkedResourceToReturn = departmentToReturn.ShapeData(null)
                    as IDictionary<string, object>;

                linkedResourceToReturn.Add("links", links);

                return CreatedAtRoute("GetDepartment",
                    new { id = linkedResourceToReturn["Id"] },
                    linkedResourceToReturn);
            }

            [HttpPost("{id}")]
            public IActionResult BlockDepartmentCreation(Guid id)
            {
                if (_libraryRepository.DepartmentExists(id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }

                return NotFound();
            }

            [HttpDelete("{id}", Name = "DeleteDepartment")]
            public IActionResult DeleteDepartment(Guid id)
            {
                var departmentFromRepo = _libraryRepository.GetDepartment(id);
                if (departmentFromRepo == null)
                {
                    return NotFound();
                }

                _libraryRepository.DeleteDepartment(departmentFromRepo);

                if (!_libraryRepository.Save())
                {
                    throw new Exception($"Deleting department {id} failed on save.");
                }

                return NoContent();
            }

            private IEnumerable<LinkDto> CreateLinksForDepartment(Guid id, string fields)
            {
                var links = new List<LinkDto>();

                if (string.IsNullOrWhiteSpace(fields))
                {
                    links.Add(
                      new LinkDto(_urlHelper.Link("GetDepartment", new { id = id }),
                      "self",
                      "GET"));
                }
                else
                {
                    links.Add(
                      new LinkDto(_urlHelper.Link("GetDepartment", new { id = id, fields = fields }),
                      "self",
                      "GET"));
                }

                links.Add(
                  new LinkDto(_urlHelper.Link("DeleteDepartment", new { id = id }),
                  "delete_department",
                  "DELETE"));

                links.Add(
                  new LinkDto(_urlHelper.Link("CreateBookForDepartment", new { departmentId = id }),
                  "create_book_for_department",
                  "POST"));

                links.Add(
                   new LinkDto(_urlHelper.Link("GetBooksForDepartment", new { departmentId = id }),
                   "books",
                   "GET"));

                return links;
            }

            private IEnumerable<LinkDto> CreateLinksForOccurrenceType(
                OccurrenceTypeResourceParameters occurrenceTypeResourceParameters,
                bool hasNext, bool hasPrevious)
            {
                var links = new List<LinkDto>();

                // self 
                links.Add(
                   new LinkDto(CreateOccurrenceTypeResourceUri(occurrenceTypeResourceParameters,
                   ResourceUriType.Current)
                   , "self", "GET"));

                if (hasNext)
                {
                    links.Add(
                      new LinkDto(CreateOccurrenceTypeResourceUri(occurrenceTypeResourceParameters,
                      ResourceUriType.NextPage),
                      "nextPage", "GET"));
                }

                if (hasPrevious)
                {
                    links.Add(
                        new LinkDto(CreateOccurrenceTypeResourceUri(occurrenceTypeResourceParameters,
                        ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
                }

                return links;
            }

            [HttpOptions]
            public IActionResult GetOccurrenceTypeOptions()
            {
                Response.Headers.Add("Allow", "GET,OPTIONS,POST");
                return Ok();
            }
        
    }
}