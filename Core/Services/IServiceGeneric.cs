using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IServiceGeneric<TEntity, TContext> where TEntity : class
    {
        Task<Response<TEntity>> GetByIdAsync(int id);

        Task<Response<IEnumerable<TEntity>>> GetAllAsync();

        Task<Response<IEnumerable<TEntity>>> GetAllCategoryAsync();

        Task<Response<IEnumerable<TEntity>>> Where(Expression<Func<TEntity, bool>> predicate);

        Task<Response<TEntity>> AddAsync(TEntity entity);

        Task<Response<NoDataDto>> Remove(int id);

        Task<Response<NoDataDto>> Update(TEntity entity, int id);
    }
}