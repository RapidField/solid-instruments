// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Processes a single <see cref="IEvent" />.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the event that is processed by the handler.
    /// </typeparam>
    public interface IEventHandler<in TEvent> : ICommandHandler<TEvent>, IEventHandler
        where TEvent : class, IEvent
    {
    }

    /// <summary>
    /// Processes a single <see cref="IEvent" />.
    /// </summary>
    public interface IEventHandler : ICommandHandler
    {
    }
}