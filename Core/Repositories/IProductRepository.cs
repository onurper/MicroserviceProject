using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IProductRepository<TContext> : IGenericRepository<Product, TContext> where TContext : DbContext
    {
        public Task<bool> CategoryCheck(int categoryId);
    }
}