// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.UnitTests
{
    /// <summary>
    /// Represents a <see cref="ResponseMessage" /> derivative that is used for testing.
    /// </summary>
    [DataContract]
    internal sealed class SimulatedResponseMessage : ResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedResponseMessage" /> class.
        /// </summary>
        public SimulatedResponseMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedResponseMessage" /> class.
        /// </summary>
        /// <param name="requestMessageIdentifier">
        /// The identifier for the associated request message.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="requestMessageIdentifier" /> is equal to <see cref="Guid.Empty" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SimulatedResponseMessage(Guid requestMessageIdentifier, Guid correlationIdentifier, TimeSpan result)
            : base(requestMessageIdentifier, correlationIdentifier)
        {
            Result = result;
        }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        [DataMember]
        public TimeSpan Result
        {
            get;
            set;
        }
    }
}