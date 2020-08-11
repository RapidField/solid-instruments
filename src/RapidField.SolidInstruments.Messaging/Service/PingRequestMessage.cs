// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Represents a message that requests an arbitrary response from a service.
    /// </summary>
    /// <remarks>
    /// <see cref="PingRequestMessage" /> and <see cref="PingResponseMessage" /> can be used to test transport and/or service
    /// availability.
    /// </remarks>
    [DataContract]
    public sealed class PingRequestMessage : RequestMessage<PingResponseMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingRequestMessage" /> class.
        /// </summary>
        public PingRequestMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingRequestMessage" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public PingRequestMessage(Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            return;
        }
    }
}