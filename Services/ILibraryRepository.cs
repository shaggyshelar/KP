using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using System;
using System.Collections.Generic;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Helpers.Department;
using ESPL.KP.Helpers.OccurrenceType;

namespace ESPL.KP.Services
{
    public interface ILibraryRepository
    {
        #region Authors
        PagedList<Author> GetAuthors(AuthorsResourceParameters authorsResourceParameters);
        Author GetAuthor(Guid authorId);
        IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds);
        void AddAuthor(Author author);
        void DeleteAuthor(Author author);
        void UpdateAuthor(Author author);
        bool AuthorExists(Guid authorId);

        #endregion

        #region Books
        IEnumerable<Book> GetBooksForAuthor(Guid authorId);
        Book GetBookForAuthor(Guid authorId, Guid bookId);
        void AddBookForAuthor(Guid authorId, Book book);
        void UpdateBookForAuthor(Book book);
        void DeleteBook(Book book);

        #endregion


        #region Department
        PagedList<MstDepartment> GetDepartments(DepartmentsResourceParameters departmentResourceParameters);
        MstDepartment GetDepartment(Guid departmentId);
        IEnumerable<MstDepartment> GetDepartments(IEnumerable<Guid> departmentIds);
        void AddDepartment(MstDepartment department);
        void DeleteDepartment(MstDepartment department);
        void UpdateDepartment(MstDepartment department);
        bool DepartmentExists(Guid authorId);

        #endregion

        #region OccurrenceType
        PagedList<MstOccurrenceType> GetOccurrenceTypes(OccurrenceTypeResourceParameters occurrenceTypeResourceParameters);
        MstOccurrenceType GetOccurrenceType(Guid occurrenceTypeId);
        IEnumerable<MstOccurrenceType> GetOccurrenceType(IEnumerable<Guid> occurrenceTypeIds);
        void AddOccurrenceType(MstOccurrenceType occurrenceType);
        void DeleteOccurrenceType(MstOccurrenceType occurrenceType);
        void UpdateOccurrenceType(MstOccurrenceType occurrenceType);
        bool OccurrenceTypeExists(Guid occurrenceTypeId);
        #endregion
        bool Save();
    }
}
