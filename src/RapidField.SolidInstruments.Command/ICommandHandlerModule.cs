// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.InversionOfControl;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Encapsulates container configuration for command handlers.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface ICommandHandlerModule<TConfigurator> : ICommandHandlerModule, IDependencyModule<TConfigurator>
        where TConfigurator : class, new()
    {
    }

    /// <summary>
    /// Encapsulates container configuration for command handlers.
    /// </summary>
    public interface ICommandHandlerModule : IDependencyModule
    {
    }
}