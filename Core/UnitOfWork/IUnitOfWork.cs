using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Core.UnitOfWork
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        Task CommmitAsync();

        void Commit();
    }
}