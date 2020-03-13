// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents the operational state of an <see cref="IMessagingEntity" />.
    /// </summary>
    [DataContract]
    internal enum MessagingEntityOperationalState : Int32
    {
        /// <summary>
        /// The entity's operational state is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The entity is enabled for both enqueue and dequeue operations.
        /// </summary>
        [EnumMember]
        Ready = 1,

        /// <summary>
        /// The entity is enabled for dequeue operations only.
        /// </summary>
        [EnumMember]
        DequeueOnly = 2,

        /// <summary>
        /// The entity is enabled for enqueue operations only.
        /// </summary>
        [EnumMember]
        EnqueueOnly = 3,

        /// <summary>
        /// The entity is temporarily disabled.
        /// </summary>
        [EnumMember]
        Paused = 4,

        /// <summary>
        /// entity is permanently disabled.
        /// </summary>
        [EnumMember]
        Disabled = 5
    }
}