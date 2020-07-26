// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about a security-relevant event.
    /// </summary>
    /// <remarks>
    /// <see cref="SecurityEventMessage" /> is the default implementation of <see cref="ISecurityEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class SecurityEventMessage : SecurityEventMessage<SecurityEvent>, ISecurityEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEventMessage" /> class.
        /// </summary>
        public SecurityEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public SecurityEventMessage(SecurityEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification about a security-relevant event.
    /// </summary>
    /// <remarks>
    /// <see cref="SecurityEventMessage{TEvent}" /> is the default implementation of <see cref="ISecurityEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class SecurityEventMessage<TEvent> : EventMessage<TEvent>, ISecurityEventMessage<TEvent>
        where TEvent : SecurityEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEventMessage{TEvent}" /> class.
        /// </summary>
        protected SecurityEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected SecurityEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}