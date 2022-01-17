using Domain.Nucleus.Entities;
using Domain.Nucleus.Entities.Application;
using Domain.Nucleus.Entities.Transactional;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#nullable disable

namespace Insfraestructure.PrincipalContext.Data
{
    public partial class CountingAndCollectionContext : DbContext
    {
        public CountingAndCollectionContext()
        {
        }

        public CountingAndCollectionContext(DbContextOptions<CountingAndCollectionContext> options)
            : base(options)
        {
        }

        #region Application

        public virtual DbSet<Parameters> Parameters
        {
            get; set;
        }

        public virtual DbSet<LogRequestAndResponse> LogRequestAndResponses
        {
            get; set;
        }

        #endregion

        #region Transactionals


        public virtual DbSet<Collection> Transactions
        {
            get; set;
        }     

    

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var AddedEntities = ChangeTracker.Entries<BaseEntity>().Where(E => E.State == EntityState.Added).ToList();

            AddedEntities.ForEach(E =>
            {
                E.Property(x => x.CreatedDate).CurrentValue = DateTime.Now;
                E.Property(x => x.CreatedDate).IsModified = true;
            });

            var ModifyEntities = ChangeTracker.Entries<BaseEntity>().Where(E => E.State == EntityState.Modified).ToList();

            ModifyEntities.ForEach(E =>
            {
                E.Property(x => x.UpdatedDate).CurrentValue = DateTime.Now;
                E.Property(x => x.UpdatedDate).IsModified = true;

                E.Property(x => x.CreatedDate).CurrentValue = E.Property(x => x.CreatedDate).OriginalValue;
                E.Property(x => x.CreatedDate).IsModified = false;
            });

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            var AddedEntities = ChangeTracker.Entries<BaseEntity>().Where(E => E.State == EntityState.Added).ToList();

            AddedEntities.ForEach(E =>
            {
                E.Property(x => x.CreatedDate).CurrentValue = DateTime.UtcNow;
                E.Property(x => x.CreatedDate).IsModified = true;
            });

            var ModifyEntities = ChangeTracker.Entries<BaseEntity>().Where(E => E.State == EntityState.Modified).ToList();

            ModifyEntities.ForEach(E =>
            {
                E.Property(x => x.UpdatedDate).CurrentValue = DateTime.UtcNow;
                E.Property(x => x.UpdatedDate).IsModified = true;

                E.Property(x => x.CreatedDate).CurrentValue = E.Property(x => x.CreatedDate).OriginalValue;
                E.Property(x => x.CreatedDate).IsModified = false;
            });

            return base.SaveChanges();
        }

    }
}
