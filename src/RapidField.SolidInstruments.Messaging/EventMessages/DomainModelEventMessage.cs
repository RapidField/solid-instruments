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
    /// Represents a message that provides notification about an event related to an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelEventMessage{TModel, TEvent}" /> is the default implementation of
    /// <see cref="IDomainModelEventMessage{TModel, TEvent}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class DomainModelEventMessage<TModel, TEvent> : DomainEventMessage<TEvent>, IDomainModelEventMessage<TModel, TEvent>
        where TModel : class, IDomainModel
        where TEvent : DomainModelEvent<TModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEventMessage{TModel, TEvent}" /> class.
        /// </summary>
        protected DomainModelEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEventMessage{TModel, TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected DomainModelEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }

        /// <summary>
        /// Gets the resulting state of the associated domain model.
        /// </summary>
        [IgnoreDataMember]
        public TModel Model => Event.Model;
    }
}