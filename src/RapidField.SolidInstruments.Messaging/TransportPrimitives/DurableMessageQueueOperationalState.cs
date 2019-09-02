// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents the operational state of an <see cref="IDurableMessageQueue" />.
    /// </summary>
    [DataContract]
    public enum DurableMessageQueueOperationalState : Int32
    {
        /// <summary>
        /// The queue's operational state is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The queue is enabled for both enqueue and dequeue operations.
        /// </summary>
        [EnumMember]
        Ready = 1,

        /// <summary>
        /// The queue is enabled for dequeue operations only.
        /// </summary>
        [EnumMember]
        DequeueOnly = 2,

        /// <summary>
        /// The queue is enabled for enqueue operations only.
        /// </summary>
        [EnumMember]
        EnqueueOnly = 3,

        /// <summary>
        /// The queue is temporarily disabled.
        /// </summary>
        [EnumMember]
        Paused = 4,

        /// <summary>
        /// The queue is permanently disabled.
        /// </summary>
        [EnumMember]
        Disabled = 5
    }
}