// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.UnitTests
{
    /// <summary>
    /// Represents a <see cref="RequestMessage{TResponseMessage}" /> derivative that is used for testing.
    /// </summary>
    [DataContract]
    internal sealed class SimulatedRequestMessage : RequestMessage<SimulatedResponseMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedRequestMessage" /> class.
        /// </summary>
        public SimulatedRequestMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedRequestMessage" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <param name="dateRange">
        /// A date range.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SimulatedRequestMessage(Guid correlationIdentifier, DateTimeRange dateRange)
            : base(correlationIdentifier)
        {
            DateRange = dateRange;
        }

        /// <summary>
        /// Gets or sets a date range.
        /// </summary>
        [DataMember]
        public DateTimeRange DateRange
        {
            get;
            set;
        }
    }
}