using System;

namespace KP.Domain.Common
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreatedOn { get; set; }
        Guid? CreatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
        Guid? UpdatedBy { get; set; }
        bool IsDelete { get; set; }
    }
}