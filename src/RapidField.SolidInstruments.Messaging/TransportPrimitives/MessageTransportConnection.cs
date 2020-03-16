// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client connection to an <see cref="IMessageTransport" />.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageTransportConnection" /> is the default implementation of <see cref="IMessageTransportConnection" />.
    /// </remarks>
    internal sealed class MessageTransportConnection : Instrument, IMessageTransportConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportConnection" /> class.
        /// </summary>
        /// <param name="transport">
        /// The associated transport.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="transport" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageTransportConnection(IMessageTransport transport)
            : base()
        {
            Handlers = new List<Handler>();
            Identifier = Guid.NewGuid();
            State = MessageTransportConnectionState.Open;
            TransportReference = transport.RejectIf().IsNull(nameof(transport)).TargetArgument;
        }

        /// <summary>
        /// Closes the current <see cref="MessageTransportConnection" /> as an idempotent operation.
        /// </summary>
        public void Close()
        {
            if (State == MessageTransportConnectionState.Open)
            {
                State = MessageTransportConnectionState.Closed;

                if (TransportReference.Connections.Any(connection => connection.Identifier == Identifier))
                {
                    TransportReference.CloseConnection(this);
                }
            }
        }

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
        public void RegisterQueueHandler(IMessagingEntityPath queuePath, Action<PrimitiveMessage> handleMessageAction)
        {
            RejectIfDisposed();

            if (Transport.QueueExists(queuePath))
            {
                Handlers.Add(new Handler(queuePath, MessagingEntityType.Queue, null, handleMessageAction));
                return;
            }

            throw new InvalidOperationException($"Failed to register queue handler. The specified queue, \"{queuePath}\", does not exist.");
        }

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
        public void RegisterSubscriptionHandler(IMessagingEntityPath topicPath, String subscriptionName, Action<PrimitiveMessage> handleMessageAction)
        {
            RejectIfDisposed();

            if (Transport.SubscriptionExists(topicPath, subscriptionName))
            {
                Handlers.Add(new Handler(topicPath, MessagingEntityType.Topic, subscriptionName, handleMessageAction));
                return;
            }

            throw new InvalidOperationException($"Failed to register subscription handler. The specified subscription, \"{subscriptionName}\", does not exist.");
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageTransportConnection" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    Close();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets a value that uniquely identifies the current <see cref="MessageTransportConnection" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the state of the current <see cref="MessageTransportConnection" />.
        /// </summary>
        public MessageTransportConnectionState State
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the associated <see cref="IMessageTransport" />.
        /// </summary>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The connection is closed.
        /// </exception>
        public IMessageTransport Transport => State == MessageTransportConnectionState.Open ? TransportReference : throw new MessageTransportConnectionClosedException($"Connection {Identifier.ToSerializedString()} is closed.");

        /// <summary>
        /// Represents a collection of actions that is performed upon message receipt from specific entities.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ICollection<Handler> Handlers;

        /// <summary>
        /// Represents the associated <see cref="IMessageTransport" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IMessageTransport TransportReference;

        /// <summary>
        /// Represents an action that is performed upon message receipt from a specific entity.
        /// </summary>
        private sealed class Handler
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            /// <param name="path">
            /// A unique textual path that identifies the associated entity.
            /// </param>
            /// <param name="entityType">
            /// The entity type of the associated entity.
            /// </param>
            /// <param name="subscriptionName">
            /// The unique name of the associated subscription, or <see langword="null" /> if the entity is a queue.
            /// </param>
            /// <param name="handleMessageAction">
            /// An action to perform upon message receipt.
            /// </param>
            /// <exception cref="ArgumentEmptyException">
            /// <paramref name="subscriptionName" /> is empty and <paramref name="entityType" /> is equal to
            /// <see cref="MessagingEntityType.Topic" />.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="handleMessageAction" /> is
            /// <see langword="null" /> -or- <paramref name="subscriptionName" /> is <see langword="null" /> and
            /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Topic" />.
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
            /// </exception>
            [DebuggerHidden]
            internal Handler(IMessagingEntityPath path, MessagingEntityType entityType, String subscriptionName, Action<PrimitiveMessage> handleMessageAction)
            {
                EntityType = entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType));
                HandleMessageAction = handleMessageAction.RejectIf().IsNull(nameof(handleMessageAction));
                Path = path.RejectIf().IsNull(nameof(path)).TargetArgument;
                SubscriptionName = entityType == MessagingEntityType.Queue ? null : subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName));
            }

            /// <summary>
            /// Gets the entity type of the associated <see cref="IMessagingEntity" />.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal MessagingEntityType EntityType
            {
                get;
            }

            /// <summary>
            /// Gets an action to perform upon message receipt.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal Action<PrimitiveMessage> HandleMessageAction
            {
                get;
            }

            /// <summary>
            /// Gets the unique textual path for the messaging entity.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal IMessagingEntityPath Path
            {
                get;
            }

            /// <summary>
            /// Gets the unique name of the associated subscription, or <see langword="null" /> if the entity is a queue.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal String SubscriptionName
            {
                get;
            }
        }
    }
}