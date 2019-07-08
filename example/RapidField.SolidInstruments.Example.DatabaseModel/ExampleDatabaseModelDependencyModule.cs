// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Example.DatabaseModel.CommandHandlers;
using RapidField.SolidInstruments.Example.DatabaseModel.Commands;
using RapidField.SolidInstruments.Example.DatabaseModel.Repositories;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using System;

namespace RapidField.SolidInstruments.Example.DatabaseModel
{
    /// <summary>
    /// Encapsulates configuration for Example database model dependencies.
    /// </summary>
    public sealed class ExampleDatabaseModelDependencyModule : DotNetNativeDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleDatabaseModelDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public ExampleDatabaseModelDependencyModule(IConfiguration applicationConfiguration)
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
            configurator.AddScoped<ExampleSqlServerContext>();
            configurator.AddScoped<ExampleInMemoryContext>();
            configurator.AddScoped<ExampleContext, ExampleInMemoryContext>(provider => new ExampleInMemoryContext(provider.GetService<IConfiguration>(), "Example"));
            configurator.AddScoped<ExampleTransaction>();
            configurator.AddScoped<ExampleRepositoryFactory>();

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