// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client that interacts with an <see cref="IMessagingEntity" />.
    /// </summary>
    public interface IMessagingEntityClient
    {
        /// <summary>
        /// Gets the client's connection to the associated entity's <see cref="IMessageTransport" />.
        /// </summary>
        IMessageTransportConnection Connection
        {
            get;
        }

        /// <summary>
        /// Gets the entity type of the associated <see cref="IMessagingEntity" />.
        /// </summary>
        MessagingEntityType EntityType
        {
            get;
        }

        /// <summary>
        /// Gets the unique textual path for the messaging entity with which the client transacts.
        /// </summary>
        IMessagingEntityPath Path
        {
            get;
        }
    }
}