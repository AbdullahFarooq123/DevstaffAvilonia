using Repositories;

namespace Services;

public class GenericService<TEntity> : IGenericService<TEntity>
	where TEntity : class
{
	protected readonly IGenericRepository<TEntity> _repository;

	public GenericService(IGenericRepository<TEntity> genericRepository) =>
		_repository = genericRepository ?? throw new ArgumentNullException(nameof(genericRepository));

	public IQueryable<TEntity> GetAll() => _repository.GetAll();

	public async Task<TEntity?> GetByIdAsync(object id) => await _repository.GetByIdAsync(id);

	public async Task InsertAsync(TEntity entity) => await _repository.InsertAsync(entity);

	public async Task UpdateAsync(TEntity entity) => await _repository.UpdateAsync(entity);

	public async Task DeleteAsync(object id) => await _repository.DeleteAsync(id);

	public async Task InsertManyAsync(IEnumerable<TEntity> entities) => await _repository.InsertManyAsync(entities);
}
