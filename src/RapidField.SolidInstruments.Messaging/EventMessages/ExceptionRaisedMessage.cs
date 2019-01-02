// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.EventAuthoring.Extensions;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that notifies a system about a raised exception.
    /// </summary>
    [DataContract]
    public class ExceptionRaisedMessage : ApplicationEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedMessage" /> class.
        /// </summary>
        public ExceptionRaisedMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedMessage" /> class.
        /// </summary>
        /// <param name="exception">
        /// An object representing the reportable event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception" /> is <see langword="null" />.
        /// </exception>
        public ExceptionRaisedMessage(Exception exception)
            : this(exception, ApplicationEventVerbosity.Normal)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedMessage" /> class.
        /// </summary>
        /// <param name="exception">
        /// An object representing the reportable event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event. The default value is <see cref="ApplicationEventVerbosity.Normal" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public ExceptionRaisedMessage(Exception exception, ApplicationEventVerbosity verbosity)
            : this(exception, verbosity, Guid.NewGuid())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedMessage" /> class.
        /// </summary>
        /// <param name="exception">
        /// An object representing the reportable event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event. The default value is <see cref="ApplicationEventVerbosity.Normal" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ExceptionRaisedMessage(Exception exception, ApplicationEventVerbosity verbosity, Guid correlationIdentifier)
            : this(exception, verbosity, correlationIdentifier, Guid.NewGuid())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedMessage" /> class.
        /// </summary>
        /// <param name="exception">
        /// An object representing the reportable event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event. The default value is <see cref="ApplicationEventVerbosity.Normal" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <param name="identifier">
        /// A unique identifier for the message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="identifier" /> is
        /// equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ExceptionRaisedMessage(Exception exception, ApplicationEventVerbosity verbosity, Guid correlationIdentifier, Guid identifier)
            : base(exception.RejectIf().IsNull(nameof(exception)).TargetArgument.ComposeReportableEvent(verbosity), correlationIdentifier, identifier)
        {
            return;
        }
    }
}