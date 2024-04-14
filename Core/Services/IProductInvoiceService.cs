using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public interface IProductInvoiceService : IServiceGeneric<ProductInvoice, DbContext>
    {
    }
}