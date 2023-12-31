﻿using Repositories;

namespace Services;

public class GenericService<TEntity> : IGenericService<TEntity>
    where TEntity : class
{
    protected readonly IGenericRepository<TEntity> Repository;

    protected GenericService(IGenericRepository<TEntity> genericRepository) =>
        Repository = genericRepository ?? throw new ArgumentNullException(nameof(genericRepository));

    public IQueryable<TEntity> GetAll() => Repository.GetAll();

    public Task<TEntity?> GetByIdAsync(object id) => Repository.GetByIdAsync(id: id);

    public Task InsertAsync(TEntity entity) => Repository.InsertAsync(entity: entity);

    public Task UpdateAsync(TEntity entity) => Repository.UpdateAsync(entity: entity);

    public Task DeleteAsync(object id) => Repository.DeleteAsync(id: id);

    public Task InsertManyAsync(IEnumerable<TEntity> entities) => Repository.InsertManyAsync(entities: entities);
}