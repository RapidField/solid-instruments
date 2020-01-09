// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information that is conveyed when an application has started.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationStartedEvent" /> is the default implementation of <see cref="IApplicationStartedEvent" />.
    /// </remarks>
    [DataContract]
    public class ApplicationStartedEvent : ApplicationStateEvent, IApplicationStartedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartedEvent" /> class.
        /// </summary>
        public ApplicationStartedEvent()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartedEvent" /> class.
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
        public ApplicationStartedEvent(String applicationIdentity)
            : this(applicationIdentity, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartedEvent" /> class.
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
        public ApplicationStartedEvent(String applicationIdentity, EventVerbosity verbosity)
            : this(applicationIdentity, verbosity, GetGranularDefaultDescription(applicationIdentity))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartedEvent" /> class.
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
        public ApplicationStartedEvent(String applicationIdentity, EventVerbosity verbosity, String description)
            : base(applicationIdentity, verbosity, description)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartedEvent" /> class.
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
        public ApplicationStartedEvent(String applicationIdentity, EventVerbosity verbosity, String description, DateTime timeStamp)
            : base(applicationIdentity, verbosity, description, timeStamp)
        {
            return;
        }

        /// <summary>
        /// Composes a granular default textual description of the event.
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
        [DebuggerHidden]
        private static String GetGranularDefaultDescription(String applicationIdentity) => $"The application \"{applicationIdentity.RejectIf().IsNullOrEmpty(nameof(applicationIdentity))}\" was started.";
    }
}