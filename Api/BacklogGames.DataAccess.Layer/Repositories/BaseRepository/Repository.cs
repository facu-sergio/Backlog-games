using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BacklogGames.DataAccess.Layer.Data;

namespace BacklogGames.DataAccess.Layer.Repositories.BaseRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
             await _dbSet.AddAsync(entity);
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            // Aplicar filtro si existe
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Incluir propiedades de navegación si se especifican
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            // Aplicar ordenamiento si existe
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            // Aplicar filtro si existe
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Incluir propiedades de navegación si se especifican
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

    }
}
