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
    /// Represents information about a general event.
    /// </summary>
    /// <remarks>
    /// <see cref="GeneralInformationEvent" /> is the default implementation of <see cref="IGeneralInformationEvent" />.
    /// </remarks>
    [DataContract]
    public class GeneralInformationEvent : Event, IGeneralInformationEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInformationEvent" /> class.
        /// </summary>
        public GeneralInformationEvent()
            : this(Array.Empty<String>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInformationEvent" /> class.
        /// </summary>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public GeneralInformationEvent(IEnumerable<String> labels)
            : this(labels, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInformationEvent" /> class.
        /// </summary>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public GeneralInformationEvent(IEnumerable<String> labels, EventVerbosity verbosity)
            : this(labels, verbosity, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInformationEvent" /> class.
        /// </summary>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public GeneralInformationEvent(IEnumerable<String> labels, EventVerbosity verbosity, String description)
            : this(labels, verbosity, description, DefaultTimeStamp)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInformationEvent" /> class.
        /// </summary>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public GeneralInformationEvent(IEnumerable<String> labels, EventVerbosity verbosity, String description, DateTime timeStamp)
            : base(StaticCategory, verbosity, description, timeStamp)
        {
            Labels = new List<String>((labels.RejectIf().IsNull(nameof(labels)).TargetArgument));
            Metadata = new Dictionary<String, String>();
        }

        /// <summary>
        /// Gets or sets a collection of textual labels that provide categorical and/or contextual information about the current
        /// <see cref="GeneralInformationEvent" />.
        /// </summary>
        [DataMember]
        public ICollection<String> Labels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a dictionary of metadata for the current <see cref="GeneralInformationEvent" />.
        /// </summary>
        [DataMember]
        public IDictionary<String, String> Metadata
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the static event category for instances of the <see cref="GeneralInformationEvent" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const EventCategory StaticCategory = EventCategory.GeneralInformation;
    }
}