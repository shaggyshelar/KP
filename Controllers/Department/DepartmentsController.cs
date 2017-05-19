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
using ESPL.KP.Helpers.Department;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using ESPL.KP.DapperRepositoryInterfaces;

namespace ESPL.KP.Controllers.Department
{
    [Route("api/departments")]
    [Authorize]
    public class DepartmentsController : Controller
    {
        private IDapperRepository _appRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public DepartmentsController(IDapperRepository appRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _appRepository = appRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetDepartments")]
        [HttpHead]
        //[Authorize(Policy = "DP.R")]
        [Authorize(Policy = Permissions.DepartmentRead)]
        public IActionResult GetDepartments(DepartmentsResourceParameters departmentsResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<DepartmentDto, MstDepartment>
               (departmentsResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<DepartmentDto>
                (departmentsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var departmentsFromRepo = _appRepository.GetDepartments(departmentsResourceParameters);

            var departments = Mapper.Map<IEnumerable<DepartmentDto>>(departmentsFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = departmentsFromRepo.TotalCount,
                    pageSize = departmentsFromRepo.PageSize,
                    currentPage = departmentsFromRepo.CurrentPage,
                    totalPages = departmentsFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForDepartments(departmentsResourceParameters,
                    departmentsFromRepo.HasNext, departmentsFromRepo.HasPrevious);

                var shapedDepartments = departments.ShapeData(departmentsResourceParameters.Fields);

                var shapedDepartmentsWithLinks = shapedDepartments.Select(department =>
                {
                    var departmentAsDictionary = department as IDictionary<string, object>;
                    var departmentLinks = CreateLinksForDepartment(
                        (Guid)departmentAsDictionary["Id"], departmentsResourceParameters.Fields);

                    departmentAsDictionary.Add("links", departmentLinks);

                    return departmentAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedDepartmentsWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = departmentsFromRepo.HasPrevious ?
                    CreateDepartmentsResourceUri(departmentsResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = departmentsFromRepo.HasNext ?
                    CreateDepartmentsResourceUri(departmentsResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = departmentsFromRepo.TotalCount,
                    pageSize = departmentsFromRepo.PageSize,
                    currentPage = departmentsFromRepo.CurrentPage,
                    totalPages = departmentsFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(departments.ShapeData(departmentsResourceParameters.Fields));
            }
        }

        private string CreateDepartmentsResourceUri(
            DepartmentsResourceParameters departmentsResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetDepartments",
                      new
                      {
                          fields = departmentsResourceParameters.Fields,
                          orderBy = departmentsResourceParameters.OrderBy,
                          searchQuery = departmentsResourceParameters.SearchQuery,
                          pageNumber = departmentsResourceParameters.PageNumber - 1,
                          pageSize = departmentsResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetDepartments",
                      new
                      {
                          fields = departmentsResourceParameters.Fields,
                          orderBy = departmentsResourceParameters.OrderBy,
                          searchQuery = departmentsResourceParameters.SearchQuery,
                          pageNumber = departmentsResourceParameters.PageNumber + 1,
                          pageSize = departmentsResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetDepartments",
                    new
                    {
                        fields = departmentsResourceParameters.Fields,
                        orderBy = departmentsResourceParameters.OrderBy,
                        searchQuery = departmentsResourceParameters.SearchQuery,
                        pageNumber = departmentsResourceParameters.PageNumber,
                        pageSize = departmentsResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{id}", Name = "GetDepartment")]
        [Authorize(Policy = Permissions.DepartmentRead)]
        public IActionResult GetDepartment(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<DepartmentDto>
              (fields))
            {
                return BadRequest();
            }

            var departmentFromRepo = _appRepository.GetDepartment(id);

            if (departmentFromRepo == null)
            {
                return NotFound();
            }

            var department = Mapper.Map<DepartmentDto>(departmentFromRepo);

            var links = CreateLinksForDepartment(id, fields);

            var linkedResourceToReturn = department.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateDepartment")]
        [Authorize(Policy = Permissions.DepartmentCreate)]
        public IActionResult CreateDepartment([FromBody] DepartmentForCreationDto department)
        {
            if (department == null)
            {
                return BadRequest();
            }

            var departmentEntity = Mapper.Map<MstDepartment>(department);

            SetCreationUserData(departmentEntity);

            _appRepository.AddDepartment(departmentEntity);

            var departmentToReturn = Mapper.Map<DepartmentDto>(departmentEntity);

            var links = CreateLinksForDepartment(departmentToReturn.DepartmentID, null);

            var linkedResourceToReturn = departmentToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetDepartment",
                new { id = linkedResourceToReturn["DepartmentID"] },
                linkedResourceToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockDepartmentCreation(Guid id)
        {
            if (_appRepository.DepartmentExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteDepartment")]
        [Authorize(Policy = Permissions.DepartmentDelete)]
        public IActionResult DeleteDepartment(Guid id)
        {
            var departmentFromRepo = _appRepository.GetDepartment(id);
            if (departmentFromRepo == null)
            {
                return NotFound();
            }

            //_appRepository.DeleteDepartment(departmentFromRepo);
            //....... Soft Delete
            _appRepository.DeleteDepartment(id);

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateDepartment")]
        [Authorize(Policy = Permissions.DepartmentUpdate)]
        public IActionResult UpdateDepartment(Guid id, [FromBody] DepartmentForUpdationDto department)
        {
            if (department == null)
            {
                return BadRequest();
            }
            // if (!_appRepository.OccurrenceBookExists(id))
            // {
            //     return NotFound();
            // }
            //Mapper.Map(source,destination);
            var departmentRepo = _appRepository.GetDepartment(id);

            if (departmentRepo == null)
            {
                return NotFound();
            }
            SetItemHistoryData(department, departmentRepo);
            Mapper.Map(department, departmentRepo);
            _appRepository.UpdateDepartment(departmentRepo);


            return Ok(departmentRepo);
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateDepartment")]
        [Authorize(Policy = Permissions.DepartmentUpdate)]
        public IActionResult PartiallyUpdateDepartment(Guid id,
                    [FromBody] JsonPatchDocument<DepartmentForUpdationDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var departmentFromRepo = _appRepository.GetDepartment(id);

            if (departmentFromRepo == null)
            {
                return NotFound();
            }

            var departmentToPatch = Mapper.Map<DepartmentForUpdationDto>(departmentFromRepo);

            patchDoc.ApplyTo(departmentToPatch, ModelState);

            // patchDoc.ApplyTo(departmentToPatch);

            TryValidateModel(departmentToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            SetItemHistoryData(departmentToPatch, departmentFromRepo);
            Mapper.Map(departmentToPatch, departmentFromRepo);

            _appRepository.UpdateDepartment(departmentFromRepo);

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

        private IEnumerable<LinkDto> CreateLinksForDepartments(
            DepartmentsResourceParameters departmentsResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateDepartmentsResourceUri(departmentsResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateDepartmentsResourceUri(departmentsResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateDepartmentsResourceUri(departmentsResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        [HttpOptions]
        public IActionResult GetDepartmentsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        private void SetItemHistoryData(DepartmentForUpdationDto model, MstDepartment modelRepo)
        {
            model.CreatedOn = modelRepo.CreatedOn;
            if (modelRepo.CreatedBy != null)
                model.CreatedBy = modelRepo.CreatedBy.Value;
            model.UpdatedOn = DateTime.Now;
            var EmployeeID = User.Claims.FirstOrDefault(cl => cl.Type == "EmployeeID");
            model.UpdatedBy = new Guid(EmployeeID.Value);
        }

        private void SetCreationUserData(MstDepartment model)
        {
            var EmployeeID = User.Claims.FirstOrDefault(cl => cl.Type == "EmployeeID");
            model.CreatedBy = new Guid(EmployeeID.Value);
        }

    }
}