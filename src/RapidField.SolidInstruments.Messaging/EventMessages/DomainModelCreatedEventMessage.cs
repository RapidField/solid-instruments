// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about the creation of an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelCreatedEventMessage{TModel, TEvent}" /> is the default implementation of
    /// <see cref="IDomainModelCreatedEventMessage{TModel, TEvent}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class DomainModelCreatedEventMessage<TModel, TEvent> : DomainModelEventMessage<TModel, TEvent>, IDomainModelCreatedEventMessage<TModel, TEvent>
        where TModel : class, IDomainModel
        where TEvent : DomainModelCreatedEvent<TModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCreatedEventMessage{TModel, TEvent}" /> class.
        /// </summary>
        protected DomainModelCreatedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCreatedEventMessage{TModel, TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected DomainModelCreatedEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}