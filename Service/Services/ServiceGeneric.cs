using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ServiceGeneric<TEntity, TContext> : IServiceGeneric<TEntity, TContext> where TEntity : class where TContext : DbContext
    {
        private readonly IUnitOfWork<TContext> _unitOfWork;

        private readonly IGenericRepository<TEntity, TContext> _genericRepository;

        public ServiceGeneric(IUnitOfWork<TContext> unitOfWork, IGenericRepository<TEntity, TContext> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<Response<TEntity>> AddAsync(TEntity entity)
        {
            await _genericRepository.AddAsync(entity);

            await _unitOfWork.CommmitAsync();

            return Response<TEntity>.Success(entity, 200);
        }

        public async Task<Response<IEnumerable<TEntity>>> GetAllAsync()
        {
            var products = await _genericRepository.GetAllAsync();

            return Response<IEnumerable<TEntity>>.Success(products, 200);
        }

        public async Task<Response<TEntity>> GetByIdAsync(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);

            if (product == null)
                return Response<TEntity>.Fail("Id not found", 404, true);

            return Response<TEntity>.Success(product, 200);
        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);

            if (isExistEntity == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }

            _genericRepository.Remove(isExistEntity);

            await _unitOfWork.CommmitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> Update(TEntity entity, int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);

            if (isExistEntity == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }

            var updateEntity = entity;

            _genericRepository.Update(updateEntity);

            await _unitOfWork.CommmitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<IEnumerable<TEntity>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = _genericRepository.Where(predicate);

            var ss = await list.ToListAsync();
            return Response<IEnumerable<TEntity>>.Success(await list.ToListAsync(), 200);
        }
    }
}