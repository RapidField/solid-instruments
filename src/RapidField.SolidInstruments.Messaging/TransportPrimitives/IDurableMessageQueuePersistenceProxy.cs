// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Performs thread-safe snapshot persistence for <see cref="IDurableMessageQueue" /> instances.
    /// </summary>
    public interface IDurableMessageQueuePersistenceProxy : IDisposable
    {
        /// <summary>
        /// Asynchronously persists a thread-safe snapshot of the associated queue.
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
    }
}