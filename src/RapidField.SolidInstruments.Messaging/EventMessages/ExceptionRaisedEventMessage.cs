// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification when an <see cref="Exception" /> has been raised.
    /// </summary>
    /// <remarks>
    /// <see cref="ExceptionRaisedEventMessage" /> is the default implementation of <see cref="IExceptionRaisedEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class ExceptionRaisedEventMessage : ExceptionRaisedEventMessage<ExceptionRaisedEvent>, IExceptionRaisedEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedEventMessage" /> class.
        /// </summary>
        public ExceptionRaisedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public ExceptionRaisedEventMessage(ExceptionRaisedEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification when an <see cref="Exception" /> has been raised.
    /// </summary>
    /// <remarks>
    /// <see cref="ExceptionRaisedEventMessage{TEvent}" /> is the default implementation of
    /// <see cref="IExceptionRaisedEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class ExceptionRaisedEventMessage<TEvent> : ErrorEventMessage<TEvent>, IExceptionRaisedEventMessage<TEvent>
        where TEvent : ExceptionRaisedEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedEventMessage{TEvent}" /> class.
        /// </summary>
        protected ExceptionRaisedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected ExceptionRaisedEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}