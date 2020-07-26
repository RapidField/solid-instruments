// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an system state change event.
    /// </summary>
    /// <remarks>
    /// <see cref="SystemStateEvent" /> is the default implementation of <see cref="ISystemStateEvent" />.
    /// </remarks>
    [DataContract]
    public class SystemStateEvent : Event, ISystemStateEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        public SystemStateEvent()
            : this(DefaultSystemIdentity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SystemStateEvent(Guid correlationIdentifier)
            : this(DefaultSystemIdentity, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        /// <param name="systemIdentity">
        /// A name or value that uniquely identifies the associated system.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="systemIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="systemIdentity" /> is <see langword="null" />.
        /// </exception>
        public SystemStateEvent(String systemIdentity)
            : this(systemIdentity, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        /// <param name="systemIdentity">
        /// A name or value that uniquely identifies the associated system.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="systemIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="systemIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SystemStateEvent(String systemIdentity, Guid correlationIdentifier)
            : this(systemIdentity, DefaultVerbosity, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        /// <param name="systemIdentity">
        /// A name or value that uniquely identifies the associated system.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="systemIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="systemIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public SystemStateEvent(String systemIdentity, EventVerbosity verbosity)
            : this(systemIdentity, verbosity, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        /// <param name="systemIdentity">
        /// A name or value that uniquely identifies the associated system.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="systemIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="systemIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SystemStateEvent(String systemIdentity, EventVerbosity verbosity, Guid correlationIdentifier)
            : this(systemIdentity, verbosity, null, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        /// <param name="systemIdentity">
        /// A name or value that uniquely identifies the associated system.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="systemIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="systemIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public SystemStateEvent(String systemIdentity, EventVerbosity verbosity, String description)
            : this(systemIdentity, verbosity, description, DefaultTimeStamp)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        /// <param name="systemIdentity">
        /// A name or value that uniquely identifies the associated system.
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
        /// <paramref name="systemIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="systemIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SystemStateEvent(String systemIdentity, EventVerbosity verbosity, String description, Guid correlationIdentifier)
            : this(systemIdentity, verbosity, description, DefaultTimeStamp, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        /// <param name="systemIdentity">
        /// A name or value that uniquely identifies the associated system.
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
        /// <paramref name="systemIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="systemIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public SystemStateEvent(String systemIdentity, EventVerbosity verbosity, String description, DateTime timeStamp)
            : base(StaticCategory, verbosity, description, timeStamp)
        {
            Metadata = new Dictionary<String, String>();
            SystemIdentity = systemIdentity.RejectIf().IsNullOrEmpty(nameof(systemIdentity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        /// <param name="systemIdentity">
        /// A name or value that uniquely identifies the associated system.
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
        /// <paramref name="systemIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="systemIdentity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public SystemStateEvent(String systemIdentity, EventVerbosity verbosity, String description, DateTime timeStamp, Guid correlationIdentifier)
            : base(StaticCategory, verbosity, description, timeStamp, correlationIdentifier)
        {
            Metadata = new Dictionary<String, String>();
            SystemIdentity = systemIdentity.RejectIf().IsNullOrEmpty(nameof(systemIdentity));
        }

        /// <summary>
        /// Gets a dictionary of metadata for the current <see cref="SystemStateEvent" />.
        /// </summary>
        [DataMember]
        public IDictionary<String, String> Metadata
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a name or value that uniquely identifies the associated system.
        /// </summary>
        [DataMember]
        public String SystemIdentity
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default name or value that uniquely identifies the associated system.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DefaultSystemIdentity = "unspecified";

        /// <summary>
        /// Represents the static event category for instances of the <see cref="SystemStateEvent" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const EventCategory StaticCategory = EventCategory.SystemState;
    }
}