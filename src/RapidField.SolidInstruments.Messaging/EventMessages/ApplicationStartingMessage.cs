// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that notifies a system that an application is starting.
    /// </summary>
    [DataContract]
    public class ApplicationStartingMessage : ApplicationEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartingMessage" /> class.
        /// </summary>
        public ApplicationStartingMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartingMessage" /> class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the application that is starting.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationName" /> is <see langword="null" />.
        /// </exception>
        public ApplicationStartingMessage(String applicationName)
            : this(applicationName, ApplicationEventVerbosity.Normal)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartingMessage" /> class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the application that is starting.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event. The default value is <see cref="ApplicationEventVerbosity.Normal" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public ApplicationStartingMessage(String applicationName, ApplicationEventVerbosity verbosity)
            : base(new ApplicationEvent(ApplicationEventCategory.ApplicationState, verbosity, $"{applicationName.RejectIf().IsNullOrEmpty(nameof(applicationName)).TargetArgument} is starting."))
        {
            return;
        }
    }
}