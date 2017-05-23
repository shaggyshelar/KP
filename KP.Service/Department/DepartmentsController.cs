using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using KP.Application.Interfaces;
using KP.Common.Services;
using KP.Application.Departments;
using KP.Common.Helpers;

namespace KP.Service.Department
{
    [Route("api/departments")]
    public class DepartmentsController : Controller
    {
        private IAppRepository _appRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public DepartmentsController(IAppRepository appRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _appRepository = appRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
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


        [HttpGet(Name = "GetDepartments")]
        [HttpHead]
        //[Authorize(Policy = "DP.R")]
        public IActionResult GetDepartments(DepartmentsResourceParameters departmentsResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<DepartmentDto, KP.Domain.Department.Department>
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
    }
}