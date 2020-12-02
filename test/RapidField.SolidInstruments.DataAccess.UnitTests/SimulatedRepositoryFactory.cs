// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.ObjectComposition;
using System;

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    /// <summary>
    /// Represents an <see cref="ObjectFactory" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedRepositoryFactory : ObjectFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedRepositoryFactory" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public SimulatedRepositoryFactory(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Configures the current <see cref="SimulatedInstrumentFactory" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="SimulatedInstrumentFactory" />.
        /// </param>
        protected override void Configure(ObjectFactoryConfiguration configuration)
        {
            configuration.StateControlMode = ConcurrencyControlMode.SingleThreadSpinLock;
            configuration.ProductionFunctions
                .Add(() => new SimulatedBarRepository(BarData))
                .Add(() => new SimulatedFooRepository(FooData));
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedRepositoryFactory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents a store for <see cref="SimulatedBarEntity" /> data.
        /// </summary>
        private static SimulatedBarDataStore BarData => new(new Int32[] { 1, 2, 3, 4, 5 });

        /// <summary>
        /// Represents a store for <see cref="SimulatedFooEntity" /> data.
        /// </summary>
        private static SimulatedFooDataStore FooData => new(new String[] { "foo1", "foo2", "foo3", "foo4", "foo5" });
    }
}