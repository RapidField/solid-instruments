// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.DataAccess.DotNetNative.Extensions;
using RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.CommandHandlers;
using RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.Commands;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests
{
    /// <summary>
    /// Encapsulates configuration for Simulated database model dependencies.
    /// </summary>
    public sealed class SimulatedDatabaseModelDependencyModule : DotNetNativeDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedDatabaseModelDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public SimulatedDatabaseModelDependencyModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
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
        protected override void Configure(ServiceCollection configurator, IConfiguration applicationConfiguration)
        {
            // Register unit-of-work types.
            configurator.AddScoped<SimulatedContext, SimulatedInMemoryContext>(provider => new(provider.GetService<IConfiguration>(), "Simulated"));
            configurator.AddScoped<SimulatedRepositoryFactory>();

            // Register data access command handlers.
            configurator.AddDataAccessCommandHandler<AddFibonacciNumberCommand, AddFibonacciNumberCommandHandler>();
            configurator.AddDataAccessCommandHandler<GetFibonacciNumberValuesCommand, IEnumerable<Int64>, GetFibonacciNumberValuesCommandHandler>();
        }
    }
}