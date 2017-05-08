using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.Area;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ESPL.KP.Controllerss.Area
{
    [Route("api/areas")]
    [Authorize]
    public class AreasController : Controller
    {
        private IAppRepository _appRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public AreasController(IAppRepository appRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _appRepository = appRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetAreas")]
        [HttpHead]
        [Authorize(Policy = Permissions.AreaRead)]
        public IActionResult GetAreas(AreasResourceParameters AreasResourceParameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<AreaDto, MstArea>
                (AreasResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<AreaDto>
                (AreasResourceParameters.Fields))
            {
                return BadRequest();
            }

            var AreasFromRepo = _appRepository.GetAreas(AreasResourceParameters);

            var Areas = Mapper.Map<IEnumerable<AreaDto>>(AreasFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = AreasFromRepo.TotalCount,
                    pageSize = AreasFromRepo.PageSize,
                    currentPage = AreasFromRepo.CurrentPage,
                    totalPages = AreasFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForAreas(AreasResourceParameters,
                    AreasFromRepo.HasNext, AreasFromRepo.HasPrevious);

                var shapedAreas = Areas.ShapeData(AreasResourceParameters.Fields);

                var shapedAreasWithLinks = shapedAreas.Select(Area =>
                {
                    var AreaAsDictionary = Area as IDictionary<string, object>;
                    var AreaLinks = CreateLinksForArea(
                        (Guid)AreaAsDictionary["Id"], AreasResourceParameters.Fields);

                    AreaAsDictionary.Add("links", AreaLinks);

                    return AreaAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedAreasWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = AreasFromRepo.HasPrevious ?
                    CreateAreasResourceUri(AreasResourceParameters,
                        ResourceUriType.PreviousPage) : null;

                var nextPageLink = AreasFromRepo.HasNext ?
                    CreateAreasResourceUri(AreasResourceParameters,
                        ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = AreasFromRepo.TotalCount,
                    pageSize = AreasFromRepo.PageSize,
                    currentPage = AreasFromRepo.CurrentPage,
                    totalPages = AreasFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(Areas.ShapeData(AreasResourceParameters.Fields));
            }
        }

        private string CreateAreasResourceUri(
            AreasResourceParameters AreasResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetAreas",
                        new
                        {
                            fields = AreasResourceParameters.Fields,
                            orderBy = AreasResourceParameters.OrderBy,
                            searchQuery = AreasResourceParameters.SearchQuery,
                            pageNumber = AreasResourceParameters.PageNumber - 1,
                            pageSize = AreasResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetAreas",
                        new
                        {
                            fields = AreasResourceParameters.Fields,
                            orderBy = AreasResourceParameters.OrderBy,
                            searchQuery = AreasResourceParameters.SearchQuery,
                            pageNumber = AreasResourceParameters.PageNumber + 1,
                            pageSize = AreasResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetAreas",
                        new
                        {
                            fields = AreasResourceParameters.Fields,
                            orderBy = AreasResourceParameters.OrderBy,
                            searchQuery = AreasResourceParameters.SearchQuery,
                            pageNumber = AreasResourceParameters.PageNumber,
                            pageSize = AreasResourceParameters.PageSize
                        });
            }
        }

        [HttpGet("{id}", Name = "GetArea")]
        [Authorize(Policy = Permissions.AreaRead)]
        public IActionResult GetArea(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<AreaDto>
                (fields))
            {
                return BadRequest();
            }

            var AreaFromRepo = _appRepository.GetArea(id);

            if (AreaFromRepo == null)
            {
                return NotFound();
            }

            var Area = Mapper.Map<AreaDto>(AreaFromRepo);

            var links = CreateLinksForArea(id, fields);

            var linkedResourceToReturn = Area.ShapeData(fields)
            as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateArea")]
        [Authorize(Policy = Permissions.AreaCreate)]
        public IActionResult CreateArea([FromBody] AreaForCreationDto Area)
        {
            if (Area == null)
            {
                return BadRequest();
            }

            var AreaEntity = Mapper.Map<MstArea>(Area);

            SetCreationUserData(AreaEntity);

            _appRepository.AddArea(AreaEntity);

            if (!_appRepository.Save())
            {
                throw new Exception("Creating an Area failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var AreaToReturn = Mapper.Map<AreaDto>(AreaEntity);

            var links = CreateLinksForArea(AreaToReturn.AreaID, null);

            var linkedResourceToReturn = AreaToReturn.ShapeData(null)
            as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetArea",
                new { id = linkedResourceToReturn["AreaID"] },
                linkedResourceToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockAreaCreation(Guid id)
        {
            if (_appRepository.AreaExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteArea")]
        [Authorize(Policy = Permissions.AreaDelete)]
        public IActionResult DeleteArea(Guid id)
        {
            var AreaFromRepo = _appRepository.GetArea(id);
            if (AreaFromRepo == null)
            {
                return NotFound();
            }

            //....... Soft Delete
            AreaFromRepo.IsDelete = true;
            if (!_appRepository.Save())
            {
                throw new Exception($"Deleting Area {id} failed on save.");
            }

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateArea")]
        [Authorize(Policy = Permissions.AreaUpdate)]
        public IActionResult UpdateArea(Guid id, [FromBody] AreaForUpdationDto area)
        {
            if (area == null)
            {
                return BadRequest();
            }
            var areaRepo = _appRepository.GetArea(id);

            if (areaRepo == null)
            {
                return NotFound();
            }

            SetItemHistoryData(area, areaRepo);
            Mapper.Map(area, areaRepo);
            _appRepository.UpdateArea(areaRepo);
            if (!_appRepository.Save())
            {
                throw new Exception("Updating an area failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }


            return Ok(areaRepo);
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateArea")]
        [Authorize(Policy = Permissions.AreaUpdate)]
        public IActionResult PartiallyUpdateArea(Guid id,
                    [FromBody] JsonPatchDocument<AreaForUpdationDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var areaFromRepo = _appRepository.GetArea(id);

            if (areaFromRepo == null)
            {
                return NotFound();
            }

            var areaToPatch = Mapper.Map<AreaForUpdationDto>(areaFromRepo);

            patchDoc.ApplyTo(areaToPatch, ModelState);

            // patchDoc.ApplyTo(areaToPatch);

            TryValidateModel(areaToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            SetItemHistoryData(areaToPatch, areaFromRepo);
            Mapper.Map(areaToPatch, areaFromRepo);

            _appRepository.UpdateArea(areaFromRepo);

            if (!_appRepository.Save())
            {
                throw new Exception($"Patching  area {id} failed on save.");
            }

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForArea(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetArea", new { id = id }),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetArea", new { id = id, fields = fields }),
                        "self",
                        "GET"));
            }

            links.Add(
                new LinkDto(_urlHelper.Link("DeleteArea", new { id = id }),
                    "delete_Area",
                    "DELETE"));

            links.Add(
                new LinkDto(_urlHelper.Link("CreateBookForArea", new { AreaId = id }),
                    "create_book_for_Area",
                    "POST"));

            links.Add(
                new LinkDto(_urlHelper.Link("GetBooksForArea", new { AreaId = id }),
                    "books",
                    "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForAreas(
            AreasResourceParameters AreasResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
                new LinkDto(CreateAreasResourceUri(AreasResourceParameters,
                    ResourceUriType.Current), "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateAreasResourceUri(AreasResourceParameters,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateAreasResourceUri(AreasResourceParameters,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }

        [HttpOptions]
        public IActionResult GetAreasOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        private void SetItemHistoryData(AreaForUpdationDto model, MstArea modelRepo)
        {
            model.CreatedOn = modelRepo.CreatedOn;
            if (modelRepo.CreatedBy != null)
                model.CreatedBy = modelRepo.CreatedBy.Value;
            model.UpdatedOn = DateTime.Now;
            var EmployeeID = User.Claims.FirstOrDefault(cl => cl.Type == "EmployeeID");
            model.UpdatedBy = new Guid(EmployeeID.Value);
        }

        private void SetCreationUserData(MstArea model)
        {
            var EmployeeID = User.Claims.FirstOrDefault(cl => cl.Type == "EmployeeID");
            model.CreatedBy = new Guid(EmployeeID.Value);
        }


    }
}