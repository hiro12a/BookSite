using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Book.Database.Repository.IRepository
{
    public interface IRepository<T> where T : class 
    {
        // T - Category
        IEnumerable<T> GetAll(string? includeProperties = null);
        T Get(Expression<Func<T,bool>> filter, string? includeProperties = null); // Similar to linq expression, general syntac
        void Add(T item);
        void Remove(T item);
        void RemoveRange(IEnumerable<T> items);

        // Best not to add Update in Repository
        // void Update(T item);
    }
}
