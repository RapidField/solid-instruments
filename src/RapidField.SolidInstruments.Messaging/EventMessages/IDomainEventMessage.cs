// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about domain activity.
    /// </summary>
    public interface IDomainEventMessage : IDomainEventMessage<DomainEvent>
    {
    }

    /// <summary>
    /// Represents a message that provides notification about domain activity.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    public interface IDomainEventMessage<TEvent> : IEventMessage<TEvent>
        where TEvent : class, IDomainEvent
    {
    }
}