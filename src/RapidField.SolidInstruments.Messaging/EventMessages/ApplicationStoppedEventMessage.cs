// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification when an application has been stopped.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationStoppedEventMessage" /> is the default implementation of
    /// <see cref="IApplicationStoppedEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class ApplicationStoppedEventMessage : ApplicationStoppedEventMessage<ApplicationStoppedEvent>, IApplicationStoppedEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppedEventMessage" /> class.
        /// </summary>
        public ApplicationStoppedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppedEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public ApplicationStoppedEventMessage(ApplicationStoppedEvent eventObject)
            : base(eventObject)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppedEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ApplicationStoppedEventMessage(ApplicationStoppedEvent eventObject, Guid correlationIdentifier)
            : base(eventObject, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppedEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <param name="identifier">
        /// A unique identifier for the message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="identifier" /> is
        /// equal to <see cref="Guid.Empty" />.
        /// </exception>
        public ApplicationStoppedEventMessage(ApplicationStoppedEvent eventObject, Guid correlationIdentifier, Guid identifier)
            : base(eventObject, correlationIdentifier, identifier)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification when an application has been stopped.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationStoppedEventMessage{TEvent}" /> is the default implementation of
    /// <see cref="IApplicationStoppedEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class ApplicationStoppedEventMessage<TEvent> : ApplicationStateEventMessage<TEvent>, IApplicationStoppedEventMessage<TEvent>
        where TEvent : ApplicationStoppedEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppedEventMessage{TEvent}" /> class.
        /// </summary>
        protected ApplicationStoppedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppedEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected ApplicationStoppedEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppedEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected ApplicationStoppedEventMessage(TEvent eventObject, Guid correlationIdentifier)
            : base(eventObject, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStoppedEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <param name="identifier">
        /// A unique identifier for the message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="identifier" /> is
        /// equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected ApplicationStoppedEventMessage(TEvent eventObject, Guid correlationIdentifier, Guid identifier)
            : base(eventObject, correlationIdentifier, identifier)
        {
            return;
        }
    }
}