namespace Domain.Interface;

public interface IEntityBase<T>
{
    T Id { get; set; }
}