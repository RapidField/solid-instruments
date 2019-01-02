// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    /// <summary>
    /// Represents an entity that is used for testing.
    /// </summary>
    internal sealed class SimulatedBarEntity : SimulatedEntity<Int32>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedBarEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// The entity's identifier.
        /// </param>
        /// <param name="value">
        /// The entity's value.
        /// </param>
        public SimulatedBarEntity(Int32 value)
            : base(value)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedBarEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// The entity's identifier.
        /// </param>
        /// <param name="value">
        /// The entity's value.
        /// </param>
        public SimulatedBarEntity(Guid identifier, Int32 value)
            : base(identifier, value)
        {
            return;
        }
    }
}