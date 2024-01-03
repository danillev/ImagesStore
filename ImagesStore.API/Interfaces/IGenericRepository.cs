namespace ImagesStore.API.Interfaces
{
    public interface IGenericRepository<T> : IDisposable
                        where T : class
    {
        ValueTask<IEnumerable<T>> GetAll();
        ValueTask<T> GetById(int id);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task Save();
    }
}
