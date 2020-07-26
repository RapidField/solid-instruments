// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about an application state change event.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationStateEventMessage" /> is the default implementation of <see cref="IApplicationStateEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class ApplicationStateEventMessage : ApplicationStateEventMessage<ApplicationStateEvent>, IApplicationStateEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateEventMessage" /> class.
        /// </summary>
        public ApplicationStateEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public ApplicationStateEventMessage(ApplicationStateEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification about an application state change event.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationStateEventMessage{TEvent}" /> is the default implementation of
    /// <see cref="IApplicationStateEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class ApplicationStateEventMessage<TEvent> : EventMessage<TEvent>, IApplicationStateEventMessage<TEvent>
        where TEvent : ApplicationStateEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateEventMessage{TEvent}" /> class.
        /// </summary>
        protected ApplicationStateEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected ApplicationStateEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}