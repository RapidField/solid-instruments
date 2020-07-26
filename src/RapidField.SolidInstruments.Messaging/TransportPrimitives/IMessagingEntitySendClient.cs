// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client that facilitates send operations to an <see cref="IMessagingEntity" />.
    /// </summary>
    public interface IMessagingEntitySendClient : IMessagingEntityClient
    {
        /// <summary>
        /// Asynchronously sends the specified message to the associated <see cref="IMessagingEntity" />.
        /// </summary>
        /// <param name="message">
        /// The message to send.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The connection is closed.
        /// </exception>
        public Task SendAsync(PrimitiveMessage message);
    }
}