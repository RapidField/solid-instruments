// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about a transaction event.
    /// </summary>
    public interface ITransactionEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the outcome of the current <see cref="ITransactionEvent" />.
        /// </summary>
        public TransactionEventOutcome Outcome
        {
            get;
            set;
        }
    }
}