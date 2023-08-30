using GadgetHub.Data;
using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Entities;
using Microsoft.EntityFrameworkCore;

namespace GadgetHub.DataAccess.Implementation;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);

        return entity.Id;
    }

    public async Task<List<TEntity>> GetAll()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> GetById(int id)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(TEntity entity)
    {
        _context.Attach(entity);

        _context.Entry(entity).State = EntityState.Modified;
    }
}
