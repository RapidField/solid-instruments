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
    /// Facilitates implementation-specific transmission operations for a message bus.
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
    /// <see cref="MessageTransmittingFacade{TSender, TReceiver, TAdaptedMessage}" /> is the default implementation of
    /// <see cref="IMessageTransmittingFacade{TSender, TReceiver, TAdaptedMessage}" />.
    /// </remarks>
    public abstract class MessageTransmittingFacade<TSender, TReceiver, TAdaptedMessage> : MessagingFacade<TSender, TReceiver, TAdaptedMessage>, IMessageTransmittingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransmittingFacade{TSender, TReceiver, TAdaptedMessage}" /> class.
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
        protected MessageTransmittingFacade(IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage> clientFactory, IMessageAdapter<TAdaptedMessage> messageAdapter)
            : base(clientFactory, messageAdapter)
        {
            return;
        }

        /// <summary>
        /// Asynchronously transmits the specified message to a queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <param name="message">
        /// The message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task TransmitToQueueAsync<TMessage>(TMessage message)
            where TMessage : class, IMessageBase => TransmitToQueueAsync(message, null);

        /// <summary>
        /// Asynchronously transmits the specified message to a queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <param name="message">
        /// The message to transmit.
        /// </param>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task TransmitToQueueAsync<TMessage>(TMessage message, IEnumerable<String> pathLabels)
            where TMessage : class, IMessageBase => TransmitAsync(message, pathLabels, MessagingEntityType.Queue);

        /// <summary>
        /// Asynchronously transmits the specified message to a topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <param name="message">
        /// The message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task TransmitToTopicAsync<TMessage>(TMessage message)
            where TMessage : class, IMessageBase => TransmitToTopicAsync(message, null);

        /// <summary>
        /// Asynchronously transmits the specified message to a topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <param name="message">
        /// The message to transmit.
        /// </param>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task TransmitToTopicAsync<TMessage>(TMessage message, IEnumerable<String> pathLabels)
            where TMessage : class, IMessageBase => TransmitAsync(message, pathLabels, MessagingEntityType.Topic);

        /// <summary>
        /// Asynchronously transmits the specified message to a bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <param name="message">
        /// The message to transmit.
        /// </param>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        internal Task TransmitAsync<TMessage>(TMessage message, IEnumerable<String> pathLabels, MessagingEntityType entityType)
            where TMessage : class, IMessageBase
        {
            message = message.RejectIf().IsNull(nameof(message)).TargetArgument;
            entityType = entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                var sendClient = default(TSender);

#pragma warning disable PH_S032

                sendClient = entityType switch
                {
                    MessagingEntityType.Queue => ClientFactory.GetQueueSender<TMessage>(pathLabels),
                    MessagingEntityType.Topic => ClientFactory.GetTopicSender<TMessage>(pathLabels),
                    _ => throw new UnsupportedSpecificationException($"The specified messaging entity type, {entityType}, is not supported.")
                };

#pragma warning restore PH_S032

                try
                {
                    var adaptedMessage = MessageAdapter.ConvertForward(message) as TAdaptedMessage;
                    return TransmitAsync(adaptedMessage, sendClient, controlToken);
                }
                catch (MessageTransmissionException exception)
                {
                    return Task.FromException<TMessage>(exception);
                }
                catch (ObjectDisposedException exception)
                {
                    return Task.FromException<TMessage>(exception);
                }
                catch (Exception exception)
                {
                    return Task.FromException<TMessage>(new MessageTransmissionException(typeof(TMessage), exception));
                }
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessageTransmittingFacade{TSender, TReceiver, TAdaptedMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Asynchronously transmits the specified message to a bus.
        /// </summary>
        /// <param name="message">
        /// The message to transmit.
        /// </param>
        /// <param name="sendClient">
        /// An implementation-specific receive client.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task TransmitAsync(TAdaptedMessage message, TSender sendClient, IConcurrencyControlToken controlToken);
    }
}