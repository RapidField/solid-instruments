// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.EventAuthoring.Extensions
{
    /// <summary>
    /// Extends the <see cref="IEventRegister" /> interface with general purpose event creation methods.
    /// </summary>
    public static class IEventRegisterExtensions
    {
        /// <summary>
        /// Creates a new <see cref="ErrorEvent" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IEventRegister" />.
        /// </param>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <returns>
        /// A new <see cref="ErrorEvent" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception" /> is <see langword="null" />.
        /// </exception>
        public static ErrorEvent Error(this IEventRegister target, Exception exception) => new(exception);

        /// <summary>
        /// Creates a new <see cref="ErrorEvent" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IEventRegister" />.
        /// </param>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <returns>
        /// A new <see cref="ErrorEvent" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public static ErrorEvent Error(this IEventRegister target, Exception exception, Guid correlationIdentifier) => new(exception, correlationIdentifier);

        /// <summary>
        /// Creates a new <see cref="ErrorEvent" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IEventRegister" />.
        /// </param>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <returns>
        /// A new <see cref="ErrorEvent" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" /> -or- <paramref name="exception" /> is
        /// <see langword="null" />.
        /// </exception>
        public static ErrorEvent Error(this IEventRegister target, String applicationIdentity, Exception exception) => new(applicationIdentity, exception);

        /// <summary>
        /// Creates a new <see cref="ErrorEvent" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IEventRegister" />.
        /// </param>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <returns>
        /// A new <see cref="ErrorEvent" />.
        /// </returns>
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
        public static ErrorEvent Error(this IEventRegister target, String applicationIdentity, Exception exception, Guid correlationIdentifier) => new(applicationIdentity, exception, correlationIdentifier);

        /// <summary>
        /// Creates a new <see cref="GeneralInformationEvent" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IEventRegister" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <returns>
        /// A new <see cref="GeneralInformationEvent" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public static GeneralInformationEvent GeneralInformation(this IEventRegister target, IEnumerable<String> labels) => new(labels);

        /// <summary>
        /// Creates a new <see cref="GeneralInformationEvent" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IEventRegister" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <returns>
        /// A new <see cref="GeneralInformationEvent" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public static GeneralInformationEvent GeneralInformation(this IEventRegister target, IEnumerable<String> labels, EventVerbosity verbosity) => new(labels, verbosity);

        /// <summary>
        /// Creates a new <see cref="GeneralInformationEvent" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IEventRegister" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <returns>
        /// A new <see cref="GeneralInformationEvent" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public static GeneralInformationEvent GeneralInformation(this IEventRegister target, IEnumerable<String> labels, EventVerbosity verbosity, String description) => new(labels, verbosity, description);

        /// <summary>
        /// Creates a new <see cref="GeneralInformationEvent" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IEventRegister" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
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
        /// <returns>
        /// A new <see cref="GeneralInformationEvent" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public static GeneralInformationEvent GeneralInformation(this IEventRegister target, IEnumerable<String> labels, EventVerbosity verbosity, String description, Guid correlationIdentifier) => new(labels, verbosity, description, correlationIdentifier);
    }
}