// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification when an application has been resumed.
    /// </summary>
    public interface IApplicationResumedEventMessage : IApplicationResumedEventMessage<ApplicationResumedEvent>
    {
    }

    /// <summary>
    /// Represents a message that provides notification when an application has been resumed.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    public interface IApplicationResumedEventMessage<TEvent> : IApplicationStateEventMessage<TEvent>
        where TEvent : class, IApplicationResumedEvent
    {
    }
}