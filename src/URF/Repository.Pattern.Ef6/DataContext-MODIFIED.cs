using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Repository.Pattern.DataContext;
using Repository.Pattern.Infrastructure;


namespace Repository.Pattern.Ef6
{
    /// <summary>
    /// This code has been modified from the URF Library release to support the
    /// functionality of the Microsoft Identity framework.  The modifications are 
    /// limited to the single method SyncObjectsStatePreCommit() found near the end
    /// of the file.
    /// 
    /// WARNING!
    /// This method has had only limited testing and should be adopted with caution!
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


        /********************************************************************************
        **                                                                             **
        **        THIS METHOD HAS BEEN MODIFIED AND REQUIRES FURTHER TESTING!          **
        **                                                                             **
        ********************************************************************************/

        // Original URF library code
        //
        // private void SyncObjectsStatePreCommit()
        // {
        //     foreach (var dbEntityEntry in ChangeTracker.Entries())
        //     {
        //         dbEntityEntry.State =
        //             StateHelper.ConvertState(
        //                 ((IObjectState) dbEntityEntry.Entity).ObjectState);
        //     }
        // }

        private void SyncObjectsStatePreCommit()
        {
            // The body of the following foreach() loop originally simply assigned the
            // urfEntityState to the dbEntityEntry.State.  Unfortunately, this approach
            // failed with Identity 2.0 managed entities.
            //
            // Microsoft's Identity 2.0 framework knows nothing about the URF's
            // IObject state and works directly with the DbEntityEntry state so the URF
            // is unaware of any changes made to entities managed by Identity 2.0.
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                var objectState = ((IObjectState) dbEntityEntry.Entity).ObjectState;
                var urfEntityState = StateHelper.ConvertState(objectState);
                var entityState = dbEntityEntry.State;

                // The URF ObjectState, if set, always wins
                // This logic needs testing in a wider variety of applications
                if ((urfEntityState != EntityState.Unchanged) &&
                    (entityState == EntityState.Unchanged))
                {
                    dbEntityEntry.State = urfEntityState;
                }
                else if (urfEntityState != entityState)
                {
                    ((IObjectState) dbEntityEntry.Entity).ObjectState =
                        StateHelper.ConvertState(entityState);
                }
            }
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