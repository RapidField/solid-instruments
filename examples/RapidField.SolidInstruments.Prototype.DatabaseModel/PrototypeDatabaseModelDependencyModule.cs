// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Prototype.DatabaseModel.CommandHandlers;
using RapidField.SolidInstruments.Prototype.DatabaseModel.Commands;
using RapidField.SolidInstruments.Prototype.DatabaseModel.Repositories;
using System;

namespace RapidField.SolidInstruments.Prototype.DatabaseModel
{
    /// <summary>
    /// Encapsulates configuration for Prototype database model dependencies.
    /// </summary>
    public sealed class PrototypeDatabaseModelDependencyModule : DotNetNativeDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrototypeDatabaseModelDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public PrototypeDatabaseModelDependencyModule(IConfiguration applicationConfiguration)
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
            configurator.AddScoped<PrototypeSqlServerContext>();
            configurator.AddScoped<PrototypeInMemoryContext>();
            configurator.AddScoped<PrototypeContext, PrototypeInMemoryContext>(provider => new PrototypeInMemoryContext(provider.GetService<IConfiguration>(), "Prototype"));
            configurator.AddScoped<PrototypeTransaction>();
            configurator.AddScoped<PrototypeRepositoryFactory>();

            // Register repositories.
            configurator.AddScoped<NumberRepository>();
            configurator.AddScoped<NumberSeriesNumberRepository>();
            configurator.AddScoped<NumberSeriesRepository>();

            // Register command handlers.
            configurator.AddTransient<ICommandHandler<AddFibonacciNumberCommand>, AddFibonacciNumberCommandHandler>();
            configurator.AddTransient<ICommandHandler<GetFibonacciNumberValuesCommand>, GetFibonacciNumberValuesCommandHandler>();
        }
    }
}