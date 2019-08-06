// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Example.Contracts.MessagingModels;
using RapidField.SolidInstruments.Example.Contracts.Models;

namespace RapidField.SolidInstruments.Example.Contracts.Extensions
{
    /// <summary>
    /// Extends the <see cref="INumber" /> interface with type conversion features.
    /// </summary>
    public static class INumberExtensions
    {
        /// <summary>
        /// Converts the specified model to a messaging model.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="INumber" />.
        /// </param>
        /// <returns>
        /// The converted object.
        /// </returns>
        public static Number ToMessagingModel(this INumber target) => new Number(target);
    }
}