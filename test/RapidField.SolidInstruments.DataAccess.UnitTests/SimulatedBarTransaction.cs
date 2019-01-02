// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    /// <summary>
    /// Represents a <see cref="DataAccessTransaction" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedBarTransaction : SimulatedTransaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedBarTransaction" /> class.
        /// </summary>
        public SimulatedBarTransaction()
            : base()
        {
            return;
        }
    }
}