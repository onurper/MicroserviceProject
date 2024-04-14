using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public interface IUserRepository<TContext> : IGenericRepository<User, TContext> where TContext : DbContext
    {
    }
}