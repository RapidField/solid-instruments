// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Encapsulates container configuration for data access command handlers.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IDataAccessCommandHandlerModule<TConfigurator> : IDataAccessCommandHandlerModule, ICommandHandlerModule<TConfigurator>
        where TConfigurator : class, new()
    {
    }

    /// <summary>
    /// Encapsulates container configuration for data access command handlers.
    /// </summary>
    public interface IDataAccessCommandHandlerModule : ICommandHandlerModule
    {
    }
}