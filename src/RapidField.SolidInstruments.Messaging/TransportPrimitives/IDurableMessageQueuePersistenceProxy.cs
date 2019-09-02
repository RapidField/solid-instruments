// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Manages persistent state for <see cref="IDurableMessageQueue" /> instances.
    /// </summary>
    public interface IDurableMessageQueuePersistenceProxy : IDisposable
    {
        /// <summary>
        /// Asynchronously persists the specified operation record.
        /// </summary>
        /// <typeparam name="TOperation">
        /// The type of the operation record to persist.
        /// </typeparam>
        /// <param name="operation">
        /// The operation record to persist.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the operation record.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task PersistOperationRecordAsync<TOperation>(TOperation operation)
            where TOperation : class, IDurableMessageQueueOperation;

        /// <summary>
        /// Asynchronously persists a thread-safe snapshot of the associated queue and flattens the persistent operation records
        /// preceding it.
        /// </summary>
        /// <param name="snapshot">
        /// The snapshot to persist.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task PersistSnapshotAsync(DurableMessageQueueSnapshot snapshot);

        /// <summary>
        /// Asynchronously persists a thread-safe snapshot of the associated queue and, optionally, flattens the persistent operation
        /// records preceding it.
        /// </summary>
        /// <param name="snapshot">
        /// The snapshot to persist.
        /// </param>
        /// <param name="flattenOperationRecords">
        /// A value indicating whether or not all persisted operation records preceding the snapshot state are destroyed. The default
        /// value is <see langword="true" />.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task PersistSnapshotAsync(DurableMessageQueueSnapshot snapshot, Boolean flattenOperationRecords);
    }
}