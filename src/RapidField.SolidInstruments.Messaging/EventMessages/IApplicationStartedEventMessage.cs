// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification when an application has been started.
    /// </summary>
    public interface IApplicationStartedEventMessage : IApplicationStartedEventMessage<ApplicationStartedEvent>
    {
    }

    /// <summary>
    /// Represents a message that provides notification when an application has been started.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    public interface IApplicationStartedEventMessage<TEvent> : IApplicationStateEventMessage<TEvent>
        where TEvent : class, IApplicationStartedEvent
    {
    }
}