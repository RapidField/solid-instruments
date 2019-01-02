// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Prototype.Contracts.MessagingModels;
using RapidField.SolidInstruments.Prototype.Contracts.Models;

namespace RapidField.SolidInstruments.Prototype.Contracts.Extensions
{
    /// <summary>
    /// Extends the <see cref="INumberSeriesNumber" /> interface with type conversion features.
    /// </summary>
    public static class INumberSeriesNumberExtensions
    {
        /// <summary>
        /// Converts the specified model to a messaging model.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="INumberSeriesNumber" />.
        /// </param>
        /// <returns>
        /// The converted object.
        /// </returns>
        public static NumberSeriesNumber ToMessagingModel(this INumberSeriesNumber target) => new NumberSeriesNumber(target);
    }
}