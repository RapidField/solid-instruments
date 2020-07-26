// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Encapsulates container configuration for event handlers.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IEventHandlerModule<TConfigurator> : IEventHandlerModule, ICommandHandlerModule<TConfigurator>
        where TConfigurator : class, new()
    {
    }

    /// <summary>
    /// Encapsulates container configuration for event handlers.
    /// </summary>
    public interface IEventHandlerModule : ICommandHandlerModule
    {
    }
}