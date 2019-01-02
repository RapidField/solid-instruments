// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    /// <summary>
    /// Represents a <see cref="DataAccessRepository{TEntity}" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedFooRepository : SimulatedRepository<SimulatedFooEntity, String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedFooRepository" /> class.
        /// </summary>
        /// <param name="dataStore">
        /// The data store for the repository.
        /// </param>
        public SimulatedFooRepository(SimulatedFooDataStore dataStore)
            : base(dataStore)
        {
            return;
        }
    }
}