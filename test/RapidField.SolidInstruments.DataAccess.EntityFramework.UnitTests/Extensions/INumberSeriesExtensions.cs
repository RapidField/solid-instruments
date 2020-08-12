// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.Entities;
using RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.Models;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.Extensions
{
    /// <summary>
    /// Extends the <see cref="INumberSeries" /> interface with type conversion features.
    /// </summary>
    public static class INumberSeriesExtensions
    {
        /// <summary>
        /// Converts the specified model to a messaging model.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="INumberSeries" />.
        /// </param>
        /// <returns>
        /// The converted object.
        /// </returns>
        public static NumberSeries ToSimulatedDatabaseEntity(this INumberSeries target) => new NumberSeries(target);
    }
}