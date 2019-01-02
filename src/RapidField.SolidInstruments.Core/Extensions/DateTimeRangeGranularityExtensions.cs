// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="DateTimeRangeGranularity" /> enumeration with general purpose features.
    /// </summary>
    public static class DateTimeRangeGranularityExtensions
    {
        /// <summary>
        /// Converts the current <see cref="DateTimeRangeGranularity" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTimeRangeGranularity" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="DateTimeRangeGranularity" />.
        /// </returns>
        public static Byte[] ToByteArray(this DateTimeRangeGranularity target) => BitConverter.GetBytes((Int32)target);
    }
}