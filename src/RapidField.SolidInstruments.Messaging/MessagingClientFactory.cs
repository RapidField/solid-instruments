// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.TextEncoding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an appliance that manages implementation-specific messaging clients.
    /// </summary>
    /// <typeparam name="TSender">
    /// The type of the implementation-specific send client.
    /// </typeparam>
    /// <typeparam name="TReceiver">
    /// The type of the implementation-specific receive client.
    /// </typeparam>
    /// <typeparam name="TAdaptedMessage">
    /// The type of implementation-specific adapted messages.
    /// </typeparam>
    /// <typeparam name="TConnection">
    /// The type of the connection
    /// </typeparam>
    /// <remarks>
    /// <see cref="MessagingClientFactory{TSender, TReceiver, TAdaptedMessage, TConnection}" /> is the default implementation of
    /// <see cref="IMessagingClientFactory{TSender, TReceiver, TAdaptedMessage}" />.
    /// </remarks>
    public abstract class MessagingClientFactory<TSender, TReceiver, TAdaptedMessage, TConnection> : Instrument, IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
        where TConnection : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingClientFactory{TSender, TReceiver, TConnection, TWrappedMessage}" />
        /// class.
        /// </summary>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        protected MessagingClientFactory(TConnection connection)
        {
            Connection = connection.RejectIf().IsNull(nameof(connection));
        }

        /// <summary>
        /// Gets a shared, managed, implementation-specific message receiver.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessageSubscriptionException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TReceiver GetMessageReceiver<TMessage>(MessagingEntityType entityType)
            where TMessage : class
        {
            var entityPath = GetEntityPath<TMessage>(entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType)));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (MessageReceivers.TryGetValue(entityPath, out var receiver))
                {
                    return receiver;
                }

                try
                {
                    receiver = CreateMessageReceiver<TMessage>(entityType, Connection);
                }
                catch (Exception exception)
                {
                    throw new MessageSubscriptionException(typeof(TMessage), exception);
                }

                MessageReceivers.Add(entityPath, receiver);
                return receiver;
            }
        }

        /// <summary>
        /// Gets a shared, managed, implementation-specific message sender.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TSender GetMessageSender<TMessage>(MessagingEntityType entityType)
            where TMessage : class
        {
            var entityPath = GetEntityPath<TMessage>(entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType)));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (MessageSenders.TryGetValue(entityPath, out var sender))
                {
                    return sender;
                }

                try
                {
                    sender = CreateMessageSender<TMessage>(entityType, Connection);
                }
                catch (Exception exception)
                {
                    throw new MessagePublishingException(typeof(TMessage), exception);
                }

                MessageSenders.Add(entityPath, sender);
                return sender;
            }
        }

        /// <summary>
        /// Creates a new implementation-specific client that facilitates receive operations.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <returns>
        /// A new implementation-specific client that facilitates receive operations.
        /// </returns>
        protected abstract TReceiver CreateMessageReceiver<TMessage>(MessagingEntityType entityType, TConnection connection)
            where TMessage : class;

        /// <summary>
        /// Creates a new implementation-specific client that facilitates send operations.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <returns>
        /// A new implementation-specific client that facilitates send operations.
        /// </returns>
        protected abstract TSender CreateMessageSender<TMessage>(MessagingEntityType entityType, TConnection connection)
            where TMessage : class;

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessagingClientFactory{TSender, TReceiver, TConnection, TWrappedMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Returns a queue entity path for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <returns>
        /// A queue entity path for the specified message type.
        /// </returns>
        protected virtual String GetQueuePath<TMessage>()
             where TMessage : class => $"Queue-{typeof(TMessage).Name}";

        /// <summary>
        /// Returns a subscription name for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <returns>
        /// A subscription name for the specified message type.
        /// </returns>
        protected virtual String GetSubscriptionName<TMessage>()
             where TMessage : class => $"Subscription-{EnhancedReadabilityGuid.New().ToString()}";

        /// <summary>
        /// Returns a topic entity path for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <returns>
        /// A topic entity path for the specified message type.
        /// </returns>
        protected virtual String GetTopicPath<TMessage>()
             where TMessage : class => $"Topic-{typeof(TMessage).Name}";

        /// <summary>
        /// Returns an entity path for the specified entity type and message type combination.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <returns>
        /// An entity path for the specified entity type and message type combination.
        /// </returns>
        [DebuggerHidden]
        private String GetEntityPath<TMessage>(MessagingEntityType entityType)
            where TMessage : class
        {
            switch (entityType)
            {
                case MessagingEntityType.Queue:

                    return GetQueuePath<TMessage>();

                case MessagingEntityType.Topic:

                    return GetTopicPath<TMessage>();

                default:

                    throw new InvalidOperationException($"The specified entity type, {entityType}, is not supported.");
            }
        }

        /// <summary>
        /// Gets a collection of message receivers that are keyed by entity path.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary<String, TReceiver> MessageReceivers => LazyMessageReceivers.Value;

        /// <summary>
        /// Gets a collection of message senders that are keyed by entity path.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary<String, TSender> MessageSenders => LazyMessageSenders.Value;

        /// <summary>
        /// Represents a connection that governs interaction with messaging entities.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TConnection Connection;

        /// <summary>
        /// Represents a lazily-initialized collection of message receivers that are keyed by entity path.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDictionary<String, TReceiver>> LazyMessageReceivers = new Lazy<IDictionary<String, TReceiver>>(() => new Dictionary<String, TReceiver>(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Represents a lazily-initialized collection of message senders that are keyed by entity path.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDictionary<String, TSender>> LazyMessageSenders = new Lazy<IDictionary<String, TSender>>(() => new Dictionary<String, TSender>(), LazyThreadSafetyMode.ExecutionAndPublication);
    }
}