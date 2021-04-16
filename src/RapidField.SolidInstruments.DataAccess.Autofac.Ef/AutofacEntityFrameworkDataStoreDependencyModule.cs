// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.DataAccess.Autofac.Ef.Extensions;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.InversionOfControl.Autofac.Extensions;
using System;
using System.ComponentModel;

namespace RapidField.SolidInstruments.DataAccess.Autofac.Ef
{
    /// <summary>
    /// Encapsulates Autofac container configuration for an Entity Framework data store connection and related data access
    /// dependencies.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the database session that is used by the produced repositories.
    /// </typeparam>
    /// <typeparam name="TRepositoryFactory">
    /// The type of the data access repository factory that is registered.
    /// </typeparam>
    public class AutofacEntityFrameworkDataStoreDependencyModule<TContext, TRepositoryFactory> : AutofacDataStoreDependencyModule
        where TContext : ConfiguredContext
        where TRepositoryFactory : EntityFrameworkRepositoryFactory<TContext>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AutofacEntityFrameworkDataStoreDependencyModule{TContext, TRepositoryFactory}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public AutofacEntityFrameworkDataStoreDependencyModule(IConfiguration applicationConfiguration)
            : this(applicationConfiguration, ConfiguredContext.GetConventionalDatabaseName<TContext>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AutofacEntityFrameworkDataStoreDependencyModule{TContext, TRepositoryFactory}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="dataStoreName">
        /// The name of the associated data store.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected AutofacEntityFrameworkDataStoreDependencyModule(IConfiguration applicationConfiguration, String dataStoreName)
            : base(applicationConfiguration, dataStoreName, dataStoreName)
        {
            return;
        }

        /// <summary>
        /// Configures the module.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override void Configure(ContainerBuilder configurator, IConfiguration applicationConfiguration)
        {
            configurator.RegisterApplicationConfiguration(applicationConfiguration);
            configurator.RegisterSupportingTypesForEntityFrameworkDataAccess<TContext, TRepositoryFactory>(applicationConfiguration);
            RegisterCustomComponents(configurator, applicationConfiguration);
        }

        /// <summary>
        /// Registers additional components.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected virtual void RegisterCustomComponents(ContainerBuilder configurator, IConfiguration applicationConfiguration)
        {
            return;
        }
    }
}