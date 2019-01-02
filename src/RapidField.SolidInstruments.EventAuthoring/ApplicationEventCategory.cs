// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Specifies a category of application event.
    /// </summary>
    [DataContract]
    public enum ApplicationEventCategory : Int32
    {
        /// <summary>
        /// The application event category is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The event represents an application state change.
        /// </summary>
        [EnumMember]
        ApplicationState = 1,

        /// <summary>
        /// The event represents an error.
        /// </summary>
        [EnumMember]
        Error = 2,

        /// <summary>
        /// The event is informational.
        /// </summary>
        [EnumMember]
        Information = 3,

        /// <summary>
        /// The event is relevant to application security.
        /// </summary>
        [EnumMember]
        Security = 4,

        /// <summary>
        /// The event represents a transaction.
        /// </summary>
        [EnumMember]
        Transaction = 5,

        /// <summary>
        /// The event represents a user action.
        /// </summary>
        [EnumMember]
        UserAction = 6
    }
}