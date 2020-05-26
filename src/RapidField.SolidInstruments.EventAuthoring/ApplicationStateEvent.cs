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
    /// Represents information about an application state change event.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationStateEvent" /> is the default implementation of <see cref="IApplicationStateEvent" />.
    /// </remarks>
    [DataContract]
    public class ApplicationStateEvent : Event, IApplicationStateEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateEvent" /> class.
        /// </summary>
        public ApplicationStateEvent()
            : this(DefaultApplicationIdentity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the associated application.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" />.
        /// </exception>
        public ApplicationStateEvent(String applicationIdentity)
            : this(applicationIdentity, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the associated application.
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
        public ApplicationStateEvent(String applicationIdentity, EventVerbosity verbosity)
            : this(applicationIdentity, verbosity, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the associated application.
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
        public ApplicationStateEvent(String applicationIdentity, EventVerbosity verbosity, String description)
            : this(applicationIdentity, verbosity, description, DefaultTimeStamp)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the associated application.
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
        public ApplicationStateEvent(String applicationIdentity, EventVerbosity verbosity, String description, DateTime timeStamp)
            : base(StaticCategory, verbosity, description, timeStamp)
        {
            ApplicationIdentity = applicationIdentity.RejectIf().IsNullOrEmpty(nameof(applicationIdentity));
            Metadata = new Dictionary<String, String>();
        }

        /// <summary>
        /// Gets or sets a name or value that uniquely identifies the associated application.
        /// </summary>
        [DataMember]
        public String ApplicationIdentity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a dictionary of metadata for the current <see cref="ApplicationStateEvent" />.
        /// </summary>
        [DataMember]
        public IDictionary<String, String> Metadata
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default name or value that uniquely identifies the associated application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DefaultApplicationIdentity = "unspecified";

        /// <summary>
        /// Represents the static event category for instances of the <see cref="ApplicationStateEvent" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const EventCategory StaticCategory = EventCategory.ApplicationState;
    }
}