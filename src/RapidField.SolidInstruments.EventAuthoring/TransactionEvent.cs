// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about a transaction event.
    /// </summary>
    /// <remarks>
    /// <see cref="TransactionEvent" /> is the default implementation of <see cref="ITransactionEvent" />.
    /// </remarks>
    [DataContract]
    public class TransactionEvent : Event, ITransactionEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEvent" /> class.
        /// </summary>
        public TransactionEvent()
            : this(DefaultOutcome)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEvent" /> class.
        /// </summary>
        /// <param name="outcome">
        /// <paramref name="outcome" /> is equal to <see cref="TransactionEventOutcome.Unspecified" />.
        /// </param>
        public TransactionEvent(TransactionEventOutcome outcome)
            : this(outcome, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEvent" /> class.
        /// </summary>
        /// <param name="outcome">
        /// The outcome of the transaction. The default value is <see cref="TransactionEventOutcome.Succeeded" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="outcome" /> is equal to <see cref="TransactionEventOutcome.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public TransactionEvent(TransactionEventOutcome outcome, EventVerbosity verbosity)
            : this(outcome, verbosity, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEvent" /> class.
        /// </summary>
        /// <param name="outcome">
        /// The outcome of the transaction. The default value is <see cref="TransactionEventOutcome.Succeeded" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="outcome" /> is equal to <see cref="TransactionEventOutcome.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public TransactionEvent(TransactionEventOutcome outcome, EventVerbosity verbosity, String description)
            : this(outcome, verbosity, description, DefaultTimeStamp)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEvent" /> class.
        /// </summary>
        /// <param name="outcome">
        /// The outcome of the transaction. The default value is <see cref="TransactionEventOutcome.Succeeded" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="timeStamp">
        /// A <see cref="DateTime" /> that indicates when the event occurred. The default value is <see cref="TimeStamp.Current" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="outcome" /> is equal to <see cref="TransactionEventOutcome.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public TransactionEvent(TransactionEventOutcome outcome, EventVerbosity verbosity, String description, DateTime timeStamp)
            : base(StaticCategory, verbosity, description, timeStamp)
        {
            Outcome = outcome.RejectIf().IsEqualToValue(TransactionEventOutcome.Unspecified, nameof(outcome));
        }

        /// <summary>
        /// Gets or sets the outcome of the current <see cref="TransactionEvent" />.
        /// </summary>
        [DataMember]
        public TransactionEventOutcome Outcome
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default outcome.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const TransactionEventOutcome DefaultOutcome = TransactionEventOutcome.Succeeded;

        /// <summary>
        /// Represents the static event category for instances of the <see cref="TransactionEvent" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const EventCategory StaticCategory = EventCategory.Transaction;
    }
}