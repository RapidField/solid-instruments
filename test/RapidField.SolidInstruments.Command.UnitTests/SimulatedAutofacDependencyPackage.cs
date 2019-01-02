// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.Autofac;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Command.UnitTests
{
    /// <summary>
    /// Represents an <see cref="SimulatedAutofacDependencyPackage" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedAutofacDependencyPackage : AutofacDependencyPackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedAutofacDependencyPackage" /> class.
        /// </summary>
        public SimulatedAutofacDependencyPackage()
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
        protected override IEnumerable<IDependencyModule<ContainerBuilder>> CreateModules(IConfiguration applicationConfiguration) => new IDependencyModule<ContainerBuilder>[]
        {
            new SimulatedAutofacDependencyModule(applicationConfiguration)
        };
    }
}