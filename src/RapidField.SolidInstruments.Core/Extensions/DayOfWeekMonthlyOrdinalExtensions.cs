// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="DayOfWeekMonthlyOrdinal" /> enumeration with general purpose features.
    /// </summary>
    public static class DayOfWeekMonthlyOrdinalExtensions
    {
        /// <summary>
        /// Converts the current <see cref="DayOfWeekMonthlyOrdinal" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DayOfWeekMonthlyOrdinal" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="DayOfWeekMonthlyOrdinal" />.
        /// </returns>
        public static Byte[] ToByteArray(this DayOfWeekMonthlyOrdinal target) => BitConverter.GetBytes((Int32)target);
    }
}