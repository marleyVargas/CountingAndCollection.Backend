using Domain.Nucleus.Interfaces;
using Domain.Nucleus.Interfaces.Application;
using Domain.Nucleus.Interfaces.Transactional;
using Insfraestructure.PrincipalContext.Data;
using Insfraestructure.PrincipalContext.Repositories.Application;
using Insfraestructure.PrincipalContext.Repositories.Transactional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Insfraestructure.PrincipalContext.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CountingAndCollectionContext _context;

        #region Application
        private readonly IParametersRepository _parametersRepository;
        private readonly ILogRequestAndResponseRepository _logRequestAndResponseRepository;
        #endregion

        #region Transactional
        private readonly ICollectionRepository _collectionRepository;
        #endregion

        private IDbContextTransaction _transaction
        {
            get; set;
        }

        private bool OnTransaction
        {
            get; set;
        }

        public UnitOfWork(CountingAndCollectionContext context)
        {
            this._context = context;
        }

        #region Application
        public IParametersRepository ParametersRepository => this._parametersRepository ?? new ParametersRepository(this._context);
        public ILogRequestAndResponseRepository LogRequestAndResponseRepository => this._logRequestAndResponseRepository ?? new LogRequestAndResponseRepository(this._context);
           #endregion

        #region Transactional
        public ICollectionRepository CollectionRepository => this._collectionRepository ?? new CollectionRepository(this._context);

        #endregion

        public void Dispose()
        {
            if (this._context != null)
            {
                this._context.Dispose();
            }
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
        }

        public bool IsOnTransaction()
        {
            return OnTransaction;
        }

        public void BeginTransaction()
        {
            if (!OnTransaction)
            {
                _transaction = this._context.Database.BeginTransaction();

                OnTransaction = true;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                SaveChanges();

                _transaction.Commit();
            }
            finally
            {
                OnTransaction = false;

                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                _transaction.Commit();
            }
            finally
            {
                OnTransaction = false;

                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                DismissChanges();

                _transaction.Rollback();
            }
            finally
            {
                OnTransaction = false;

                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        private int DismissChanges()
        {
            int i = 0;

            foreach (var entry in this._context.ChangeTracker.Entries())
            {
                i++;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }

            return i;
        }
    }
}
