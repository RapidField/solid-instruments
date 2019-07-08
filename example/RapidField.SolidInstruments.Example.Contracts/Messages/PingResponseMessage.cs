// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Messaging;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Example.Contracts.Messages
{
    /// <summary>
    /// Represents a response message.
    /// </summary>
    [DataContract]
    public sealed class PingResponseMessage : ResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingResponseMessage" /> class.
        /// </summary>
        public PingResponseMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingResponseMessage" /> class.
        /// </summary>
        /// <param name="requestMessageIdentifier">
        /// The identifier for the associated request message.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="requestMessageIdentifier" /> is equal to <see cref="Guid.Empty" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public PingResponseMessage(Guid requestMessageIdentifier, Guid correlationIdentifier)
            : base(requestMessageIdentifier, correlationIdentifier)
        {
            return;
        }
    }
}