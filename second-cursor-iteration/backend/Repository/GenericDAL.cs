using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QuizBackend;
/// <summary>
/// This connects all the controllers to the databasecontext
/// </summary>
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    private readonly ILogger<GenericRepository<TEntity>> _logger; 
    public GenericRepository(ApplicationDbContext context, ILogger<GenericRepository<TEntity>> logger)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _logger = logger;
    }

    public IQueryable<TEntity> GetAll() => _dbSet.AsNoTracking();
    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        => _dbSet.AsNoTracking().Where(predicate);

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _dbSet.FindAsync(new object?[] { id }, ct);

    public Task AddAsync(TEntity entity, CancellationToken ct = default)
        => _dbSet.AddAsync(entity, ct).AsTask();

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        => _dbSet.AddRangeAsync(entities, ct);

    public async Task Update(TEntity entity) => _dbSet.Update(entity);
    public void Remove(TEntity entity) => _dbSet.Remove(entity);
    public void RemoveRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);
    
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}