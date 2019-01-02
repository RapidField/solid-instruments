// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Fulfills the unit of work pattern for data access operations.
    /// </summary>
    public interface IDataAccessTransaction : IDisposable
    {
        /// <summary>
        /// Initiates the current <see cref="IDataAccessTransaction" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.Ready" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        void Begin();

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
        Task BeginAsync();

        /// <summary>
        /// Commits all changes made within the scope of the current <see cref="IDataAccessTransaction" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.InProgress" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        void Commit();

        /// <summary>
        /// Asynchronously commits all changes made within the scope of the current <see cref="IDataAccessTransaction" />.
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
        Task CommitAsync();

        /// <summary>
        /// Rejects all changes made within the scope of the current <see cref="IDataAccessTransaction" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.InProgress" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        void Reject();

        /// <summary>
        /// Asynchronously rejects all changes made within the scope of the current <see cref="IDataAccessTransaction" />.
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
        Task RejectAsync();

        /// <summary>
        /// Gets the state of the current <see cref="IDataAccessTransaction" />.
        /// </summary>
        DataAccessTransactionState State
        {
            get;
        }
    }
}