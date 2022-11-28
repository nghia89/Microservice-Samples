using Domain.Interface;

namespace Domain.Common;

public abstract class EntityBase<Tkey> : IEntityBase<Tkey>
{
    public Tkey Id { get; set; }
}