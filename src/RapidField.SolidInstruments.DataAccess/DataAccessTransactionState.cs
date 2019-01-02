// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Specifies the state of an <see cref="IDataAccessTransaction" />.
    /// </summary>
    public enum DataAccessTransactionState : Int32
    {
        /// <summary>
        /// The transaction state is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The transaction is ready to begin.
        /// </summary>
        Ready = 1,

        /// <summary>
        /// The transaction has begun.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// The transaction was committed.
        /// </summary>
        Committed = 3,

        /// <summary>
        /// The transaction was rejected.
        /// </summary>
        Rejected = 4
    }
}