// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents configuration information for an <see cref="ObjectFactory{TProductBase}" /> instance.
    /// </summary>
    /// <typeparam name="TProductBase">
    /// The base type from which objects produced by the associated factory derive.
    /// </typeparam>
    public sealed class ObjectFactoryConfiguration<TProductBase> : InstrumentConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryConfiguration{TProductBase}" /> class.
        /// </summary>
        public ObjectFactoryConfiguration()
            : base()
        {
            ProductionFunctions = new ObjectFactoryConfigurationProductionFunctions<TProductBase>();
        }

        /// <summary>
        /// Gets a collection of functions that produce supported types for the associated
        /// <see cref="ObjectFactory{TProductBase}" />.
        /// </summary>
        public ObjectFactoryConfigurationProductionFunctions<TProductBase> ProductionFunctions
        {
            get;
        }
    }
}