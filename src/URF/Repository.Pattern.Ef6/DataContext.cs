using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Repository.Pattern.DataContext;
using Repository.Pattern.Infrastructure;


namespace Repository.Pattern.Ef6
{
    /// <summary>
    /// This class has been modified to allow more flexible SyncObjectsStatePreCommit()
    /// processing for Microsoft's Identity 2.0 entities - specifically the management 
    /// and synchronisation of the base entity state and the URF's IObjectState.
    /// 
    /// See the SyncObjectsStatePreCommit() method below and the introduced
    /// ProcessObjectStatePreCommit(DbEntityEntry dbEntityEntry) virtual method.
    /// 
    /// Neil Martin - 16th November 2015
    /// </summary>
    public class DataContext : DbContext, IDataContextAsync
    {
        #region Private Fields
        private readonly Guid _instanceId;
        private bool _disposed;
        #endregion Private Fields



        public DataContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            _instanceId = Guid.NewGuid();
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }


        public Guid InstanceId
        {
            get { return _instanceId; }
        }


        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChanges"/>
        /// <returns>The number of objects written to the underlying database.</returns>
        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }


        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChangesAsync"/>
        /// <returns>A task that represents the asynchronous save operation.  The 
        ///     <see cref="Task.Result">Task.Result</see> contains the number of 
        ///     objects written to the underlying database.</returns>
        public override async Task<int> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(CancellationToken.None);
        }


        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChangesAsync"/>
        /// <returns>A task that represents the asynchronous save operation.  The 
        ///     <see cref="Task.Result">Task.Result</see> contains the number of 
        ///     objects written to the underlying database.</returns>
        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }


        public void SyncObjectState<TEntity>(TEntity entity)
            where TEntity : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }


        /// <remarks>
        /// This method has been modified to call the newly introduced 
        /// ProcessObjectStatePreCommit() virtual method.
        /// 
        /// Neil Martin - 16th November 2015
        /// </remarks>
        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ProcessObjectStatePreCommit(dbEntityEntry);
            }
        }


        /// <remarks>
        /// This virtual method was introduced to allow the final application DataContext
        /// the opportunity to process Microsoft's Identity 2.0 entities separately from
        /// native URF dervived and managed entities.
        /// 
        /// Microsoft's Identity 2.0 framework knows nothing about IObjectState so it is
        /// therfore necessary to override the default URF SyncObjectsStatePreCommit()
        /// method to ensure that the DataContext entity state is managed correctly.
        /// 
        /// Neil Martin - 16th November 2015
        /// </remarks>
        protected virtual void ProcessObjectStatePreCommit(DbEntityEntry dbEntityEntry)
        {
            dbEntityEntry.State =
                StateHelper.ConvertState(((IObjectState) dbEntityEntry.Entity).ObjectState);
        }


        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState) dbEntityEntry.Entity).ObjectState =
                    StateHelper.ConvertState(dbEntityEntry.State);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // free other managed objects that implement
                    // IDisposable only
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}