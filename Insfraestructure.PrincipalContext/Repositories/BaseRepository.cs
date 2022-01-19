using Domain.Nucleus.Entities;
using Domain.Nucleus.Interfaces;
using Insfraestructure.PrincipalContext.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Insfraestructure.PrincipalContext.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly CountingAndCollectionContext _context;
        protected DbSet<T> _entities;

        public BaseRepository(CountingAndCollectionContext context)
        {
            this._context = context;
            this._entities = _context.Set<T>();
        }

        public IEnumerable<T> GetByFilter(Expression<Func<T, bool>> query)
        {
            return this._entities.Where(query);
        }

        public IEnumerable<T> GetAll()
        {
            return this._entities.AsEnumerable();
        }

        public async Task<T> GetById(long id)
        {
            return await this._entities.FindAsync(id);
        }

        public async Task<T> LoadSingleWithRelatedAndFiltersAsync<T>(Expression<Func<T, bool>> filters, T entity, List<string> includes) where T : BaseEntity
        {

            var query = this._context.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.Where(filters).FirstOrDefaultAsync(p => p.Id == entity.Id);
        }

        public async Task<T> LoadSingleWithRelatedAsync<T>(T entity, List<string> includes) where T : BaseEntity
        {
        
            var query = this._context.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(p => p.Id == entity.Id);
        }

        public async Task<T> LoadSingleWithRelatedNoTrackingAsync<T>(T entity, List<string> includes) where T : BaseEntity
        {

            var query = this._context.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(p => p.Id == entity.Id);
        }

        public async Task<IEnumerable<T>> LoadAllWithRelatedAndFiltersAsync<T>(Expression<Func<T, bool>> filters, List<string> expressionList) where T : class
        {
            var query = this._context.Set<T>().Where(filters).AsQueryable();

            foreach (var expression in expressionList)
            {
                query = query.Include(expression);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> LoadAllWithRelatedAsync<T>(List<string> expressionList) where T : class
        {
            var query = this._context.Set<T>().AsQueryable();

            foreach (var expression in expressionList)
            {
                query = query.Include(expression);
            }

            return await query.ToListAsync();
        }

        public async Task Add(T entity)
        {
            await this._entities.AddAsync(entity);
        }

        public async Task AddRange(T[] entity)
        {
            this._entities.AddRange(entity);
        }

        public Task AddAsync(T entity)
        {
            this._entities.Add(entity);
            return this._context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            this._entities.Update(entity);
        }

        public async Task Delete(long id)
        {
            T entity = await GetById(id);
            this._entities.Remove(entity);
        }

    }
}
