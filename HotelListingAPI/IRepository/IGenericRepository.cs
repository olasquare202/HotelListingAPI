using HotelListingAPI.Pagination;
using System.Linq.Expressions;
using X.PagedList;

namespace HotelListingAPI.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAllAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes = null
            );
        Task<IPagedList<T>> GetPageList(
            Paging paging,
            List<string> includes = null
            );
        Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null);
        Task insert(T entity); //Create
        Task insertRange(IEnumerable<T> entities);
        Task Delete(int id);//Delete
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);//Update
    }
}
