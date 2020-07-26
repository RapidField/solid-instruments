// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Manages the listener types that are supported by an
    /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageListeningProfile" /> is the default implementation of <see cref="IMessageListeningProfile" />.
    /// </remarks>
    public sealed class MessageListeningProfile : IMessageListeningProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListeningProfile" /> class.
        /// </summary>
        /// <param name="rootDependencyScope">
        /// A dependency scope that spans the full lifetime of execution for the associated
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rootDependencyScope" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageListeningProfile(IDependencyScope rootDependencyScope)
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
        public void AddQueueListener<TMessage>()
            where TMessage : class, IMessage => AddListener<TMessage>(MessagingEntityType.Queue);

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
        public void AddRequestListener<TRequestMessage, TResponseMessage>()
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
        {
            var requestMessageType = typeof(TRequestMessage);

            if (SupportedMessageTypesReference.Contains(requestMessageType))
            {
                throw new InvalidOperationException($"Support for the type {requestMessageType.FullName} was already added.");
            }

            SupportedMessageTypesReference.Add(requestMessageType);
            RootDependencyScope.Resolve<IMessageListeningFacade>().RegisterRequestMessageHandler<TRequestMessage, TResponseMessage>((requestMessage) => HandleRequestMessage<TRequestMessage, TResponseMessage>(requestMessage));
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
        public void AddTopicListener<TMessage>()
            where TMessage : class, IMessage => AddListener<TMessage>(MessagingEntityType.Topic);

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
        [DebuggerHidden]
        private void AddListener<TMessage>(MessagingEntityType entityType)
            where TMessage : class, IMessage
        {
            var messageType = typeof(TMessage);

            if (SupportedMessageTypesReference.Contains(messageType))
            {
                throw new InvalidOperationException($"Support for the type {messageType.FullName} was already added.");
            }

            SupportedMessageTypesReference.Add(messageType);

            switch (entityType)
            {
                case MessagingEntityType.Queue:

                    RootDependencyScope.Resolve<IMessageListeningFacade>().RegisterQueueMessageHandler<TMessage>((message) => HandleMessage(message));
                    break;

                case MessagingEntityType.Topic:

                    RootDependencyScope.Resolve<IMessageListeningFacade>().RegisterTopicMessageHandler<TMessage>((message) => HandleMessage(message));
                    break;

                default:

                    throw new UnsupportedSpecificationException($"The specified entity type, {entityType}, is not supported.");
            }
        }

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
                    using (var listener = dependencyScope.Resolve<IMessageListener<TMessage>>())
                    {
                        listener.Process(message);
                    }
                }
            }
            catch (MessageListeningException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessageListeningException(typeof(TMessage), exception);
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
                    using (var listener = dependencyScope.Resolve<IMessageListener<TRequestMessage, TResponseMessage>>())
                    {
                        return listener.Process(requestMessage);
                    }
                }
            }
            catch (MessageListeningException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessageListeningException(typeof(TRequestMessage), exception);
            }
        }

        /// <summary>
        /// Gets a collection of message types that are supported by the associated
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        public IEnumerable<Type> SupportedMessageTypes
        {
            get
            {
                foreach (var supportedMessageType in SupportedMessageTypesReference)
                {
                    yield return supportedMessageType;
                }
            }
        }

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