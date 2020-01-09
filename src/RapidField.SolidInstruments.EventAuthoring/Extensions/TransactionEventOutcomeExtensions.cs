// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring.Extensions
{
    /// <summary>
    /// Extends the <see cref="TransactionEventOutcome" /> enumeration with general purpose features.
    /// </summary>
    public static class TransactionEventOutcomeExtensions
    {
        /// <summary>
        /// Converts the current <see cref="TransactionEventOutcome" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="TransactionEventOutcome" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="TransactionEventOutcome" />.
        /// </returns>
        public static Byte[] ToByteArray(this TransactionEventOutcome target) => BitConverter.GetBytes((Int32)target);
    }
}