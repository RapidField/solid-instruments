// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using System;

namespace RapidField.SolidInstruments.Messaging.RabbitMq
{
    /// <summary>
    /// Facilitates requesting operations for RabbitMQ.
    /// </summary>
    public sealed class RabbitMqRequestingFacade : MessageRequestingFacade<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage, RabbitMqTransmittingFacade, RabbitMqListeningFacade>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqRequestingFacade" /> class.
        /// </summary>
        /// <param name="listeningFacade">
        /// An implementation-specific messaging facade that listens for request messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="listeningFacade" /> is <see langword="null" />.
        /// </exception>
        public RabbitMqRequestingFacade(RabbitMqListeningFacade listeningFacade)
            : base(listeningFacade)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="RabbitMqRequestingFacade" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}