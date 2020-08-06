// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Represents a message that is transmitted at a regular interval.
    /// </summary>
    [DataContract]
    public class HeartbeatMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatMessage" /> class.
        /// </summary>
        public HeartbeatMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatMessage" /> class.
        /// </summary>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is transmitted.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero.
        /// </exception>
        public HeartbeatMessage(Int32 intervalInSeconds)
            : this(intervalInSeconds, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatMessage" /> class.
        /// </summary>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is transmitted.
        /// </param>
        /// <param name="label">
        /// The label, if any, that is associated with the message. This argument can be null.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero.
        /// </exception>
        public HeartbeatMessage(Int32 intervalInSeconds, String label)
            : base()
        {
            IntervalInSeconds = intervalInSeconds.RejectIf().IsLessThanOrEqualTo(0, nameof(intervalInSeconds));
            Label = label;
        }

        /// <summary>
        /// Gets or sets the regular interval, in seconds, at which the message is transmitted.
        /// </summary>
        [DataMember]
        public Int32 IntervalInSeconds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label, if any, that is associated with the message.
        /// </summary>
        [DataMember]
        public String Label
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the regular interval, in seconds, at which frequency "A" heartbeat messages are published.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 FrequencyAIntervalInSeconds = 13;

        /// <summary>
        /// Represents a label applied to heartbeat messages that are published with a known frequency "A".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String FrequencyALabel = "FrequencyA";

        /// <summary>
        /// Represents the regular interval, in seconds, at which frequency "B" heartbeat messages are published.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 FrequencyBIntervalInSeconds = 89;

        /// <summary>
        /// Represents a label applied to heartbeat messages that are published with a known frequency "B".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String FrequencyBLabel = "FrequencyB";

        /// <summary>
        /// Represents the regular interval, in seconds, at which frequency "C" heartbeat messages are published.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 FrequencyCIntervalInSeconds = 233;

        /// <summary>
        /// Represents a label applied to heartbeat messages that are published with a known frequency "C".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String FrequencyCLabel = "FrequencyC";

        /// <summary>
        /// Represents the regular interval, in seconds, at which frequency "D" heartbeat messages are published.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 FrequencyDIntervalInSeconds = 1597;

        /// <summary>
        /// Represents a label applied to heartbeat messages that are published with a known frequency "D".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String FrequencyDLabel = "FrequencyD";

        /// <summary>
        /// Represents the regular interval, in seconds, at which frequency "E" heartbeat messages are published.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 FrequencyEIntervalInSeconds = 28657;

        /// <summary>
        /// Represents a label applied to heartbeat messages that are published with a known frequency "E".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String FrequencyELabel = "FrequencyE";
    }
}