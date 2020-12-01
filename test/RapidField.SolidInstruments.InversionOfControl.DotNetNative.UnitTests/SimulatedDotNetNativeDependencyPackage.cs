// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.InversionOfControl.DotNetNative.UnitTests
{
    /// <summary>
    /// Represents an <see cref="SimulatedDotNetNativeDependencyPackage" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedDotNetNativeDependencyPackage : DotNetNativeDependencyPackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedDotNetNativeDependencyPackage" /> class.
        /// </summary>
        public SimulatedDotNetNativeDependencyPackage()
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
            new SimulatedDotNetNativeDependencyModule(applicationConfiguration)
        };
    }
}