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
using KP.Persistence;
using KP.Common;

namespace KP.Service.Department
{
    [Route("api/departments")]
    public class DepartmentsController : Controller
    {
        private IAppRepository _appRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        private IGenericRepository<KP.Domain.Department.Department> _repo;

        public DepartmentsController(IAppRepository appRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            IGenericRepository<KP.Domain.Department.Department> repo)
        {
            _appRepository = appRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
            _repo = repo;
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

            var collectionBeforePaging =
                _repo.Query().Where(a => a.IsDelete == false).ApplySort(departmentsResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<DepartmentDto, KP.Domain.Department.Department>());

            if (!string.IsNullOrEmpty(departmentsResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause = departmentsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.DepartmentName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || (a.DepartmentDespcription != null
                        && a.DepartmentDespcription.ToLowerInvariant().Contains(searchQueryForWhereClause)));
            }

            var departmentsFromRepo = PagedList<KP.Domain.Department.Department>.Create(collectionBeforePaging,
                departmentsResourceParameters.PageNumber,
                departmentsResourceParameters.PageSize);

            //var departmentsFromRepo = _appRepository.GetDepartments(departmentsResourceParameters);

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

                var links = Utilities.CreateLinks(departmentsResourceParameters,
                    departmentsFromRepo.HasNext, departmentsFromRepo.HasPrevious, _urlHelper, "Department");

                var shapedDepartments = departments.ShapeData(departmentsResourceParameters.Fields);

                var shapedDepartmentsWithLinks = shapedDepartments.Select(department =>
                {
                    var departmentAsDictionary = department as IDictionary<string, object>;
                    var departmentLinks = Utilities.CreateLinks(
                        (Guid)departmentAsDictionary["Id"], departmentsResourceParameters.Fields,
                        _urlHelper, "Department");

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
                    Utilities.CreateResourceUri(departmentsResourceParameters,
                    ResourceUriType.PreviousPage, _urlHelper, "GetDepartments") : null;

                var nextPageLink = departmentsFromRepo.HasNext ?
                    Utilities.CreateResourceUri(departmentsResourceParameters,
                    ResourceUriType.NextPage, _urlHelper, "GetDepartments") : null;

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