using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ESPL.KP.Helpers.OccurrenceType;
using ESPL.KP.Models.OccurrenceType;

namespace ESPL.KP.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private LibraryContext _context;
        private IPropertyMappingService _propertyMappingService;

        public LibraryRepository(LibraryContext context,
            IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        #region Author and books
        public void AddAuthor(Author author)
        {
            author.Id = Guid.NewGuid();
            _context.Authors.Add(author);

            // the repository fills the id (instead of using identity columns)
            if (author.Books.Any())
            {
                foreach (var book in author.Books)
                {
                    book.Id = Guid.NewGuid();
                }
            }
        }

        public void AddBookForAuthor(Guid authorId, Book book)
        {
            var author = GetAuthor(authorId);
            if (author != null)
            {
                // if there isn't an id filled out (ie: we're not upserting),
                // we should generate one
                if (book.Id == null)
                {
                    book.Id = Guid.NewGuid();
                }
                author.Books.Add(book);
            }
        }

        public bool AuthorExists(Guid authorId)
        {
            return _context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            _context.Authors.Remove(author);
        }

        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
        }

        public Author GetAuthor(Guid authorId)
        {
            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public PagedList<Author> GetAuthors(
            AuthorsResourceParameters authorsResourceParameters)
        {
            //var collectionBeforePaging = _context.Authors
            //    .OrderBy(a => a.FirstName)
            //    .ThenBy(a => a.LastName).AsQueryable();

            var collectionBeforePaging =
                _context.Authors.ApplySort(authorsResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            if (!string.IsNullOrEmpty(authorsResourceParameters.Genre))
            {
                // trim & ignore casing
                var genreForWhereClause = authorsResourceParameters.Genre
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant() == genreForWhereClause);
            }

            if (!string.IsNullOrEmpty(authorsResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = authorsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<Author>.Create(collectionBeforePaging,
                authorsResourceParameters.PageNumber,
                authorsResourceParameters.PageSize);
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            return _context.Authors.Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateAuthor(Author author)
        {
            // no code in this implementation
        }

        public Book GetBookForAuthor(Guid authorId, Guid bookId)
        {
            return _context.Books
              .Where(b => b.AuthorId == authorId && b.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> GetBooksForAuthor(Guid authorId)
        {
            return _context.Books
                        .Where(b => b.AuthorId == authorId).OrderBy(b => b.Title).ToList();
        }

        public void UpdateBookForAuthor(Book book)
        {
            // no code in this implementation
        }
        #endregion


        #region OccurrenceType

        public PagedList<MstOccurrenceType> GetOccurrenceTypes(OccurrenceTypeResourceParameters occurrenceTypeResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstOccurrenceType.ApplySort(occurrenceTypeResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<OccurrenceTypeDto, MstOccurrenceType>());

            if (!string.IsNullOrEmpty(occurrenceTypeResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = occurrenceTypeResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.OBTypeName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<MstOccurrenceType>.Create(collectionBeforePaging,
                occurrenceTypeResourceParameters.PageNumber,
                occurrenceTypeResourceParameters.PageSize);
        }

        public MstOccurrenceType GetOccurrenceType(Guid occurrenceTypeId)
        {
            return _context.MstOccurrenceType.FirstOrDefault(a => a.OBTypeID == occurrenceTypeId);
        }

        public IEnumerable<MstOccurrenceType> GetOccurrenceType(IEnumerable<Guid> occurrenceTypeIds)
        {
            return _context.MstOccurrenceType.Where(a => occurrenceTypeIds.Contains(a.OBTypeID))
                .OrderBy(a => a.OBTypeName)
                .ToList();
        }

        public void AddOccurrenceType(MstOccurrenceType occurrenceType)
        {
            occurrenceType.OBTypeID = Guid.NewGuid();
            _context.MstOccurrenceType.Add(occurrenceType);
        }

        public void DeleteOccurrenceType(MstOccurrenceType occurrenceType)
        {
            _context.MstOccurrenceType.Remove(occurrenceType);
        }

        public void UpdateOccurrenceType(MstOccurrenceType occurrenceType)
        {
            // no code in this implementation
        }

        public bool OccurrenceTypeExists(Guid occurrenceTypeId)
        {
            return _context.MstOccurrenceType.Any(a => a.OBTypeID == occurrenceTypeId);
        }

        #endregion

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
