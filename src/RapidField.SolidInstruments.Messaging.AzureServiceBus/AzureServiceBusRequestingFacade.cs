﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus.Core;
using System;
using AzureServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace RapidField.SolidInstruments.Messaging.AzureServiceBus
{
    /// <summary>
    /// Facilitates requesting operations for Azure Service Bus.
    /// </summary>
    public sealed class AzureServiceBusRequestingFacade : MessageRequestingFacade<ISenderClient, IReceiverClient, AzureServiceBusMessage, AzureServiceBusPublishingFacade, AzureServiceBusSubscribingFacade>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusRequestingFacade" /> class.
        /// </summary>
        /// <param name="subscribingFacade">
        /// An implementation-specific messaging facade that subscribes to request messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="subscribingFacade" /> is <see langword="null" />.
        /// </exception>
        public AzureServiceBusRequestingFacade(AzureServiceBusSubscribingFacade subscribingFacade)
            : base(subscribingFacade)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AzureServiceBusRequestingFacade" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}