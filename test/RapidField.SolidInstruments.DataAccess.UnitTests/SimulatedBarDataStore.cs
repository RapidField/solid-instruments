// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    /// <summary>
    /// Represents an entity data store that is used for testing.
    /// </summary>
    internal sealed class SimulatedBarDataStore : SimulatedDataStore<SimulatedBarEntity, Int32>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedBarDataStore" /> class.
        /// </summary>
        /// <param name="entities">
        /// A collection of values comprising the data store's entities.
        /// </param>
        public SimulatedBarDataStore(IEnumerable<Int32> values)
            : base(values)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedBarDataStore" /> class.
        /// </summary>
        /// <param name="entities">
        /// A collection of key-value pairs comprising the data store's entities.
        /// </param>
        public SimulatedBarDataStore(IEnumerable<SimulatedBarEntity> entities)
            : base(entities)
        {
            return;
        }

        /// <summary>
        /// Returns a new data store containing the default values.
        /// </summary>
        /// <returns>
        /// A new data store containing the default contents.
        /// </returns>
        public static SimulatedBarDataStore NewDefaultInstance() => new SimulatedBarDataStore(DefaultValues);

        /// <summary>
        /// Returns all entities that exist in the data store.
        /// </summary>
        /// <returns>
        /// All entities that exist in the data store.
        /// </returns>
        public override IQueryable<SimulatedBarEntity> GetAllEntities() => this.Select(element => new SimulatedBarEntity(element.Key, element.Value)).AsQueryable();

        /// <summary>
        /// Represents the default values for a data store.
        /// </summary>
        private static readonly Int32[] DefaultValues = { 0, 1, 2, 3, 4, 5, 6, 7 };
    }
}