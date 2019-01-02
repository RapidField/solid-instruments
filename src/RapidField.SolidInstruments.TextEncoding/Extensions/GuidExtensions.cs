// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.TextEncoding.Extensions
{
    /// <summary>
    /// Extends the <see cref="Guid" /> structure with general purpose features.
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Converts the value of the current <see cref="Guid" /> to its equivalent <see cref="EnhancedReadabilityGuid" />
        /// representation.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Guid" />.
        /// </param>
        /// <returns>
        /// A <see cref="EnhancedReadabilityGuid" /> representation of the current <see cref="Guid" />.
        /// </returns>
        public static EnhancedReadabilityGuid ToEnhancedReadabilityGuid(this Guid target) => new EnhancedReadabilityGuid(target);
    }
}