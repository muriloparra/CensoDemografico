using CensoDemografico.Infra.Context;
using CensoDemografico.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CensoDemografico.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected CensoDemograficoContext _context;

        public BaseRepository(CensoDemograficoContext censoDemograficoContext)
        {
            _context = censoDemograficoContext;
        }

        public IQueryable<T> AllAsNoTracking => _context.Set<T>().AsQueryable().AsNoTracking();

        public void Add(T obj)
        {
            _context.Set<T>().Add(obj);
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Detached;
            _context.Update(obj);
            _context.SaveChanges();
        }

        public void AddRange(List<T> obj)
        {
            _context.Set<T>().AddRange(obj);
            _context.SaveChanges();
        }

        public T Find(int key)
        {
            return _context.Find<T>(key);
        }

        public List<T> GetAsNoTracking(Expression<Func<T, bool>> predicate, params string[] includes)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
            
            if (query == null)
                return null;

            foreach (string include in includes)
            {
                query = query.Include(include);
            }
            return query.ToList();
        }
    }
}
