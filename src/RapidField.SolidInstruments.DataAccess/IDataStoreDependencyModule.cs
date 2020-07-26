// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.InversionOfControl;
using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Encapsulates container configuration for a data store connection and related data access dependencies.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IDataStoreDependencyModule<TConfigurator> : IDataStoreDependencyModule, IDependencyModule<TConfigurator>
        where TConfigurator : class, new()
    {
    }

    /// <summary>
    /// Encapsulates container configuration for a data store connection and related data access dependencies.
    /// </summary>
    public interface IDataStoreDependencyModule : IDependencyModule
    {
        /// <summary>
        /// Gets the name of the associated data store.
        /// </summary>
        public String DataStoreName
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the module registers in-memory database components.
        /// </summary>
        public Boolean RegistersInMemoryDatabaseComponents
        {
            get;
        }
    }
}