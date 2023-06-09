using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IvNav.Store.Infrastructure.Abstractions.Contexts.Base;

public interface IContextBase
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry Entry(object entity);

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        where TEntity : class;

    Task<T> BeginTransaction<T>(Func<Task<T>> func, CancellationToken cancellationToken);
}
