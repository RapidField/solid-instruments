// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client connection to an <see cref="IMessageTransport" />.
    /// </summary>
    public interface IMessageTransportConnection : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Closes the current <see cref="IMessageTransportConnection" /> as an idempotent operation.
        /// </summary>
        void Close();

        /// <summary>
        /// Registers the specified message handler for the specified queue.
        /// </summary>
        /// <param name="queuePath">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="handleMessageAction">
        /// An action to perform upon message receipt.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queuePath" /> is <see langword="null" /> -or- <paramref name="handleMessageAction" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified queue does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        void RegisterQueueHandler(IMessagingEntityPath queuePath, Action<PrimitiveMessage> handleMessageAction);

        /// <summary>
        /// Registers the specified message handler for the specified topic subscription.
        /// </summary>
        /// <param name="topicPath">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <param name="handleMessageAction">
        /// An action to perform upon message receipt.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="topicPath" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" /> -or- <paramref name="handleMessageAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified subscription does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        void RegisterSubscriptionHandler(IMessagingEntityPath topicPath, String subscriptionName, Action<PrimitiveMessage> handleMessageAction);

        /// <summary>
        /// Gets a value that uniquely identifies the current <see cref="IMessageTransportConnection" />.
        /// </summary>
        Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the state of the current <see cref="IMessageTransportConnection" />.
        /// </summary>
        MessageTransportConnectionState State
        {
            get;
        }

        /// <summary>
        /// Gets the associated <see cref="IMessageTransport" />.
        /// </summary>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The connection is closed.
        /// </exception>
        IMessageTransport Transport
        {
            get;
        }
    }
}