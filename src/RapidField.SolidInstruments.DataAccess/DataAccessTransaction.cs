// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Fulfills the unit of work pattern for data access operations.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessTransaction" /> is the default implementation of <see cref="IDataAccessTransaction" />.
    /// </remarks>
    public abstract class DataAccessTransaction : Instrument, IDataAccessTransaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessTransaction" /> class.
        /// </summary>
        protected DataAccessTransaction()
            : base()
        {
            State = DataAccessTransactionState.Ready;
        }

        /// <summary>
        /// Initiates the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.Ready" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Begin()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (State == DataAccessTransactionState.Ready)
                {
                    State = DataAccessTransactionState.InProgress;
                    Begin(controlToken);
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(Begin)} cannot be invoked when the transaction state is {State}.");
                }
            }
        }

        /// <summary>
        /// Asynchronously initiates the current <see cref="IDataAccessTransaction" />.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.Ready" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task BeginAsync()
        {
            var controlToken = StateControl.Enter();

            try
            {
                RejectIfDisposed();

                if (State == DataAccessTransactionState.Ready)
                {
                    State = DataAccessTransactionState.InProgress;

                    return BeginAsync(controlToken).ContinueWith(beginTask =>
                    {
                        controlToken.Dispose();
                    });
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(BeginAsync)} cannot be invoked when the transaction state is {State}.");
                }
            }
            catch
            {
                controlToken.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Commits all changes made within the scope of the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.InProgress" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Commit()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (State == DataAccessTransactionState.InProgress)
                {
                    State = DataAccessTransactionState.Committed;
                    Commit(controlToken);
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(Commit)} cannot be invoked when the transaction state is {State}.");
                }
            }
        }

        /// <summary>
        /// Asynchronously commits all changes made within the scope of the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.InProgress" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task CommitAsync()
        {
            var controlToken = StateControl.Enter();

            try
            {
                RejectIfDisposed();

                if (State == DataAccessTransactionState.InProgress)
                {
                    State = DataAccessTransactionState.Committed;

                    return CommitAsync(controlToken).ContinueWith(commitTask =>
                    {
                        controlToken.Dispose();
                    });
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(CommitAsync)} cannot be invoked when the transaction state is {State}.");
                }
            }
            catch
            {
                controlToken.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Rejects all changes made within the scope of the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.InProgress" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Reject()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (State == DataAccessTransactionState.InProgress)
                {
                    State = DataAccessTransactionState.Rejected;
                    Reject(controlToken);
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(Reject)} cannot be invoked when the transaction state is {State}.");
                }
            }
        }

        /// <summary>
        /// Asynchronously rejects all changes made within the scope of the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.InProgress" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task RejectAsync()
        {
            var controlToken = StateControl.Enter();

            try
            {
                RejectIfDisposed();

                if (State == DataAccessTransactionState.InProgress)
                {
                    State = DataAccessTransactionState.Rejected;

                    return RejectAsync(controlToken).ContinueWith(rejectTask =>
                    {
                        controlToken.Dispose();
                    });
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(RejectAsync)} cannot be invoked when the transaction state is {State}.");
                }
            }
            catch
            {
                controlToken.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Initiates the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected abstract void Begin(ConcurrencyControlToken controlToken);

        /// <summary>
        /// Asynchronously initiates the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task BeginAsync(ConcurrencyControlToken controlToken);

        /// <summary>
        /// Commits all changes made within the scope of the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected abstract void Commit(ConcurrencyControlToken controlToken);

        /// <summary>
        /// Asynchronously commits all changes made within the scope of the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task CommitAsync(ConcurrencyControlToken controlToken);

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
                    State = DataAccessTransactionState.Unspecified;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Rejects all changes made within the scope of the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected abstract void Reject(ConcurrencyControlToken controlToken);

        /// <summary>
        /// Asynchronously all changes made within the scope of the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected abstract Task RejectAsync(ConcurrencyControlToken controlToken);

        /// <summary>
        /// Gets the state of the current <see cref="DataAccessTransaction" />.
        /// </summary>
        public DataAccessTransactionState State
        {
            get;
            private set;
        }
    }
}