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
    /// Represents information about a security-relevant event.
    /// </summary>
    /// <remarks>
    /// <see cref="SecurityEvent" /> is the default implementation of <see cref="ISecurityEvent" />.
    /// </remarks>
    [DataContract]
    public class SecurityEvent : Event, ISecurityEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent" /> class.
        /// </summary>
        public SecurityEvent()
            : this(DefaultSeverity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SecurityEvent(Guid correlationIdentifier)
            : this(DefaultSeverity, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent" /> class.
        /// </summary>
        /// <param name="severity">
        /// The severity of the event. The default value is <see cref="SecurityEventSeverity.Medium" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="severity" /> is equal to <see cref="SecurityEventSeverity.Unspecified" />.
        /// </exception>
        public SecurityEvent(SecurityEventSeverity severity)
            : this(severity, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent" /> class.
        /// </summary>
        /// <param name="severity">
        /// The severity of the event. The default value is <see cref="SecurityEventSeverity.Medium" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="severity" /> is equal to <see cref="SecurityEventSeverity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SecurityEvent(SecurityEventSeverity severity, Guid correlationIdentifier)
            : this(severity, DefaultVerbosity, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent" /> class.
        /// </summary>
        /// <param name="severity">
        /// The severity of the event. The default value is <see cref="SecurityEventSeverity.Medium" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="severity" /> is equal to <see cref="SecurityEventSeverity.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public SecurityEvent(SecurityEventSeverity severity, EventVerbosity verbosity)
            : this(severity, verbosity, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent" /> class.
        /// </summary>
        /// <param name="severity">
        /// The severity of the event. The default value is <see cref="SecurityEventSeverity.Medium" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="severity" /> is equal to <see cref="SecurityEventSeverity.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SecurityEvent(SecurityEventSeverity severity, EventVerbosity verbosity, Guid correlationIdentifier)
            : this(severity, verbosity, null, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent" /> class.
        /// </summary>
        /// <param name="severity">
        /// The severity of the event. The default value is <see cref="SecurityEventSeverity.Medium" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="severity" /> is equal to <see cref="SecurityEventSeverity.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public SecurityEvent(SecurityEventSeverity severity, EventVerbosity verbosity, String description)
            : this(severity, verbosity, description, DefaultTimeStamp)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent" /> class.
        /// </summary>
        /// <param name="severity">
        /// The severity of the event. The default value is <see cref="SecurityEventSeverity.Medium" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="severity" /> is equal to <see cref="SecurityEventSeverity.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SecurityEvent(SecurityEventSeverity severity, EventVerbosity verbosity, String description, Guid correlationIdentifier)
            : this(severity, verbosity, description, DefaultTimeStamp, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent" /> class.
        /// </summary>
        /// <param name="severity">
        /// The severity of the event. The default value is <see cref="SecurityEventSeverity.Medium" />.
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
        /// <paramref name="severity" /> is equal to <see cref="SecurityEventSeverity.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public SecurityEvent(SecurityEventSeverity severity, EventVerbosity verbosity, String description, DateTime timeStamp)
            : base(StaticCategory, verbosity, description, timeStamp)
        {
            Severity = severity.RejectIf().IsEqualToValue(SecurityEventSeverity.Unspecified, nameof(severity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent" /> class.
        /// </summary>
        /// <param name="severity">
        /// The severity of the event. The default value is <see cref="SecurityEventSeverity.Medium" />.
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
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="severity" /> is equal to <see cref="SecurityEventSeverity.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SecurityEvent(SecurityEventSeverity severity, EventVerbosity verbosity, String description, DateTime timeStamp, Guid correlationIdentifier)
            : base(StaticCategory, verbosity, description, timeStamp, correlationIdentifier)
        {
            Severity = severity.RejectIf().IsEqualToValue(SecurityEventSeverity.Unspecified, nameof(severity));
        }

        /// <summary>
        /// Gets or sets the severity of the current <see cref="SecurityEvent" />.
        /// </summary>
        [DataMember]
        public SecurityEventSeverity Severity
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default severity.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SecurityEventSeverity DefaultSeverity = SecurityEventSeverity.Medium;

        /// <summary>
        /// Represents the static event category for instances of the <see cref="SecurityEvent" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const EventCategory StaticCategory = EventCategory.Security;
    }
}