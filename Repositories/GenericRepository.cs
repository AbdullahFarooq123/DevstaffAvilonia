using GlobalExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
	where TEntity : class
{
	protected readonly DbContext _context;
	protected readonly DbSet<TEntity> _dbSet;

	public GenericRepository(DbContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_dbSet = _context.Set<TEntity>();
	}

	public IQueryable<TEntity> GetAll() => _dbSet.AsQueryable();

	public async Task<TEntity?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

	public async Task InsertAsync(TEntity entity)
	{
		await _dbSet.AddAsync(entity);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(TEntity entity)
	{
		_context.Entry(entity).State = EntityState.Modified;
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(object id)
	{
		TEntity? entityToDelete = await _dbSet.FindAsync(id);
		if (entityToDelete.HasValue())
		{
			_dbSet.Remove(entityToDelete.Value());
			await _context.SaveChangesAsync();
		}
	}

	public async Task InsertManyAsync(IEnumerable<TEntity> entity)
	{
		await _dbSet.AddRangeAsync(entity);
		await _context.SaveChangesAsync();
	}
}
