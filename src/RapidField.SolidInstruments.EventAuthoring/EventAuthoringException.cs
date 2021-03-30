// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents an exception that is raised by an <see cref="IReportable" /> instance while composing an <see cref="IEvent" />.
    /// </summary>
    public class EventAuthoringException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventAuthoringException" /> class.
        /// </summary>
        public EventAuthoringException()
            : this(eventType: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAuthoringException" />
        /// </summary>
        /// <param name="eventType">
        /// The type of the event that was being composed when the exception was raised.
        /// </param>
        public EventAuthoringException(Type eventType)
            : this(eventType: eventType, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAuthoringException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public EventAuthoringException(String message)
            : this(message: message, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAuthoringException" />
        /// </summary>
        /// <param name="eventType">
        /// The type of the event that was being composed when the exception was raised.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public EventAuthoringException(Type eventType, Exception innerException)
            : this(eventType is null ? "An exception was raised while composing an event." : $"An exception was raised while composing an event instance of type {eventType}.", innerException)
        {
            EventType = eventType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAuthoringException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public EventAuthoringException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }

        /// <summary>
        /// Gets the type of the event that was being composed when the exception was raised.
        /// </summary>
        public Type EventType
        {
            get;
        }
    }
}