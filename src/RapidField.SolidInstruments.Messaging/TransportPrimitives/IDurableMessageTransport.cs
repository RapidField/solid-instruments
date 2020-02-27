// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Supports message exchange for a collection of queues and topics.
    /// </summary>
    public interface IDurableMessageTransport : IInstrument
    {
        /// <summary>
        /// Asynchronously creates a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// A
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="path" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task CreateQueueAsync(String path);
    }
}