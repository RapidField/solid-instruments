// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Specifies the outcome of the transaction associated with an <see cref="ITransactionEvent" />.
    /// </summary>
    [DataContract]
    public enum TransactionEventOutcome : Int32
    {
        /// <summary>
        /// The outcome of the transaction is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The transaction was canceled.
        /// </summary>
        [EnumMember]
        Cancelled = 1,

        /// <summary>
        /// The transaction failed.
        /// </summary>
        [EnumMember]
        Failed = 2,

        /// <summary>
        /// The transaction is in progress.
        /// </summary>
        [EnumMember]
        InProgress = 3,

        /// <summary>
        /// The transaction succeeded.
        /// </summary>
        [EnumMember]
        Succeeded = 4
    }
}