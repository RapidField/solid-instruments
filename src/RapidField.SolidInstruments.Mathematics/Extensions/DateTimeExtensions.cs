// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;

namespace RapidField.SolidInstruments.Mathematics.Extensions
{
    /// <summary>
    /// Extends the <see cref="DateTime" /> structure with mathematics features.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns the position of the current <see cref="DateTime" /> within the specified range, expressed as a percentage where
        /// zero is equal to the lower boundary and one is equal to the upper boundary.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="DateTime" />.
        /// </param>
        /// <param name="lowerBoundary">
        /// The inclusive lower boundary of the range to evaluate.
        /// </param>
        /// <param name="upperBoundary">
        /// The inclusive upper boundary of the range to evaluate.
        /// </param>
        /// <returns>
        /// The position of the current <see cref="DateTime" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The current value is less than <paramref name="lowerBoundary" /> -or- the current value is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="lowerBoundary" /> is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="upperBoundary" /> is equal to <paramref name="lowerBoundary" />.
        /// </exception>
        public static Double PositionInRange(this DateTime target, DateTime lowerBoundary, DateTime upperBoundary)
        {
            target = target.RejectIf().IsLessThan(lowerBoundary, nameof(lowerBoundary)).OrIf().IsGreaterThan(upperBoundary, nameof(upperBoundary));
            lowerBoundary = lowerBoundary.RejectIf().IsGreaterThan(upperBoundary, nameof(lowerBoundary));
            upperBoundary = upperBoundary.RejectIf().IsEqualToValue(lowerBoundary, nameof(upperBoundary));
            return (Convert.ToDouble((target - lowerBoundary).Ticks) / Convert.ToDouble((upperBoundary - lowerBoundary).Ticks));
        }
    }
}