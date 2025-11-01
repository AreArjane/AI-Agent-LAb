using System.Linq.Expressions;

/// <summary>
/// Generic repository interface for CRUD operations
/// </summary>
public interface IGenericRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
    Task Update(TEntity entity);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
