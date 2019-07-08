﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
        /// Gets a shared, managed, implementation-specific message receiver for a type-defined queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TReceiver GetQueueReceiver<TMessage>()
            where TMessage : class => GetQueueReceiver<TMessage>(pathTokens: null);

        /// <summary>
        /// Gets a shared, managed, implementation-specific message receiver for a type-defined queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TReceiver GetQueueReceiver<TMessage>(IEnumerable<String> pathTokens)
            where TMessage : class => GetMessageReceiver<TMessage>(MessagingEntityType.Queue, null, pathTokens);

        /// <summary>
        /// Gets a shared, managed, implementation-specific message sender for a type-defined queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TSender GetQueueSender<TMessage>()
            where TMessage : class => GetQueueSender<TMessage>(pathTokens: null);

        /// <summary>
        /// Gets a shared, managed, implementation-specific message sender for a type-defined queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TSender GetQueueSender<TMessage>(IEnumerable<String> pathTokens)
            where TMessage : class => GetMessageSender<TMessage>(MessagingEntityType.Queue, pathTokens);

        /// <summary>
        /// Gets a shared, managed, implementation-specific message receiver for a type-defined topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="receiverIdentifier">
        /// A unique textual identifier for the message receiver, which is appended to the path.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="receiverIdentifier" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="receiverIdentifier" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TReceiver GetTopicReceiver<TMessage>(String receiverIdentifier)
            where TMessage : class => GetTopicReceiver<TMessage>(receiverIdentifier, pathTokens: null);

        /// <summary>
        /// Gets a shared, managed, implementation-specific message receiver for a type-defined topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="receiverIdentifier">
        /// A unique textual identifier for the message receiver, which is appended to the path.
        /// </param>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="receiverIdentifier" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="receiverIdentifier" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TReceiver GetTopicReceiver<TMessage>(String receiverIdentifier, IEnumerable<String> pathTokens)
            where TMessage : class => GetMessageReceiver<TMessage>(MessagingEntityType.Topic, receiverIdentifier.RejectIf().IsNullOrEmpty(nameof(receiverIdentifier)), pathTokens);

        /// <summary>
        /// Gets a shared, managed, implementation-specific message sender for a type-defined topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TSender GetTopicSender<TMessage>()
            where TMessage : class => GetTopicSender<TMessage>(pathTokens: null);

        /// <summary>
        /// Gets a shared, managed, implementation-specific message sender for a type-defined topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TSender GetTopicSender<TMessage>(IEnumerable<String> pathTokens)
            where TMessage : class => GetMessageSender<TMessage>(MessagingEntityType.Topic, pathTokens);

        /// <summary>
        /// Creates a new implementation-specific client that facilitates receive operations.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <param name="entityPath">
        /// The unique path for the entity.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// A new implementation-specific client that facilitates receive operations.
        /// </returns>
        protected abstract TReceiver CreateMessageReceiver<TMessage>(TConnection connection, MessagingEntityType entityType, String entityPath, String subscriptionName)
            where TMessage : class;

        /// <summary>
        /// Creates a new implementation-specific client that facilitates send operations.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <param name="entityPath">
        /// The unique path for the entity.
        /// </param>
        /// <returns>
        /// A new implementation-specific client that facilitates send operations.
        /// </returns>
        protected abstract TSender CreateMessageSender<TMessage>(TConnection connection, MessagingEntityType entityType, String entityPath)
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
        /// Returns an entity path for the specified entity type and message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// An entity path for the specified entity type and message type combination.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        [DebuggerHidden]
        private String GetEntityPath<TMessage>(MessagingEntityType entityType, IEnumerable<String> pathTokens)
            where TMessage : class
        {
            switch (entityType)
            {
                case MessagingEntityType.Queue:

                    return GetQueuePath<TMessage>(pathTokens);

                case MessagingEntityType.Topic:

                    return GetTopicPath<TMessage>(pathTokens);

                default:

                    throw new UnsupportedSpecificationException($"The specified entity type, {entityType}, is not supported.");
            }
        }

        /// <summary>
        /// Returns an entity path for the specified message type.
        /// </summary>
        /// <param name="pathPrefix">
        /// A lower-case alphabetic path prefix, or <see langword="null" /> to omit a prefix.
        /// </param>
        /// <param name="messageType">
        /// The type of the message.
        /// </param>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// An entity path for the specified message type.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        [DebuggerHidden]
        private String GetEntityPath(String pathPrefix, Type messageType, IEnumerable<String> pathTokens)
        {
            var messageTypeName = messageType.Name.ToLower();
            var processedPathPrefix = pathPrefix.IsNullOrEmpty() ? String.Empty : $"{pathPrefix}{EntityPathDelimitingCharacter}";
            var processedMessageTypeName = messageTypeName.EndsWith(TrimmedMessageTypeNamePostfix) ? messageTypeName.Substring(0, (messageTypeName.Length - TrimmedMessageTypeNamePostfix.Length)) : messageTypeName;
            var rootPath = $"{processedPathPrefix}{processedMessageTypeName}";

            if (pathTokens.IsNullOrEmpty())
            {
                return rootPath;
            }

            var pathBuilder = new StringBuilder(rootPath);

            foreach (var pathToken in pathTokens)
            {
                if (pathToken.IsNullOrEmpty())
                {
                    throw new ArgumentException("One of the specified tokens is null or empty.", nameof(pathTokens));
                }
                else if (pathToken.MatchesRegularExpression(FullyAlphanumericRegularExpression) == false)
                {
                    throw new ArgumentException("One of the specified tokens contains non-alphanumeric characters.", nameof(pathTokens));
                }

                pathBuilder.Append($"{EntityPathDelimitingCharacter}{pathToken.ToLower()}");
            }

            return pathBuilder.ToString();
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
        /// <param name="receiverIdentifier">
        /// A unique textual identifier for the message receiver, or <see langword="null" /> if the receiver is unspecified.
        /// </param>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private TReceiver GetMessageReceiver<TMessage>(MessagingEntityType entityType, String receiverIdentifier, IEnumerable<String> pathTokens)
            where TMessage : class
        {
            var entityPath = GetEntityPath<TMessage>(entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType)), pathTokens);

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (MessageReceivers.TryGetValue(entityPath, out var receiver))
                {
                    return receiver;
                }

                try
                {
                    var processedSubscriptionNamePrefix = SubscriptionNamePrefix.IsNullOrEmpty() ? String.Empty : $"{SubscriptionNamePrefix}{EntityPathDelimitingCharacter}";
                    var subscriptionName = receiverIdentifier.IsNullOrEmpty() ? null : $"{processedSubscriptionNamePrefix}{entityPath}{EntityPathDelimitingCharacter}{receiverIdentifier}";
                    receiver = CreateMessageReceiver<TMessage>(Connection, entityType, entityPath, subscriptionName);
                }
                catch (Exception exception)
                {
                    throw new MessageSubscribingException(typeof(TMessage), exception);
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
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private TSender GetMessageSender<TMessage>(MessagingEntityType entityType, IEnumerable<String> pathTokens)
            where TMessage : class
        {
            var entityPath = GetEntityPath<TMessage>(entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType)), pathTokens);

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (MessageSenders.TryGetValue(entityPath, out var sender))
                {
                    return sender;
                }

                try
                {
                    sender = CreateMessageSender<TMessage>(Connection, entityType, entityPath);
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
        /// Returns a queue entity path for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A queue entity path for the specified message type.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        [DebuggerHidden]
        private String GetQueuePath<TMessage>(IEnumerable<String> pathTokens)
             where TMessage : class => GetEntityPath(EntityPathQueuePrefix, typeof(TMessage), pathTokens);

        /// <summary>
        /// Returns a topic entity path for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A topic entity path for the specified message type.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        [DebuggerHidden]
        private String GetTopicPath<TMessage>(IEnumerable<String> pathTokens)
             where TMessage : class => GetEntityPath(EntityPathTopicPrefix, typeof(TMessage), pathTokens);

        /// <summary>
        /// Gets a character that is used to separate tokens within an entity path.
        /// </summary>
        protected virtual Char EntityPathDelimitingCharacter => '-';

        /// <summary>
        /// Gets an entity path prefix for queues.
        /// </summary>
        protected virtual String EntityPathQueuePrefix => null;

        /// <summary>
        /// Gets an entity path prefix for topics.
        /// </summary>
        protected virtual String EntityPathTopicPrefix => null;

        /// <summary>
        /// Gets a name prefix for subscriptions.
        /// </summary>
        protected virtual String SubscriptionNamePrefix => null;

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
        /// Represents a regular expression that matches fully alphanumeric strings.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String FullyAlphanumericRegularExpression = "^[a-zA-Z0-9]*$";

        /// <summary>
        /// Represents a postfix for message type names which is trimmed when constructing entity paths.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String TrimmedMessageTypeNamePostfix = "message";

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