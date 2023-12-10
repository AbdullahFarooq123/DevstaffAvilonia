using GlobalExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    protected readonly DbSet<TEntity> DbSet;

    protected GenericRepository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = _context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll() => DbSet.AsQueryable();

    public async Task<TEntity?> GetByIdAsync(object id) => await DbSet.FindAsync(keyValues: id);

    public async Task InsertAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity: entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Entry(entity: entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(object id)
    {
        TEntity? entityToDelete = await DbSet.FindAsync(keyValues: id);
        if (entityToDelete.HasValue())
        {
            DbSet.Remove(entity: entityToDelete.Value());
            await _context.SaveChangesAsync();
        }
    }

    public async Task InsertManyAsync(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities: entities);
        await _context.SaveChangesAsync();
    }
}