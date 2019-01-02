// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="DayOfWeek" /> enumeration with general purpose features.
    /// </summary>
    public static class DayOfWeekExtensions
    {
        /// <summary>
        /// Converts the current <see cref="DayOfWeek" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DayOfWeek" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="DayOfWeek" />.
        /// </returns>
        public static Byte[] ToByteArray(this DayOfWeek target) => BitConverter.GetBytes((Int32)target);
    }
}