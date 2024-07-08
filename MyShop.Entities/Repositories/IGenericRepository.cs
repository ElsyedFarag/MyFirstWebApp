using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositories
{
    public interface IGenericRepository<T> where T : class
    {

        T Find(Expression<Func<T, bool>> predicate, string? include);
        IEnumerable<T> FindAll(Expression<Func<T,bool>>? predicate,string? include);
        T Get();
        IEnumerable<T> GetAll();
        void Remove(T entity);
        void Add(T entity);
        void RemoveAll();
        void RemoveRange(IEnumerable<T> entities);  
    }
}
