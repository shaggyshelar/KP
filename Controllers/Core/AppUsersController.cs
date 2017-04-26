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
using ESPL.KP.Models.Core;
using ESPL.KP.Helpers.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESPL.KP.Controllers.Core
{
    [Route("api/appusers")]
    public class AppUserController : Controller
    {
        private ILibraryRepository _libraryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<ESPLUser> _userMgr;


        public AppUserController(ILibraryRepository libraryRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            UserManager<ESPLUser> userMgr,
            RoleManager<IdentityRole> roleMgr)
        {
            _libraryRepository = libraryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        [HttpGet(Name = "GetESPLUsers")]
        public IActionResult GetESPLUsers(ESPLUsersResourceParameters esplUsersResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<ESPLUserDto, ESPLUser>
               (esplUsersResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<ESPLUserDto>
                (esplUsersResourceParameters.Fields))
            {
                return BadRequest();
            }

            var esplUsersFromRepo = _libraryRepository.GetESPLUsers(esplUsersResourceParameters);

            var esplUsers = new List<ESPLUserDto>();
            esplUsersFromRepo.ForEach(esplUser =>
            {
                esplUsers.Add(
                new ESPLUserDto()
                {
                    Id = new Guid(esplUser.Id),
                    FirstName = esplUser.FirstName,
                    LastName = esplUser.LastName,
                    Email = esplUser.Email,
                    UserName = esplUser.UserName
                });
            });

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = esplUsersFromRepo.TotalCount,
                    pageSize = esplUsersFromRepo.PageSize,
                    currentPage = esplUsersFromRepo.CurrentPage,
                    totalPages = esplUsersFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForESPLUsers(esplUsersResourceParameters,
                    esplUsersFromRepo.HasNext, esplUsersFromRepo.HasPrevious);

                var shapedESPLUsers = esplUsers.ShapeData(esplUsersResourceParameters.Fields);
                var linkedCollectionResource = new
                {
                    value = shapedESPLUsers,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = esplUsersFromRepo.HasPrevious ?
                    CreateESPLUsersResourceUri(esplUsersResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = esplUsersFromRepo.HasNext ?
                    CreateESPLUsersResourceUri(esplUsersResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = esplUsersFromRepo.TotalCount,
                    pageSize = esplUsersFromRepo.PageSize,
                    currentPage = esplUsersFromRepo.CurrentPage,
                    totalPages = esplUsersFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(esplUsers);
            }
        }

        private string CreateESPLUsersResourceUri(
            ESPLUsersResourceParameters esplUsersResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetESPLUsers",
                      new
                      {
                          fields = esplUsersResourceParameters.Fields,
                          orderBy = esplUsersResourceParameters.OrderBy,
                          searchQuery = esplUsersResourceParameters.SearchQuery,
                          pageNumber = esplUsersResourceParameters.PageNumber - 1,
                          pageSize = esplUsersResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetESPLUsers",
                      new
                      {
                          fields = esplUsersResourceParameters.Fields,
                          orderBy = esplUsersResourceParameters.OrderBy,
                          searchQuery = esplUsersResourceParameters.SearchQuery,
                          pageNumber = esplUsersResourceParameters.PageNumber + 1,
                          pageSize = esplUsersResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetESPLUsers",
                    new
                    {
                        fields = esplUsersResourceParameters.Fields,
                        orderBy = esplUsersResourceParameters.OrderBy,
                        searchQuery = esplUsersResourceParameters.SearchQuery,
                        pageNumber = esplUsersResourceParameters.PageNumber,
                        pageSize = esplUsersResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{id}", Name = "GetESPLUser")]
        public IActionResult GetESPLUser(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<ESPLUserDto>
              (fields))
            {
                return BadRequest();
            }

            var esplUserFromRepo = _libraryRepository.GetESPLUser(id);

            if (esplUserFromRepo == null)
            {
                return NotFound();
            }

            var esplUser = Mapper.Map<ESPLUserDto>(esplUserFromRepo);

            var links = CreateLinksForESPLUser(id, fields);

            var linkedResourceToReturn = esplUser.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateESPLUser")]
        public IActionResult CreateESPLUser([FromBody] ESPLUserForCreationDto esplUser)
        {
            if (esplUser == null)
            {
                return BadRequest();
            }

            var esplUserEntity = Mapper.Map<ESPLUser>(esplUser);

            _libraryRepository.AddESPLUser(esplUserEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an esplUser failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var esplUserToReturn = Mapper.Map<ESPLUserDto>(esplUserEntity);

            var links = CreateLinksForESPLUser(esplUserToReturn.Id, null);

            var linkedResourceToReturn = esplUserToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetESPLUser",
                new { id = linkedResourceToReturn["Id"] },
                linkedResourceToReturn);
        }


        [HttpPost("{id}")]
        public IActionResult BlockESPLUserCreation(Guid id)
        {
            if (_libraryRepository.ESPLUserExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteESPLUser")]
        public IActionResult DeleteESPLUser(Guid id)
        {
            var esplUserFromRepo = _libraryRepository.GetESPLUser(id);
            if (esplUserFromRepo == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteESPLUser(esplUserFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Deleting esplUser {id} failed on save.");
            }

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForESPLUser(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetESPLUser", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetESPLUser", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("DeleteESPLUser", new { id = id }),
              "delete_esplUser",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForESPLUser", new { esplUserId = id }),
              "create_book_for_esplUser",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link("GetBooksForESPLUser", new { esplUserId = id }),
               "books",
               "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForESPLUsers(
            ESPLUsersResourceParameters esplUsersResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateESPLUsersResourceUri(esplUsersResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateESPLUsersResourceUri(esplUsersResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateESPLUsersResourceUri(esplUsersResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        [HttpOptions]
        public IActionResult GetESPLUsersOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
    }
}