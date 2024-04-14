using Core.Models;
using Core.Repositories;
using Core.Services;
using Data;

namespace Service.Services
{
    public class ProductInvoiceService : ServiceGeneric<ProductInvoice, AppProductInvoiceContext>, IProductInvoiceService
    {
        public ProductInvoiceService(Core.UnitOfWork.IUnitOfWork<AppProductInvoiceContext> unitOfWork, IGenericRepository<ProductInvoice, AppProductInvoiceContext> genericRepository) : base(unitOfWork, genericRepository)
        {
        }
    }
}