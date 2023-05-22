using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public T GetById(object id);
        public IEnumerable<T> GetAll();
        public IEnumerable<T> GetFull<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);
        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(object id);
    }
}
