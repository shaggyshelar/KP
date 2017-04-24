using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ESPL.KP.Entities.Core;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Models.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ESPL.KP.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private LibraryContext _context;
        private IPropertyMappingService _propertyMappingService;
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<ESPLUser> _userMgr;

        public LibraryRepository(LibraryContext context,
            IPropertyMappingService propertyMappingService,
            UserManager<ESPLUser> userMgr,
            RoleManager<IdentityRole> roleMgr)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

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

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }


        #region AppModule

        public PagedList<AppModule> GetAppModules(AppModulesResourceParameters appModuleResourceParameters)
        {
            var collectionBeforePaging =
                _context.AppModules.ApplySort(appModuleResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<AppModuleDto, AppModule>());

            if (!string.IsNullOrEmpty(appModuleResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = appModuleResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.MenuText.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<AppModule>.Create(collectionBeforePaging,
                appModuleResourceParameters.PageNumber,
                appModuleResourceParameters.PageSize);
        }

        public AppModule GetAppModule(Guid appModuleId)
        {
            return _context.AppModules.FirstOrDefault(a => a.Id == appModuleId);
        }

        public IEnumerable<AppModule> GetAppModules(IEnumerable<Guid> appModuleIds)
        {
            return _context.AppModules.Where(a => appModuleIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .ToList();
        }

        public void AddAppModule(AppModule appModule)
        {
            appModule.Id = Guid.NewGuid();
            _context.AppModules.Add(appModule);
        }

        public void DeleteAppModule(AppModule appModule)
        {
            _context.AppModules.Remove(appModule);
        }

        public void UpdateAppModule(AppModule appModule)
        {
            // no code in this implementation
        }

        public bool AppModuleExists(Guid appModuleId)
        {
            return _context.AppModules.Any(a => a.Id == appModuleId);
        }

        #endregion AppModule



        #region ESPLUser

        public PagedList<ESPLUser> GetESPLUsers(ESPLUsersResourceParameters esplUserResourceParameters)
        {
            throw new NotImplementedException();
        }

        public ESPLUser GetESPLUser(Guid esplUserId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ESPLUser> GetESPLUsers(IEnumerable<Guid> esplUserIds)
        {
            throw new NotImplementedException();
        }

        public void AddESPLUser(ESPLUser esplUser)
        {
            throw new NotImplementedException();
        }

        public void DeleteESPLUser(ESPLUser esplUser)
        {
            throw new NotImplementedException();
        }

        public void UpdateESPLUser(ESPLUser esplUser)
        {
            // no code in this implementation
        }

        public bool ESPLUserExists(Guid esplUserId)
        {
            throw new NotImplementedException();
        }

        #endregion ESPLUser


        #region ESPLRole

        public PagedList<IdentityRole> GetESPLRoles(ESPLRolesResourceParameters esplRoleResourceParameters)
        {
            var collectionBeforePaging =
               _roleMgr.Roles.ApplySort(esplRoleResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<AppModuleDto, AppModule>());

            if (!string.IsNullOrEmpty(esplRoleResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = esplRoleResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<IdentityRole>.Create(collectionBeforePaging,
                esplRoleResourceParameters.PageNumber,
                esplRoleResourceParameters.PageSize);
        }

        public IdentityRole GetESPLRole(Guid esplRoleId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IdentityRole> GetESPLRoles(IEnumerable<Guid> esplRoleIds)
        {
            throw new NotImplementedException();
        }

        public void AddESPLRole(IdentityRole esplRole)
        {
            throw new NotImplementedException();
        }

        public void DeleteESPLRole(IdentityRole esplRole)
        {
            throw new NotImplementedException();
        }

        public void UpdateESPLRole(IdentityRole esplRole)
        {
            // no code in this implementation
        }

        public bool ESPLRoleExists(Guid esplRoleId)
        {
            throw new NotImplementedException();
        }

        #endregion ESPLRole
    }
}
