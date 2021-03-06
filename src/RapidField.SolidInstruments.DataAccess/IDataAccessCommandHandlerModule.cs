﻿// =================================================================================================================================
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
    /// <typeparam name="TBaseDataAccessCommandHandler">
    /// The base class or implemented interface type that is shared by all data access command handler types that are registered by
    /// the module.
    /// </typeparam>
    public interface IDataAccessCommandHandlerModule<TConfigurator, TBaseDataAccessCommandHandler> : ICommandHandlerModule<TConfigurator, TBaseDataAccessCommandHandler>, IDataAccessCommandHandlerModule<TConfigurator>
        where TConfigurator : class, new()
        where TBaseDataAccessCommandHandler : IDataAccessCommandHandler
    {
    }

    /// <summary>
    /// Encapsulates container configuration for data access command handlers.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IDataAccessCommandHandlerModule<TConfigurator> : ICommandHandlerModule<TConfigurator>, IDataAccessCommandHandlerModule
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