﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification when an application has been resumed.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationResumedEventMessage" /> is the default implementation of
    /// <see cref="IApplicationResumedEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class ApplicationResumedEventMessage : ApplicationResumedEventMessage<ApplicationResumedEvent>, IApplicationResumedEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationResumedEventMessage" /> class.
        /// </summary>
        public ApplicationResumedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationResumedEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public ApplicationResumedEventMessage(ApplicationResumedEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification when an application has been resumed.
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationResumedEventMessage{TEvent}" /> is the default implementation of
    /// <see cref="IApplicationResumedEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class ApplicationResumedEventMessage<TEvent> : ApplicationStateEventMessage<TEvent>, IApplicationResumedEventMessage<TEvent>
        where TEvent : ApplicationResumedEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationResumedEventMessage{TEvent}" /> class.
        /// </summary>
        protected ApplicationResumedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationResumedEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected ApplicationResumedEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}