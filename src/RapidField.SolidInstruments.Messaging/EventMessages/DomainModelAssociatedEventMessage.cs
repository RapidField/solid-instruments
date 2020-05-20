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
    /// Represents a message that provides notification about an event related to an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelAssociatedEventMessage{TModel, TEvent}" /> is the default implementation of
    /// <see cref="IDomainModelAssociatedEventMessage{TModel, TEvent}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class DomainModelAssociatedEventMessage<TModel, TEvent> : DomainModelEventMessage<TModel, TEvent>, IDomainModelAssociatedEventMessage<TModel, TEvent>
        where TModel : class, IDomainModel
        where TEvent : DomainModelAssociatedEvent<TModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelAssociatedEventMessage{TModel, TEvent}" /> class.
        /// </summary>
        protected DomainModelAssociatedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelAssociatedEventMessage{TModel, TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected DomainModelAssociatedEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelAssociatedEventMessage{TModel, TEvent}" /> class.
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
        protected DomainModelAssociatedEventMessage(TEvent eventObject, Guid correlationIdentifier)
            : base(eventObject, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelAssociatedEventMessage{TModel, TEvent}" /> class.
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
        protected DomainModelAssociatedEventMessage(TEvent eventObject, Guid correlationIdentifier, Guid identifier)
            : base(eventObject, correlationIdentifier, identifier)
        {
            return;
        }
    }
}