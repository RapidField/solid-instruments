// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.RequestResponse.Ping
{
    /// <summary>
    /// Represents a message that requests a response from a service.
    /// </summary>
    [DataContract(Name = DataContractName)]
    internal sealed class RequestMessage : RequestMessage<ResponseMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingRequestMessage" /> class.
        /// </summary>
        public RequestMessage()
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
        public RequestMessage(Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Represents the name that is used when representing this current type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DataContractName = "PingRequestMessage";
    }
}