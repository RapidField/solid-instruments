// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client that facilitates receive operations to an <see cref="IMessagingEntity" />.
    /// </summary>
    public interface IMessagingEntityReceiveClient : IMessagingEntityClient
    {
        /// <summary>
        /// Registers the specified message handler for the associated <see cref="IMessagingEntity" />.
        /// </summary>
        /// <param name="handleMessageAction">
        /// An action to perform upon message receipt.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handleMessageAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The connection is closed.
        /// </exception>
        void RegisterMessageHandler(Action<PrimitiveMessage> handleMessageAction);
    }
}