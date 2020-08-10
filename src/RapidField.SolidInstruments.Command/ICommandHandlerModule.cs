// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Encapsulates container configuration for command handlers.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    /// <typeparam name="TBaseCommandHandler">
    /// The base class or implemented interface type that is shared by all command handler types that are registered by the module.
    /// </typeparam>
    public interface ICommandHandlerModule<TConfigurator, TBaseCommandHandler> : ICommandHandlerModule, IDependencyModule<TConfigurator>
        where TConfigurator : class, new()
        where TBaseCommandHandler : ICommandHandler
    {
    }

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
        /// <summary>
        /// Gets the base class or implemented interface type that is shared by all command handler types that are registered by the
        /// current <see cref="ICommandHandlerModule" />.
        /// </summary>
        public Type BaseCommandHandlerType
        {
            get;
        }

        /// <summary>
        /// Gets the collection of non-abstract public class types defined by <see cref="TargetAssembly" /> that are registered by
        /// the current <see cref="ICommandHandlerModule" />.
        /// </summary>
        public IEnumerable<Type> MatchedTypes
        {
            get;
        }

        /// <summary>
        /// Gets the assembly from which command handler types are registered by the current <see cref="ICommandHandlerModule" />.
        /// </summary>
        public Assembly TargetAssembly
        {
            get;
        }
    }
}