using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IBaseRepository <T, TKey> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync ();
        Task<T> GetAsync (TKey id);
        Task AddAsync (T entity);
        Task RemoveAsync (T entity);
        Task RemoveAllAsync (IEnumerable<T> entities);
    }
}
