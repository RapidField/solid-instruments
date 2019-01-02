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

namespace RapidField.SolidInstruments.Messaging.AzureServiceBus
{
    /// <summary>
    /// Facilitates publishing operations for Azure Service Bus queues.
    /// </summary>
    public sealed class AzureServiceBusPublishingClient : MessagePublishingClient
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
        public AzureServiceBusPublishingClient(ServiceBusConnection connection)
            : base()
        {
            Connection = connection.RejectIf().IsNull(nameof(connection)).TargetArgument;
            LazyClientManager = new Lazy<AzureServiceBusClientManager>(CreateClientManager, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusPublishingClient" />.
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
        public AzureServiceBusPublishingClient(ServiceBusConnection connection, SerializationFormat messageSerializationFormat)
            : base(messageSerializationFormat)
        {
            Connection = connection.RejectIf().IsNull(nameof(connection)).TargetArgument;
            LazyClientManager = new Lazy<AzureServiceBusClientManager>(CreateClientManager, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusPublishingClient" />.
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
        public AzureServiceBusPublishingClient(ServiceBusConnection connection, SerializationFormat messageSerializationFormat, ConcurrencyControlMode stateControlMode)
            : base(messageSerializationFormat, stateControlMode)
        {
            Connection = connection.RejectIf().IsNull(nameof(connection)).TargetArgument;
            LazyClientManager = new Lazy<AzureServiceBusClientManager>(CreateClientManager, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AzureServiceBusPublishingClient" />.
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
        protected sealed override async Task PublishAsync<TMessage>(TMessage message, MessagingEntityType entityType, ConcurrencyControlToken controlToken)
        {
            var messageType = typeof(TMessage);
            var serializedMessage = SerializeMessage(message);
            var azureServiceBusMessage = AzureServiceBusClientManager.CreateAzureServiceBusMessage(serializedMessage, messageType, MessageSerializationFormat, message.Identifier, message.CorrelationIdentifier);
            var sendClient = ClientManager.GetSendClient<TMessage>(entityType);
            await sendClient.SendAsync(azureServiceBusMessage).ConfigureAwait(false);
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
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the correlated response message.
        /// </returns>
        protected sealed override async Task<TResponseMessage> RequestAsync<TRequestMessage, TResponseMessage>(TRequestMessage requestMessage, ConcurrencyControlToken controlToken)
        {
            var requestMessageType = typeof(TRequestMessage);
            var serializedRequestMessage = SerializeMessage(requestMessage);
            var azureServiceBusRequestMessage = AzureServiceBusClientManager.CreateAzureServiceBusMessage(serializedRequestMessage, requestMessageType, MessageSerializationFormat, requestMessage.Identifier, requestMessage.CorrelationIdentifier);
            var requestClient = ClientManager.GetRequestClient<TRequestMessage, TResponseMessage>();
            ClientManager.AddOutstandingRequest(requestMessage.Identifier);

            using (var sendTask = requestClient.SendAsync(azureServiceBusRequestMessage))
            {
                using (var waitForResponseTask = ClientManager.WaitForResponseAsync(requestMessage.Identifier))
                {
                    await Task.WhenAll(sendTask, waitForResponseTask);
                    return DeserializeMessage<TResponseMessage>(waitForResponseTask.Result.Body);
                }
            }
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