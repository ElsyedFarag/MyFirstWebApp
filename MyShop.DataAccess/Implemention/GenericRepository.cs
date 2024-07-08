using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyShop.DataAccess.Data;
using MyShop.Entities;
using MyShop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implemention
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;   
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
                _dbSet.Add(entity);
                _context.SaveChanges();
            
        }

        public T Find(Expression<Func<T, bool>>? predicate, string? include)
        {
            IQueryable<T> quary = _dbSet;
            if (predicate != null)
            {
                quary = quary.Where(predicate);
            }
            if (include != null)
            {
                foreach (var item in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    quary = quary.Include(item);
                }
            }
            return quary.FirstOrDefault();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, string? include)
        {

            IQueryable<T> quary = _dbSet;
            if(predicate != null)
            {
                quary = quary.Where(predicate); 
            }
            if(include != null)
            {
                foreach(var item in include.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    quary = quary.Include(item);    
                }
            }
            return quary.ToList();
        }

        public T Get()
        {
            return _dbSet.FirstOrDefault();
        }
        
        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveAll()
        {
            _dbSet.ToList().Clear();
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);   
            _context.SaveChanges(); 
        }
    }
}
