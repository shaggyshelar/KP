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
using ESPL.KP.Models.Core;
using ESPL.KP.Entities.Core;
using Microsoft.AspNetCore.Authorization;

namespace ESPL.KP.Controllers.Core
{
    [Route("api/appmodules")]
    [Authorize(Policy = "IsSuperAdmin")]
    public class AppModulesController : Controller
    {
        private ILibraryRepository _libraryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public AppModulesController(ILibraryRepository libraryRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _libraryRepository = libraryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetAppModules")]
        [HttpHead]
        public IActionResult GetAppModules(AppModulesResourceParameters appModulesResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<AppModuleDto, AppModule>
               (appModulesResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<AppModuleDto>
                (appModulesResourceParameters.Fields))
            {
                return BadRequest();
            }

            var appModulesFromRepo = _libraryRepository.GetAppModules(appModulesResourceParameters);

            var appModules = Mapper.Map<IEnumerable<AppModuleDto>>(appModulesFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = appModulesFromRepo.TotalCount,
                    pageSize = appModulesFromRepo.PageSize,
                    currentPage = appModulesFromRepo.CurrentPage,
                    totalPages = appModulesFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForAppModules(appModulesResourceParameters,
                    appModulesFromRepo.HasNext, appModulesFromRepo.HasPrevious);

                var shapedAppModules = appModules.ShapeData(appModulesResourceParameters.Fields);

                var shapedAppModulesWithLinks = shapedAppModules.Select(appModule =>
                {
                    var appModuleAsDictionary = appModule as IDictionary<string, object>;
                    var appModuleLinks = CreateLinksForAppModule(
                        (Guid)appModuleAsDictionary["Id"], appModulesResourceParameters.Fields);

                    appModuleAsDictionary.Add("links", appModuleLinks);

                    return appModuleAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedAppModulesWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = appModulesFromRepo.HasPrevious ?
                    CreateAppModulesResourceUri(appModulesResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = appModulesFromRepo.HasNext ?
                    CreateAppModulesResourceUri(appModulesResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = appModulesFromRepo.TotalCount,
                    pageSize = appModulesFromRepo.PageSize,
                    currentPage = appModulesFromRepo.CurrentPage,
                    totalPages = appModulesFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(appModules.ShapeData(appModulesResourceParameters.Fields));
            }
        }

        private string CreateAppModulesResourceUri(
            AppModulesResourceParameters appModulesResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetAppModules",
                      new
                      {
                          fields = appModulesResourceParameters.Fields,
                          orderBy = appModulesResourceParameters.OrderBy,
                          searchQuery = appModulesResourceParameters.SearchQuery,
                          pageNumber = appModulesResourceParameters.PageNumber - 1,
                          pageSize = appModulesResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetAppModules",
                      new
                      {
                          fields = appModulesResourceParameters.Fields,
                          orderBy = appModulesResourceParameters.OrderBy,
                          searchQuery = appModulesResourceParameters.SearchQuery,
                          pageNumber = appModulesResourceParameters.PageNumber + 1,
                          pageSize = appModulesResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetAppModules",
                    new
                    {
                        fields = appModulesResourceParameters.Fields,
                        orderBy = appModulesResourceParameters.OrderBy,
                        searchQuery = appModulesResourceParameters.SearchQuery,
                        pageNumber = appModulesResourceParameters.PageNumber,
                        pageSize = appModulesResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{id}", Name = "GetAppModule")]
        public IActionResult GetAppModule(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<AppModuleDto>
              (fields))
            {
                return BadRequest();
            }

            var appModuleFromRepo = _libraryRepository.GetAppModule(id);

            if (appModuleFromRepo == null)
            {
                return NotFound();
            }

            var appModule = Mapper.Map<AppModuleDto>(appModuleFromRepo);

            var links = CreateLinksForAppModule(id, fields);

            var linkedResourceToReturn = appModule.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateAppModule")]
        public IActionResult CreateAppModule([FromBody] AppModuleForCreationDto appModule)
        {
            if (appModule == null)
            {
                return BadRequest();
            }

            var appModuleEntity = Mapper.Map<AppModule>(appModule);

            _libraryRepository.AddAppModule(appModuleEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an appModule failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var appModuleToReturn = Mapper.Map<AppModuleDto>(appModuleEntity);

            var links = CreateLinksForAppModule(appModuleToReturn.Id, null);

            var linkedResourceToReturn = appModuleToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetAppModule",
                new { id = linkedResourceToReturn["Id"] },
                linkedResourceToReturn);
        }


        [HttpPost("{id}")]
        public IActionResult BlockAppModuleCreation(Guid id)
        {
            if (_libraryRepository.AppModuleExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteAppModule")]
        public IActionResult DeleteAppModule(Guid id)
        {
            var appModuleFromRepo = _libraryRepository.GetAppModule(id);
            if (appModuleFromRepo == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteAppModule(appModuleFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Deleting appModule {id} failed on save.");
            }

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForAppModule(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetAppModule", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetAppModule", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("DeleteAppModule", new { id = id }),
              "delete_appModule",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForAppModule", new { appModuleId = id }),
              "create_book_for_appModule",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link("GetBooksForAppModule", new { appModuleId = id }),
               "books",
               "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForAppModules(
            AppModulesResourceParameters appModulesResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateAppModulesResourceUri(appModulesResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateAppModulesResourceUri(appModulesResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateAppModulesResourceUri(appModulesResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        [HttpOptions]
        public IActionResult GetAppModulesOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
    }
}