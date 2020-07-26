// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.InversionOfControl;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Encapsulates container configuration for a service bus connection and related transport dependencies.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface ITransportDependencyModule<TConfigurator> : ITransportDependencyModule, IDependencyModule<TConfigurator>
        where TConfigurator : class, new()
    {
    }

    /// <summary>
    /// Encapsulates container configuration for a service bus connection and related transport dependencies.
    /// </summary>
    public interface ITransportDependencyModule : IDependencyModule
    {
        /// <summary>
        /// Gets a value indicating whether or not the module registers in-memory service bus components.
        /// </summary>
        public Boolean RegistersInMemoryServiceBusComponents
        {
            get;
        }
    }
}