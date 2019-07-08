// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            LockedMessages = new Dictionary<DurableMessageLockToken, DurableMessage>();
            Messages = new List<DurableMessage>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueueSnapshot" /> class.
        /// </summary>
        /// <param name="messageQueue">
        /// The associated message queue.
        /// </param>
        [DebuggerHidden]
        internal DurableMessageQueueSnapshot(DurableMessageQueue messageQueue)
        {
            EnqueueTimeoutThreshold = messageQueue.EnqueueTimeoutThreshold;
            Identifier = messageQueue.Identifier;
            LockedMessages = new Dictionary<DurableMessageLockToken, DurableMessage>(messageQueue.LockedMessages);
            MessageBodySerializationFormat = messageQueue.MessageBodySerializationFormat;
            MessageLockExpirationThreshold = messageQueue.MessageLockExpirationThreshold;
            Messages = new List<DurableMessage>(messageQueue.Messages.ToArray());
            OperationalState = messageQueue.OperationalState;
            Path = messageQueue.Path;
            TimeStamp = Core.TimeStamp.Current;
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
        /// Gets or sets a unique identifier for the associated <see cref="IDurableMessageQueue" />.
        /// </summary>
        [DataMember]
        public Guid Identifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the first-in first-out collection that contains messages that are locked for processing by the associated
        /// <see cref="IDurableMessageQueue" />.
        /// </summary>
        [DataMember]
        public IDictionary<DurableMessageLockToken, DurableMessage> LockedMessages
        {
            get;
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
        /// Gets the first-in first-out collection that contains enqueued messages for the associated
        /// <see cref="IDurableMessageQueue" />.
        /// </summary>
        [DataMember]
        public ICollection<DurableMessage> Messages
        {
            get;
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

        /// <summary>
        /// Gets or sets the date and time when the current <see cref="DurableMessageQueueSnapshot" /> was created.
        /// </summary>
        [DataMember]
        public DateTime TimeStamp
        {
            get;
            set;
        }
    }
}