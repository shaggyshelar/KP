using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Helpers.OccurrenceBook;
using ESPL.KP.Helpers.Reports;
using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace KP.Controllers.ReportsController
{
    [Route("api/reports")]
    [Authorize]
    public class ReportsController : Controller
    {
        private IAppRepository _appRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public ReportsController(IAppRepository appRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _appRepository = appRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        #region Occurance Reports
        [Route("GetOccurrences")]
        [HttpGet(Name = "GetOccurrences")]
        [Authorize(Policy = Permissions.ReportsRead)]
        public IActionResult GetOccurrences(OccurrenceReportResourceParameters occurrenceReportResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<OccurrenceReportDto, MstOccurrenceBook>
               (occurrenceReportResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<OccurrenceReportDto>
                (occurrenceReportResourceParameters.Fields))
            {
                return BadRequest();
            }

            var occurrenceBookFromRepo = _appRepository.GetOccurrenceBooks(occurrenceReportResourceParameters);

            var occurrenceBook = Mapper.Map<IEnumerable<OccurrenceReportDto>>(occurrenceBookFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = occurrenceBookFromRepo.TotalCount,
                    pageSize = occurrenceBookFromRepo.PageSize,
                    currentPage = occurrenceBookFromRepo.CurrentPage,
                    totalPages = occurrenceBookFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForOccurrenceBook(occurrenceReportResourceParameters,
                    occurrenceBookFromRepo.HasNext, occurrenceBookFromRepo.HasPrevious);

                var shapedoccurrenceBook = occurrenceBook.ShapeData(occurrenceReportResourceParameters.Fields);

                var shapedoccurrenceBookWithLinks = shapedoccurrenceBook.Select(occType =>
                {
                    var occurrenceBookAsDictionary = occType as IDictionary<string, object>;
                    var occurrenceBookLinks = CreateLinksForOccurrenceBook(
                        (Guid)occurrenceBookAsDictionary["Id"], occurrenceReportResourceParameters.Fields);

                    occurrenceBookAsDictionary.Add("links", occurrenceBookLinks);

                    return occurrenceBookAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedoccurrenceBookWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = occurrenceBookFromRepo.HasPrevious ?
                    CreateOccurrenceBookResourceUri(occurrenceReportResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = occurrenceBookFromRepo.HasNext ?
                    CreateOccurrenceBookResourceUri(occurrenceReportResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = occurrenceBookFromRepo.TotalCount,
                    pageSize = occurrenceBookFromRepo.PageSize,
                    currentPage = occurrenceBookFromRepo.CurrentPage,
                    totalPages = occurrenceBookFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(occurrenceBook.ShapeData(occurrenceReportResourceParameters.Fields));
                //return Ok(occurrenceBook);
            }
        }

        [Route("GetOccurrenceBooksStatistics")]
        [HttpGet(Name = "GetOccurrenceBooksStatistics")]
        [Authorize(Policy = Permissions.ReportsRead)]
        public IActionResult GetOccurrenceBooksStatistics(OccurrenceStatisticsResourceParameters occurrenceBookResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<Statistics, MstOccurrenceBook>
               (occurrenceBookResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<Statistics>
                (occurrenceBookResourceParameters.Fields))
            {
                return BadRequest();
            }

            var occurrenceBookFromRepo = _appRepository.GetOccurrenceBooksStatistics(occurrenceBookResourceParameters);

            // var occurrenceBook = Mapper.Map<IEnumerable<OccurreceStatistics>>(occurrenceBookFromRepo);

            // if (mediaType == "application/vnd.marvin.hateoas+json")
            // {
            //     var paginationMetadata = new
            //     {
            //         totalCount = occurrenceBookFromRepo.TotalCount,
            //         pageSize = occurrenceBookFromRepo.PageSize,
            //         currentPage = occurrenceBookFromRepo.CurrentPage,
            //         totalPages = occurrenceBookFromRepo.TotalPages,
            //     };

            //     Response.Headers.Add("X-Pagination",
            //         Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            //     var links = CreateLinksForOccurrenceBook(occurrenceBookResourceParameters,
            //         occurrenceBookFromRepo.HasNext, occurrenceBookFromRepo.HasPrevious);

            //     var shapedoccurrenceBook = occurrenceBook.ShapeData(occurrenceBookResourceParameters.Fields);

            //     var shapedoccurrenceBookWithLinks = shapedoccurrenceBook.Select(occType =>
            //     {
            //         var occurrenceBookAsDictionary = occType as IDictionary<string, object>;
            //         var occurrenceBookLinks = CreateLinksForOccurrenceBook(
            //             (Guid)occurrenceBookAsDictionary["Id"], occurrenceBookResourceParameters.Fields);

            //         occurrenceBookAsDictionary.Add("links", occurrenceBookLinks);

            //         return occurrenceBookAsDictionary;
            //     });

            //     var linkedCollectionResource = new
            //     {
            //         value = shapedoccurrenceBookWithLinks,
            //         links = links
            //     };

            //     return Ok(linkedCollectionResource);
            // }
            // else
            // {
            //     var previousPageLink = occurrenceBookFromRepo.HasPrevious ?
            //         CreateOccurrenceBookResourceUri(occurrenceBookResourceParameters,
            //         ResourceUriType.PreviousPage) : null;

            //     var nextPageLink = occurrenceBookFromRepo.HasNext ?
            //         CreateOccurrenceBookResourceUri(occurrenceBookResourceParameters,
            //         ResourceUriType.NextPage) : null;

            //     var paginationMetadata = new
            //     {
            //         previousPageLink = previousPageLink,
            //         nextPageLink = nextPageLink,
            //         totalCount = occurrenceBookFromRepo.TotalCount,
            //         pageSize = occurrenceBookFromRepo.PageSize,
            //         currentPage = occurrenceBookFromRepo.CurrentPage,
            //         totalPages = occurrenceBookFromRepo.TotalPages
            //     };

            //     Response.Headers.Add("X-Pagination",
            //         Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            //     return Ok(occurrenceBook.ShapeData(occurrenceBookResourceParameters.Fields));
            // }

            return Ok(occurrenceBookFromRepo);
        }

        [HttpOptions]
        public IActionResult GetReportsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        //  [Route("GetOfficersStatistics")]
        // [HttpGet(Name = "GetOfficersStatistics")]
        // public IActionResult GetOfficersStatistics(OccurrenceStatisticsResourceParameters occurrenceBookResourceParameters,
        //     [FromHeader(Name = "Accept")] string mediaType)
        // {
        //     if (!_propertyMappingService.ValidMappingExistsFor<OccurreceStatistics, MstOccurrenceBook>
        //        (occurrenceBookResourceParameters.OrderBy))
        //     {
        //         return BadRequest();
        //     }

        //     if (!_typeHelperService.TypeHasProperties<OccurreceStatistics>
        //         (occurrenceBookResourceParameters.Fields))
        //     {
        //         return BadRequest();
        //     }

        //     var occurrenceBookFromRepo = _appRepository.GetOfficersStatistics(occurrenceBookResourceParameters);

        //     return Ok(occurrenceBookFromRepo);
        // }
        private string CreateOccurrenceBookResourceUri(
                  OccurrenceReportResourceParameters occurrenceReportResourceParameters,
                  ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetOccurrenceBook",
                      new
                      {
                          fields = occurrenceReportResourceParameters.Fields,
                          orderBy = occurrenceReportResourceParameters.OrderBy,
                          searchQuery = occurrenceReportResourceParameters.SearchQuery,
                          pageNumber = occurrenceReportResourceParameters.PageNumber - 1,
                          pageSize = occurrenceReportResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetOccurrenceBook",
                      new
                      {
                          fields = occurrenceReportResourceParameters.Fields,
                          orderBy = occurrenceReportResourceParameters.OrderBy,
                          searchQuery = occurrenceReportResourceParameters.SearchQuery,
                          pageNumber = occurrenceReportResourceParameters.PageNumber + 1,
                          pageSize = occurrenceReportResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetOccurrenceBook",
                    new
                    {
                        fields = occurrenceReportResourceParameters.Fields,
                        orderBy = occurrenceReportResourceParameters.OrderBy,
                        searchQuery = occurrenceReportResourceParameters.SearchQuery,
                        pageNumber = occurrenceReportResourceParameters.PageNumber,
                        pageSize = occurrenceReportResourceParameters.PageSize
                    });
            }
        }

        private string CreateOccurrenceBookResourceUri(
                          OccurrenceStatisticsResourceParameters occurrenceReportResourceParameters,
                          ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetOccurrenceBook",
                      new
                      {
                          fields = occurrenceReportResourceParameters.Fields,
                          orderBy = occurrenceReportResourceParameters.OrderBy,
                          searchQuery = occurrenceReportResourceParameters.SearchQuery,
                          pageNumber = occurrenceReportResourceParameters.PageNumber - 1,
                          pageSize = occurrenceReportResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetOccurrenceBook",
                      new
                      {
                          fields = occurrenceReportResourceParameters.Fields,
                          orderBy = occurrenceReportResourceParameters.OrderBy,
                          searchQuery = occurrenceReportResourceParameters.SearchQuery,
                          pageNumber = occurrenceReportResourceParameters.PageNumber + 1,
                          pageSize = occurrenceReportResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetOccurrenceBook",
                    new
                    {
                        fields = occurrenceReportResourceParameters.Fields,
                        orderBy = occurrenceReportResourceParameters.OrderBy,
                        searchQuery = occurrenceReportResourceParameters.SearchQuery,
                        pageNumber = occurrenceReportResourceParameters.PageNumber,
                        pageSize = occurrenceReportResourceParameters.PageSize
                    });
            }
        }
        private IEnumerable<LinkDto> CreateLinksForOccurrenceBook(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetOccurrenceBook", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetOccurrenceBook", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("DeleteOccurrenceBook", new { id = id }),
              "delete_occurrenceBook",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForOccurrenceBook", new { occurrenceBookId = id }),
              "create_book_for_occurrenceBook",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link("GetBooksForOccurrenceBook", new { occurrenceBookId = id }),
               "books",
               "GET"));

            return links;
        }



        private IEnumerable<LinkDto> CreateLinksForOccurrenceBook(
            OccurrenceReportResourceParameters occurrenceReportResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateOccurrenceBookResourceUri(occurrenceReportResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateOccurrenceBookResourceUri(occurrenceReportResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateOccurrenceBookResourceUri(occurrenceReportResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForOccurrenceBook(
           OccurrenceStatisticsResourceParameters occurrenceReportResourceParameters,
           bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateOccurrenceBookResourceUri(occurrenceReportResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateOccurrenceBookResourceUri(occurrenceReportResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateOccurrenceBookResourceUri(occurrenceReportResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }
        #endregion Occurrance Report
    }
}