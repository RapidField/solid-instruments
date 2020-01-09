// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Specifies the category of an <see cref="IEvent" />.
    /// </summary>
    [DataContract]
    public enum EventCategory : Int32
    {
        /// <summary>
        /// The application event category is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The event represents information about an application state change.
        /// </summary>
        [EnumMember]
        ApplicationState = 1,

        /// <summary>
        /// The event represents information about domain activity.
        /// </summary>
        [EnumMember]
        Domain = 2,

        /// <summary>
        /// The event represents information about an error.
        /// </summary>
        [EnumMember]
        Error = 3,

        /// <summary>
        /// The event represents general information.
        /// </summary>
        [EnumMember]
        GeneralInformation = 4,

        /// <summary>
        /// The event represents security-relevant information.
        /// </summary>
        [EnumMember]
        Security = 5,

        /// <summary>
        /// The event represents information about a system state change.
        /// </summary>
        [EnumMember]
        SystemState = 6,

        /// <summary>
        /// The event represents information about a transaction.
        /// </summary>
        [EnumMember]
        Transaction = 7,

        /// <summary>
        /// The event represents information about a user action.
        /// </summary>
        [EnumMember]
        UserAction = 8
    }
}