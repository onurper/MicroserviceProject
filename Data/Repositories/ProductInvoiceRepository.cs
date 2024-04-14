using Core.Models;
using Core.Repositories;

namespace Data.Repositories
{
    public class ProductInvoiceRepository : GenericRepository<User, AppProductInvoiceContext>, IProductInvoiceRepository<AppProductInvoiceContext>
    {
        public ProductInvoiceRepository(AppProductInvoiceContext context) : base(context)
        {
        }
    }
}