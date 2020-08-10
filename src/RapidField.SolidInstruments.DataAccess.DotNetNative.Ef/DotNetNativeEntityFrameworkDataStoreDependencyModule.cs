// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.DataAccess.DotNetNative.Ef.Extensions;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.DataAccess.DotNetNative.Ef
{
    /// <summary>
    /// Encapsulates native .NET container configuration for an Entity Framework data store connection and related data access
    /// dependencies.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the database session that is used by the produced repositories.
    /// </typeparam>
    /// <typeparam name="TRepositoryFactory">
    /// The type of the data access repository factory that is registered.
    /// </typeparam>
    public class DotNetNativeEntityFrameworkDataStoreDependencyModule<TContext, TRepositoryFactory> : DotNetNativeDataStoreDependencyModule
        where TContext : ConfiguredContext
        where TRepositoryFactory : EntityFrameworkRepositoryFactory<TContext>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DotNetNativeEntityFrameworkDataStoreDependencyModule{TContext, TRepositoryFactory}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public DotNetNativeEntityFrameworkDataStoreDependencyModule(IConfiguration applicationConfiguration)
            : this(applicationConfiguration, ConfiguredContext.GetConventionalDatabaseName<TContext>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DotNetNativeEntityFrameworkDataStoreDependencyModule{TContext, TRepositoryFactory}" /> class.
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
        [DebuggerHidden]
        private DotNetNativeEntityFrameworkDataStoreDependencyModule(IConfiguration applicationConfiguration, String dataStoreName)
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
        protected sealed override void Configure(ServiceCollection configurator, IConfiguration applicationConfiguration)
        {
            configurator.AddApplicationConfiguration(applicationConfiguration);
            configurator.AddSupportingTypesForEntityFrameworkDataAccess<TContext, TRepositoryFactory>(applicationConfiguration);
        }
    }
}