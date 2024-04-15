using Core.Models;
using Core.Repositories;
using Core.Services;
using Data;
using SharedLibrary.Dtos;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ProductService : ServiceGeneric<Product, AppProductContext>, IProductService
    {
        private IProductRepository<AppProductContext> productRepository;

        public ProductService(IProductRepository<AppProductContext> productRepository, Core.UnitOfWork.IUnitOfWork<AppProductContext> unitOfWork, IGenericRepository<Product, AppProductContext> genericRepository) : base(unitOfWork, genericRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Response<NoDataDto>> CategoryCheck(int categoryId)
        {
            var result = await productRepository.CategoryCheck(categoryId);

            if (!result)
                return Response<NoDataDto>.Fail("Hatalı kategory girişi", 500, true);

            return Response<NoDataDto>.Success(new NoDataDto(), 204);
        }
    }
}