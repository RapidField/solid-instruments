// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about a system state change event.
    /// </summary>
    /// <remarks>
    /// <see cref="SystemStateEventMessage" /> is the default implementation of <see cref="ISystemStateEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class SystemStateEventMessage : SystemStateEventMessage<SystemStateEvent>, ISystemStateEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEventMessage" /> class.
        /// </summary>
        public SystemStateEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public SystemStateEventMessage(SystemStateEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification about a system state change event.
    /// </summary>
    /// <remarks>
    /// <see cref="SystemStateEventMessage{TEvent}" /> is the default implementation of
    /// <see cref="ISystemStateEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class SystemStateEventMessage<TEvent> : EventMessage<TEvent>, ISystemStateEventMessage<TEvent>
        where TEvent : SystemStateEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEventMessage{TEvent}" /> class.
        /// </summary>
        protected SystemStateEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStateEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected SystemStateEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}