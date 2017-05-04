using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Helpers.OccurrenceType;
using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ESPL.KP.Controllers.OccurrenceType
{
    [Route("api/occurrencetype")]
    [Authorize]
    public class OccurrenceTypeController : Controller
    {
        private IAppRepository _appRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public OccurrenceTypeController(IAppRepository appRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _appRepository = appRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetOccurrenceTypes")]
        [HttpHead]
        [Authorize(Policy = Permissions.OccurrenceTypeRead)]
        public IActionResult GetOccurrenceTypes(OccurrenceTypeResourceParameters occurrenceTypeResourceParameters,
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

            var occurrenceTypeFromRepo = _appRepository.GetOccurrenceTypes(occurrenceTypeResourceParameters);

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

                var shapedoccurrenceTypeWithLinks = shapedoccurrenceType.Select(occType =>
                {
                    var occurrenceTypeAsDictionary = occType as IDictionary<string, object>;
                    var occurrenceTypeLinks = CreateLinksForOccurrenceType(
                        (Guid)occurrenceTypeAsDictionary["Id"], occurrenceTypeResourceParameters.Fields);

                    occurrenceTypeAsDictionary.Add("links", occurrenceTypeLinks);

                    return occurrenceTypeAsDictionary;
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
        [Authorize(Policy = Permissions.OccurrenceTypeRead)]
        public IActionResult GetOccurrenceType(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<OccurrenceTypeDto>
              (fields))
            {
                return BadRequest();
            }

            var occurrenceTypeFromRepo = _appRepository.GetOccurrenceType(id);

            if (occurrenceTypeFromRepo == null)
            {
                return NotFound();
            }

            var occurrenceType = Mapper.Map<OccurrenceTypeDto>(occurrenceTypeFromRepo);

            var links = CreateLinksForOccurrenceType(id, fields);

            var linkedResourceToReturn = occurrenceType.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateOccurrenceType")]
        [Authorize(Policy = Permissions.OccurrenceTypeCreate)]
        // [RequestHeaderMatchesMediaType("Content-Type",
        //     new[] { "application/vnd.marvin.occurrenceType.full+json" })]
        public IActionResult CreateOccurrenceType([FromBody] OccurrenceTypeForCreationDto occurrenceType)
        {
            if (occurrenceType == null)
            {
                return BadRequest();
            }

            var occurrenceTypeEntity = Mapper.Map<MstOccurrenceType>(occurrenceType);

            SetCreationUserData(occurrenceTypeEntity);

            _appRepository.AddOccurrenceType(occurrenceTypeEntity);

            if (!_appRepository.Save())
            {
                throw new Exception("Creating an occurrenceType failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var occurrenceTypeToReturn = Mapper.Map<OccurrenceTypeDto>(occurrenceTypeEntity);

            var links = CreateLinksForOccurrenceType(occurrenceTypeToReturn.OBTypeID, null);

            var linkedResourceToReturn = occurrenceTypeToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetOccurrenceType",
                new { id = linkedResourceToReturn["OBTypeID"] },
                linkedResourceToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockOccurrenceTypeCreation(Guid id)
        {
            if (_appRepository.OccurrenceTypeExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteOccurrenceType")]
        [Authorize(Policy = Permissions.OccurrenceTypeDelete)]
        public IActionResult DeleteOccurrenceType(Guid id)
        {
            var occurrenceTypeFromRepo = _appRepository.GetOccurrenceType(id);
            if (occurrenceTypeFromRepo == null)
            {
                return NotFound();
            }

            //_appRepository.DeleteOccurrenceType(occurrenceTypeFromRepo);

            //....... Soft Delete
            occurrenceTypeFromRepo.IsDelete = true;

            if (!_appRepository.Save())
            {
                throw new Exception($"Deleting occurrenceType {id} failed on save.");
            }

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateOccurrenceType")]
        [Authorize(Policy = Permissions.OccurrenceTypeUpdate)]
        public IActionResult UpdateOccurrenceType(Guid id, [FromBody] OccurrenceTypeForUpdationsDto occurrenceType)
        {
            if (occurrenceType == null)
            {
                return BadRequest();
            }
            // if (!_appRepository.OccurrenceBookExists(id))
            // {
            //     return NotFound();
            // }
            //Mapper.Map(source,destination);
            var occurrenceTypeRepo = _appRepository.GetOccurrenceType(id);

            if (occurrenceTypeRepo == null)
            {
                // var occurrenceTypeAdd = Mapper.Map<MstOccurrenceType>(occurrenceType);
                // occurrenceTypeAdd.OBTypeID = id;

                // _appRepository.AddOccurrenceType(occurrenceTypeAdd);

                // if (!_appRepository.Save())
                // {
                //     throw new Exception($"Upserting occurrenceType {id} failed on save.");
                // }

                // var occurrenceTypeReturnVal = Mapper.Map<OccurrenceTypeDto>(occurrenceTypeAdd);

                // return CreatedAtRoute("GetOccurrenceType",
                //     new { OccurrenceTypeID = occurrenceTypeReturnVal.OBTypeID },
                //     occurrenceTypeReturnVal);
                return NotFound();
            }
            SetItemHistoryData(occurrenceType, occurrenceTypeRepo);
            Mapper.Map(occurrenceType, occurrenceTypeRepo);
            _appRepository.UpdateOccurrenceType(occurrenceTypeRepo);
            if (!_appRepository.Save())
            {
                throw new Exception("Updating an occurrenceType failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }


            return Ok(occurrenceTypeRepo);
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateOccurrenceType")]
        [Authorize(Policy = Permissions.OccurrenceTypeUpdate)]
        public IActionResult PartiallyUpdateOccurrenceType(Guid id,
                    [FromBody] JsonPatchDocument<OccurrenceTypeForUpdationsDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var occurrenceTypeFromRepo = _appRepository.GetOccurrenceType(id);

            if (occurrenceTypeFromRepo == null)
            {
                // var occurrenceTypeDto = new OccurrenceTypeForCreationDto();
                // patchDoc.ApplyTo(occurrenceTypeDto, ModelState);

                // TryValidateModel(occurrenceTypeDto);

                // if (!ModelState.IsValid)
                // {
                //     return new UnprocessableEntityObjectResult(ModelState);
                // }

                // var occurrenceTypeToAdd = Mapper.Map<MstOccurrenceType>(occurrenceTypeDto);
                // occurrenceTypeToAdd.OBTypeID = id;

                // _appRepository.AddOccurrenceType(occurrenceTypeToAdd);

                // if (!_appRepository.Save())
                // {
                //     throw new Exception($"Upserting in occurrenceType {id} failed on save.");
                // }

                // var occurrenceTypeToReturn = Mapper.Map<OccurrenceTypeDto>(occurrenceTypeToAdd);
                // return CreatedAtRoute("GetOccurrenceType",
                //     new { OccurrenceTypeID = occurrenceTypeToReturn.OBTypeID },
                //     occurrenceTypeToReturn);
                return NotFound();
            }

            var occurrenceTypeToPatch = Mapper.Map<OccurrenceTypeForUpdationsDto>(occurrenceTypeFromRepo);

            patchDoc.ApplyTo(occurrenceTypeToPatch, ModelState);

            // patchDoc.ApplyTo(occurrenceTypeToPatch);

            TryValidateModel(occurrenceTypeToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            SetItemHistoryData(occurrenceTypeToPatch, occurrenceTypeFromRepo);
            Mapper.Map(occurrenceTypeToPatch, occurrenceTypeFromRepo);

            _appRepository.UpdateOccurrenceType(occurrenceTypeFromRepo);

            if (!_appRepository.Save())
            {
                throw new Exception($"Patching  occurrenceType {id} failed on save.");
            }

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForOccurrenceType(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetOccurrenceType", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetOccurrenceType", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("DeleteOccurrenceType", new { id = id }),
              "delete_occurrenceType",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForOccurrenceType", new { occurrenceTypeId = id }),
              "create_book_for_occurrenceType",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link("GetBooksForOccurrenceType", new { occurrenceTypeId = id }),
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

        private void SetItemHistoryData(OccurrenceTypeForUpdationsDto model, MstOccurrenceType modelRepo)
        {
            model.CreatedOn = modelRepo.CreatedOn;
            if (modelRepo.CreatedBy != null)
                model.CreatedBy = modelRepo.CreatedBy.Value;
            model.UpdatedOn = DateTime.Now;
            var userId = User.Claims.FirstOrDefault(cl => cl.Type == "UserId");
            model.UpdatedBy = new Guid(userId.Value);
        }

        private void SetCreationUserData(MstOccurrenceType model)
        {
            var userId = User.Claims.FirstOrDefault(cl => cl.Type == "UserId");
            model.CreatedBy = new Guid(userId.Value);
        }


    }
}