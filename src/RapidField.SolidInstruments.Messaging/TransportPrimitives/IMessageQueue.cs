// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a message queue.
    /// </summary>
    internal interface IMessageQueue : IMessagingEntity
    {
        /// <summary>
        /// Asynchronously and non-destructively returns the next available messages from the current <see cref="IMessageQueue" />,
        /// if any, up to the specified maximum count.
        /// </summary>
        /// <param name="count">
        /// The maximum number of messages to read from the queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the next available messages from the queue, or an empty
        /// collection if no messages are available.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        Task<IEnumerable<PrimitiveMessage>> DequeueAsync(Int32 count);
    }
}