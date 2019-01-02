// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that notifies a system about an application event.
    /// </summary>
    [DataContract]
    public class ApplicationEventMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEventMessage" /> class.
        /// </summary>
        public ApplicationEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEventMessage" /> class.
        /// </summary>
        /// <param name="reportableObject">
        /// An object representing the reportable event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reportableObject" /> is <see langword="null" />.
        /// </exception>
        public ApplicationEventMessage(IReportable reportableObject)
            : this(reportableObject, Guid.NewGuid())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEventMessage" /> class.
        /// </summary>
        /// <param name="reportableObject">
        /// An object representing the reportable event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reportableObject" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ApplicationEventMessage(IReportable reportableObject, Guid correlationIdentifier)
            : this(reportableObject, correlationIdentifier, Guid.NewGuid())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEventMessage" /> class.
        /// </summary>
        /// <param name="reportableObject">
        /// An object representing the reportable event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <param name="identifier">
        /// A unique identifier for the message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reportableObject" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="identifier" /> is
        /// equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ApplicationEventMessage(IReportable reportableObject, Guid correlationIdentifier, Guid identifier)
            : this(reportableObject.RejectIf().IsNull(nameof(reportableObject)).TargetArgument.ComposeReportableEvent(), correlationIdentifier, identifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEventMessage" /> class.
        /// </summary>
        /// <param name="applicationEvent">
        /// The application event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationEvent" /> is <see langword="null" />.
        /// </exception>
        public ApplicationEventMessage(ApplicationEvent applicationEvent)
            : this(applicationEvent, Guid.NewGuid())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEventMessage" /> class.
        /// </summary>
        /// <param name="applicationEvent">
        /// The application event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationEvent" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ApplicationEventMessage(ApplicationEvent applicationEvent, Guid correlationIdentifier)
            : this(applicationEvent, correlationIdentifier, Guid.NewGuid())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEventMessage" /> class.
        /// </summary>
        /// <param name="applicationEvent">
        /// The application event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <param name="identifier">
        /// A unique identifier for the message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationEvent" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="identifier" /> is
        /// equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ApplicationEventMessage(ApplicationEvent applicationEvent, Guid correlationIdentifier, Guid identifier)
            : base(correlationIdentifier, identifier)
        {
            ApplicationEvent = applicationEvent.RejectIf().IsNull(nameof(applicationEvent));
        }

        /// <summary>
        /// Gets or sets the application event.
        /// </summary>
        [DataMember]
        public ApplicationEvent ApplicationEvent
        {
            get;
            set;
        }
    }
}