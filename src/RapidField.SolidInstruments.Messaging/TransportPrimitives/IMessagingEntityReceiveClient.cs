// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client that facilitates receive operations to an <see cref="IMessagingEntity" />.
    /// </summary>
    public interface IMessagingEntityReceiveClient : IMessagingEntityClient
    {
        /// <summary>
        /// Asynchronously notifies the associated <see cref="IMessagingEntity" /> that a locked message was not processed and can
        /// be made available for processing by other consumers.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was not processed.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        Task ConveyFailureAsync(MessageLockToken lockToken);

        /// <summary>
        /// Asynchronously notifies the associated <see cref="IMessagingEntity" /> that a locked message was processed successfully
        /// and can be destroyed permanently.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was processed successfully.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task ConveySuccessAsync(MessageLockToken lockToken);

        /// <summary>
        /// Registers the specified message handler for the associated <see cref="IMessagingEntity" />.
        /// </summary>
        /// <param name="handleMessageAction">
        /// An action to perform upon message receipt.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handleMessageAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The connection is closed.
        /// </exception>
        void RegisterMessageHandler(Action<PrimitiveMessage> handleMessageAction);
    }
}