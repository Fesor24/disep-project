﻿using GadgetHub.Entities;

namespace GadgetHub.DataAccess.Abstractions;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<List<TEntity>> GetAll();

    Task<TEntity> GetById(int id);

    Task<int> AddAsync(TEntity entity);

    void Update(TEntity entity);
}
