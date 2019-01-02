// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Single" /> structure with general purpose features.
    /// </summary>
    public static class SingleExtensions
    {
        /// <summary>
        /// Rounds the current <see cref="Single" /> to the specified number of fractional digits.
        /// </summary>
        /// <remarks>
        /// This method rounds away from zero by default.
        /// </remarks>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Single" /> to.
        /// </param>
        /// <returns>
        /// The current <see cref="Single" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than seven.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Single" />.
        /// </exception>
        public static Single RoundedTo(this Single target, Int32 digits) => target.RoundedTo(digits, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Rounds the current <see cref="Single" /> to the specified number of fractional digits.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Single" /> to.
        /// </param>
        /// <param name="midpointRoundingMode">
        /// A specification for how to round if the current value is midway between two numbers.
        /// </param>
        /// <returns>
        /// The current <see cref="Single" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="midpointRoundingMode" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 7.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Single" />.
        /// </exception>
        public static Single RoundedTo(this Single target, Int32 digits, MidpointRounding midpointRoundingMode) => Convert.ToSingle(Math.Round(target, digits.RejectIf().IsGreaterThan(7, nameof(digits)), midpointRoundingMode));

        /// <summary>
        /// Converts the current <see cref="Single" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Single" />.
        /// </returns>
        public static Byte[] ToByteArray(this Single target) => BitConverter.GetBytes(target);
    }
}