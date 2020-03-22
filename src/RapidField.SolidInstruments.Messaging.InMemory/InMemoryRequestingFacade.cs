// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using System;

namespace RapidField.SolidInstruments.Messaging.InMemory
{
    /// <summary>
    /// Facilitates requesting operations for in-memory.
    /// </summary>
    public sealed class InMemoryRequestingFacade : MessageRequestingFacade<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage, InMemoryTransmittingFacade, InMemoryListeningFacade>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryRequestingFacade" /> class.
        /// </summary>
        /// <param name="listeningFacade">
        /// An implementation-specific messaging facade that listens for request messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="listeningFacade" /> is <see langword="null" />.
        /// </exception>
        public InMemoryRequestingFacade(InMemoryListeningFacade listeningFacade)
            : base(listeningFacade)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="InMemoryRequestingFacade" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}