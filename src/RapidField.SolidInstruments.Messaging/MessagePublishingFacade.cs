// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates implementation-specific publishing operations for a message bus.
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
    /// <see cref="MessagePublishingFacade{TSender, TReceiver, TAdaptedMessage}" /> is the default implementation of
    /// <see cref="IMessagePublishingFacade{TSender, TReceiver, TAdaptedMessage}" />.
    /// </remarks>
    public abstract class MessagePublishingFacade<TSender, TReceiver, TAdaptedMessage> : MessagingFacade<TSender, TReceiver, TAdaptedMessage>, IMessagePublishingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublishingFacade{TSender, TReceiver, TAdaptedMessage}" /> class.
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
        protected MessagePublishingFacade(IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage> clientFactory, IMessageAdapter<TAdaptedMessage> messageAdapter)
            : base(clientFactory, messageAdapter)
        {
            return;
        }

        /// <summary>
        /// Asynchronously publishes the specified message to a queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to publish.
        /// </typeparam>
        /// <param name="message">
        /// The message to publish.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while attempting to publish <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task PublishToQueueAsync<TMessage>(TMessage message)
            where TMessage : class, IMessageBase => PublishToQueueAsync(message, null);

        /// <summary>
        /// Asynchronously publishes the specified message to a queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to publish.
        /// </typeparam>
        /// <param name="message">
        /// The message to publish.
        /// </param>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while attempting to publish <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task PublishToQueueAsync<TMessage>(TMessage message, IEnumerable<String> pathTokens)
            where TMessage : class, IMessageBase => PublishAsync(message, pathTokens, MessagingEntityType.Queue);

        /// <summary>
        /// Asynchronously publishes the specified message to a topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to publish.
        /// </typeparam>
        /// <param name="message">
        /// The message to publish.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while attempting to publish <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task PublishToTopicAsync<TMessage>(TMessage message)
            where TMessage : class, IMessageBase => PublishToTopicAsync(message, null);

        /// <summary>
        /// Asynchronously publishes the specified message to a topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to publish.
        /// </typeparam>
        /// <param name="message">
        /// The message to publish.
        /// </param>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while attempting to publish <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task PublishToTopicAsync<TMessage>(TMessage message, IEnumerable<String> pathTokens)
            where TMessage : class, IMessageBase => PublishAsync(message, pathTokens, MessagingEntityType.Topic);

        /// <summary>
        /// Asynchronously publishes the specified message to a bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to publish.
        /// </typeparam>
        /// <param name="message">
        /// The message to publish.
        /// </param>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while attempting to publish <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        internal Task PublishAsync<TMessage>(TMessage message, IEnumerable<String> pathTokens, MessagingEntityType entityType)
            where TMessage : class, IMessageBase
        {
            message = message.RejectIf().IsNull(nameof(message)).TargetArgument;
            entityType = entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                var sendClient = default(TSender);

                switch (entityType)
                {
                    case MessagingEntityType.Queue:

                        sendClient = ClientFactory.GetQueueSender<TMessage>(pathTokens);
                        break;

                    case MessagingEntityType.Topic:

                        sendClient = ClientFactory.GetTopicSender<TMessage>(pathTokens);
                        break;

                    default:

                        throw new UnsupportedSpecificationException($"The specified messaging entity type, {entityType}, is not supported.");
                }

                try
                {
                    var adaptedMessage = MessageAdapter.ConvertForward(message) as TAdaptedMessage;
                    return PublishAsync(adaptedMessage, sendClient, controlToken);
                }
                catch (MessagePublishingException)
                {
                    throw;
                }
                catch (ObjectDisposedException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new MessagePublishingException(typeof(TMessage), exception);
                }
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessagePublishingFacade{TSender, TReceiver, TAdaptedMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Asynchronously publishes the specified message to a bus.
        /// </summary>
        /// <param name="message">
        /// The message to publish.
        /// </param>
        /// <param name="sendClient">
        /// An implementation-specific receive client.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task PublishAsync(TAdaptedMessage message, TSender sendClient, ConcurrencyControlToken controlToken);
    }
}