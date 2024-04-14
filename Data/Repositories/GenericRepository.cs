using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class GenericRepository<Tentity, TContext> : IGenericRepository<Tentity, TContext> where Tentity : class where TContext : DbContext
    {
        private readonly DbContext _context;

        public GenericRepository(TContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Tentity entity)
        {
            await _context.Set<Tentity>().AddAsync(entity);
        }

        public async Task<IEnumerable<Tentity>> GetAllAsync()
        {
            return await _context.Set<Tentity>().ToListAsync();
        }

        public async Task<Tentity> GetByIdAsync(int id)
        {
            var entity = await _context.Set<Tentity>().FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public void Remove(Tentity entity)
        {
            _context.Set<Tentity>().Remove(entity);
        }

        public Tentity Update(Tentity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public IQueryable<Tentity> Where(Expression<Func<Tentity, bool>> predicate)
        {
            return _context.Set<Tentity>().Where(predicate);
        }
    }
}