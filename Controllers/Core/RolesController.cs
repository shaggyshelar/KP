using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESPL.KP.Helpers;
using AutoMapper;
using ESPL.KP.Entities;
using Microsoft.AspNetCore.Http;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Entities.Core;
using ESPL.KP.Models.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ESPL.KP.Controllers.Core
{
    [Route("api/roles")]
    [Authorize(Policy = "IsSuperAdmin")]
    public class RolesController : Controller
    {
        private ILibraryRepository _libraryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public RolesController(ILibraryRepository libraryRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _libraryRepository = libraryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetESPLRoles")]
        [HttpHead]
        public IActionResult GetESPLRoles(ESPLRolesResourceParameters esplRolesResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<ESPLRoleDto, IdentityRole>
               (esplRolesResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<ESPLRoleDto>
                (esplRolesResourceParameters.Fields))
            {
                return BadRequest();
            }

            var esplRolesFromRepo = _libraryRepository.GetESPLRoles(esplRolesResourceParameters);

            var esplRoles = new List<ESPLRoleDto>();
            esplRolesFromRepo.ForEach(esplRole =>
            {
                esplRoles.Add(
                new ESPLRoleDto()
                {
                    Id = new Guid(esplRole.Id),
                    Name = esplRole.Name
                });
            });

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = esplRolesFromRepo.TotalCount,
                    pageSize = esplRolesFromRepo.PageSize,
                    currentPage = esplRolesFromRepo.CurrentPage,
                    totalPages = esplRolesFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForESPLRoles(esplRolesResourceParameters,
                    esplRolesFromRepo.HasNext, esplRolesFromRepo.HasPrevious);

                var shapedESPLRoles = esplRoles.ShapeData(esplRolesResourceParameters.Fields);
                var linkedCollectionResource = new
                {
                    value = shapedESPLRoles,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = esplRolesFromRepo.HasPrevious ?
                    CreateESPLRolesResourceUri(esplRolesResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = esplRolesFromRepo.HasNext ?
                    CreateESPLRolesResourceUri(esplRolesResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = esplRolesFromRepo.TotalCount,
                    pageSize = esplRolesFromRepo.PageSize,
                    currentPage = esplRolesFromRepo.CurrentPage,
                    totalPages = esplRolesFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(esplRoles);
            }
        }

        private string CreateESPLRolesResourceUri(
            ESPLRolesResourceParameters esplRolesResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetESPLRoles",
                      new
                      {
                          fields = esplRolesResourceParameters.Fields,
                          orderBy = esplRolesResourceParameters.OrderBy,
                          searchQuery = esplRolesResourceParameters.SearchQuery,
                          pageNumber = esplRolesResourceParameters.PageNumber - 1,
                          pageSize = esplRolesResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetESPLRoles",
                      new
                      {
                          fields = esplRolesResourceParameters.Fields,
                          orderBy = esplRolesResourceParameters.OrderBy,
                          searchQuery = esplRolesResourceParameters.SearchQuery,
                          pageNumber = esplRolesResourceParameters.PageNumber + 1,
                          pageSize = esplRolesResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetESPLRoles",
                    new
                    {
                        fields = esplRolesResourceParameters.Fields,
                        orderBy = esplRolesResourceParameters.OrderBy,
                        searchQuery = esplRolesResourceParameters.SearchQuery,
                        pageNumber = esplRolesResourceParameters.PageNumber,
                        pageSize = esplRolesResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{id}", Name = "GetESPLRole")]
        public IActionResult GetESPLRole(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<ESPLRoleDto>
              (fields))
            {
                return BadRequest();
            }

            var esplRoleFromRepo = _libraryRepository.GetESPLRole(id);

            if (esplRoleFromRepo == null)
            {
                return NotFound();
            }

            var esplRole = Mapper.Map<ESPLRoleDto>(esplRoleFromRepo);

            var links = CreateLinksForESPLRole(id, fields);

            var linkedResourceToReturn = esplRole.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateESPLRole")]
        public IActionResult CreateESPLRole([FromBody] ESPLRoleForCreationDto esplRole)
        {
            if (esplRole == null)
            {
                return BadRequest();
            }

            var esplRoleEntity = Mapper.Map<IdentityRole>(esplRole);

            _libraryRepository.AddESPLRole(esplRoleEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an esplRole failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var esplRoleToReturn = Mapper.Map<ESPLRoleDto>(esplRoleEntity);

            var links = CreateLinksForESPLRole(esplRoleToReturn.Id, null);

            var linkedResourceToReturn = esplRoleToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetESPLRole",
                new { id = linkedResourceToReturn["Id"] },
                linkedResourceToReturn);
        }


        [HttpPost("{id}")]
        public IActionResult BlockESPLRoleCreation(Guid id)
        {
            if (_libraryRepository.ESPLRoleExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteESPLRole")]
        public IActionResult DeleteESPLRole(Guid id)
        {
            var esplRoleFromRepo = _libraryRepository.GetESPLRole(id);
            if (esplRoleFromRepo == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteESPLRole(esplRoleFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Deleting esplRole {id} failed on save.");
            }

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForESPLRole(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetESPLRole", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetESPLRole", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("DeleteESPLRole", new { id = id }),
              "delete_esplRole",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForESPLRole", new { esplRoleId = id }),
              "create_book_for_esplRole",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link("GetBooksForESPLRole", new { esplRoleId = id }),
               "books",
               "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForESPLRoles(
            ESPLRolesResourceParameters esplRolesResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateESPLRolesResourceUri(esplRolesResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateESPLRolesResourceUri(esplRolesResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateESPLRolesResourceUri(esplRolesResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        [HttpOptions]
        public IActionResult GetESPLRolesOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
    }
}