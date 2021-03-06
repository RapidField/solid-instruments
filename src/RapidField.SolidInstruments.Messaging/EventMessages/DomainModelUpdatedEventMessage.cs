﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about an update to an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelUpdatedEventMessage{TModel, TEvent}" /> is the default implementation of
    /// <see cref="IDomainModelUpdatedEventMessage{TModel, TEvent}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class DomainModelUpdatedEventMessage<TModel, TEvent> : DomainModelEventMessage<TModel, TEvent>, IDomainModelUpdatedEventMessage<TModel, TEvent>
        where TModel : class, IDomainModel
        where TEvent : DomainModelUpdatedEvent<TModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelUpdatedEventMessage{TModel, TEvent}" /> class.
        /// </summary>
        protected DomainModelUpdatedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelUpdatedEventMessage{TModel, TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected DomainModelUpdatedEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}