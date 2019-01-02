// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Prototype.DatabaseModel;
using RapidField.SolidInstruments.Prototype.Domain;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Prototype.ServiceApplication
{
    /// <summary>
    /// Encapsulates container configuration for the application.
    /// </summary>
    public class ApplicationDependencyPackage : DotNetNativeDependencyPackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDependencyPackage" /> class.
        /// </summary>
        public ApplicationDependencyPackage()
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
        protected override IEnumerable<IDependencyModule<ServiceCollection>> CreateModules(IConfiguration applicationConfiguration) => new IDependencyModule<ServiceCollection>[]
        {
            new ApplicationDependencyModule(applicationConfiguration),
            new PrototypeContractsDependencyModule(applicationConfiguration),
            new PrototypeDomainDependencyModule(applicationConfiguration)
        };
    }
}