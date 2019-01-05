// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Prototype.Contracts.Models;
using RapidField.SolidInstruments.Prototype.Domain.Models;

namespace RapidField.SolidInstruments.Prototype.Domain.Extensions
{
    /// <summary>
    /// Extends the <see cref="INumber" /> interface with type conversion features.
    /// </summary>
    public static class INumberExtensions
    {
        /// <summary>
        /// Converts the specified model to a domain model.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="INumber" />.
        /// </param>
        /// <returns>
        /// The converted object.
        /// </returns>
        public static Number ToDomainModel(this INumber target) => new Number(target);
    }
}