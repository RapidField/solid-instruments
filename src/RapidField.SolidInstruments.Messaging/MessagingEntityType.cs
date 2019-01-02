// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Defines the targeted entity type for a message publishing or subscription operation.
    /// </summary>
    public enum MessagingEntityType : Int32
    {
        /// <summary>
        /// The entity type is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The operation targets a queue.
        /// </summary>
        Queue = 1,

        /// <summary>
        /// The operation targets a topic.
        /// </summary>
        Topic = 2
    }
}