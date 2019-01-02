// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AzureServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace RapidField.SolidInstruments.Messaging.AzureServiceBus
{
    /// <summary>
    /// Facilitates subscription operations for Azure Service Bus queues.
    /// </summary>
    public sealed class AzureServiceBusSubscriptionClient : MessageSubscriptionClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusSubscriptionClient" />.
        /// </summary>
        /// <param name="connection">
        /// A connection that governs interaction with Azure Service Bus entities.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        public AzureServiceBusSubscriptionClient(ServiceBusConnection connection)
            : base()
        {
            Connection = connection.RejectIf().IsNull(nameof(connection)).TargetArgument;
            LazyClientManager = new Lazy<AzureServiceBusClientManager>(CreateClientManager, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusSubscriptionClient" />.
        /// </summary>
        /// <param name="connection">
        /// A connection that governs interaction with Azure Service Bus entities.
        /// </param>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        public AzureServiceBusSubscriptionClient(ServiceBusConnection connection, SerializationFormat messageSerializationFormat)
            : base(messageSerializationFormat)
        {
            Connection = connection.RejectIf().IsNull(nameof(connection)).TargetArgument;
            LazyClientManager = new Lazy<AzureServiceBusClientManager>(CreateClientManager, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusSubscriptionClient" />.
        /// </summary>
        /// <param name="connection">
        /// A connection that governs interaction with Azure Service Bus entities.
        /// </param>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.ProcessorCountSemaphore" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" /> -or-
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" />.
        /// </exception>
        public AzureServiceBusSubscriptionClient(ServiceBusConnection connection, SerializationFormat messageSerializationFormat, ConcurrencyControlMode stateControlMode)
            : base(messageSerializationFormat, stateControlMode)
        {
            Connection = connection.RejectIf().IsNull(nameof(connection)).TargetArgument;
            LazyClientManager = new Lazy<AzureServiceBusClientManager>(CreateClientManager, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AzureServiceBusSubscriptionClient" />.
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
                    LazyClientManager.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
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
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected sealed override void RegisterHandler<TMessage>(Action<TMessage> messageHandler, MessagingEntityType entityType, ConcurrencyControlToken controlToken)
        {
            var receiveClient = ClientManager.GetReceiveClient<TMessage>(entityType);

            var messageHandlerFunction = new Func<AzureServiceBusMessage, CancellationToken, Task>(async (azureServiceBusMessage, cancellationToken) =>
            {
                var lockToken = azureServiceBusMessage.SystemProperties?.LockToken;

                if (lockToken is null)
                {
                    throw new MessageSubscriptionException("The message cannot be processed because the lock token is invalid.");
                }

                var deserializedMessage = DeserializeMessage<TMessage>(azureServiceBusMessage.Body);

                if (receiveClient.IsClosedOrClosing)
                {
                    throw new MessageSubscriptionException("The message cannot be processed because the receive client is unavailable.");
                }

                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    messageHandler(deserializedMessage);
                    await receiveClient.CompleteAsync(lockToken).ConfigureAwait(false);
                }
                catch
                {
                    await receiveClient.AbandonAsync(lockToken).ConfigureAwait(false);
                    throw;
                }
            });

            receiveClient.RegisterMessageHandler(messageHandlerFunction, ClientManager.ReceiverOptionsForGeneralMessages);
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
        /// <param name="requestMessageEntityType">
        /// The entity type that is used for publishing and subscribing to request messages.
        /// </param>
        /// <param name="responseMessageEntityType">
        /// The entity type that is used for publishing and subscribing to response messages.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected sealed override void RegisterHandler<TRequestMessage, TResponseMessage>(Func<TRequestMessage, TResponseMessage> requestMessageHandler, MessagingEntityType requestMessageEntityType, MessagingEntityType responseMessageEntityType, ConcurrencyControlToken controlToken)
        {
            var responseMessageType = typeof(TResponseMessage);
            var requestReceiveClient = ClientManager.GetReceiveClient<TRequestMessage>(requestMessageEntityType);
            var responseSendClient = ClientManager.GetSendClient<TResponseMessage>(responseMessageEntityType);

            var messageHandlerFunction = new Func<AzureServiceBusMessage, CancellationToken, Task>(async (azureServiceBusRequestMessage, cancellationToken) =>
            {
                var lockToken = azureServiceBusRequestMessage.SystemProperties?.LockToken;

                if (lockToken is null)
                {
                    throw new MessageSubscriptionException("The lock token is invalid.");
                }

                var deserializedRequestMessage = DeserializeMessage<TRequestMessage>(azureServiceBusRequestMessage.Body);
                var requestMessageIdentifier = deserializedRequestMessage.Identifier.ToSerializedString();

                if (requestReceiveClient.IsClosedOrClosing)
                {
                    throw new MessageSubscriptionException("The message cannot be processed because the receive client is unavailable.");
                }

                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var responseMessage = requestMessageHandler(deserializedRequestMessage);
                    var serializedResponseMessage = SerializeMessage(responseMessage);
                    var azureServiceBusResponseMessage = AzureServiceBusClientManager.CreateAzureServiceBusMessage(serializedResponseMessage, responseMessageType, MessageSerializationFormat, responseMessage.Identifier, responseMessage.CorrelationIdentifier, deserializedRequestMessage.Identifier);

                    await responseSendClient.SendAsync(azureServiceBusResponseMessage).ContinueWith(async (task) =>
                    {
                        switch (task.Status)
                        {
                            case TaskStatus.RanToCompletion:

                                await requestReceiveClient.CompleteAsync(lockToken).ConfigureAwait(false);
                                break;

                            default:

                                await requestReceiveClient.AbandonAsync(lockToken).ConfigureAwait(false);
                                break;
                        }
                    }).ConfigureAwait(false);
                }
                catch
                {
                    await requestReceiveClient.AbandonAsync(lockToken).ConfigureAwait(false);
                    throw;
                }
            });

            requestReceiveClient.RegisterMessageHandler(messageHandlerFunction, ClientManager.ReceiverOptionsForRequestMessages);
        }

        /// <summary>
        /// Creates a new <see cref="AzureServiceBusClientManager" /> using <see cref="Connection" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="AzureServiceBusClientManager" /> instance.
        /// </returns>
        [DebuggerHidden]
        private AzureServiceBusClientManager CreateClientManager() => new AzureServiceBusClientManager(Connection);

        /// <summary>
        /// Gets an object that manages creation and resolution for a shared pool of Azure Service Bus send and receive clients.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AzureServiceBusClientManager ClientManager => LazyClientManager.Value;

        /// <summary>
        /// Represents a connection that governs interaction with Azure Service Bus entities.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ServiceBusConnection Connection;

        /// <summary>
        /// Represents a lazily-initialized object that manages creation and resolution for a shared pool of Azure Service Bus send
        /// and receive clients.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<AzureServiceBusClientManager> LazyClientManager;
    }
}