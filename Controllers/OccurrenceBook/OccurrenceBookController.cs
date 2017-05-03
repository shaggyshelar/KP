using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Helpers.OccurrenceBook;
using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace KP.Controllers.OccurrenceBook
{
    [Route("api/occurrencebook")]
    [Authorize]
    public class OccurrenceBookController : Controller
    {
        private IAppRepository _appRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public OccurrenceBookController(IAppRepository appRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _appRepository = appRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetOccurrenceBooks")]
        [HttpHead]
        [Authorize(Policy = Permissions.OccurrenceBookRead)]
        public IActionResult GetOccurrenceBooks(OccurrenceBookResourceParameters occurrenceBookResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<OccurrenceBookDto, MstOccurrenceBook>
               (occurrenceBookResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<OccurrenceBookDto>
                (occurrenceBookResourceParameters.Fields))
            {
                return BadRequest();
            }

            var occurrenceBookFromRepo = _appRepository.GetOccurrenceBooks(occurrenceBookResourceParameters);

            var occurrenceBook = Mapper.Map<IEnumerable<OccurrenceBookDto>>(occurrenceBookFromRepo);

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

                var links = CreateLinksForOccurrenceBook(occurrenceBookResourceParameters,
                    occurrenceBookFromRepo.HasNext, occurrenceBookFromRepo.HasPrevious);

                var shapedoccurrenceBook = occurrenceBook.ShapeData(occurrenceBookResourceParameters.Fields);

                var shapedoccurrenceBookWithLinks = shapedoccurrenceBook.Select(occType =>
                {
                    var occurrenceBookAsDictionary = occType as IDictionary<string, object>;
                    var occurrenceBookLinks = CreateLinksForOccurrenceBook(
                        (Guid)occurrenceBookAsDictionary["Id"], occurrenceBookResourceParameters.Fields);

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
                    CreateOccurrenceBookResourceUri(occurrenceBookResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = occurrenceBookFromRepo.HasNext ?
                    CreateOccurrenceBookResourceUri(occurrenceBookResourceParameters,
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

                return Ok(occurrenceBook.ShapeData(occurrenceBookResourceParameters.Fields));
            }
        }

        [HttpGet("{id}", Name = "GetOccurrenceBook")]
        [Authorize(Policy = Permissions.OccurrenceBookRead)]
        public IActionResult GetOccurrenceBook(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<OccurrenceBookDto>
              (fields))
            {
                return BadRequest();
            }

            var occurrenceBookFromRepo = _appRepository.GetOccurrenceBook(id);

            if (occurrenceBookFromRepo == null)
            {
                return NotFound();
            }

            var occurrenceBook = Mapper.Map<OccurrenceBookDto>(occurrenceBookFromRepo);

            var links = CreateLinksForOccurrenceBook(id, fields);

            var linkedResourceToReturn = occurrenceBook.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost(Name = "CreateOccurrenceBook")]
        [Authorize(Policy = Permissions.OccurrenceBookCreate)]
        // [RequestHeaderMatchesMediaType("Content-Type",
        //     new[] { "application/vnd.marvin.occurrenceBook.full+json" })]
        public IActionResult CreateOccurrenceBook([FromBody] OccurrenceBookForCreationDto occurrenceBook)
        {
            if (occurrenceBook == null)
            {
                return BadRequest();
            }

            var occurrenceBookEntity = Mapper.Map<MstOccurrenceBook>(occurrenceBook);

            //occurrenceBookEntity.OBNumber = Convert.ToString(DateTime.Now.Ticks);
            Random randomObject = new Random();
            occurrenceBookEntity.OBNumber = Convert.ToString(randomObject.Next(1, 100000));

            _appRepository.AddOccurrenceBook(occurrenceBookEntity);

            if (!_appRepository.Save())
            {
                throw new Exception("Creating an occurrenceBook failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var occurrenceBookToReturn = Mapper.Map<OccurrenceBookDto>(occurrenceBookEntity);

            var links = CreateLinksForOccurrenceBook(occurrenceBookToReturn.OBTypeID, null);

            var linkedResourceToReturn = occurrenceBookToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetOccurrenceBook",
                new { id = linkedResourceToReturn["OBTypeID"] },
                linkedResourceToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockOccurrenceBookCreation(Guid id)
        {
            if (_appRepository.OccurrenceBookExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteOccurrenceBook")]
        [Authorize(Policy = Permissions.OccurrenceBookDelete)]
        public IActionResult DeleteOccurrenceBook(Guid id)
        {
            var occurrenceBookFromRepo = _appRepository.GetOccurrenceBook(id);
            if (occurrenceBookFromRepo == null)
            {
                return NotFound();
            }

            //_appRepository.DeleteOccurrenceBook(occurrenceBookFromRepo);

            //....... Soft Delete
            occurrenceBookFromRepo.IsDelete = true;

            if (!_appRepository.Save())
            {
                throw new Exception($"Deleting occurrenceBook {id} failed on save.");
            }

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateOccurrenceBook")]
        [Authorize(Policy = Permissions.OccurrenceBookUpdate)]
        public IActionResult UpdateOccurrenceBook(Guid id, [FromBody] OccurrenceBookForUpdationDto occurrenceBook)
        {
            if (occurrenceBook == null)
            {
                return BadRequest();
            }
            // if (!_appRepository.OccurrenceBookExists(id))
            // {
            //     return NotFound();
            // }
            //Mapper.Map(source,destination);
            var occurrenceBookFromRepo = _appRepository.GetOccurrenceBook(id);

            if (occurrenceBookFromRepo == null)
            {
                // var occurrenceBookAdd = Mapper.Map<MstOccurrenceBook>(occurrenceBook);
                // occurrenceBookAdd.OBID = id;

                // _appRepository.AddOccurrenceBook(occurrenceBookAdd);

                // if (!_appRepository.Save())
                // {
                //     throw new Exception($"Upserting book {id} for author {id} failed on save.");
                // }

                // var occurrenceBookReturnVal = Mapper.Map<OccurrenceBookDto>(occurrenceBookAdd);

                // return CreatedAtRoute("GetOccurrenceBook",
                //     new { OBID = occurrenceBookReturnVal.OBID },
                //     occurrenceBookReturnVal);
                return NotFound();
            }

            bool isStatusChange = false, isAssignedToChange = false;
            if (occurrenceBookFromRepo.AssignedTO != occurrenceBook.AssignedTO)
            {
                isAssignedToChange = true;
            }
            else if (occurrenceBookFromRepo.StatusID != occurrenceBook.StatusID)
            {
                isStatusChange = true;
            }

            SetItemHistoryData(occurrenceBook, occurrenceBookFromRepo);
            Mapper.Map(occurrenceBook, occurrenceBookFromRepo);
            _appRepository.UpdateOccurrenceBook(occurrenceBookFromRepo);
            if (!_appRepository.Save())
            {
                throw new Exception("Updating an occurrenceBook failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            if (isAssignedToChange)
            {
                OccurrenceBookForAssignmentDto occurrenceBookAssignment = new OccurrenceBookForAssignmentDto();
                Mapper.Map(occurrenceBookFromRepo, occurrenceBookAssignment);
                AddAssignedToHistory(id, occurrenceBookAssignment);
            }
            else if (isStatusChange)
            {
                OccurrenceBookForStatusHistoryCreationDto occurrenceBookStatusHistory = new OccurrenceBookForStatusHistoryCreationDto();
                Mapper.Map(occurrenceBookFromRepo, occurrenceBookStatusHistory);
                AddStatusHistory(id, occurrenceBookStatusHistory);
            }

            return Ok(occurrenceBookFromRepo);
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateOccurrenceBook")]
        [Authorize(Policy = Permissions.OccurrenceBookUpdate)]
        public IActionResult PartiallyUpdateOccurrenceBook(Guid id,
                    [FromBody] JsonPatchDocument<OccurrenceBookForUpdationDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var bookForAuthorFromRepo = _appRepository.GetOccurrenceBook(id);

            if (bookForAuthorFromRepo == null)
            {
                // var bookDto = new OccurrenceBookForUpdationDto();
                // patchDoc.ApplyTo(bookDto, ModelState);

                // TryValidateModel(bookDto);

                // if (!ModelState.IsValid)
                // {
                //     return new UnprocessableEntityObjectResult(ModelState);
                // }

                // var bookToAdd = Mapper.Map<MstOccurrenceBook>(bookDto);
                // bookToAdd.OBID = id;

                // _appRepository.AddOccurrenceBook(bookToAdd);

                // if (!_appRepository.Save())
                // {
                //     throw new Exception($"Upserting in Occurrence Book {id} failed on save.");
                // }

                // var bookToReturn = Mapper.Map<OccurrenceBookDto>(bookToAdd);
                // return CreatedAtRoute("GetOccurrenceBook",
                //     new { id = bookToReturn.OBID },
                //     bookToReturn);
                return NotFound();
            }

            var bookToPatch = Mapper.Map<OccurrenceBookForUpdationDto>(bookForAuthorFromRepo);

            patchDoc.ApplyTo(bookToPatch, ModelState);

            // patchDoc.ApplyTo(bookToPatch);

            TryValidateModel(bookToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            SetItemHistoryData(bookToPatch, bookForAuthorFromRepo);
            Mapper.Map(bookToPatch, bookForAuthorFromRepo);

            _appRepository.UpdateOccurrenceBook(bookForAuthorFromRepo);

            if (!_appRepository.Save())
            {
                throw new Exception($"Patching  Occurrence Book {id} failed on save.");
            }
            foreach (var path in patchDoc.Operations)
            {
                if (path.path.ToLowerInvariant().Equals("/assignedto"))
                {
                    OccurrenceBookForAssignmentDto occurrenceBook = new OccurrenceBookForAssignmentDto();
                    Mapper.Map(bookForAuthorFromRepo, occurrenceBook);
                    AddAssignedToHistory(id, occurrenceBook);
                }
                else if (path.path.ToLowerInvariant().Equals("/statusid"))
                {
                    OccurrenceBookForStatusHistoryCreationDto occurrenceBookStatusHistory = new OccurrenceBookForStatusHistoryCreationDto();
                    Mapper.Map(bookForAuthorFromRepo, occurrenceBookStatusHistory);
                    AddStatusHistory(id, occurrenceBookStatusHistory);
                }
            }
            return NoContent();
        }

        [Route("{id}/UpdateAssignedOfficer")]
        [HttpPost("{id}")]
        [Authorize(Policy = Permissions.OccurrenceBookUpdate)]
        public IActionResult SetAssignedTo(Guid id, [FromBody] OccurrenceBookForAssignmentDto occurrenceBook)
        {
            if (occurrenceBook == null)
            {
                return BadRequest();
            }
            var occurrenceBookFromRepo = _appRepository.GetOccurrenceBook(id);

            if (occurrenceBookFromRepo == null)
            {
                return NotFound();
            }

            Mapper.Map(occurrenceBook, occurrenceBookFromRepo);
            _appRepository.UpdateOccurrenceBook(occurrenceBookFromRepo);
            if (!_appRepository.Save())
            {
                return StatusCode(500, "A problem happened with handling your request.");
            }
            AddAssignedToHistory(id, occurrenceBook);
            return Ok(occurrenceBookFromRepo);
        }

        [Route("{id}/assignedhistory")]
        [HttpGet("{id}/assignedhistory", Name = "GetAssignmentHistory")]
        [Authorize(Policy = Permissions.OccurrenceBookUpdate)]
        public IActionResult GetAssignmentHistory(Guid id, OccurrenceBookAssignedToResourceParameters occurrenceBookAssignedToResourceParameters,
        [FromHeader(Name = "Accept")]string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<OccurrenceBookForAssignmentDto, OccurrenceAssignmentHistory>
               (occurrenceBookAssignedToResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<OccurrenceBookForAssignmentDto>
                (occurrenceBookAssignedToResourceParameters.Fields))
            {
                return BadRequest();
            }

            var occurrenceBookAssignmentFromRepo = _appRepository.GetAssignmentHistory(occurrenceBookAssignedToResourceParameters);

            var occurrenceBookAssignedTo = Mapper.Map<IEnumerable<OccurrenceBookForAssignmentDto>>(occurrenceBookAssignmentFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = occurrenceBookAssignmentFromRepo.TotalCount,
                    pageSize = occurrenceBookAssignmentFromRepo.PageSize,
                    currentPage = occurrenceBookAssignmentFromRepo.CurrentPage,
                    totalPages = occurrenceBookAssignmentFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForOccurrenceBookAssignedHistory(occurrenceBookAssignedToResourceParameters,
                    occurrenceBookAssignmentFromRepo.HasNext, occurrenceBookAssignmentFromRepo.HasPrevious);

                var shapedOccurrenceBookAssignedTo = occurrenceBookAssignedTo.ShapeData(occurrenceBookAssignedToResourceParameters.Fields);

                var shapedOccurrenceBookAssignedToWithLinks = shapedOccurrenceBookAssignedTo.Select(occurrenceBookAssignedToHistory =>
                {
                    var occurrenceBookAssignedToAsDictionary = occurrenceBookAssignedToHistory as IDictionary<string, object>;
                    var occurrenceBookAssignedToLinks = CreateLinksForOccurrenceBookReview(
                        (Guid)occurrenceBookAssignedToAsDictionary["Id"], occurrenceBookAssignedToResourceParameters.Fields);

                    occurrenceBookAssignedToAsDictionary.Add("links", occurrenceBookAssignedToLinks);

                    return occurrenceBookAssignedToAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedOccurrenceBookAssignedToWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = occurrenceBookAssignmentFromRepo.HasPrevious ?
                    CreateOccurrenceBookAssignedHistoryResourceUri(occurrenceBookAssignedToResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = occurrenceBookAssignmentFromRepo.HasNext ?
                    CreateOccurrenceBookAssignedHistoryResourceUri(occurrenceBookAssignedToResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = occurrenceBookAssignmentFromRepo.TotalCount,
                    pageSize = occurrenceBookAssignmentFromRepo.PageSize,
                    currentPage = occurrenceBookAssignmentFromRepo.CurrentPage,
                    totalPages = occurrenceBookAssignmentFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(occurrenceBookAssignedTo.ShapeData(occurrenceBookAssignedToResourceParameters.Fields));
            }
        }

        [Route("{id}/Reviews")]
        [HttpGet("{id}/Reviews", Name = "GetReviews")]
        [Authorize(Policy = Permissions.OccurrenceReviewRead)]
        public IActionResult GetReviews(Guid id, OccurrenceBookReviewResourceParameters occurrenceBookReviewResourceParameters,
        [FromHeader(Name = "Accept")]string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<OccurrenceBookReviewDto, OccurrenceReviewHistory>
               (occurrenceBookReviewResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<OccurrenceBookReviewDto>
                (occurrenceBookReviewResourceParameters.Fields))
            {
                return BadRequest();
            }

            var occurrenceBookReviewsFromRepo = _appRepository.GetOccurrenceReviewHistories(occurrenceBookReviewResourceParameters);

            var occurrenceBookReviews = Mapper.Map<IEnumerable<OccurrenceBookReviewDto>>(occurrenceBookReviewsFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = occurrenceBookReviewsFromRepo.TotalCount,
                    pageSize = occurrenceBookReviewsFromRepo.PageSize,
                    currentPage = occurrenceBookReviewsFromRepo.CurrentPage,
                    totalPages = occurrenceBookReviewsFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForOccurrenceBookReviews(occurrenceBookReviewResourceParameters,
                    occurrenceBookReviewsFromRepo.HasNext, occurrenceBookReviewsFromRepo.HasPrevious);

                var shapedOccurrenceBookReview = occurrenceBookReviews.ShapeData(occurrenceBookReviewResourceParameters.Fields);

                var shapedOccurrenceBookReviewWithLinks = shapedOccurrenceBookReview.Select(occurrenceBookReview =>
                {
                    var occurrenceBookReviewAsDictionary = occurrenceBookReview as IDictionary<string, object>;
                    var occurrenceBookReviewLinks = CreateLinksForOccurrenceBookReview(
                        (Guid)occurrenceBookReviewAsDictionary["Id"], occurrenceBookReviewResourceParameters.Fields);

                    occurrenceBookReviewAsDictionary.Add("links", occurrenceBookReviewLinks);

                    return occurrenceBookReviewAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedOccurrenceBookReviewWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = occurrenceBookReviewsFromRepo.HasPrevious ?
                    CreateOccurrenceBookReviewsResourceUri(occurrenceBookReviewResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = occurrenceBookReviewsFromRepo.HasNext ?
                    CreateOccurrenceBookReviewsResourceUri(occurrenceBookReviewResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = occurrenceBookReviewsFromRepo.TotalCount,
                    pageSize = occurrenceBookReviewsFromRepo.PageSize,
                    currentPage = occurrenceBookReviewsFromRepo.CurrentPage,
                    totalPages = occurrenceBookReviewsFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(occurrenceBookReviews.ShapeData(occurrenceBookReviewResourceParameters.Fields));
            }
        }

        [Route("{id}/review/{reviewId}")]
        [HttpGet("{id}/review/{reviewId}", Name = "GetOccurrenceBookReview")]
        [Authorize(Policy = Permissions.OccurrenceReviewRead)]
        public IActionResult GetOccurrenceBookReview(Guid id, Guid reviewId, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<OccurrenceBookReviewDto>
              (fields))
            {
                return BadRequest();
            }

            var occurrenceBookFromRepo = _appRepository.GetReviewById(id, reviewId);

            if (occurrenceBookFromRepo == null)
            {
                return NotFound();
            }

            var occurrenceBook = Mapper.Map<OccurrenceBookReviewDto>(occurrenceBookFromRepo);

            var links = CreateLinksForOccurrenceBookReview(id, fields);

            var linkedResourceToReturn = occurrenceBook.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [Route("{id}/addreview")]
        [HttpPost("{id}")]
        [Authorize(Policy = Permissions.OccurrenceReviewCreate)]
        public IActionResult SetOccurrenceBookReview(Guid id, [FromBody] OccurrenceBookReviewsForCreationDto occurrenceBookReview)
        {
            if (occurrenceBookReview == null)
            {
                return BadRequest();
            }
            var occurrenceBookFromRepo = _appRepository.GetOccurrenceBook(id);

            if (occurrenceBookFromRepo == null)
            {
                return NotFound();
            }

            var occurrenceBookHistoryEntity = Mapper.Map<OccurrenceReviewHistory>(occurrenceBookReview);
            occurrenceBookHistoryEntity.OBID = id;

            _appRepository.AddOccurrenceReviewHistories(occurrenceBookHistoryEntity);

            if (!_appRepository.Save())
            {
                throw new Exception("Creating an occurrenceBook Review failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }
            return Ok(occurrenceBookFromRepo);
        }


        [Route("{id}/UpdateStatus")]
        [HttpPost("{id}")]
        [Authorize(Policy = Permissions.OccurrenceBookUpdate)]
        public IActionResult SetStatus(Guid id, [FromBody]OccurrenceBookForStatusHistoryCreationDto occurrenceBookStatusHistory)
        {
            if (occurrenceBookStatusHistory == null)
            {
                return BadRequest();
            }
            var occurrenceBookFromRepo = _appRepository.GetOccurrenceBook(id);

            if (occurrenceBookFromRepo == null)
            {
                return NotFound();
            }

            //Mapper.Map(occurrenceBookStatusHistory, occurrenceBookFromRepo);
            occurrenceBookFromRepo.StatusID = occurrenceBookStatusHistory.StatusID;
            occurrenceBookFromRepo.Remark = occurrenceBookStatusHistory.Comments;
            _appRepository.UpdateOccurrenceBook(occurrenceBookFromRepo);
            if (!_appRepository.Save())
            {
                return StatusCode(500, "A problem happened with handling your request.");
            }
            AddStatusHistory(id, occurrenceBookStatusHistory);

            return Ok(occurrenceBookFromRepo);
        }

        [Route("{id}/statushistory")]
        [HttpGet("{id}/statushistory", Name = "GetStatusHistory")]
        [Authorize(Policy = Permissions.OccurrenceBookRead)]
        public IActionResult GetStatusHistory(Guid id, OccurrenceBookStatusResourceParameters occurrenceBookStatusResourceParameters,
        [FromHeader(Name = "Accept")]string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<OccurrenceBookStatusHistoryDto, OccurrenceStatusHistory>
                (occurrenceBookStatusResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<OccurrenceBookStatusHistoryDto>
                (occurrenceBookStatusResourceParameters.Fields))
            {
                return BadRequest();
            }

            var occurrenceBookStatusFromRepo = _appRepository.GetStatusHistory(occurrenceBookStatusResourceParameters);

            var occurrenceBookStatus = Mapper.Map<IEnumerable<OccurrenceBookStatusHistoryDto>>(occurrenceBookStatusFromRepo);

            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = occurrenceBookStatusFromRepo.TotalCount,
                    pageSize = occurrenceBookStatusFromRepo.PageSize,
                    currentPage = occurrenceBookStatusFromRepo.CurrentPage,
                    totalPages = occurrenceBookStatusFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForOccurrenceBookStatusHistory(occurrenceBookStatusResourceParameters,
                        occurrenceBookStatusFromRepo.HasNext, occurrenceBookStatusFromRepo.HasPrevious);

                var shapedOccurrenceBookStatus = occurrenceBookStatus.ShapeData(occurrenceBookStatusResourceParameters.Fields);

                var shapedOccurrenceBookStatusWithLinks = shapedOccurrenceBookStatus.Select(_occurrenceBookStatus =>
                {
                    var occurrenceBookStatusAsDictionary = _occurrenceBookStatus as IDictionary<string, object>;
                    var occurrenceBookStatusLinks = CreateLinksForOccurrenceBookStatushistory((Guid)occurrenceBookStatusAsDictionary["Id"], occurrenceBookStatusResourceParameters.Fields);

                    occurrenceBookStatusAsDictionary.Add("links", occurrenceBookStatusLinks);

                    return occurrenceBookStatusAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedOccurrenceBookStatusWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = occurrenceBookStatusFromRepo.HasPrevious ?
                    CreateOccurrenceBookStatusHistoryResourceUri(occurrenceBookStatusResourceParameters,
                        ResourceUriType.PreviousPage) : null;

                var nextPageLink = occurrenceBookStatusFromRepo.HasNext ?
                    CreateOccurrenceBookStatusHistoryResourceUri(occurrenceBookStatusResourceParameters,
                        ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = occurrenceBookStatusFromRepo.TotalCount,
                    pageSize = occurrenceBookStatusFromRepo.PageSize,
                    currentPage = occurrenceBookStatusFromRepo.CurrentPage,
                    totalPages = occurrenceBookStatusFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(occurrenceBookStatus.ShapeData(occurrenceBookStatusResourceParameters.Fields));
            }
        }

        private void AddAssignedToHistory(Guid id, OccurrenceBookForAssignmentDto occurrenceBook)
        {
            var occurrenceBookHistoryEntity = Mapper.Map<OccurrenceAssignmentHistory>(occurrenceBook);
            occurrenceBookHistoryEntity.OBID = id;

            _appRepository.AddOccurrenceAssignmentHistory(occurrenceBookHistoryEntity);

            if (!_appRepository.Save())
            {
                throw new Exception("Creating an occurrenceBook failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }
        }

        private void AddStatusHistory(Guid id, OccurrenceBookForStatusHistoryCreationDto occurrenceBookStatusHistory)
        {

            var occurrenceBookHistoryEntity = Mapper.Map<OccurrenceStatusHistory>(occurrenceBookStatusHistory);
            occurrenceBookHistoryEntity.OBID = id;

            _appRepository.AddOccurrenceStatusHistory(occurrenceBookHistoryEntity);

            if (!_appRepository.Save())
            {
                throw new Exception("Creating an occurrenceBookStatusHistory failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }
        }

        private IEnumerable<LinkDto> CreateLinksForOccurrenceBookStatushistory(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetStatuses", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetStatuses", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("UpdateStatus", new { OccurrenceStatusHistoryID = id }),
              "update_status_for_OccurrenceBook",
              "POST"));

            return links;
        }
        private IEnumerable<LinkDto> CreateLinksForOccurrenceBookStatusHistory(
            OccurrenceBookStatusResourceParameters occurrenceBookStatusResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self
            links.Add(
                new LinkDto(CreateOccurrenceBookStatusHistoryResourceUri(occurrenceBookStatusResourceParameters,
                        ResourceUriType.Current), "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateOccurrenceBookStatusHistoryResourceUri(occurrenceBookStatusResourceParameters,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateOccurrenceBookStatusHistoryResourceUri(occurrenceBookStatusResourceParameters,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }

        private string CreateOccurrenceBookStatusHistoryResourceUri(
            OccurrenceBookStatusResourceParameters occurrenceBookStatusResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetOccurrenceBookStatuses",
                        new
                        {
                            fields = occurrenceBookStatusResourceParameters.Fields,
                            orderBy = occurrenceBookStatusResourceParameters.OrderBy,
                            searchQuery = occurrenceBookStatusResourceParameters.SearchQuery,
                            pageNumber = occurrenceBookStatusResourceParameters.PageNumber - 1,
                            pageSize = occurrenceBookStatusResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetOccurrenceBookStatuses",
                        new
                        {
                            fields = occurrenceBookStatusResourceParameters.Fields,
                            orderBy = occurrenceBookStatusResourceParameters.OrderBy,
                            searchQuery = occurrenceBookStatusResourceParameters.SearchQuery,
                            pageNumber = occurrenceBookStatusResourceParameters.PageNumber + 1,
                            pageSize = occurrenceBookStatusResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetOccurrenceBookStatuses",
                        new
                        {
                            fields = occurrenceBookStatusResourceParameters.Fields,
                            orderBy = occurrenceBookStatusResourceParameters.OrderBy,
                            searchQuery = occurrenceBookStatusResourceParameters.SearchQuery,
                            pageNumber = occurrenceBookStatusResourceParameters.PageNumber,
                            pageSize = occurrenceBookStatusResourceParameters.PageSize
                        });
            }
        }


        [HttpOptions]
        public IActionResult GetOccurrenceBookOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        private string CreateOccurrenceBookResourceUri(
            OccurrenceBookResourceParameters occurrenceBookResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetOccurrenceBook",
                      new
                      {
                          fields = occurrenceBookResourceParameters.Fields,
                          orderBy = occurrenceBookResourceParameters.OrderBy,
                          searchQuery = occurrenceBookResourceParameters.SearchQuery,
                          pageNumber = occurrenceBookResourceParameters.PageNumber - 1,
                          pageSize = occurrenceBookResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetOccurrenceBook",
                      new
                      {
                          fields = occurrenceBookResourceParameters.Fields,
                          orderBy = occurrenceBookResourceParameters.OrderBy,
                          searchQuery = occurrenceBookResourceParameters.SearchQuery,
                          pageNumber = occurrenceBookResourceParameters.PageNumber + 1,
                          pageSize = occurrenceBookResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetOccurrenceBook",
                    new
                    {
                        fields = occurrenceBookResourceParameters.Fields,
                        orderBy = occurrenceBookResourceParameters.OrderBy,
                        searchQuery = occurrenceBookResourceParameters.SearchQuery,
                        pageNumber = occurrenceBookResourceParameters.PageNumber,
                        pageSize = occurrenceBookResourceParameters.PageSize
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
            OccurrenceBookResourceParameters occurrenceBookResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateOccurrenceBookResourceUri(occurrenceBookResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateOccurrenceBookResourceUri(occurrenceBookResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateOccurrenceBookResourceUri(occurrenceBookResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }
        private void SetItemHistoryData(OccurrenceBookForUpdationDto model, MstOccurrenceBook modelRepo)
        {
            model.CreatedOn = modelRepo.CreatedOn;
            model.UpdatedOn = DateTime.Now;
        }

        private string CreateOccurrenceBookReviewsResourceUri(
                    OccurrenceBookReviewResourceParameters occurrenceBookReviewsResourceParameters,
                    ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetOccurrenceBookReviews",
                      new
                      {
                          fields = occurrenceBookReviewsResourceParameters.Fields,
                          orderBy = occurrenceBookReviewsResourceParameters.OrderBy,
                          searchQuery = occurrenceBookReviewsResourceParameters.SearchQuery,
                          pageNumber = occurrenceBookReviewsResourceParameters.PageNumber - 1,
                          pageSize = occurrenceBookReviewsResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetOccurrenceBookReviews",
                      new
                      {
                          fields = occurrenceBookReviewsResourceParameters.Fields,
                          orderBy = occurrenceBookReviewsResourceParameters.OrderBy,
                          searchQuery = occurrenceBookReviewsResourceParameters.SearchQuery,
                          pageNumber = occurrenceBookReviewsResourceParameters.PageNumber + 1,
                          pageSize = occurrenceBookReviewsResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetOccurrenceBookReviews",
                    new
                    {
                        fields = occurrenceBookReviewsResourceParameters.Fields,
                        orderBy = occurrenceBookReviewsResourceParameters.OrderBy,
                        searchQuery = occurrenceBookReviewsResourceParameters.SearchQuery,
                        pageNumber = occurrenceBookReviewsResourceParameters.PageNumber,
                        pageSize = occurrenceBookReviewsResourceParameters.PageSize
                    });
            }
        }

        private IEnumerable<LinkDto> CreateLinksForOccurrenceBookReview(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetOccurrenceBookReview", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetOccurrenceBookReview", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForOccurrenceBookReview", new { departmentId = id }),
              "create_review_for_OccurrenceBook",
              "POST"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForOccurrenceBookReviews(
            OccurrenceBookReviewResourceParameters occurrenceBookReviewsResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateOccurrenceBookReviewsResourceUri(occurrenceBookReviewsResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateOccurrenceBookReviewsResourceUri(occurrenceBookReviewsResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateOccurrenceBookReviewsResourceUri(occurrenceBookReviewsResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForOccurrenceBookAssignedHistory(
            OccurrenceBookAssignedToResourceParameters occurrenceBookAssignedToResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateOccurrenceBookAssignedHistoryResourceUri(occurrenceBookAssignedToResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateOccurrenceBookAssignedHistoryResourceUri(occurrenceBookAssignedToResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateOccurrenceBookAssignedHistoryResourceUri(occurrenceBookAssignedToResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        private string CreateOccurrenceBookAssignedHistoryResourceUri(
                   OccurrenceBookAssignedToResourceParameters occurrenceBookAssignedToResourceParameters,
                   ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetOccurrenceBookReviews",
                      new
                      {
                          fields = occurrenceBookAssignedToResourceParameters.Fields,
                          orderBy = occurrenceBookAssignedToResourceParameters.OrderBy,
                          searchQuery = occurrenceBookAssignedToResourceParameters.SearchQuery,
                          pageNumber = occurrenceBookAssignedToResourceParameters.PageNumber - 1,
                          pageSize = occurrenceBookAssignedToResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetOccurrenceBookReviews",
                      new
                      {
                          fields = occurrenceBookAssignedToResourceParameters.Fields,
                          orderBy = occurrenceBookAssignedToResourceParameters.OrderBy,
                          searchQuery = occurrenceBookAssignedToResourceParameters.SearchQuery,
                          pageNumber = occurrenceBookAssignedToResourceParameters.PageNumber + 1,
                          pageSize = occurrenceBookAssignedToResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetOccurrenceBookReviews",
                    new
                    {
                        fields = occurrenceBookAssignedToResourceParameters.Fields,
                        orderBy = occurrenceBookAssignedToResourceParameters.OrderBy,
                        searchQuery = occurrenceBookAssignedToResourceParameters.SearchQuery,
                        pageNumber = occurrenceBookAssignedToResourceParameters.PageNumber,
                        pageSize = occurrenceBookAssignedToResourceParameters.PageSize
                    });
            }
        }

    }
}
