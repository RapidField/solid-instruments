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
    /// Represents information about a user action event.
    /// </summary>
    /// <remarks>
    /// <see cref="UserActionEvent" /> is the default implementation of <see cref="IUserActionEvent" />.
    /// </remarks>
    [DataContract]
    public class UserActionEvent : Event, IUserActionEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserActionEvent" /> class.
        /// </summary>
        public UserActionEvent()
            : this(DefaultOutcome)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActionEvent" /> class.
        /// </summary>
        /// <param name="outcome">
        /// <paramref name="outcome" /> is equal to <see cref="UserActionEventOutcome.Unspecified" />.
        /// </param>
        public UserActionEvent(UserActionEventOutcome outcome)
            : this(outcome, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActionEvent" /> class.
        /// </summary>
        /// <param name="outcome">
        /// The outcome of the user action. The default value is <see cref="UserActionEventOutcome.Succeeded" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="outcome" /> is equal to <see cref="UserActionEventOutcome.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public UserActionEvent(UserActionEventOutcome outcome, EventVerbosity verbosity)
            : this(outcome, verbosity, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActionEvent" /> class.
        /// </summary>
        /// <param name="outcome">
        /// The outcome of the user action. The default value is <see cref="UserActionEventOutcome.Succeeded" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="outcome" /> is equal to <see cref="UserActionEventOutcome.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public UserActionEvent(UserActionEventOutcome outcome, EventVerbosity verbosity, String description)
            : this(outcome, verbosity, description, DefaultTimeStamp)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActionEvent" /> class.
        /// </summary>
        /// <param name="outcome">
        /// The outcome of the user action. The default value is <see cref="UserActionEventOutcome.Succeeded" />.
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
        /// <paramref name="outcome" /> is equal to <see cref="UserActionEventOutcome.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public UserActionEvent(UserActionEventOutcome outcome, EventVerbosity verbosity, String description, DateTime timeStamp)
            : base(StaticCategory, verbosity, description, timeStamp)
        {
            Outcome = outcome.RejectIf().IsEqualToValue(UserActionEventOutcome.Unspecified, nameof(outcome));
        }

        /// <summary>
        /// Gets or sets the outcome of the current <see cref="UserActionEvent" />.
        /// </summary>
        [DataMember]
        public UserActionEventOutcome Outcome
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default outcome.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const UserActionEventOutcome DefaultOutcome = UserActionEventOutcome.Succeeded;

        /// <summary>
        /// Represents the static event category for instances of the <see cref="UserActionEvent" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const EventCategory StaticCategory = EventCategory.UserAction;
    }
}