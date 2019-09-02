// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Fulfills the unit of work pattern for Entity Framework data access operations.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the database session for the transaction.
    /// </typeparam>
    public class EntityFrameworkTransaction<TContext> : DataAccessTransaction
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkTransaction{TContext}" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the transaction.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="context" /> has outstanding changes tracked against it.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public EntityFrameworkTransaction(TContext context)
            : this(context, DefaultIsolationLevel)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkTransaction{TContext}" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the transaction.
        /// </param>
        /// <param name="isolationLevel">
        /// The isolation level for the transaction, or <see cref="IsolationLevel.Unspecified" /> to use the database default. The
        /// default value is <see cref="IsolationLevel.Unspecified" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="context" /> has outstanding changes tracked against it.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public EntityFrameworkTransaction(TContext context, IsolationLevel isolationLevel)
            : base()
        {
            Context = context.RejectIf().IsNull(nameof(context));
            IsolationLevel = isolationLevel;
            Transaction = null;

            if (context.ChangeTracker.HasChanges())
            {
                throw new ArgumentException("The specified context cannot be used in a transaction because there are outstanding changes tracked against it.", nameof(context));
            }
        }

        /// <summary>
        /// Initiates the current <see cref="EntityFrameworkTransaction{TContext}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that ensures tread safety for the operation.
        /// </param>
        protected override void Begin(ConcurrencyControlToken controlToken) => Transaction = Context.Database.BeginTransaction(IsolationLevel);

        /// <summary>
        /// Asynchronously initiates the current <see cref="EntityFrameworkTransaction{TContext}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that ensures tread safety for the operation.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected override Task BeginAsync(ConcurrencyControlToken controlToken) => Context.Database.BeginTransactionAsync(IsolationLevel).ContinueWith(beginTransactionTask =>
        {
            Transaction = beginTransactionTask.Result;
        });

        /// <summary>
        /// Commits all changes made within the scope of the current <see cref="EntityFrameworkTransaction{TContext}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that ensures tread safety for the operation.
        /// </param>
        protected override void Commit(ConcurrencyControlToken controlToken)
        {
            Context.SaveChanges(true);
            Transaction?.Commit();
        }

        /// <summary>
        /// Asynchronously commits all changes made within the scope of the current
        /// <see cref="EntityFrameworkTransaction{TContext}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that ensures tread safety for the operation.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected override Task CommitAsync(ConcurrencyControlToken controlToken) => Context.SaveChangesAsync(true).ContinueWith(saveChangesTask =>
        {
            Transaction?.Commit();
        });

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    Transaction?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Rejects all changes made within the scope of the current <see cref="EntityFrameworkTransaction{TContext}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that ensures tread safety for the operation.
        /// </param>
        protected override void Reject(ConcurrencyControlToken controlToken)
        {
            var entities = Context.ChangeTracker.Entries();

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Added:

                        entity.State = EntityState.Detached;
                        break;

                    default:

                        entity.Reload();
                        break;
                }
            }

            Transaction?.Rollback();
        }

        /// <summary>
        /// Asynchronously rejects all changes made within the scope of the current
        /// <see cref="EntityFrameworkTransaction{TContext}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that ensures tread safety for the operation.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected override Task RejectAsync(ConcurrencyControlToken controlToken)
        {
            var entities = Context.ChangeTracker.Entries();
            var reloadTasks = new List<Task>();

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Added:

                        entity.State = EntityState.Detached;
                        break;

                    default:

                        reloadTasks.Add(entity.ReloadAsync());
                        break;
                }
            }

            if (reloadTasks.Any())
            {
                return Task.WhenAll(reloadTasks.ToArray()).ContinueWith(reloadAllEntitiesTask =>
                {
                    Transaction?.Rollback();
                });
            }

            return Task.Factory.StartNew(() => Transaction?.Rollback());
        }

        /// <summary>
        /// Gets or sets the underlying database transaction for the current <see cref="EntityFrameworkTransaction{TContext}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDbContextTransaction Transaction
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default isolation level for transactions.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const IsolationLevel DefaultIsolationLevel = IsolationLevel.Unspecified;

        /// <summary>
        /// Represents the database session for the current <see cref="EntityFrameworkTransaction{TContext}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TContext Context;

        /// <summary>
        /// Represents the isolation level for the current <see cref="EntityFrameworkTransaction{TContext}" />, or
        /// <see cref="IsolationLevel.Unspecified" /> if the database default is used.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IsolationLevel IsolationLevel;
    }
}