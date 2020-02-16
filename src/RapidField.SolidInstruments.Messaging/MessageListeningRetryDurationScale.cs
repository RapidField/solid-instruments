// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents the retry duration scaling behavior employed by a listener in response to message processing failure.
    /// </summary>
    [DataContract]
    public enum MessageListeningRetryDurationScale : Int32
    {
        /// <summary>
        /// The retry duration scaling behavior is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// Retry duration scaling follows a linear path, such that each retry event is equidistant in time from its neighboring
        /// retry events.
        /// </summary>
        [EnumMember]
        Linear = 1,

        /// <summary>
        /// Retry duration scaling follows a Fibonacci curve, such that each retry event occurs with a delay duration equal to the
        /// sum of the previous two delay durations.
        /// </summary>
        [EnumMember]
        Fibonacci = 2
    }
}