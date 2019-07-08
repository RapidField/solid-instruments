// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.EventAuthoring.Extensions
{
    /// <summary>
    /// Extends the <see cref="Exception" /> class with event authoring features.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Compose a reportable application event for the current <see cref="Exception" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Exception" />.
        /// </param>
        /// <returns>
        /// A reportable <see cref="ApplicationEvent" /> representing the current <see cref="Exception" />.
        /// </returns>
        public static ApplicationEvent ComposeReportableEvent(this Exception target) => target.ComposeReportableEvent(ApplicationEventVerbosity.Normal);

        /// <summary>
        /// Compose a reportable application event for the current <see cref="Exception" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Exception" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event. The default value is <see cref="ApplicationEventVerbosity.Normal" />.
        /// </param>
        /// <returns>
        /// A reportable <see cref="ApplicationEvent" /> representing the current <see cref="Exception" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public static ApplicationEvent ComposeReportableEvent(this Exception target, ApplicationEventVerbosity verbosity) => target.ComposeReportableEvent(verbosity, null);

        /// <summary>
        /// Compose a reportable application event for the current <see cref="Exception" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Exception" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event. The default value is <see cref="ApplicationEventVerbosity.Normal" />.
        /// </param>
        /// <param name="environmentName">
        /// The name of the environment on which the exception was raised, or <see langword="null" /> to indicate the current
        /// environment.
        /// </param>
        /// <returns>
        /// A reportable <see cref="ApplicationEvent" /> representing the current <see cref="Exception" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public static ApplicationEvent ComposeReportableEvent(this Exception target, ApplicationEventVerbosity verbosity, String environmentName) => target.ComposeReportableEvent(verbosity, environmentName, null);

        /// <summary>
        /// Compose a reportable application event for the current <see cref="Exception" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Exception" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event. The default value is <see cref="ApplicationEventVerbosity.Normal" />.
        /// </param>
        /// <param name="environmentName">
        /// The name of the environment on which the exception was raised, or <see langword="null" /> to indicate the current
        /// environment.
        /// </param>
        /// <param name="userInformation">
        /// Information about a user whose action(s) caused the exception to be raised. This argument can be <see langword="null" />.
        /// </param>
        /// <returns>
        /// A reportable <see cref="ApplicationEvent" /> representing the current <see cref="Exception" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public static ApplicationEvent ComposeReportableEvent(this Exception target, ApplicationEventVerbosity verbosity, String environmentName, String userInformation) => target.ComposeReportableEvent(verbosity, environmentName, userInformation, TimeStamp.Current);

        /// <summary>
        /// Compose a reportable application event for the current <see cref="Exception" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Exception" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event. The default value is <see cref="ApplicationEventVerbosity.Normal" />.
        /// </param>
        /// <param name="environmentName">
        /// The name of the environment on which the exception was raised, or <see langword="null" /> to indicate the current
        /// environment.
        /// </param>
        /// <param name="userInformation">
        /// Information about a user whose action(s) caused the exception to be raised. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="occurrenceDateTime">
        /// A <see cref="DateTime" /> that indicates when the exception was raised. The default value is
        /// <see cref="TimeStamp.Current" />.
        /// </param>
        /// <returns>
        /// A reportable <see cref="ApplicationEvent" /> representing the current <see cref="Exception" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public static ApplicationEvent ComposeReportableEvent(this Exception target, ApplicationEventVerbosity verbosity, String environmentName, String userInformation, DateTime occurrenceDateTime) => new ApplicationEvent(ApplicationEventCategory.Error, verbosity, $"{target.Message} {target.StackTrace}", environmentName, userInformation, occurrenceDateTime);
    }
}