// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.InMemory
{
    /// <summary>
    /// Facilitates transmission operations for in-memory queues.
    /// </summary>
    public sealed class InMemoryTransmittingFacade : MessageTransmittingFacade<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryTransmittingFacade" /> class.
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
        public InMemoryTransmittingFacade(IMessagingClientFactory<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage> clientFactory, IMessageAdapter<PrimitiveMessage> messageAdapter)
            : base(clientFactory, messageAdapter)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="InMemoryTransmittingFacade" />.
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
        protected sealed override Task TransmitAsync(PrimitiveMessage message, IMessagingEntitySendClient sendClient, IConcurrencyControlToken controlToken) => sendClient.SendAsync(message);
    }
}