// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client that interacts with an <see cref="IMessageQueue" />.
    /// </summary>
    public interface IMessageQueueClient : IMessagingEntityReceiveClient, IMessagingEntitySendClient
    {
    }
}