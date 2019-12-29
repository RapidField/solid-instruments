// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.TextEncoding;
using System;
using System.Text;

namespace RapidField.SolidInstruments.ObjectComposition.UnitTests
{
    /// <summary>
    /// Represents an <see cref="ObjectFactory" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedEncodingFactory : ObjectFactory<Encoding>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedEncodingFactory" /> class.
        /// </summary>
        public SimulatedEncodingFactory()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedEncodingFactory" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public SimulatedEncodingFactory(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Configures the current <see cref="SimulatedEncodingFactory" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="SimulatedEncodingFactory" />.
        /// </param>
        protected override void Configure(ObjectFactoryConfiguration<Encoding> configuration)
        {
            configuration.StateControlMode = ConcurrencyControlMode.SingleThreadSpinLock;
            configuration.ProductionFunctions
                .Add(() => Base32Encoding.Default)
                .Add(() => Base32Encoding.ZBase32 as ZBase32Encoding);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedEncodingFactory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}