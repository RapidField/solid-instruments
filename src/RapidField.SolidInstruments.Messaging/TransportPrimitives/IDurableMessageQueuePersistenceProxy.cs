// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Manages thread-safe snapshot persistence for a <see cref="IDurableMessageQueue" /> instance.
    /// </summary>
    public interface IDurableMessageQueuePersistenceProxy : IDisposable
    {
        /// <summary>
        /// Asynchronously persists a thread-safe snapshot of the associated queue.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task PersistSnapshotAsync();

        /// <summary>
        /// Gets the associated <see cref="IDurableMessageQueue" />.
        /// </summary>
        IDurableMessageQueue Queue
        {
            get;
        }
    }
}