// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Serialization;
using RapidField.SolidInstruments.Serialization.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AzureServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace RapidField.SolidInstruments.Messaging.AzureServiceBus
{
    /// <summary>
    /// Manages creation and resolution for a shared pool of Azure Service Bus send and receive clients.
    /// </summary>
    internal sealed class AzureServiceBusClientManager : Instrument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusPublishingClient" />.
        /// </summary>
        /// <param name="connection">
        /// A connection that governs interaction with Azure Service Bus entities.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal AzureServiceBusClientManager(ServiceBusConnection connection)
            : base()
        {
            ClientFactory = new AzureServiceBusClientFactory(connection);
        }

        /// <summary>
        /// Gets a shared, managed <see cref="IReceiverClient" /> instance.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <returns>
        /// The managed <see cref="IReceiverClient" /> instance.
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
        public IReceiverClient GetReceiveClient<TMessage>(MessagingEntityType entityType)
            where TMessage : class
        {
            var entityPath = GetEntityPath<TMessage>(entityType);

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (ReceiveClientDictionary.TryGetValue(entityPath, out var client))
                {
                    return client;
                }

                client = ClientFactory.CreateReceiveClient<TMessage>(entityType);
                ReceiveClientDictionary.Add(entityPath, client);
                return client;
            }
        }

        /// <summary>
        /// Gets a shared, managed <see cref="ISenderClient" /> instance that is used to submit request messages.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <returns>
        /// The managed <see cref="ISenderClient" /> instance.
        /// </returns>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while creating the send client.
        /// </exception>
        /// <exception cref="MessageSubscriptionException">
        /// An exception was raised while creating the receive client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public ISenderClient GetRequestClient<TRequestMessage, TResponseMessage>()
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
        {
            var requestMessageEntityType = MessagePublishingClient.RequestMessageEntityType;
            var responseMessageEntityType = MessageSubscriptionClient.ResponseMessageEntityType;
            var responseMessageEntityPath = GetEntityPath<TResponseMessage>(responseMessageEntityType);
            var requestMessageSendClient = GetSendClient<TRequestMessage>(requestMessageEntityType);

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (SendClientDictionary.ContainsKey(responseMessageEntityPath) == false)
                {
                    var responseMessageSendClient = ClientFactory.CreateSendClient<TResponseMessage>(responseMessageEntityType);
                    SendClientDictionary.Add(responseMessageEntityPath, responseMessageSendClient);
                }

                if (ReceiveClientDictionary.ContainsKey(responseMessageEntityPath) == false)
                {
                    var responseMessageReceiveClient = ClientFactory.CreateReceiveClient<TResponseMessage>(responseMessageEntityType);
                    responseMessageReceiveClient.RegisterMessageHandler(HandleResponseMessage, ReceiverOptionsForResponseMessages);
                    ReceiveClientDictionary.Add(responseMessageEntityPath, responseMessageReceiveClient);
                }
            }

            return requestMessageSendClient;
        }

        /// <summary>
        /// Gets a shared, managed <see cref="ISenderClient" /> instance.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <returns>
        /// The managed <see cref="ISenderClient" /> instance.
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
        public ISenderClient GetSendClient<TMessage>(MessagingEntityType entityType)
            where TMessage : class
        {
            var entityPath = GetEntityPath<TMessage>(entityType);

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (SendClientDictionary.TryGetValue(entityPath, out var client))
                {
                    return client;
                }

                client = ClientFactory.CreateSendClient<TMessage>(entityType);
                SendClientDictionary.Add(entityPath, client);
                return client;
            }
        }

        /// <summary>
        /// Creates a new Azure Service Bus message.
        /// </summary>
        /// <param name="messageBody">
        /// The serialized message body.
        /// </param>
        /// <param name="messageType">
        /// The type of the message.
        /// </param>
        /// <param name="serializationFormat">
        /// The format that was used to serialized the message.
        /// </param>
        /// <param name="messageIdentifier">
        /// The message identifier.
        /// </param>
        /// <param name="correlationIdentifier">
        /// The message correlation identifier.
        /// </param>
        /// <returns>
        /// The resulting Azure Service Bus message.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="messageBody" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageBody" /> is <see langword="null" /> -or- <paramref name="messageType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="serializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" /> -or-
        /// <paramref name="messageIdentifier" /> is equal to <see cref="Guid.Empty" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        internal static AzureServiceBusMessage CreateAzureServiceBusMessage(Byte[] messageBody, Type messageType, SerializationFormat serializationFormat, Guid messageIdentifier, Guid correlationIdentifier) => CreateAzureServiceBusMessage(messageBody, messageType, serializationFormat, messageIdentifier, correlationIdentifier, null);

        /// <summary>
        /// Creates a new Azure Service Bus message.
        /// </summary>
        /// <param name="messageBody">
        /// The serialized message body.
        /// </param>
        /// <param name="messageType">
        /// The type of the message.
        /// </param>
        /// <param name="serializationFormat">
        /// The format that was used to serialized the message.
        /// </param>
        /// <param name="messageIdentifier">
        /// The message identifier.
        /// </param>
        /// <param name="correlationIdentifier">
        /// The message correlation identifier.
        /// </param>
        /// <param name="requestMessageIdentifier">
        /// The identifier for the associated request message if the specified message is a response message, other wise
        /// <see langword="null" />. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The resulting Azure Service Bus message.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="messageBody" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageBody" /> is <see langword="null" /> -or- <paramref name="messageType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="serializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" /> -or-
        /// <paramref name="messageIdentifier" /> is equal to <see cref="Guid.Empty" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        internal static AzureServiceBusMessage CreateAzureServiceBusMessage(Byte[] messageBody, Type messageType, SerializationFormat serializationFormat, Guid messageIdentifier, Guid correlationIdentifier, Guid? requestMessageIdentifier)
        {
            var azureServiceBusMessage = new AzureServiceBusMessage()
            {
                Body = messageBody.RejectIf().IsNullOrEmpty(nameof(messageBody)),
                ContentType = serializationFormat.RejectIf().IsEqualToValue(SerializationFormat.Unspecified, nameof(serializationFormat)).TargetArgument.ToMimeMediaType(),
                CorrelationId = correlationIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(correlationIdentifier)).TargetArgument.ToSerializedString(),
                MessageId = messageIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(messageIdentifier)).TargetArgument.ToSerializedString()
            };

            azureServiceBusMessage.UserProperties.Add(FullTypeNameUserPropertyKey, messageType.RejectIf().IsNull(nameof(messageType)).TargetArgument.FullName);

            if (requestMessageIdentifier.HasValue)
            {
                azureServiceBusMessage.UserProperties.Add(RequestMessageIdentifierUserPropertyKey, requestMessageIdentifier.Value.ToSerializedString());
            }

            return azureServiceBusMessage;
        }

        /// <summary>
        /// Adds the specified request identifier to a list of outstanding requests.
        /// </summary>
        /// <param name="requestMessageIdentifier">
        /// The identifier for the new request.
        /// </param>
        [DebuggerHidden]
        internal void AddOutstandingRequest(Guid requestMessageIdentifier) => OutstandingResponseKeys.Add(requestMessageIdentifier.ToSerializedString());

        /// <summary>
        /// Waits for a response message matching the specified request to populate the client manager's response dictionary.
        /// </summary>
        /// <param name="requestMessageIdentifier">
        /// An identifier for the associated request message.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the response message.
        /// </returns>
        /// <exception cref="TimeoutException">
        /// The timeout threshold duration was exceeded while waiting for a response.
        /// </exception>
        [DebuggerHidden]
        internal async Task<AzureServiceBusMessage> WaitForResponseAsync(Guid requestMessageIdentifier) => await WaitForResponseAsync(requestMessageIdentifier, DefaultWaitForResponseTimeoutThreshold).ConfigureAwait(false);

        /// <summary>
        /// Waits for a response message matching the specified request to populate the client manager's response dictionary.
        /// </summary>
        /// <param name="requestMessageIdentifier">
        /// An identifier for the associated request message.
        /// </param>
        /// <param name="timeoutThreshold">
        /// The maximum length of time to wait for the response before raising an exception, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> to specify an infinite duration. The default value is one minute.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the response message.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> -and-
        /// <paramref name="timeoutThreshold" /> is not equal to <see cref="Timeout.InfiniteTimeSpan" />.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The timeout threshold duration was exceeded while waiting for a response.
        /// </exception>
        [DebuggerHidden]
        internal async Task<AzureServiceBusMessage> WaitForResponseAsync(Guid requestMessageIdentifier, TimeSpan timeoutThreshold)
        {
            var responseMessageKey = requestMessageIdentifier.ToSerializedString();
            var stopwatch = new Stopwatch();

            if (timeoutThreshold.RejectIf(argument => argument <= TimeSpan.Zero && argument != Timeout.InfiniteTimeSpan, nameof(timeoutThreshold)) != Timeout.InfiniteTimeSpan)
            {
                stopwatch.Start();
            }

            while (stopwatch.Elapsed < timeoutThreshold)
            {
                await Task.Delay(ResponseMessagePollingInterval).ConfigureAwait(false);

                if (ResponseMessageDictionary.TryRemove(responseMessageKey, out var responseMessage))
                {
                    return responseMessage;
                }
            }

            throw new TimeoutException($"The timeout threshold duration was exceeded while waiting for a response to request message {responseMessageKey}.");
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AzureServiceBusClientManager" />.
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
                    using (var controlToken = StateControl.Enter())
                    {
                        if (LazyReceiveClientDictionary.IsValueCreated)
                        {
                            foreach (var client in ReceiveClientDictionary.Values)
                            {
                                if (client.IsClosedOrClosing)
                                {
                                    continue;
                                }

                                if (client is SubscriptionClient subscriptionClient)
                                {
                                    ClientFactory.DestroySubscription(subscriptionClient.TopicPath, subscriptionClient.SubscriptionName);
                                }

                                controlToken.AttachTask(client.CloseAsync());
                            }
                        }

                        if (LazySendClientDictionary.IsValueCreated)
                        {
                            foreach (var client in SendClientDictionary.Values)
                            {
                                if (client.IsClosedOrClosing)
                                {
                                    continue;
                                }

                                controlToken.AttachTask(client.CloseAsync());
                            }
                        }
                    }

                    ClientFactory.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

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
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private static String GetEntityPath<TMessage>(MessagingEntityType entityType)
            where TMessage : class
        {
            switch (entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType)).TargetArgument)
            {
                case MessagingEntityType.Queue:

                    return AzureServiceBusClientFactory.GetQueuePath<TMessage>();

                case MessagingEntityType.Topic:

                    return AzureServiceBusClientFactory.GetTopicPath<TMessage>();

                default:

                    throw new InvalidOperationException($"The specified entity type, {entityType}, is not supported.");
            }
        }

        /// <summary>
        /// Handles an exception that is raised by a message receive client.
        /// </summary>
        /// <param name="exceptionReceivedArguments">
        /// Contextual information about the raised exception.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while trying to publish an <see cref="ExceptionRaisedMessage" />.
        /// </exception>
        [DebuggerHidden]
        private Task HandleReceiverException(ExceptionReceivedEventArgs exceptionReceivedArguments)
        {
            var receivedException = exceptionReceivedArguments.Exception;

            try
            {
                var exceptionRaisedMessage = new ExceptionRaisedMessage(receivedException);
                var serializer = new DynamicSerializer<ExceptionRaisedMessage>(ExceptionRaisedMessageSerializationFormat);
                var messageBody = serializer.Serialize(exceptionRaisedMessage);
                var azureServiceBusMessage = CreateAzureServiceBusMessage(messageBody, typeof(ExceptionRaisedMessage), ExceptionRaisedMessageSerializationFormat, exceptionRaisedMessage.Identifier, exceptionRaisedMessage.CorrelationIdentifier);
                var sendClient = GetSendClient<ExceptionRaisedMessage>(ExceptionRaisedMessageEntityType);
                return sendClient.SendAsync(azureServiceBusMessage);
            }
            catch (Exception exception)
            {
                throw new AggregateException("An exception was raised while trying to publish an ExceptionRaisedMessage.", exception, receivedException);
            }
        }

        /// <summary>
        /// Handles a message that is published in response to a request submitted by the current
        /// <see cref="AzureServiceBusPublishingClient" />.
        /// </summary>
        /// <param name="message">
        /// The response message to handle.
        /// </param>
        /// <param name="cancellationToken">
        /// An object that propagates a cancellation signal.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The response subscriber received multiple responses or the client state is corrupt.
        /// </exception>
        [DebuggerHidden]
        private Task HandleResponseMessage(AzureServiceBusMessage message, CancellationToken cancellationToken)
        {
            if (message.UserProperties.TryGetValue(RequestMessageIdentifierUserPropertyKey, out var requestMessageIdentifier))
            {
                var responseMessageKey = requestMessageIdentifier?.ToString();

                if (responseMessageKey.IsNullOrEmpty())
                {
                    throw new InvalidOperationException("The response message did not contain a request identifier.");
                }

                if (OutstandingResponseKeys.Contains(responseMessageKey))
                {
                    OutstandingResponseKeys.Remove(responseMessageKey);

                    if (ResponseMessageDictionary.TryAdd(responseMessageKey, message))
                    {
                        return Task.CompletedTask;
                    }
                }
                else
                {
                    // The response is intended for another client manager.
                    return Task.CompletedTask;
                }
            }

            throw new InvalidOperationException("The response subscriber received multiple responses or the system state is corrupt.");
        }

        /// <summary>
        /// Gets options that specify how receive clients handle general messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal MessageHandlerOptions ReceiverOptionsForGeneralMessages => new MessageHandlerOptions(HandleReceiverException)
        {
            AutoComplete = false
        };

        /// <summary>
        /// Gets options that specify how receive clients handle request messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal MessageHandlerOptions ReceiverOptionsForRequestMessages => new MessageHandlerOptions(HandleReceiverException)
        {
            AutoComplete = false
        };

        /// <summary>
        /// Gets options that specify how receive clients handle response messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal MessageHandlerOptions ReceiverOptionsForResponseMessages => new MessageHandlerOptions(HandleReceiverException)
        {
            AutoComplete = true
        };

        /// <summary>
        /// Represents a collection of outstanding request identifiers that key pending response messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IList<String> OutstandingResponseKeys => LazyOutstandingResponseKeys.Value;

        /// <summary>
        /// Gets a collection of Azure Service Bus receive clients that are keyed by entity path.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary<String, IReceiverClient> ReceiveClientDictionary => LazyReceiveClientDictionary.Value;

        /// <summary>
        /// Gets a collection of unprocessed response messages that are keyed by request message identifier.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConcurrentDictionary<String, AzureServiceBusMessage> ResponseMessageDictionary => LazyResponseMessageDictionary.Value;

        /// <summary>
        /// Gets a collection of Azure Service Bus send clients that are keyed by entity path.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary<String, ISenderClient> SendClientDictionary => LazySendClientDictionary.Value;

        /// <summary>
        /// Represents the entity type that is used for publishing the <see cref="ExceptionRaisedMessage" /> message type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const MessagingEntityType ExceptionRaisedMessageEntityType = MessagingEntityType.Queue;

        /// <summary>
        /// Represents the format that is used to serialize the <see cref="ExceptionRaisedMessage" /> message type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SerializationFormat ExceptionRaisedMessageSerializationFormat = SerializationFormat.Binary;

        /// <summary>
        /// Represents the user property key for the message's full type name.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String FullTypeNameUserPropertyKey = "FullTypeName";

        /// <summary>
        /// Represents the user property key for an associated request message identifier, if any.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String RequestMessageIdentifierUserPropertyKey = "RequestMessageIdentifier";

        /// <summary>
        /// Represents the default maximum length of time to wait for the response before raising an exception.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan DefaultWaitForResponseTimeoutThreshold = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Represents the polling interval that is used when waiting for a response message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan ResponseMessagePollingInterval = TimeSpan.FromMilliseconds(2);

        /// <summary>
        /// Represents a factory that produces Azure Service Bus clients using a shared connection.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly AzureServiceBusClientFactory ClientFactory;

        /// <summary>
        /// Represents a lazily-initialized collection of outstanding request identifiers that key pending response messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IList<String>> LazyOutstandingResponseKeys = new Lazy<IList<String>>(() => new List<String>(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Represents a lazily-initialized collection of Azure Service Bus receive clients that are keyed by entity path.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDictionary<String, IReceiverClient>> LazyReceiveClientDictionary = new Lazy<IDictionary<String, IReceiverClient>>(() => new Dictionary<String, IReceiverClient>(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Represents a lazily-initialized collection of unprocessed response messages that are keyed by request message identifier.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<ConcurrentDictionary<String, AzureServiceBusMessage>> LazyResponseMessageDictionary = new Lazy<ConcurrentDictionary<String, AzureServiceBusMessage>>(() => new ConcurrentDictionary<String, AzureServiceBusMessage>(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Represents a lazily-initialized collection of Azure Service Bus send clients that are keyed by entity path.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDictionary<String, ISenderClient>> LazySendClientDictionary = new Lazy<IDictionary<String, ISenderClient>>(() => new Dictionary<String, ISenderClient>(), LazyThreadSafetyMode.ExecutionAndPublication);
    }
}