// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information that is conveyed when an <see cref="Exception" /> has been raised.
    /// </summary>
    /// <remarks>
    /// <see cref="ExceptionRaisedEvent" /> is the default implementation of <see cref="IExceptionRaisedEvent" />.
    /// </remarks>
    [DataContract]
    public class ExceptionRaisedEvent : ErrorEvent, IExceptionRaisedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedEvent" /> class.
        /// </summary>
        public ExceptionRaisedEvent()
            : base()
        {
            ExceptionTypeFullName = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedEvent" /> class.
        /// </summary>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception" /> is <see langword="null" />.
        /// </exception>
        public ExceptionRaisedEvent(Exception exception)
            : this(DefaultApplicationIdentity, exception)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" /> -or- <paramref name="exception" /> is
        /// <see langword="null" />.
        /// </exception>
        public ExceptionRaisedEvent(String applicationIdentity, Exception exception)
            : this(applicationIdentity, exception, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" /> -or- <paramref name="exception" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public ExceptionRaisedEvent(String applicationIdentity, Exception exception, EventVerbosity verbosity)
            : this(applicationIdentity, exception, verbosity, DefaultTimeStamp)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedEvent" /> class.
        /// </summary>
        /// <param name="applicationIdentity">
        /// A name or value that uniquely identifies the application in which the associated error occurred.
        /// </param>
        /// <param name="exception">
        /// An associated exception.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="timeStamp">
        /// A <see cref="DateTime" /> that indicates when the event occurred. The default value is <see cref="TimeStamp.Current" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationIdentity" /> is <see langword="null" /> -or- <paramref name="exception" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public ExceptionRaisedEvent(String applicationIdentity, Exception exception, EventVerbosity verbosity, DateTime timeStamp)
            : base(applicationIdentity, exception.RejectIf().IsNull(nameof(exception)).TargetArgument.StackTrace, verbosity, exception.Message, timeStamp)
        {
            ExceptionTypeFullName = exception?.GetType().FullName;
        }

        /// <summary>
        /// Gets or sets the full name of the type of the associated <see cref="Exception" /> that was raised.
        /// </summary>
        [DataMember]
        public String ExceptionTypeFullName
        {
            get;
            set;
        }
    }
}