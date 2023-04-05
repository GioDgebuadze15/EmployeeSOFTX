namespace Employee.Database.DatabaseRepository;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity GetById(int id);
    
    IEnumerable<TEntity> GetAll();

    Task<TEntity> Add(TEntity entity);
    
    Task<TEntity> Update(TEntity entity);
    
    Task Remove(TEntity entity);
}