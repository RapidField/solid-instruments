// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="TimeSpan" /> structure with general purpose features.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Converts the current <see cref="TimeSpan" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="TimeSpan" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="TimeSpan" />.
        /// </returns>
        public static Byte[] ToByteArray(this TimeSpan target) => BitConverter.GetBytes(target.Ticks);

        /// <summary>
        /// Converts the value of the current <see cref="TimeSpan" /> to its equivalent serialized string representation.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="TimeSpan" />.
        /// </param>
        /// <returns>
        /// A serialized string representation of the current <see cref="TimeSpan" />.
        /// </returns>
        public static String ToSerializedString(this TimeSpan target) => target.ToString("G");
    }
}