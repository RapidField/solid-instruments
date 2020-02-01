// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Specifies the severity level of an <see cref="ISecurityEvent" />.
    /// </summary>
    [DataContract]
    public enum SecurityEventSeverity : Int32
    {
        /// <summary>
        /// The security event severity is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The event severity is low.
        /// </summary>
        [EnumMember]
        Low = 1,

        /// <summary>
        /// The event severity is medium.
        /// </summary>
        [EnumMember]
        Medium = 2,

        /// <summary>
        /// The event severity is high.
        /// </summary>
        [EnumMember]
        High = 3,

        /// <summary>
        /// The event severity is critical.
        /// </summary>
        [EnumMember]
        Critical = 4
    }
}