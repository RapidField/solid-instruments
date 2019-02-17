// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates implementation-specific subscription operations for a message bus.
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
    /// <typeparam name="TPublishingFacade">
    /// The type of the implementation-specific messaging facade that is used to publish response messages.
    /// </typeparam>
    /// <remarks>
    /// <see cref="MessageSubscriptionFacade{TSender, TReceiver, TAdaptedMessage, TPublishingFacade}" /> is the default
    /// implementation of <see cref="IMessageSubscriptionFacade{TSender, TReceiver, TAdaptedMessage, TPublishingFacade}" />.
    /// </remarks>
    public abstract class MessageSubscriptionFacade<TSender, TReceiver, TAdaptedMessage, TPublishingFacade> : MessageSubscriptionFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
        where TPublishingFacade : MessagePublishingFacade<TSender, TReceiver, TAdaptedMessage>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MessageSubscriptionFacade{TSender, TReceiver, TAdaptedMessage, TPublishingFacade}" /> class.
        /// </summary>
        /// <param name="publishingFacade">
        /// An implementation-specific messaging facade that is used to publish response messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="publishingFacade" /> is <see langword="null" />.
        /// </exception>
        protected MessageSubscriptionFacade(TPublishingFacade publishingFacade)
            : base(publishingFacade.RejectIf().IsNull(nameof(publishingFacade)).TargetArgument.ClientFactory, publishingFacade.MessageAdapter)
        {
            PublishingFacade = publishingFacade;
        }

        /// <summary>
        /// Registers the specified message handler with the bus.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessageHandler">
        /// A function that handles a request message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscriptionException">
        /// An exception was raised while attempting to register <paramref name="requestMessageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public sealed override void RegisterHandler<TRequestMessage, TResponseMessage>(Func<TRequestMessage, TResponseMessage> requestMessageHandler)
        {
            requestMessageHandler = requestMessageHandler.RejectIf().IsNull(nameof(requestMessageHandler)).TargetArgument;

            try
            {
                using (var controlToken = StateControl.Enter())
                {
                    RejectIfDisposed();

                    if (TryAddSubscribedMessageType<TRequestMessage>(controlToken) == false)
                    {
                        // Disallow registration of duplicate request handlers.
                        return;
                    }

                    var requestReceiveClient = ClientFactory.GetMessageReceiver<TRequestMessage>(Message.RequestEntityType);
                    var messageHandler = new Action<TAdaptedMessage>((adaptedRequestMessage) =>
                    {
                        RejectIfDisposed();
                        var requestMessage = MessageAdapter.ConvertReverse<TRequestMessage>(adaptedRequestMessage);
                        var responseMessage = requestMessageHandler(requestMessage);
                        PublishingFacade.PublishAsync(responseMessage, Message.ResponseEntityType).Wait();
                    });

                    RegisterHandler(messageHandler, requestReceiveClient, controlToken);
                }
            }
            catch (MessageSubscriptionException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessageSubscriptionException(typeof(TRequestMessage), exception);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessageSubscriptionFacade{TSender, TReceiver, TAdaptedMessage, TPublishingFacade}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents an implementation-specific messaging facade that is used to publish response messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly TPublishingFacade PublishingFacade;
    }

    /// <summary>
    /// Facilitates implementation-specific subscription operations for a message bus.
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
    /// <remarks>
    /// <see cref="MessageSubscriptionFacade{TSender, TReceiver, TAdaptedMessage}" /> is the default implementation of
    /// <see cref="IMessageSubscriptionFacade{TSender, TReceiver, TAdaptedMessage}" />.
    /// </remarks>
    public abstract class MessageSubscriptionFacade<TSender, TReceiver, TAdaptedMessage> : MessagingFacade<TSender, TReceiver, TAdaptedMessage>, IMessageSubscriptionFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscriptionFacade{TSender, TReceiver, TAdaptedMessage}" /> class.
        /// </summary>
        /// <param name="clientFactory">
        /// An appliance that creates manages implementation-specific messaging clients.
        /// </param>
        /// <param name="messageAdapter">
        /// An appliance that facilitates implementation-specific message conversion.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="clientFactory" /> is <see langword="null" /> -or- <paramref name="messageAdapter" /> is
        /// <see langword="null" />.
        /// </exception>
        protected MessageSubscriptionFacade(IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage> clientFactory, IMessageAdapter<TAdaptedMessage> messageAdapter)
            : base(clientFactory, messageAdapter)
        {
            return;
        }

        /// <summary>
        /// Registers the specified message handler with the bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessageSubscriptionException">
        /// An exception was raised while attempting to register <paramref name="messageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RegisterHandler<TMessage>(Action<TMessage> messageHandler, MessagingEntityType entityType)
            where TMessage : class, IMessage
        {
            messageHandler = messageHandler.RejectIf().IsNull(nameof(messageHandler)).TargetArgument;
            entityType = entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType)).TargetArgument;

            try
            {
                using (var controlToken = StateControl.Enter())
                {
                    RejectIfDisposed();
                    TryAddSubscribedMessageType<TMessage>(controlToken);

                    if (TryAddSubscribedMessageType<TMessage>(controlToken) == false && ResponseMessageInterfaceType.IsAssignableFrom(typeof(TMessage)))
                    {
                        // Disallow registration of duplicate response handlers.
                        return;
                    }

                    var receiveClient = ClientFactory.GetMessageReceiver<TMessage>(entityType);
                    var adaptedMessageHandler = new Action<TAdaptedMessage>((adaptedMessage) =>
                    {
                        RejectIfDisposed();
                        var message = MessageAdapter.ConvertReverse<TMessage>(adaptedMessage);
                        messageHandler(message);
                    });

                    RegisterHandler(adaptedMessageHandler, receiveClient, controlToken);
                }
            }
            catch (MessageSubscriptionException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessageSubscriptionException(typeof(TMessage), exception);
            }
        }

        /// <summary>
        /// Registers the specified message handler with the bus.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessageHandler">
        /// A function that handles a request message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscriptionException">
        /// An exception was raised while attempting to register <paramref name="requestMessageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public abstract void RegisterHandler<TRequestMessage, TResponseMessage>(Func<TRequestMessage, TResponseMessage> requestMessageHandler)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage;

        /// <summary>
        /// Attempts to add the specified message type to the collection of subscribed message types.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the subscribed message.
        /// </typeparam>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified message type was added; <see langword="false" /> if it was already present.
        /// </returns>
        [DebuggerHidden]
        internal Boolean TryAddSubscribedMessageType<TMessage>(ConcurrencyControlToken controlToken)
            where TMessage : IMessageBase
        {
            var messageType = typeof(TMessage);

            if (SubscribedMessageTypeList.Contains(messageType))
            {
                return false;
            }

            SubscribedMessageTypeList.Add(messageType);
            return true;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessageSubscriptionFacade{TSender, TReceiver, TAdaptedMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Registers the specified message handler with the bus.
        /// </summary>
        /// <param name="adaptedMessageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="receiveClient">
        /// An implementation-specific receive client.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected abstract void RegisterHandler(Action<TAdaptedMessage> adaptedMessageHandler, TReceiver receiveClient, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Gets the collection of message types for which the current
        /// <see cref="MessageSubscriptionFacade{TSender, TReceiver, TAdaptedMessage}" /> has one or more registered handlers.
        /// </summary>
        public IEnumerable<Type> SubscribedMessageTypes => SubscribedMessageTypeList.ToArray();

        /// <summary>
        /// Represents the <see cref="IResponseMessage" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ResponseMessageInterfaceType = typeof(IResponseMessage);

        /// <summary>
        /// Represents the collection of message types for which the current
        /// <see cref="MessageSubscriptionFacade{TSender, TReceiver, TAdaptedMessage}" /> has one or more registered handlers.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<Type> SubscribedMessageTypeList = new List<Type>();
    }
}