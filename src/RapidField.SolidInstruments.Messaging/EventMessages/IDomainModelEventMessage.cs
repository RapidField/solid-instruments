﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using RapidField.SolidInstruments.EventAuthoring;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about an event related to an object that models a domain construct.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    public interface IDomainModelEventMessage<TModel, TEvent> : IDomainEventMessage<TEvent>
        where TModel : class, IDomainModel
        where TEvent : class, IDomainModelEvent<TModel>
    {
        /// <summary>
        /// Gets the resulting state of the associated domain model.
        /// </summary>
        public TModel Model
        {
            get;
        }
    }
}