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
    /// <typeparam name="TBaseEventHandler">
    /// The base class or implemented interface type that is shared by all event handler types that are registered by the module.
    /// </typeparam>
    public interface IEventHandlerModule<TConfigurator, TBaseEventHandler> : ICommandHandlerModule<TConfigurator, TBaseEventHandler>, IEventHandlerModule<TConfigurator>
        where TConfigurator : class, new()
        where TBaseEventHandler : IEventHandler
    {
    }

    /// <summary>
    /// Encapsulates container configuration for event handlers.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IEventHandlerModule<TConfigurator> : ICommandHandlerModule<TConfigurator>, IEventHandlerModule
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