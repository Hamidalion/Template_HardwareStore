using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Template_HardwareStore.DAL.Context;
using Template_HardwareStore.DAL.Repository.Interface;

namespace Template_HardwareStore.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T FindById(int id)
        {
            return dbSet.Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, 
                                string includeProperties = null, 
                                bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) // дабовляем свойства
                {
                    query = query.Include(property);
                }
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, 
                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
                                     string includeProperties = null, 
                                     bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null) // проверяем установлен ли фильтр
            {
                query = query.Where(filter); // дабовляем фильтр
            }
            if (includeProperties != null) // проверяем установлены ли свойства
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) // дабовляем свойства
                {
                    query = query.Include(property); 
                }
            }
            if (orderBy != null) // проверяем есть ли сортировка
            {
                query = orderBy(query); // проверяем нужную сортировку
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
