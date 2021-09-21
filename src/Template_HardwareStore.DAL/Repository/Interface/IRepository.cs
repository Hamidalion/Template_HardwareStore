using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Template_HardwareStore.DAL.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        T FindById(int id);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, // это наше Where()
                              Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, // наша сортировка типа OrderBy()
                              string includeProperties = null, // включение каких либо свойств типа Include()
                              bool isTracking = true); // для отслеживания при редактиировании 

        T FirstOrDefault(Expression<Func<T, bool>> filter = null,
                              string includeProperties = null,
                              bool isTracking = true);

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);

        void Save();
    }
}
