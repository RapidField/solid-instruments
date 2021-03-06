﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about a general event.
    /// </summary>
    /// <remarks>
    /// <see cref="GeneralInformationEventMessage" /> is the default implementation of
    /// <see cref="IGeneralInformationEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class GeneralInformationEventMessage : GeneralInformationEventMessage<GeneralInformationEvent>, IGeneralInformationEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInformationEventMessage" /> class.
        /// </summary>
        public GeneralInformationEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInformationEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public GeneralInformationEventMessage(GeneralInformationEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification about a general event.
    /// </summary>
    /// <remarks>
    /// <see cref="GeneralInformationEventMessage{TEvent}" /> is the default implementation of
    /// <see cref="IGeneralInformationEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class GeneralInformationEventMessage<TEvent> : EventMessage<TEvent>, IGeneralInformationEventMessage<TEvent>
        where TEvent : GeneralInformationEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInformationEventMessage{TEvent}" /> class.
        /// </summary>
        protected GeneralInformationEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInformationEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected GeneralInformationEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}