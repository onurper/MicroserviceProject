using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public interface IProductInvoiceRepository<TContext> : IGenericRepository<User, TContext> where TContext : DbContext
    {
    }
}