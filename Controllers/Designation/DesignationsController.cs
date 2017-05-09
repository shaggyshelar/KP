using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.Designation;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace ESPL.KP.Controllerss.Designation
{
    [Route("api/Designations")]
    [Authorize]
    public class DesignationsController : Controller
    {
        private IAppRepository _appRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public DesignationsController(IAppRepository appRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _appRepository = appRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetDesignations")]
        [HttpHead]
        [Authorize(Policy = Permissions.DesignationRead)]
        public IActionResult GetDesignations(DesignationsResourceParameters DesignationsResourceParameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<DesignationDto, MstDesignation>
                (DesignationsResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<DesignationDto>
                (DesignationsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var DesignationsFromRepo = _appRepository.GetDesignations(DesignationsResourceParameters);

            var Designations = Mapper.Map<IEnumerable<DesignationDto>>(DesignationsFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = DesignationsFromRepo.TotalCount,
                    pageSize = DesignationsFromRepo.PageSize,
                    currentPage = DesignationsFromRepo.CurrentPage,
                    totalPages = DesignationsFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForDesignations(DesignationsResourceParameters,
                    DesignationsFromRepo.HasNext, DesignationsFromRepo.HasPrevious);

                var shapedDesignations = Designations.ShapeData(DesignationsResourceParameters.Fields);

                var shapedDesignationsWithLinks = shapedDesignations.Select(Designation =>
                {
                    var DesignationAsDictionary = Designation as IDictionary<string, object>;
                    var DesignationLinks = CreateLinksForDesignation(
                        (Guid)DesignationAsDictionary["Id"], DesignationsResourceParameters.Fields);

                    DesignationAsDictionary.Add("links", DesignationLinks);

                    return DesignationAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedDesignationsWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = DesignationsFromRepo.HasPrevious ?
                    CreateDesignationsResourceUri(DesignationsResourceParameters,
                        ResourceUriType.PreviousPage) : null;

                var nextPageLink = DesignationsFromRepo.HasNext ?
                    CreateDesignationsResourceUri(DesignationsResourceParameters,
                        ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = DesignationsFromRepo.TotalCount,
                    pageSize = DesignationsFromRepo.PageSize,
                    currentPage = DesignationsFromRepo.CurrentPage,
                    totalPages = DesignationsFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(Designations.ShapeData(DesignationsResourceParameters.Fields));
            }
        }

        private string CreateDesignationsResourceUri(
            DesignationsResourceParameters DesignationsResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetDesignations",
                        new
                        {
                            fields = DesignationsResourceParameters.Fields,
                            orderBy = DesignationsResourceParameters.OrderBy,
                            searchQuery = DesignationsResourceParameters.SearchQuery,
                            pageNumber = DesignationsResourceParameters.PageNumber - 1,
                            pageSize = DesignationsResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetDesignations",
                        new
                        {
                            fields = DesignationsResourceParameters.Fields,
                            orderBy = DesignationsResourceParameters.OrderBy,
                            searchQuery = DesignationsResourceParameters.SearchQuery,
                            pageNumber = DesignationsResourceParameters.PageNumber + 1,
                            pageSize = DesignationsResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetDesignations",
                        new
                        {
                            fields = DesignationsResourceParameters.Fields,
                            orderBy = DesignationsResourceParameters.OrderBy,
                            searchQuery = DesignationsResourceParameters.SearchQuery,
                            pageNumber = DesignationsResourceParameters.PageNumber,
                            pageSize = DesignationsResourceParameters.PageSize
                        });
            }
        }

        [HttpGet("{id}", Name = "GetDesignation")]
        [Authorize(Policy = Permissions.DesignationRead)]
        public IActionResult GetDesignation(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<DesignationDto>
                (fields))
            {
                return BadRequest();
            }

            var DesignationFromRepo = _appRepository.GetDesignation(id);

            if (DesignationFromRepo == null)
            {
                return NotFound();
            }

            var Designation = Mapper.Map<DesignationDto>(DesignationFromRepo);

            var links = CreateLinksForDesignation(id, fields);

            var linkedResourceToReturn = Designation.ShapeData(fields)
            as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateDesignation")]
        [Authorize(Policy = Permissions.DesignationCreate)]
        public IActionResult CreateDesignation([FromBody] DesignationForCreationDto Designation)
        {
            if (Designation == null)
            {
                return BadRequest();
            }

            var DesignationEntity = Mapper.Map<MstDesignation>(Designation);

            SetCreationUserData(DesignationEntity);

            _appRepository.AddDesignation(DesignationEntity);

            if (!_appRepository.Save())
            {
                throw new Exception("Creating an Designation failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var DesignationToReturn = Mapper.Map<DesignationDto>(DesignationEntity);

            var links = CreateLinksForDesignation(DesignationToReturn.DesignationID, null);

            var linkedResourceToReturn = DesignationToReturn.ShapeData(null)
            as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetDesignation",
                new { id = linkedResourceToReturn["DesignationID"] },
                linkedResourceToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockDesignationCreation(Guid id)
        {
            if (_appRepository.DesignationExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteDesignation")]
        [Authorize(Policy = Permissions.DesignationDelete)]
        public IActionResult DeleteDesignation(Guid id)
        {
            var DesignationFromRepo = _appRepository.GetDesignation(id);
            if (DesignationFromRepo == null)
            {
                return NotFound();
            }

            //_appRepository.DeleteDesignation(DesignationFromRepo);
            //....... Soft Delete
            DesignationFromRepo.IsDelete = true;

            if (!_appRepository.Save())
            {
                throw new Exception($"Deleting Designation {id} failed on save.");
            }

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateDesignation")]
        [Authorize(Policy = Permissions.DesignationUpdate)]
        public IActionResult UpdateDesignation(Guid id, [FromBody] DesignationForUpdationDto designation)
        {
            if (designation == null)
            {
                return BadRequest();
            }
            // if (!_appRepository.OccurrenceBookExists(id))
            // {
            //     return NotFound();
            // }
            //Mapper.Map(source,destination);
            var designationRepo = _appRepository.GetDesignation(id);

            if (designationRepo == null)
            {
                return NotFound();
            }
            SetItemHistoryData(designation, designationRepo);
            Mapper.Map(designation, designationRepo);
            _appRepository.UpdateDesignation(designationRepo);
            if (!_appRepository.Save())
            {
                throw new Exception("Updating an designation failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }


            return Ok(designationRepo);
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateDesignation")]
        [Authorize(Policy = Permissions.DesignationUpdate)]
        public IActionResult PartiallyUpdateDesignation(Guid id,
                    [FromBody] JsonPatchDocument<DesignationForUpdationDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var designationFromRepo = _appRepository.GetDesignation(id);

            if (designationFromRepo == null)
            {
                // var designationDto = new DesignationForCreationDto();
                // patchDoc.ApplyTo(designationDto, ModelState);

                // TryValidateModel(designationDto);

                // if (!ModelState.IsValid)
                // {
                //     return new UnprocessableEntityObjectResult(ModelState);
                // }

                // var designationToAdd = Mapper.Map<MstDesignation>(designationDto);
                // designationToAdd.DesignationID = id;

                // _appRepository.AddDesignation(designationToAdd);

                // if (!_appRepository.Save())
                // {
                //     throw new Exception($"Upserting in designation {id} failed on save.");
                // }

                // var designationToReturn = Mapper.Map<DesignationDto>(designationToAdd);
                // return CreatedAtRoute("GetDesignation",
                //     new { DesignationID = designationToReturn.DesignationID },
                //     designationToReturn);
                return NotFound();
            }

            var designationToPatch = Mapper.Map<DesignationForUpdationDto>(designationFromRepo);

            patchDoc.ApplyTo(designationToPatch, ModelState);

            // patchDoc.ApplyTo(designationToPatch);

            TryValidateModel(designationToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            SetItemHistoryData(designationToPatch, designationFromRepo);
            Mapper.Map(designationToPatch, designationFromRepo);

            _appRepository.UpdateDesignation(designationFromRepo);

            if (!_appRepository.Save())
            {
                throw new Exception($"Patching  designation {id} failed on save.");
            }

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForDesignation(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetDesignation", new { id = id }),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetDesignation", new { id = id, fields = fields }),
                        "self",
                        "GET"));
            }

            links.Add(
                new LinkDto(_urlHelper.Link("DeleteDesignation", new { id = id }),
                    "delete_Designation",
                    "DELETE"));

            links.Add(
                new LinkDto(_urlHelper.Link("CreateBookForDesignation", new { DesignationId = id }),
                    "create_book_for_Designation",
                    "POST"));

            links.Add(
                new LinkDto(_urlHelper.Link("GetBooksForDesignation", new { DesignationId = id }),
                    "books",
                    "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForDesignations(
            DesignationsResourceParameters DesignationsResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
                new LinkDto(CreateDesignationsResourceUri(DesignationsResourceParameters,
                    ResourceUriType.Current), "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateDesignationsResourceUri(DesignationsResourceParameters,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateDesignationsResourceUri(DesignationsResourceParameters,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }

        [HttpOptions]
        public IActionResult GetDesignationsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        private void SetItemHistoryData(DesignationForUpdationDto model, MstDesignation modelRepo)
        {
             model.CreatedOn = modelRepo.CreatedOn;
            if (modelRepo.CreatedBy != null)
                model.CreatedBy = modelRepo.CreatedBy.Value;
            model.UpdatedOn = DateTime.Now;
            var EmployeeID = User.Claims.FirstOrDefault(cl => cl.Type == "EmployeeID");
            model.UpdatedBy = new Guid(EmployeeID.Value);
        }

        private void SetCreationUserData(MstDesignation model)
        {
            var EmployeeID = User.Claims.FirstOrDefault(cl => cl.Type == "EmployeeID");
            model.CreatedBy = new Guid(EmployeeID.Value);
        }

    }
}