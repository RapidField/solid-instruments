// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates publishing operations for a message bus.
    /// </summary>
    /// <remarks>
    /// <see cref="MessagePublishingClient" /> is the default implementation of <see cref="IMessagePublishingClient" />.
    /// </remarks>
    public abstract class MessagePublishingClient : MessagingClient, IMessagePublishingClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublishingClient" /> class.
        /// </summary>
        protected MessagePublishingClient()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublishingClient" /> class.
        /// </summary>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        protected MessagePublishingClient(SerializationFormat messageSerializationFormat)
            : base(messageSerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublishingClient" /> class.
        /// </summary>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.ProcessorCountSemaphore" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" /> -or-
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" />.
        /// </exception>
        protected MessagePublishingClient(SerializationFormat messageSerializationFormat, ConcurrencyControlMode stateControlMode)
            : base(messageSerializationFormat, stateControlMode)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublishingClient" /> class.
        /// </summary>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.ProcessorCountSemaphore" />.
        /// </param>
        /// <param name="operationTimeoutThreshold">
        /// The maximum length of time for which an operation may block a thread before raising an exception, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking is permitted. The default value is
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" /> -or-
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" /> -or-
        /// <paramref name="operationTimeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> and is not equal to
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </exception>
        protected MessagePublishingClient(SerializationFormat messageSerializationFormat, ConcurrencyControlMode stateControlMode, TimeSpan operationTimeoutThreshold)
            : base(messageSerializationFormat, stateControlMode, operationTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Asynchronously publishes the specified message to a bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to publish.
        /// </typeparam>
        /// <param name="message">
        /// The message to publish.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
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
        public async Task PublishAsync<TMessage>(TMessage message, MessagingEntityType entityType)
            where TMessage : class, IMessage
        {
            message = message.RejectIf().IsNull(nameof(message)).TargetArgument;
            entityType = entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType));

            try
            {
                using (var controlToken = StateControl.Enter())
                {
                    RejectIfDisposed();
                    await PublishAsync(message, entityType, controlToken).ConfigureAwait(false);
                }
            }
            catch (MessagePublishingException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessagePublishingException(typeof(TMessage), exception);
            }
        }

        /// <summary>
        /// Asynchronously publishes the specified request message to a bus and waits for the correlated response message.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessage">
        /// The request message to publish.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the correlated response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessage" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while attempting to publish <paramref name="requestMessage" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public async Task<TResponseMessage> RequestAsync<TRequestMessage, TResponseMessage>(TRequestMessage requestMessage)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
        {
            requestMessage = requestMessage.RejectIf().IsNull(nameof(requestMessage)).TargetArgument;

            try
            {
                using (var controlToken = StateControl.Enter())
                {
                    RejectIfDisposed();
                    return await RequestAsync<TRequestMessage, TResponseMessage>(requestMessage, controlToken).ConfigureAwait(false);
                }
            }
            catch (MessagePublishingException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessagePublishingException(typeof(TRequestMessage), exception);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessagePublishingClient" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Asynchronously publishes the specified message to a bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to publish.
        /// </typeparam>
        /// <param name="message">
        /// The message to publish.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task PublishAsync<TMessage>(TMessage message, MessagingEntityType entityType, ConcurrencyControlToken controlToken)
            where TMessage : class, IMessage;

        /// <summary>
        /// Asynchronously publishes the specified request message to a bus and waits for the correlated response message.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessage">
        /// The request message to publish.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the correlated response message.
        /// </returns>
        protected abstract Task<TResponseMessage> RequestAsync<TRequestMessage, TResponseMessage>(TRequestMessage requestMessage, ConcurrencyControlToken controlToken)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage;

        /// <summary>
        /// Represents the entity type that is used for publishing and subscribing to request messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const MessagingEntityType RequestMessageEntityType = MessagingEntityType.Queue;
    }
}