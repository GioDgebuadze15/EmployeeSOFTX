using Employee.Database.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Employee.Database.DatabaseRepository;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _ctx;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(AppDbContext ctx)
    {
        _ctx = ctx;
        _dbSet = ctx.Set<TEntity>();
    }

    public TEntity GetById(int id)
        => _dbSet.Find(id) ?? throw new InvalidOperationException($"Entity with id {id} was not found");

    public IEnumerable<TEntity> GetAll()
        => _dbSet.ToList();

    public async Task<TEntity> Add(TEntity entity)
    {
        _dbSet.Add(entity);
        await _ctx.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _ctx.Entry(entity).State = EntityState.Modified;
        await _ctx.SaveChangesAsync();
        return entity;
    }

    public async Task Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _ctx.SaveChangesAsync();
    }
}