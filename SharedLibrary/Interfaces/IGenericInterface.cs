using SharedLibrary.Responses;
using System.Linq.Expressions;

namespace SharedLibrary.Interface
{
    public interface IGenericInterface<T> where T : class
    {
        Task<Response> CreateAsync(T entity, CancellationToken cancellationToken);
        Task<Response> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<Response> DeleteAsync(T entity, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> FindByIdAsync(int id, CancellationToken cancellationToken);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
