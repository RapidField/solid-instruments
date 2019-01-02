// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Prototype.Contracts.Models;
using RapidField.SolidInstruments.Prototype.DatabaseModel.Entities;

namespace RapidField.SolidInstruments.Prototype.DatabaseModel.Extensions
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
        public static NumberSeries ToPrototypeDatabaseEntity(this INumberSeries target) => new NumberSeries(target);
    }
}