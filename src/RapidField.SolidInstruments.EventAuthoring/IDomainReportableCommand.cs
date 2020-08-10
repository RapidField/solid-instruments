// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents a command to perform a domain action which can be described by an <see cref="IEvent" />.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the event that describes the command.
    /// </typeparam>
    public interface IDomainReportableCommand<TEvent> : IDomainCommand, IReportableCommand<TEvent>
        where TEvent : class, IDomainEvent
    {
    }
}