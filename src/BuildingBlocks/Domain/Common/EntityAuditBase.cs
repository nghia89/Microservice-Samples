using Domain.Common;
using Domain.Common.Interfaces;

namespace Contracts.Domains
{
    public class EntityAuditBase<T> : EntityBase<T>, IDateTracking
    {
        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}