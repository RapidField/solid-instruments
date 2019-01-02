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
    /// Represents a message that notifies a system that an application is stopping.
    /// </summary>
    [DataContract]
    public class ApplicationStoppingMessage : ApplicationEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppingMessage" /> class.
        /// </summary>
        public ApplicationStoppingMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppingMessage" /> class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the application that is stopping.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationName" /> is <see langword="null" />.
        /// </exception>
        public ApplicationStoppingMessage(String applicationName)
            : this(applicationName, ApplicationEventVerbosity.Normal)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppingMessage" /> class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the application that is stopping.
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
        public ApplicationStoppingMessage(String applicationName, ApplicationEventVerbosity verbosity)
            : base(new ApplicationEvent(ApplicationEventCategory.ApplicationState, verbosity, $"{applicationName.RejectIf().IsNullOrEmpty(nameof(applicationName)).TargetArgument} is stopping."))
        {
            return;
        }
    }
}