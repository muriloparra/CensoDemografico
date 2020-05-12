using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CensoDemografico.Infra.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        void Add(T obj);
        void AddRange(List<T> obj);
        T Find(int key);
        void Update(T obj);
        IQueryable<T> AllAsNoTracking { get; }
        List<T> GetAsNoTracking(Expression<Func<T, bool>> predicate, params string[] includes);
    }
}
