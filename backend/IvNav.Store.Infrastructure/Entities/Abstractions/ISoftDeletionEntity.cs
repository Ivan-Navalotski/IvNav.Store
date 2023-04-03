namespace IvNav.Store.Infrastructure.Entities.Abstractions;

public interface ISoftDeletionEntity
{
    public bool IsDeleted { get; }
}
