using Domain.Nucleus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Nucleus.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetByFilter(Expression<Func<T, bool>> query);

        IEnumerable<T> GetAll();

        Task<T> GetById(long id);

        Task<T> LoadSingleWithRelatedAndFiltersAsync<T>(Expression<Func<T, bool>> filters, T entity, List<string> includes) where T : BaseEntity;
        
        Task<T> LoadSingleWithRelatedAsync<T>(T entity, List<string> includes) where T : BaseEntity;

        Task<T> LoadSingleWithRelatedNoTrackingAsync<T>(T entity, List<string> includes) where T : BaseEntity;

        Task<IEnumerable<T>> LoadAllWithRelatedAndFiltersAsync<T>(Expression<Func<T, bool>> filters, List<string> expressionList) where T : class;

        Task<IEnumerable<T>> LoadAllWithRelatedAsync<T>(List<string> expressionList) where T : class;

        Task Add(T entity);

        Task AddAsync(T entity);

        void Update(T entity);

        Task Delete(long id);
    }
}
