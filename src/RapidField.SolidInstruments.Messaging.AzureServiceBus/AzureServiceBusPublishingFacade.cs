// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus.Core;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Threading.Tasks;
using AzureServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace RapidField.SolidInstruments.Messaging.AzureServiceBus
{
    /// <summary>
    /// Facilitates publishing operations for Azure Service Bus queues.
    /// </summary>
    public sealed class AzureServiceBusPublishingFacade : MessagePublishingFacade<ISenderClient, IReceiverClient, AzureServiceBusMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusPublishingFacade" /> class.
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
        public AzureServiceBusPublishingFacade(IMessagingClientFactory<ISenderClient, IReceiverClient, AzureServiceBusMessage> clientFactory, IMessageAdapter<AzureServiceBusMessage> messageAdapter)
            : base(clientFactory, messageAdapter)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AzureServiceBusPublishingFacade" />.
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
        protected sealed override Task PublishAsync(AzureServiceBusMessage message, ISenderClient sendClient, ConcurrencyControlToken controlToken) => sendClient.SendAsync(message);
    }
}