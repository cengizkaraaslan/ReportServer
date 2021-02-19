using Core.Entities;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
  public interface IEntityRepository<T> where T:class,IEntity,new()
    {
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task<List<T>> GetList(Expression<Func<T, bool>> filter = null);
        Task<Guid> Add(T entity);
        Task AddList(IEnumerable<T> entities);
        Task Update(T entity);
        Task Delete(Guid id);
        Task DeleteAll(Expression<Func<T, bool>> filter);
        Task<ListDto<T>> GetPaginatedList(Expression<Func<T, bool>> filter, int skip, int take, string orderby);
        Task<List<T>> GetListWithoutTracking(Expression<Func<T, bool>> filter = null);
        Task<Guid> AddReturnId(T entity);
    }
}
