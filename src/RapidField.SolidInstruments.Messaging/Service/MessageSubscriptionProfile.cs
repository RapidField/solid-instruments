// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Manages the subscriber types that are supported by an
    /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageSubscriptionProfile" /> is the default implementation of <see cref="IMessageSubscriptionProfile" />.
    /// </remarks>
    public sealed class MessageSubscriptionProfile : IMessageSubscriptionProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscriptionProfile" /> class.
        /// </summary>
        /// <param name="rootDependencyScope">
        /// A dependency scope that spans the full lifetime of execution for the associated
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rootDependencyScope" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageSubscriptionProfile(IDependencyScope rootDependencyScope)
            : base()
        {
            RootDependencyScope = rootDependencyScope.RejectIf().IsNull(nameof(rootDependencyScope)).TargetArgument;
        }

        /// <summary>
        /// Adds support for the specified queue message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message for which support is added.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="TMessage" /> was already added.
        /// </exception>
        public void AddQueueSubscriber<TMessage>()
            where TMessage : class, IMessage => AddSubscriber<TMessage>(MessagingEntityType.Queue);

        /// <summary>
        /// Adds support for the specified request message type.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message for which support is added.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message that is associated with <typeparamref name="TRequestMessage" />.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="TRequestMessage" /> was already added.
        /// </exception>
        public void AddRequestSubscriber<TRequestMessage, TResponseMessage>()
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
        {
            var requestMessageType = typeof(TRequestMessage);

            if (SupportedMessageTypesReference.Contains(requestMessageType))
            {
                throw new InvalidOperationException($"Support for the type {requestMessageType.FullName} was already added.");
            }

            SupportedMessageTypesReference.Add(requestMessageType);
            RootDependencyScope.Resolve<IMessageSubscriptionClient>().RegisterHandler<TRequestMessage, TResponseMessage>((requestMessage) => HandleRequestMessage<TRequestMessage, TResponseMessage>(requestMessage));
        }

        /// <summary>
        /// Adds support for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message for which support is added.
        /// </typeparam>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="TMessage" /> was already added.
        /// </exception>
        public void AddSubscriber<TMessage>(MessagingEntityType entityType)
            where TMessage : class, IMessage
        {
            var messageType = typeof(TMessage);

            if (SupportedMessageTypesReference.Contains(messageType))
            {
                throw new InvalidOperationException($"Support for the type {messageType.FullName} was already added.");
            }

            SupportedMessageTypesReference.Add(messageType);
            RootDependencyScope.Resolve<IMessageSubscriptionClient>().RegisterHandler<TMessage>((message) => HandleMessage(message), entityType);
        }

        /// <summary>
        /// Adds support for the specified topic message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message for which support is added.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="TMessage" /> was already added.
        /// </exception>
        public void AddTopicSubscriber<TMessage>()
            where TMessage : class, IMessage => AddSubscriber<TMessage>(MessagingEntityType.Topic);

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="message">
        /// The message that is handled.
        /// </param>
        [DebuggerHidden]
        private void HandleMessage<TMessage>(TMessage message)
            where TMessage : class, IMessage
        {
            try
            {
                using (var dependencyScope = RootDependencyScope.CreateChildScope())
                {
                    using (var subscriber = dependencyScope.Resolve<IMessageSubscriber<TMessage>>())
                    {
                        subscriber.Process(message);
                    }
                }
            }
            catch (MessageSubscriptionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessageSubscriptionException(typeof(TMessage), exception);
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <param name="requestMessage">
        /// The request message that is handled.
        /// </param>
        [DebuggerHidden]
        private TResponseMessage HandleRequestMessage<TRequestMessage, TResponseMessage>(TRequestMessage requestMessage)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
        {
            try
            {
                using (var dependencyScope = RootDependencyScope.CreateChildScope())
                {
                    using (var subscriber = dependencyScope.Resolve<IMessageSubscriber<TRequestMessage, TResponseMessage>>())
                    {
                        return subscriber.Process(requestMessage);
                    }
                }
            }
            catch (MessageSubscriptionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessageSubscriptionException(typeof(TRequestMessage), exception);
            }
        }

        /// <summary>
        /// Gets a collection of message types that are supported by the associated
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        public IEnumerable<Type> SupportedMessageTypes => SupportedMessageTypesReference;

        /// <summary>
        /// Represents a dependency scope that spans the full lifetime of execution for the associated
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDependencyScope RootDependencyScope;

        /// <summary>
        /// Represents a collection of message types that are supported by the associated
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<Type> SupportedMessageTypesReference = new List<Type>();
    }
}