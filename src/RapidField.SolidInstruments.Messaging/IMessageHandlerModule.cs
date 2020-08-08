// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Encapsulates container configuration for message handlers.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IMessageHandlerModule<TConfigurator> : IMessageHandlerModule
        where TConfigurator : class, new()
    {
    }

    /// <summary>
    /// Encapsulates container configuration for message handlers.
    /// </summary>
    public interface IMessageHandlerModule : IDependencyModule
    {
        /// <summary>
        /// Gets the collection of non-abstract public message types defined by <see cref="TargetAssembly" /> for which handler
        /// types are registered by the current <see cref="IMessageHandlerModule" />.
        /// </summary>
        public IEnumerable<Type> MessageTypes
        {
            get;
        }

        /// <summary>
        /// Gets the assembly from which message types are identified for handler registration.
        /// </summary>
        public Assembly TargetAssembly
        {
            get;
        }
    }
}