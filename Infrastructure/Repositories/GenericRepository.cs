using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _dataContext;
        private DbSet<T> table = null;

        public GenericRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            table = _dataContext.Set<T>();
        }

        public void Delete(object id)
        {
            T element = GetById(id);
            table.Remove(element);
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList<T>();
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public IEnumerable<T> GetFull()
        {

            switch (typeof(T))
            {
                //case Type type when type == typeof(Like):
                //    DbSet<Like> likeTable = _dataContext.Set<Like>();
                //    return (IEnumerable<T>)likeTable.Include(b => b.block).ToList();

                default:
                    return null;
                    break;
            }
        }
        public void Update(T entity)
        {
            table.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        void IGenericRepository<T>.Add(T entity)
        {
            table.Add(entity);
        }


    }
}
