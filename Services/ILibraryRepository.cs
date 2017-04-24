using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using System;
using System.Collections.Generic;
using ESPL.KP.Entities.Core;
using ESPL.KP.Helpers.Core;

namespace ESPL.KP.Services
{
    public interface ILibraryRepository
    {
        PagedList<Author> GetAuthors(AuthorsResourceParameters authorsResourceParameters);
        Author GetAuthor(Guid authorId);
        IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds);
        void AddAuthor(Author author);
        void DeleteAuthor(Author author);
        void UpdateAuthor(Author author);
        bool AuthorExists(Guid authorId);
        IEnumerable<Book> GetBooksForAuthor(Guid authorId);
        Book GetBookForAuthor(Guid authorId, Guid bookId);
        void AddBookForAuthor(Guid authorId, Book book);
        void UpdateBookForAuthor(Book book);
        void DeleteBook(Book book);
        bool Save();

        #region AppModule

        PagedList<AppModule> GetAppModules(AppModulesResourceParameters appModuleResourceParameters);
        AppModule GetAppModule(Guid appModuleId);
        IEnumerable<AppModule> GetAppModules(IEnumerable<Guid> appModuleIds);
        void AddAppModule(AppModule appModule);
        void DeleteAppModule(AppModule appModule);
        void UpdateAppModule(AppModule appModule);
        bool AppModuleExists(Guid appModuleId);

        #endregion AppModule
    }
}
