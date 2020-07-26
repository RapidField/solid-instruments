// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification when an application has been paused.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationPausedEventMessage" /> is the default implementation of <see cref="IApplicationPausedEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class ApplicationPausedEventMessage : ApplicationPausedEventMessage<ApplicationPausedEvent>, IApplicationPausedEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPausedEventMessage" /> class.
        /// </summary>
        public ApplicationPausedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPausedEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public ApplicationPausedEventMessage(ApplicationPausedEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification when an application has been paused.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationPausedEventMessage{TEvent}" /> is the default implementation of
    /// <see cref="IApplicationPausedEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class ApplicationPausedEventMessage<TEvent> : ApplicationStateEventMessage<TEvent>, IApplicationPausedEventMessage<TEvent>
        where TEvent : ApplicationPausedEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPausedEventMessage{TEvent}" /> class.
        /// </summary>
        protected ApplicationPausedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPausedEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected ApplicationPausedEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}