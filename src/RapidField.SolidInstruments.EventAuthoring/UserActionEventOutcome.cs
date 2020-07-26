// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Specifies the outcome of the user action associated with an <see cref="IUserActionEvent" />.
    /// </summary>
    [DataContract]
    public enum UserActionEventOutcome : Int32
    {
        /// <summary>
        /// The outcome of the user action is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The user action was canceled.
        /// </summary>
        [EnumMember]
        Cancelled = 1,

        /// <summary>
        /// The user action failed.
        /// </summary>
        [EnumMember]
        Failed = 2,

        /// <summary>
        /// The user action is in progress.
        /// </summary>
        [EnumMember]
        InProgress = 3,

        /// <summary>
        /// The user action succeeded.
        /// </summary>
        [EnumMember]
        Succeeded = 4
    }
}