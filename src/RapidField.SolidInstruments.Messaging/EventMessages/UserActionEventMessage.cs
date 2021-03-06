﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about a user action.
    /// </summary>
    /// <remarks>
    /// <see cref="UserActionEventMessage" /> is the default implementation of <see cref="IUserActionEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class UserActionEventMessage : UserActionEventMessage<UserActionEvent>, IUserActionEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserActionEventMessage" /> class.
        /// </summary>
        public UserActionEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActionEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public UserActionEventMessage(UserActionEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification about a user action.
    /// </summary>
    /// <remarks>
    /// <see cref="UserActionEventMessage{TEvent}" /> is the default implementation of
    /// <see cref="IUserActionEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class UserActionEventMessage<TEvent> : EventMessage<TEvent>, IUserActionEventMessage<TEvent>
        where TEvent : UserActionEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserActionEventMessage{TEvent}" /> class.
        /// </summary>
        protected UserActionEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActionEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected UserActionEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}