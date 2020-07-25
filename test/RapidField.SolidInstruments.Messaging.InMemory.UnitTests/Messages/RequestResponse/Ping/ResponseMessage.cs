// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using BaseResponseMessage = RapidField.SolidInstruments.Messaging.ResponseMessage;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.RequestResponse.Ping
{
    /// <summary>
    /// Represents a response message.
    /// </summary>
    [DataContract(Name = DataContractName)]
    internal sealed class ResponseMessage : BaseResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage" /> class.
        /// </summary>
        public ResponseMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage" /> class.
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
        public ResponseMessage(Guid requestMessageIdentifier, Guid correlationIdentifier)
            : base(requestMessageIdentifier, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Represents the name that is used when representing this current type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DataContractName = "PingResponseMessage";
    }
}