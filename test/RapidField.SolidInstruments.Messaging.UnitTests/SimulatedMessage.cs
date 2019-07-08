// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.UnitTests
{
    /// <summary>
    /// Represents a <see cref="Message" /> derivative that is used for testing.
    /// </summary>
    [DataContract]
    internal sealed class SimulatedMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedMessage" /> class.
        /// </summary>
        public SimulatedMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedMessage" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <param name="simulatedObject">
        /// A test object.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SimulatedMessage(Guid correlationIdentifier, SimulatedObject testObject)
            : base(correlationIdentifier)
        {
            TestObject = testObject;
        }

        /// <summary>
        /// Gets or sets a test object
        /// </summary>
        [DataMember]
        public SimulatedObject TestObject
        {
            get;
            set;
        }
    }
}