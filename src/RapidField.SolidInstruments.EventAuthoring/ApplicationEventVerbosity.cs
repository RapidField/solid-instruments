// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Specifies the verbosity level associated with an application event.
    /// </summary>
    [DataContract]
    public enum ApplicationEventVerbosity : Int32
    {
        /// <summary>
        /// The application event verbosity is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The event importance is high or the event occurs infrequently.
        /// </summary>
        [EnumMember]
        Minimal = 1,

        /// <summary>
        /// The event importance is normal.
        /// </summary>
        [EnumMember]
        Normal = 2,

        /// <summary>
        /// The event importance is low or the event occurs frequently.
        /// </summary>
        [EnumMember]
        Detailed = 3,

        /// <summary>
        /// The event is generally trivial or it occurs very frequently.
        /// </summary>
        [EnumMember]
        Diagnostic = 4
    }
}