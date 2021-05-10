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
    /// Represents information about an error event.
    /// </summary>
    /// <remarks>
    /// <see cref="ErrorEvent" /> is the default implementation of <see cref="IErrorEvent" />.
    /// </remarks>
    [DataContract]
    public class ErrorEvent : Event, IErrorEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        public ErrorEvent()
            : this(DefaultApplicationIdentity)
        {
            DiagnosticDetails = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ErrorEvent(Guid correlationIdentifier)
            : this(DefaultApplicationIdentity, correlationIdentifier)
        {
            DiagnosticDetails = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception" /> is <see langword="null" />.
        /// </exception>
        public ErrorEvent(Exception exception)
            : this(DefaultApplicationIdentity, exception)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ErrorEvent(Exception exception, Guid correlationIdentifier)
            : this(DefaultApplicationIdentity, exception, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity)
            : this(applicationIdentity, diagnosticDetails: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, Guid correlationIdentifier)
            : this(applicationIdentity, diagnosticDetails: null, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" /> -or- <paramref name="exception" /> is
        /// <see langword="null" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, Exception exception)
            : this(applicationIdentity, exception.RejectIf().IsNull(nameof(exception)).TargetArgument.StackTrace, DefaultVerbosity, exception.Message)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" /> -or- <paramref name="exception" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, Exception exception, Guid correlationIdentifier)
            : this(applicationIdentity, exception.RejectIf().IsNull(nameof(exception)).TargetArgument.StackTrace, DefaultVerbosity, exception.Message, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="diagnosticDetails">
        /// Textual diagnostic information about the associated error. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, String diagnosticDetails)
            : this(applicationIdentity, diagnosticDetails, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="diagnosticDetails">
        /// Textual diagnostic information about the associated error. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, String diagnosticDetails, Guid correlationIdentifier)
            : this(applicationIdentity, diagnosticDetails, DefaultVerbosity, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="diagnosticDetails">
        /// Textual diagnostic information about the associated error. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, String diagnosticDetails, EventVerbosity verbosity)
            : this(applicationIdentity, diagnosticDetails, verbosity, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="diagnosticDetails">
        /// Textual diagnostic information about the associated error. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, String diagnosticDetails, EventVerbosity verbosity, Guid correlationIdentifier)
            : this(applicationIdentity, diagnosticDetails, verbosity, null, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="diagnosticDetails">
        /// Textual diagnostic information about the associated error. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, String diagnosticDetails, EventVerbosity verbosity, String description)
            : this(applicationIdentity, diagnosticDetails, verbosity, description, DefaultTimeStamp)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="diagnosticDetails">
        /// Textual diagnostic information about the associated error. This argument can be <see langword="null" />.
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
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, String diagnosticDetails, EventVerbosity verbosity, String description, Guid correlationIdentifier)
            : this(applicationIdentity, diagnosticDetails, verbosity, description, DefaultTimeStamp, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="diagnosticDetails">
        /// Textual diagnostic information about the associated error. This argument can be <see langword="null" />.
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
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, String diagnosticDetails, EventVerbosity verbosity, String description, DateTime timeStamp)
            : base(StaticCategory, verbosity, description, timeStamp)
        {
            ApplicationIdentity = applicationIdentity.RejectIf().IsNullOrEmpty(nameof(applicationIdentity));
            DiagnosticDetails = diagnosticDetails;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="diagnosticDetails">
        /// Textual diagnostic information about the associated error. This argument can be <see langword="null" />.
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
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ErrorEvent(String applicationIdentity, String diagnosticDetails, EventVerbosity verbosity, String description, DateTime timeStamp, Guid correlationIdentifier)
            : base(StaticCategory, verbosity, description, timeStamp, correlationIdentifier)
        {
            ApplicationIdentity = applicationIdentity.RejectIf().IsNullOrEmpty(nameof(applicationIdentity));
            DiagnosticDetails = diagnosticDetails;
        }

        /// <summary>
        /// Gets or sets a name or value that uniquely identifies the application in which the associated error occurred.
        /// </summary>
        [DataMember]
        public String ApplicationIdentity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets textual diagnostic information about the associated error.
        /// </summary>
        [DataMember]
        public String DiagnosticDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default name or value that uniquely identifies the application in which the associated error occurred.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DefaultApplicationIdentity = "unspecified";

        /// <summary>
        /// Represents the static event category for instances of the <see cref="ErrorEvent" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const EventCategory StaticCategory = EventCategory.Error;
    }
}