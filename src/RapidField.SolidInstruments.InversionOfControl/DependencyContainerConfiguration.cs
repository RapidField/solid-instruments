// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Represents configuration information for a <see cref="DependencyContainer{TContainer, TConfigurator}" /> instance.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures an associated container.
    /// </typeparam>
    public sealed class DependencyContainerConfiguration<TConfigurator> : InstrumentConfiguration
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContainerConfiguration{TConfigurator}" /> class.
        /// </summary>
        public DependencyContainerConfiguration()
            : base()
        {
            Configurator = new TConfigurator();
        }

        /// <summary>
        /// Gets an object that configures the associated container.
        /// </summary>
        public TConfigurator Configurator
        {
            get;
        }
    }
}