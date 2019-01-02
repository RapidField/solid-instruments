// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Double" /> structure with general purpose features.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Rounds the current <see cref="Double" /> to the specified number of fractional digits.
        /// </summary>
        /// <remarks>
        /// This method rounds away from zero by default.
        /// </remarks>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Double" /> to.
        /// </param>
        /// <returns>
        /// The current <see cref="Double" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 15.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Double" />.
        /// </exception>
        public static Double RoundedTo(this Double target, Int32 digits) => Math.Round(target, digits, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Rounds the current <see cref="Double" /> to the specified number of fractional digits.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Double" /> to.
        /// </param>
        /// <param name="midpointRoundingMode">
        /// A specification for how to round if the current value is midway between two numbers.
        /// </param>
        /// <returns>
        /// The current <see cref="Double" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="midpointRoundingMode" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 15.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Double" />.
        /// </exception>
        public static Double RoundedTo(this Double target, Int32 digits, MidpointRounding midpointRoundingMode) => Math.Round(target, digits, midpointRoundingMode);

        /// <summary>
        /// Converts the current <see cref="Double" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Double" />.
        /// </returns>
        public static Byte[] ToByteArray(this Double target) => BitConverter.GetBytes(target);
    }
}