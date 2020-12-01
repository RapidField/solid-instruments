// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests
{
    /// <summary>
    /// Encapsulates container configuration for testing.
    /// </summary>
    public class SimulatedDependencyPackage : DotNetNativeDependencyPackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedDependencyPackage" /> class.
        /// </summary>
        public SimulatedDependencyPackage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Creates a new collection of dependency modules for the package.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The package's dependency modules.
        /// </returns>
        protected override IEnumerable<IDependencyModule<ServiceCollection>> CreateModules(IConfiguration applicationConfiguration) => new[]
        {
            new SimulatedDependencyModule(applicationConfiguration)
        };
    }
}