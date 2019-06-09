// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Serialization;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a serializable persistence snapshot of a durable message queue.
    /// </summary>
    [DataContract]
    public sealed class DurableMessageQueueSnapshot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueueSnapshot" /> class.
        /// </summary>
        public DurableMessageQueueSnapshot()
        {
            return;
        }

        /// <summary>
        /// Gets or sets the maximum length of time to wait for a message to be enqueued before raising an exception.
        /// </summary>
        [DataMember]
        public TimeSpan EnqueueTimeoutThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the format that is used to serialize enqueued message bodies.
        /// </summary>
        [DataMember]
        public SerializationFormat MessageBodySerializationFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length of time that a locked message is held before abandoning the associated token and making the
        /// message available for processing.
        /// </summary>
        [DataMember]
        public TimeSpan MessageLockExpirationThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the operational state of the associated <see cref="IDurableMessageQueue" />.
        /// </summary>
        [DataMember]
        public DurableMessageQueueOperationalState OperationalState
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unique textual path that identifies the associated <see cref="IDurableMessageQueue" />.
        /// </summary>
        [DataMember]
        public String Path
        {
            get;
            set;
        }
    }
}