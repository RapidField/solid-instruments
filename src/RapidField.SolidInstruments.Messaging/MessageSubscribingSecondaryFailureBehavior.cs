// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents behavior that is employed by a message subscriber after the associated retry policy is exhausted.
    /// </summary>
    [DataContract]
    public enum MessageSubscribingSecondaryFailureBehavior : Int32
    {
        /// <summary>
        /// The secondary failure behavior is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The associated message(s) should be discarded after the retry policy is exhausted.
        /// </summary>
        [EnumMember]
        Discard = 1,

        /// <summary>
        /// The associated message(s) should be routed to a dead letter queue after the retry policy is exhausted.
        /// </summary>
        [EnumMember]
        RouteToDeadLetterQueue = 2
    }
}