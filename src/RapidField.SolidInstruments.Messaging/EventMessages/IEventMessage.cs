// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about an event.
    /// </summary>
    public interface IEventMessage : IEventMessage<Event>
    {
    }

    /// <summary>
    /// Represents a message that provides notification about an event.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    public interface IEventMessage<TEvent> : IMessage
        where TEvent : class, IEvent
    {
        /// <summary>
        /// Gets or sets the associated event.
        /// </summary>
        public TEvent Event
        {
            get;
            set;
        }
    }
}