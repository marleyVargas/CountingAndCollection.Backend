using Domain.Nucleus.Interfaces.Application;
using Domain.Nucleus.Interfaces.Transactional;
using System;
using System.Threading.Tasks;

namespace Domain.Nucleus.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        #region Application

        IParametersRepository ParametersRepository
        {
            get;
        }

        ILogRequestAndResponseRepository LogRequestAndResponseRepository
        {
            get;
        }

        #endregion

        #region Transactional

        ICollectionRepository CollectionRepository
        {
            get;
        }

        #endregion

        bool IsOnTransaction();

        void BeginTransaction();

        void CommitTransaction();

        Task CommitTransactionAsync();

        void RollbackTransaction();

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
