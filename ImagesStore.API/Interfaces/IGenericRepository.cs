﻿namespace ImagesStore.API.Interfaces
{
    public interface IGenericRepository<T> : IDisposable
                        where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
