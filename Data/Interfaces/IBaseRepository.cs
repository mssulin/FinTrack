namespace Data.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity);
    
    Task<IEnumerable<TEntity>> GetAllAsync();
    
    Task<TEntity?> GetByIdAsync(int id);
    
    Task UpdateAsync(TEntity entity);
    
    Task DeleteAsync(TEntity entity);
}