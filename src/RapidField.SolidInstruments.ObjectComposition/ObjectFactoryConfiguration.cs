// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents configuration information for an <see cref="ObjectFactory" /> instance.
    /// </summary>
    public sealed class ObjectFactoryConfiguration : ObjectFactoryConfiguration<Object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryConfiguration" /> class.
        /// </summary>
        public ObjectFactoryConfiguration()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryConfiguration" /> class.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information that is wrapped by the new configuration.
        /// </param>
        [DebuggerHidden]
        internal ObjectFactoryConfiguration(ObjectFactoryConfiguration<Object> configuration)
            : base(configuration.ProductionFunctions)
        {
            return;
        }
    }

    /// <summary>
    /// Represents configuration information for an <see cref="ObjectFactory{TProductBase}" /> instance.
    /// </summary>
    /// <typeparam name="TProductBase">
    /// The base type from which objects produced by the associated factory derive.
    /// </typeparam>
    public class ObjectFactoryConfiguration<TProductBase> : InstrumentConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryConfiguration{TProductBase}" /> class.
        /// </summary>
        public ObjectFactoryConfiguration()
            : this(new ObjectFactoryConfigurationProductionFunctions<TProductBase>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryConfiguration{TProductBase}" /> class.
        /// </summary>
        /// <param name="productionFunctions">
        /// A collection of functions that produce supported types for the associated <see cref="ObjectFactory{TProductBase}" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="productionFunctions" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal ObjectFactoryConfiguration(ObjectFactoryConfigurationProductionFunctions<TProductBase> productionFunctions)
            : base()
        {
            ProductionFunctions = productionFunctions.RejectIf().IsNull(nameof(productionFunctions));
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