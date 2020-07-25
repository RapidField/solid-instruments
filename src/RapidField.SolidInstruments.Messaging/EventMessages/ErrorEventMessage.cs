// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about an error event.
    /// </summary>
    /// <remarks>
    /// <see cref="ErrorEventMessage" /> is the default implementation of <see cref="IErrorEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class ErrorEventMessage : ErrorEventMessage<ErrorEvent>, IErrorEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventMessage" /> class.
        /// </summary>
        public ErrorEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public ErrorEventMessage(ErrorEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification about an error event.
    /// </summary>
    /// <remarks>
    /// <see cref="ErrorEventMessage{TEvent}" /> is the default implementation of <see cref="IErrorEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class ErrorEventMessage<TEvent> : EventMessage<TEvent>, IErrorEventMessage<TEvent>
        where TEvent : ErrorEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventMessage{TEvent}" /> class.
        /// </summary>
        protected ErrorEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected ErrorEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}