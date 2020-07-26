// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents the operational state of an <see cref="IMessagingEntity" />.
    /// </summary>
    public enum MessagingEntityOperationalState : Int32
    {
        /// <summary>
        /// The entity's operational state is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The entity is enabled for both enqueue and dequeue operations.
        /// </summary>
        Ready = 1,

        /// <summary>
        /// The entity is enabled for dequeue operations only.
        /// </summary>
        DequeueOnly = 2,

        /// <summary>
        /// The entity is enabled for enqueue operations only.
        /// </summary>
        EnqueueOnly = 3,

        /// <summary>
        /// The entity is temporarily disabled.
        /// </summary>
        Paused = 4,

        /// <summary>
        /// entity is permanently disabled.
        /// </summary>
        Disabled = 5
    }
}