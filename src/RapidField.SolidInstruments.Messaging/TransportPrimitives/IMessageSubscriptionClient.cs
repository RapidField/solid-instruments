// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client that interacts with an <see cref="IMessageTopic" /> subscription.
    /// </summary>
    public interface IMessageSubscriptionClient : IMessagingEntityReceiveClient
    {
        /// <summary>
        /// Gets the unique name of the associated subscription.
        /// </summary>
        public String SubscriptionName
        {
            get;
        }
    }
}