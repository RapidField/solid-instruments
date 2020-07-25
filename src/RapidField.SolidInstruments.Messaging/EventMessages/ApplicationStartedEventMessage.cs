// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification when an application has been started.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationStartedEventMessage" /> is the default implementation of
    /// <see cref="IApplicationStartedEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class ApplicationStartedEventMessage : ApplicationStartedEventMessage<ApplicationStartedEvent>, IApplicationStartedEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartedEventMessage" /> class.
        /// </summary>
        public ApplicationStartedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartedEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public ApplicationStartedEventMessage(ApplicationStartedEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification when an application has been started.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationStartedEventMessage{TEvent}" /> is the default implementation of
    /// <see cref="IApplicationStartedEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class ApplicationStartedEventMessage<TEvent> : ApplicationStateEventMessage<TEvent>, IApplicationStartedEventMessage<TEvent>
        where TEvent : ApplicationStartedEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartedEventMessage{TEvent}" /> class.
        /// </summary>
        protected ApplicationStartedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartedEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected ApplicationStartedEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}