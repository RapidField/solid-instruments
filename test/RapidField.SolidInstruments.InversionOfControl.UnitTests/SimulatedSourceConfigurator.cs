// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.InversionOfControl.UnitTests
{
    /// <summary>
    /// Represents a dependency injection container configurator that is used for testing.
    /// </summary>
    internal sealed class SimulatedSourceConfigurator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedSourceConfigurator" /> class.
        /// </summary>
        public SimulatedSourceConfigurator()
        {
            TestValue = 54;
        }

        /// <summary>
        /// Returns a new <see cref="SimulatedSourceContainer" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="SimulatedSourceContainer" />.
        /// </returns>
        public SimulatedSourceContainer Build() => new SimulatedSourceContainer(TestValue);

        /// <summary>
        /// Represents a test value that is injected into resolved objects.
        /// </summary>
        public readonly Int32 TestValue;
    }
}