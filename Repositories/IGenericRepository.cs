namespace Repositories;

public interface IGenericRepository<TEntity>
	where TEntity : class
{
	IQueryable<TEntity> GetAll();
	Task<TEntity?> GetByIdAsync(object id);
	Task InsertAsync(TEntity entity);
	Task InsertManyAsync(IEnumerable<TEntity> entity);
	Task UpdateAsync(TEntity entity);
	Task DeleteAsync(object id);
}
