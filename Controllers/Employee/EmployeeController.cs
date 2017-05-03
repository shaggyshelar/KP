using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Helpers.Employee;
using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace KP.Controllers.Employee
{
    [Route("api/employees")]
    [Authorize]
    public class EmployeeController : Controller
    {
        private ILibraryRepository _libraryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public EmployeeController(ILibraryRepository libraryRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _libraryRepository = libraryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetEmployees")]
        [HttpHead]
        [Authorize(Policy = Permissions.EmployeeRead)]
        public IActionResult GetEmployees(EmployeesResourceParameters employeesResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<EmployeeDto, MstEmployee>
               (employeesResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<EmployeeDto>
                (employeesResourceParameters.Fields))
            {
                return BadRequest();
            }

            var employeeFromRepo = _libraryRepository.GetEmployees(employeesResourceParameters);

            var employee = Mapper.Map<IEnumerable<EmployeeDto>>(employeeFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = employeeFromRepo.TotalCount,
                    pageSize = employeeFromRepo.PageSize,
                    currentPage = employeeFromRepo.CurrentPage,
                    totalPages = employeeFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForEmployee(employeesResourceParameters,
                    employeeFromRepo.HasNext, employeeFromRepo.HasPrevious);

                var shapedemployee = employee.ShapeData(employeesResourceParameters.Fields);

                var shapedemployeeWithLinks = shapedemployee.Select(occType =>
                {
                    var employeeAsDictionary = occType as IDictionary<string, object>;
                    var employeeLinks = CreateLinksForEmployee(
                        (Guid)employeeAsDictionary["Id"], employeesResourceParameters.Fields);

                    employeeAsDictionary.Add("links", employeeLinks);

                    return employeeAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedemployeeWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = employeeFromRepo.HasPrevious ?
                    CreateEmployeeResourceUri(employeesResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = employeeFromRepo.HasNext ?
                    CreateEmployeeResourceUri(employeesResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = employeeFromRepo.TotalCount,
                    pageSize = employeeFromRepo.PageSize,
                    currentPage = employeeFromRepo.CurrentPage,
                    totalPages = employeeFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(employee.ShapeData(employeesResourceParameters.Fields));
            }
        }

        [HttpGet("{id}", Name = "GetEmployee")]
        [Authorize(Policy = Permissions.EmployeeRead)]
        public IActionResult GetEmployee(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<EmployeeDto>
              (fields))
            {
                return BadRequest();
            }

            var employeeFromRepo = _libraryRepository.GetEmployee(id);

            if (employeeFromRepo == null)
            {
                return NotFound();
            }

            var employee = Mapper.Map<EmployeeDto>(employeeFromRepo);

            var links = CreateLinksForEmployee(id, fields);

            var linkedResourceToReturn = employee.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateEmployee")]
        [Authorize(Policy = Permissions.EmployeeCreate)]
        // [RequestHeaderMatchesMediaType("Content-Type",
        //     new[] { "application/vnd.marvin.employee.full+json" })]
        public IActionResult CreateEmployee([FromBody] EmployeeForCreationDto employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }

            var employeeEntity = Mapper.Map<MstEmployee>(employee);

            _libraryRepository.AddEmployee(employeeEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an employee failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var employeeToReturn = Mapper.Map<EmployeeDto>(employeeEntity);

            var links = CreateLinksForEmployee(employeeToReturn.EmployeeID, null);

            var linkedResourceToReturn = employeeToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetEmployee",
                new { id = linkedResourceToReturn["EmployeeID"] },
                linkedResourceToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockEmployeeCreation(Guid id)
        {
            if (_libraryRepository.EmployeeExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteEmployee")]
        [Authorize(Policy = Permissions.EmployeeDelete)]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employeeFromRepo = _libraryRepository.GetEmployee(id);
            if (employeeFromRepo == null)
            {
                return NotFound();
            }

            //_libraryRepository.DeleteEmployee(employeeFromRepo);
            //....... Soft Delete
            employeeFromRepo.IsDelete = true;

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Deleting employee {id} failed on save.");
            }

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateEmployee")]
        [Authorize(Policy = Permissions.EmployeeUpdate)]
        public IActionResult UpdateEmployee(Guid id, [FromBody] EmployeeForUpdationDto employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }
            // if (!_libraryRepository.EmployeeExists(id))
            // {
            //     return NotFound();
            // }
            //Mapper.Map(source,destination);
            var employeeFromRepo = _libraryRepository.GetEmployee(id);

            if (employeeFromRepo == null)
            {
                // var employeeAdd = Mapper.Map<MstEmployee>(employee);
                // employeeAdd.EmployeeID = id;

                // _libraryRepository.AddEmployee(employeeAdd);

                // if (!_libraryRepository.Save())
                // {
                //     throw new Exception($"Upserting book {id} for author {id} failed on save.");
                // }

                // var employeeReturnVal = Mapper.Map<EmployeeDto>(employeeAdd);

                // return CreatedAtRoute("GetEmployee",
                //     new { OBID = employeeReturnVal.EmployeeID },
                //     employeeReturnVal);
                return NotFound();
            }
            SetItemHistoryData(employee, employeeFromRepo);
            Mapper.Map(employee, employeeFromRepo);
            _libraryRepository.UpdateEmployee(employeeFromRepo);
            if (!_libraryRepository.Save())
            {
                throw new Exception("Updating an employee failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }


            return Ok(employeeFromRepo);
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateEmployee")]
        [Authorize(Policy = Permissions.EmployeeUpdate)]
        public IActionResult PartiallyUpdateEmployee(Guid id,
                    [FromBody] JsonPatchDocument<EmployeeForUpdationDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var bookForAuthorFromRepo = _libraryRepository.GetEmployee(id);

            if (bookForAuthorFromRepo == null)
            {
                // var bookDto = new EmployeeForUpdationDto();
                // patchDoc.ApplyTo(bookDto, ModelState);

                // TryValidateModel(bookDto);

                // if (!ModelState.IsValid)
                // {
                //     return new UnprocessableEntityObjectResult(ModelState);
                // }

                // var bookToAdd = Mapper.Map<MstEmployee>(bookDto);
                // bookToAdd.EmployeeID = id;

                // _libraryRepository.AddEmployee(bookToAdd);

                // if (!_libraryRepository.Save())
                // {
                //     throw new Exception($"Upserting in Occurrence Book {id} failed on save.");
                // }

                // var bookToReturn = Mapper.Map<EmployeeDto>(bookToAdd);
                // return CreatedAtRoute("GetEmployee",
                //     new { id = bookToReturn.EmployeeID },
                //     bookToReturn);
                return NotFound();
            }

            var bookToPatch = Mapper.Map<EmployeeForUpdationDto>(bookForAuthorFromRepo);

            patchDoc.ApplyTo(bookToPatch, ModelState);

            // patchDoc.ApplyTo(bookToPatch);

            TryValidateModel(bookToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            SetItemHistoryData(bookToPatch, bookForAuthorFromRepo);
            Mapper.Map(bookToPatch, bookForAuthorFromRepo);

            _libraryRepository.UpdateEmployee(bookForAuthorFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Patching  Occurrence Book {id} failed on save.");
            }

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetEmployeeOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        private string CreateEmployeeResourceUri(
            EmployeesResourceParameters employeesResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetEmployee",
                      new
                      {
                          fields = employeesResourceParameters.Fields,
                          orderBy = employeesResourceParameters.OrderBy,
                          searchQuery = employeesResourceParameters.SearchQuery,
                          pageNumber = employeesResourceParameters.PageNumber - 1,
                          pageSize = employeesResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetEmployee",
                      new
                      {
                          fields = employeesResourceParameters.Fields,
                          orderBy = employeesResourceParameters.OrderBy,
                          searchQuery = employeesResourceParameters.SearchQuery,
                          pageNumber = employeesResourceParameters.PageNumber + 1,
                          pageSize = employeesResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetEmployee",
                    new
                    {
                        fields = employeesResourceParameters.Fields,
                        orderBy = employeesResourceParameters.OrderBy,
                        searchQuery = employeesResourceParameters.SearchQuery,
                        pageNumber = employeesResourceParameters.PageNumber,
                        pageSize = employeesResourceParameters.PageSize
                    });
            }
        }

        private IEnumerable<LinkDto> CreateLinksForEmployee(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetEmployee", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetEmployee", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("DeleteEmployee", new { id = id }),
              "delete_employee",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForEmployee", new { employeeId = id }),
              "create_book_for_employee",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link("GetBooksForEmployee", new { employeeId = id }),
               "books",
               "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForEmployee(
            EmployeesResourceParameters employeesResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateEmployeeResourceUri(employeesResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateEmployeeResourceUri(employeesResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateEmployeeResourceUri(employeesResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        private void SetItemHistoryData(EmployeeForUpdationDto model, MstEmployee modelRepo)
        {
            model.CreatedOn = modelRepo.CreatedOn;
            model.UpdatedOn = DateTime.Now;
        }

    }
}