﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Fulfills the unit of work pattern for data access operations.
    /// </summary>
    public interface IDataAccessTransaction : IInstrument
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
        public void Begin();

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
        public Task BeginAsync();

        /// <summary>
        /// Commits all changes made within the scope of the current <see cref="IDataAccessTransaction" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.InProgress" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Commit();

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
        public Task CommitAsync();

        /// <summary>
        /// Rejects all changes made within the scope of the current <see cref="IDataAccessTransaction" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="State" /> is not equal to <see cref="DataAccessTransactionState.InProgress" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Reject();

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
        public Task RejectAsync();

        /// <summary>
        /// Gets the state of the current <see cref="IDataAccessTransaction" />.
        /// </summary>
        public DataAccessTransactionState State
        {
            get;
        }
    }
}