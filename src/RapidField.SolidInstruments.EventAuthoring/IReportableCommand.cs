// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents a command that can be described by an <see cref="IEvent" />.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the event that describes the command.
    /// </typeparam>
    public interface IReportableCommand<TEvent> : IReportableCommand, IReportable<TEvent>
        where TEvent : class, IEvent
    {
    }

    /// <summary>
    /// Represents a command that can be described by an <see cref="IEvent" />.
    /// </summary>
    public interface IReportableCommand : ICommand, IReportable
    {
        /// <summary>
        /// Gets the type of the event that describes the current <see cref="IReportableCommand" />.
        /// </summary>
        public Type EventType
        {
            get;
        }
    }
}