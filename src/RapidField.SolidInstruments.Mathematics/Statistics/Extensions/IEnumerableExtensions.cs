// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Mathematics.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Mathematics.Statistics.Extensions
{
    /// <summary>
    /// Extends the <see cref="IEnumerable{T}" /> interface with statistics features.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Compute descriptive statistics for the current <see cref="Int32" /> collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// Descriptive statistics for the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static DescriptiveStatistics ComputeDescriptives(this IEnumerable<Int32?> target) => target.Where(number => number.HasValue).Select(number => number.Value).ComputeDescriptives();

        /// <summary>
        /// Compute descriptive statistics for the current <see cref="Int32" /> collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// Descriptive statistics for the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static DescriptiveStatistics ComputeDescriptives(this IEnumerable<Int32> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            var size = target.Count();
            var minimum = target.Min();
            var maximum = target.Max();
            var sum = target.Sum();
            var median = target.Median();
            var mean = target.Mean(sum);
            var variance = target.Variance(mean);
            var standardDeviation = target.StandardDeviation(variance);
            return new DescriptiveStatistics(size, Convert.ToDecimal(minimum), Convert.ToDecimal(maximum), Convert.ToDecimal(sum), Convert.ToDecimal(median), Convert.ToDecimal(mean), Convert.ToDecimal(variance), Convert.ToDecimal(standardDeviation));
        }

        /// <summary>
        /// Compute descriptive statistics for the current <see cref="Int64" /> collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// Descriptive statistics for the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static DescriptiveStatistics ComputeDescriptives(this IEnumerable<Int64?> target) => target.Where(number => number.HasValue).Select(number => number.Value).ComputeDescriptives();

        /// <summary>
        /// Compute descriptive statistics for the current <see cref="Int64" /> collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// Descriptive statistics for the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static DescriptiveStatistics ComputeDescriptives(this IEnumerable<Int64> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            var size = target.Count();
            var minimum = target.Min();
            var maximum = target.Max();
            var sum = target.Sum();
            var median = target.Median();
            var mean = target.Mean(sum);
            var variance = target.Variance(mean);
            var standardDeviation = target.StandardDeviation(variance);
            return new DescriptiveStatistics(size, Convert.ToDecimal(minimum), Convert.ToDecimal(maximum), Convert.ToDecimal(sum), Convert.ToDecimal(median), Convert.ToDecimal(mean), Convert.ToDecimal(variance), Convert.ToDecimal(standardDeviation));
        }

        /// <summary>
        /// Compute descriptive statistics for the current <see cref="Single" /> collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// Descriptive statistics for the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static DescriptiveStatistics ComputeDescriptives(this IEnumerable<Single?> target) => target.Where(number => number.HasValue).Select(number => number.Value).ComputeDescriptives();

        /// <summary>
        /// Compute descriptive statistics for the current <see cref="Single" /> collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// Descriptive statistics for the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static DescriptiveStatistics ComputeDescriptives(this IEnumerable<Single> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            var size = target.Count();
            var minimum = target.Min();
            var maximum = target.Max();
            var sum = target.Sum();
            var median = target.Median();
            var mean = target.Mean(sum);
            var variance = target.Variance(mean);
            var standardDeviation = target.StandardDeviation(variance);
            return new DescriptiveStatistics(size, Convert.ToDecimal(minimum), Convert.ToDecimal(maximum), Convert.ToDecimal(sum), Convert.ToDecimal(median), Convert.ToDecimal(mean), Convert.ToDecimal(variance), Convert.ToDecimal(standardDeviation));
        }

        /// <summary>
        /// Compute descriptive statistics for the current <see cref="Double" /> collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// Descriptive statistics for the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static DescriptiveStatistics ComputeDescriptives(this IEnumerable<Double?> target) => target.Where(number => number.HasValue).Select(number => number.Value).ComputeDescriptives();

        /// <summary>
        /// Compute descriptive statistics for the current <see cref="Double" /> collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// Descriptive statistics for the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static DescriptiveStatistics ComputeDescriptives(this IEnumerable<Double> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            var size = target.Count();
            var minimum = target.Min();
            var maximum = target.Max();
            var sum = target.Sum();
            var median = target.Median();
            var mean = target.Mean(sum);
            var variance = target.Variance(mean);
            var standardDeviation = target.StandardDeviation(variance);
            return new DescriptiveStatistics(size, Convert.ToDecimal(minimum), Convert.ToDecimal(maximum), Convert.ToDecimal(sum), Convert.ToDecimal(median), Convert.ToDecimal(mean), Convert.ToDecimal(variance), Convert.ToDecimal(standardDeviation));
        }

        /// <summary>
        /// Compute descriptive statistics for the current <see cref="Decimal" /> collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// Descriptive statistics for the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static DescriptiveStatistics ComputeDescriptives(this IEnumerable<Decimal?> target) => target.Where(number => number.HasValue).Select(number => number.Value).ComputeDescriptives();

        /// <summary>
        /// Compute descriptive statistics for the current <see cref="Decimal" /> collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// Descriptive statistics for the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static DescriptiveStatistics ComputeDescriptives(this IEnumerable<Decimal> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            var size = target.Count();
            var minimum = target.Min();
            var maximum = target.Max();
            var sum = target.Sum();
            var median = target.Median();
            var mean = target.Mean(sum);
            var variance = target.Variance(mean);
            var standardDeviation = target.StandardDeviation(variance);
            return new DescriptiveStatistics(size, minimum, maximum, sum, median, mean, variance, standardDeviation);
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Mean(this IEnumerable<Int32?> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.Where(number => number.HasValue).Select(number => number.Value).Mean();
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Mean(this IEnumerable<Int32> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.Mean(target.Sum());
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Mean(this IEnumerable<Int64?> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.Where(number => number.HasValue).Select(number => number.Value).Mean();
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Mean(this IEnumerable<Int64> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.Mean(target.Sum());
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Mean(this IEnumerable<Single?> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.Where(number => number.HasValue).Select(number => number.Value).Mean();
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Mean(this IEnumerable<Single> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.Mean(target.Sum());
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Mean(this IEnumerable<Double?> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.Where(number => number.HasValue).Select(number => number.Value).Mean();
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Mean(this IEnumerable<Double> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.Mean(target.Sum());
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal Mean(this IEnumerable<Decimal?> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.Where(number => number.HasValue).Select(number => number.Value).Mean();
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal Mean(this IEnumerable<Decimal> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.Mean(target.Sum());
        }

        /// <summary>
        /// Calculate the median of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The median of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Median(this IEnumerable<Int32?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Median();

        /// <summary>
        /// Calculate the median of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The median of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Median(this IEnumerable<Int32> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            var orderedCollection = target.OrderBy(number => number);
            var medianIndices = DetermineMedianIndices(orderedCollection.Count());

            if (medianIndices.Length == 1)
            {
                // The collection length is odd. Return a simple median.
                return orderedCollection.ElementAt(medianIndices[0]);
            }

            // The collection length is even. Return the mean of the values sharing the median position.
            return ((Convert.ToDouble(orderedCollection.ElementAt(medianIndices[0])) + Convert.ToDouble(orderedCollection.ElementAt(medianIndices[1]))) / 2d);
        }

        /// <summary>
        /// Calculate the median of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The median of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Median(this IEnumerable<Int64?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Median();

        /// <summary>
        /// Calculate the median of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The median of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Median(this IEnumerable<Int64> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            var orderedCollection = target.OrderBy(number => number);
            var medianIndices = DetermineMedianIndices(orderedCollection.Count());

            if (medianIndices.Length == 1)
            {
                // The collection length is odd. Return a simple median.
                return orderedCollection.ElementAt(medianIndices[0]);
            }

            // The collection length is even. Return the mean of the values sharing the median position.
            return ((Convert.ToDouble(orderedCollection.ElementAt(medianIndices[0])) + Convert.ToDouble(orderedCollection.ElementAt(medianIndices[1]))) / 2d);
        }

        /// <summary>
        /// Calculate the median of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The median of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Median(this IEnumerable<Single?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Median();

        /// <summary>
        /// Calculate the median of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The median of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Median(this IEnumerable<Single> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            var orderedCollection = target.OrderBy(number => number);
            var medianIndices = DetermineMedianIndices(orderedCollection.Count());

            if (medianIndices.Length == 1)
            {
                // The collection length is odd. Return a simple median.
                return orderedCollection.ElementAt(medianIndices[0]);
            }

            // The collection length is even. Return the mean of the values sharing the median position.
            return ((Convert.ToDouble(orderedCollection.ElementAt(medianIndices[0])) + Convert.ToDouble(orderedCollection.ElementAt(medianIndices[1]))) / 2d);
        }

        /// <summary>
        /// Calculate the median of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The median of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Median(this IEnumerable<Double?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Median();

        /// <summary>
        /// Calculate the median of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The median of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Median(this IEnumerable<Double> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            var orderedCollection = target.OrderBy(number => number);
            var medianIndices = DetermineMedianIndices(orderedCollection.Count());

            if (medianIndices.Length == 1)
            {
                // The collection length is odd. Return a simple median.
                return orderedCollection.ElementAt(medianIndices[0]);
            }

            // The collection length is even. Return the mean of the values sharing the median position.
            return ((orderedCollection.ElementAt(medianIndices[0]) + orderedCollection.ElementAt(medianIndices[1])) / 2d);
        }

        /// <summary>
        /// Calculate the median of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The median of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal Median(this IEnumerable<Decimal?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Median();

        /// <summary>
        /// Calculate the median of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The median of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal Median(this IEnumerable<Decimal> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            var orderedCollection = target.OrderBy(number => number);
            var medianIndices = DetermineMedianIndices(orderedCollection.Count());

            if (medianIndices.Length == 1)
            {
                // The collection length is odd. Return a simple median.
                return orderedCollection.ElementAt(medianIndices[0]);
            }

            // The collection length is even. Return the mean of the values sharing the median position.
            return ((orderedCollection.ElementAt(medianIndices[0]) + orderedCollection.ElementAt(medianIndices[1])) / 2m);
        }

        /// <summary>
        /// Calculate the mid-range of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The mid-range of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Midrange(this IEnumerable<Int32?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Midrange();

        /// <summary>
        /// Calculate the mid-range of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The mid-range of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Midrange(this IEnumerable<Int32> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return ((Convert.ToDouble(target.Min()) + Convert.ToDouble(target.Max())) / 2d);
        }

        /// <summary>
        /// Calculate the mid-range of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The mid-range of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Midrange(this IEnumerable<Int64?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Midrange();

        /// <summary>
        /// Calculate the mid-range of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The mid-range of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Midrange(this IEnumerable<Int64> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return ((Convert.ToDouble(target.Min()) + Convert.ToDouble(target.Max())) / 2d);
        }

        /// <summary>
        /// Calculate the mid-range of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The mid-range of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Midrange(this IEnumerable<Single?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Midrange();

        /// <summary>
        /// Calculate the mid-range of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The mid-range of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Midrange(this IEnumerable<Single> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return ((Convert.ToDouble(target.Min()) + Convert.ToDouble(target.Max())) / 2d);
        }

        /// <summary>
        /// Calculate the mid-range of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The mid-range of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Midrange(this IEnumerable<Double?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Midrange();

        /// <summary>
        /// Calculate the mid-range of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The mid-range of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Midrange(this IEnumerable<Double> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return ((target.Min() + target.Max()) / 2d);
        }

        /// <summary>
        /// Calculate the mid-range of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The mid-range of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal Midrange(this IEnumerable<Decimal?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Midrange();

        /// <summary>
        /// Calculate the mid-range of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The mid-range of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal Midrange(this IEnumerable<Decimal> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return ((target.Min() + target.Max()) / 2m);
        }

        /// <summary>
        /// Calculate the range of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The range of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Int32 Range(this IEnumerable<Int32?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Range();

        /// <summary>
        /// Calculate the range of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The range of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Int32 Range(this IEnumerable<Int32> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return (target.Max() - target.Min());
        }

        /// <summary>
        /// Calculate the range of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The range of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Int64 Range(this IEnumerable<Int64?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Range();

        /// <summary>
        /// Calculate the range of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The range of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Int64 Range(this IEnumerable<Int64> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return (target.Max() - target.Min());
        }

        /// <summary>
        /// Calculate the range of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The range of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Single Range(this IEnumerable<Single?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Range();

        /// <summary>
        /// Calculate the range of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The range of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Single Range(this IEnumerable<Single> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return (target.Max() - target.Min());
        }

        /// <summary>
        /// Calculate the range of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The range of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Range(this IEnumerable<Double?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Range();

        /// <summary>
        /// Calculate the range of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The range of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Range(this IEnumerable<Double> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return (target.Max() - target.Min());
        }

        /// <summary>
        /// Calculate the range of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The range of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal Range(this IEnumerable<Decimal?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Range();

        /// <summary>
        /// Calculate the range of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The range of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal Range(this IEnumerable<Decimal> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return (target.Max() - target.Min());
        }

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double StandardDeviation(this IEnumerable<Int32?> target) => target.Where(number => number.HasValue).Select(number => number.Value).StandardDeviation();

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double StandardDeviation(this IEnumerable<Int32> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.StandardDeviation(target.Variance());
        }

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double StandardDeviation(this IEnumerable<Int64?> target) => target.Where(number => number.HasValue).Select(number => number.Value).StandardDeviation();

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double StandardDeviation(this IEnumerable<Int64> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.StandardDeviation(target.Variance());
        }

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double StandardDeviation(this IEnumerable<Single?> target) => target.Where(number => number.HasValue).Select(number => number.Value).StandardDeviation();

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double StandardDeviation(this IEnumerable<Single> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.StandardDeviation(target.Variance());
        }

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double StandardDeviation(this IEnumerable<Double?> target) => target.Where(number => number.HasValue).Select(number => number.Value).StandardDeviation();

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double StandardDeviation(this IEnumerable<Double> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.StandardDeviation(target.Variance());
        }

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal StandardDeviation(this IEnumerable<Decimal?> target) => target.Where(number => number.HasValue).Select(number => number.Value).StandardDeviation();

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal StandardDeviation(this IEnumerable<Decimal> target)
        {
            target.RejectIf().IsNullOrEmpty(nameof(target));
            return target.StandardDeviation(target.Variance());
        }

        /// <summary>
        /// Calculate the variance of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Variance(this IEnumerable<Int32?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Variance();

        /// <summary>
        /// Calculate the variance of the <see cref="Int32" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int32" /> values.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Variance(this IEnumerable<Int32> target) => target.Variance(target.Mean());

        /// <summary>
        /// Calculate the variance of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Variance(this IEnumerable<Int64?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Variance();

        /// <summary>
        /// Calculate the variance of the <see cref="Int64" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int64" /> values.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Variance(this IEnumerable<Int64> target) => target.Variance(target.Mean());

        /// <summary>
        /// Calculate the variance of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Variance(this IEnumerable<Single?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Variance();

        /// <summary>
        /// Calculate the variance of the <see cref="Single" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Single" /> values.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Variance(this IEnumerable<Single> target) => target.Variance(target.Mean());

        /// <summary>
        /// Calculate the variance of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Variance(this IEnumerable<Double?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Variance();

        /// <summary>
        /// Calculate the variance of the <see cref="Double" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Double" /> values.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Double Variance(this IEnumerable<Double> target) => target.Variance(target.Mean());

        /// <summary>
        /// Calculate the variance of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal Variance(this IEnumerable<Decimal?> target) => target.Where(number => number.HasValue).Select(number => number.Value).Variance();

        /// <summary>
        /// Calculate the variance of the <see cref="Decimal" /> values in the current collection.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Decimal" /> values.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="target" /> does not contain any values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        public static Decimal Variance(this IEnumerable<Decimal> target) => target.Variance(target.Mean());

        /// <summary>
        /// Determine the index or pair of indices defining a median location given the provided element count.
        /// </summary>
        /// <param name="elementCount">
        /// The number of elements in a numeric collection.
        /// </param>
        /// <returns>
        /// The single index or pair of indices defining a median location.
        /// </returns>
        [DebuggerHidden]
        private static Int32[] DetermineMedianIndices(Int32 elementCount)
        {
            if (elementCount == 0)
            {
                return Array.Empty<Int32>();
            }
            else if (elementCount.IsOdd())
            {
                // Return the index at the midpoint.
                return new Int32[] { Convert.ToInt32(Math.Round((Single)(elementCount / 2), 0, MidpointRounding.AwayFromZero)) };
            }

            // Return the pair of indices sharing the midpoint.
            var firstIndex = ((elementCount / 2) - 1);
            return new Int32[] { firstIndex, (firstIndex + 1) };
        }

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Int32" /> values in the current collection given the provided sum.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int32" /> values.
        /// </param>
        /// <param name="sum">
        /// The sum of the values in the current <see cref="Int32" /> collection.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Mean(this IEnumerable<Int32?> target, Int32 sum) => target.Where(number => number.HasValue).Select(number => number.Value).Mean(sum);

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Int32" /> values in the current collection given the provided sum.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int32" /> values.
        /// </param>
        /// <param name="sum">
        /// The sum of the values in the current <see cref="Int32" /> collection.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Mean(this IEnumerable<Int32> target, Int32 sum) => (Convert.ToDouble(sum) / Convert.ToDouble(target.Count()));

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Int64" /> values in the current collection given the provided sum.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int64" /> values.
        /// </param>
        /// <param name="sum">
        /// The sum of the values in the current <see cref="Int64" /> collection.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Mean(this IEnumerable<Int64?> target, Int64 sum) => target.Where(number => number.HasValue).Select(number => number.Value).Mean(sum);

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Int64" /> values in the current collection given the provided sum.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int64" /> values.
        /// </param>
        /// <param name="sum">
        /// The sum of the values in the current <see cref="Int64" /> collection.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Mean(this IEnumerable<Int64> target, Int64 sum) => (Convert.ToDouble(sum) / Convert.ToDouble(target.Count()));

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Single" /> values in the current collection given the provided sum.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Single" /> values.
        /// </param>
        /// <param name="sum">
        /// The sum of the values in the current <see cref="Single" /> collection.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static Double Mean(this IEnumerable<Single?> target, Single sum) => target.Where(number => number.HasValue).Select(number => number.Value).Mean(sum);

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Single" /> values in the current collection given the provided sum.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Single" /> values.
        /// </param>
        /// <param name="sum">
        /// The sum of the values in the current <see cref="Single" /> collection.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static Double Mean(this IEnumerable<Single> target, Single sum) => (Convert.ToDouble(sum) / Convert.ToDouble(target.Count()));

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Double" /> values in the current collection given the provided sum.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Double" /> values.
        /// </param>
        /// <param name="sum">
        /// The sum of the values in the current <see cref="Double" /> collection.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static Double Mean(this IEnumerable<Double?> target, Double sum) => target.Where(number => number.HasValue).Select(number => number.Value).Mean(sum);

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Double" /> values in the current collection given the provided sum.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Double" /> values.
        /// </param>
        /// <param name="sum">
        /// The sum of the values in the current <see cref="Double" /> collection.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static Double Mean(this IEnumerable<Double> target, Double sum) => (sum / Convert.ToDouble(target.Count()));

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Decimal" /> values in the current collection given the provided sum.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Decimal" /> values.
        /// </param>
        /// <param name="sum">
        /// The sum of the values in the current <see cref="Decimal" /> collection.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static Decimal Mean(this IEnumerable<Decimal?> target, Decimal sum) => target.Where(number => number.HasValue).Select(number => number.Value).Mean(sum);

        /// <summary>
        /// Calculate the arithmetic mean of the <see cref="Decimal" /> values in the current collection given the provided sum.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Decimal" /> values.
        /// </param>
        /// <param name="sum">
        /// The sum of the values in the current <see cref="Decimal" /> collection.
        /// </param>
        /// <returns>
        /// The mean of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static Decimal Mean(this IEnumerable<Decimal> target, Decimal sum) => (sum / Convert.ToDecimal(target.Count()));

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Int32" /> values in the current collection given the provided
        /// variance.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int32" /> values.
        /// </param>
        /// <param name="variance">
        /// The variance of the values in the current <see cref="Int32" /> collection.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static Double StandardDeviation(this IEnumerable<Int32?> target, Double variance) => target.Where(number => number.HasValue).Select(number => number.Value).StandardDeviation(variance);

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Int32" /> values in the current collection given the provided
        /// variance.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int32" /> values.
        /// </param>
        /// <param name="variance">
        /// The variance of the values in the current <see cref="Int32" /> collection.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        [DebuggerHidden]
        private static Double StandardDeviation(this IEnumerable<Int32> target, Double variance) => Math.Sqrt(variance);

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Int64" /> values in the current collection given the provided
        /// variance.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int64" /> values.
        /// </param>
        /// <param name="variance">
        /// The variance of the values in the current <see cref="Int64" /> collection.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static Double StandardDeviation(this IEnumerable<Int64?> target, Double variance) => target.Where(number => number.HasValue).Select(number => number.Value).StandardDeviation(variance);

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Int64" /> values in the current collection given the provided
        /// variance.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int64" /> values.
        /// </param>
        /// <param name="variance">
        /// The variance of the values in the current <see cref="Int64" /> collection.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        [DebuggerHidden]
        private static Double StandardDeviation(this IEnumerable<Int64> target, Double variance) => Math.Sqrt(variance);

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Single" /> values in the current collection given the provided
        /// variance.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Single" /> values.
        /// </param>
        /// <param name="variance">
        /// The variance of the values in the current <see cref="Single" /> collection.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static Double StandardDeviation(this IEnumerable<Single?> target, Double variance) => target.Where(number => number.HasValue).Select(number => number.Value).StandardDeviation(variance);

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Single" /> values in the current collection given the provided
        /// variance.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Single" /> values.
        /// </param>
        /// <param name="variance">
        /// The variance of the values in the current <see cref="Single" /> collection.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Single" /> collection.
        /// </returns>
        [DebuggerHidden]
        private static Double StandardDeviation(this IEnumerable<Single> target, Double variance) => Math.Sqrt(variance);

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Double" /> values in the current collection given the provided
        /// variance.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Double" /> values.
        /// </param>
        /// <param name="variance">
        /// The variance of the values in the current <see cref="Double" /> collection.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static Double StandardDeviation(this IEnumerable<Double?> target, Double variance) => target.Where(number => number.HasValue).Select(number => number.Value).StandardDeviation(variance);

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Double" /> values in the current collection given the provided
        /// variance.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Double" /> values.
        /// </param>
        /// <param name="variance">
        /// The variance of the values in the current <see cref="Double" /> collection.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Double" /> collection.
        /// </returns>
        [DebuggerHidden]
        private static Double StandardDeviation(this IEnumerable<Double> target, Double variance) => Math.Sqrt(variance);

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Decimal" /> values in the current collection given the provided
        /// variance.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Decimal" /> values.
        /// </param>
        /// <param name="variance">
        /// The variance of the values in the current <see cref="Decimal" /> collection.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Decimal StandardDeviation(this IEnumerable<Decimal?> target, Decimal variance) => target.Where(number => number.HasValue).Select(number => number.Value).StandardDeviation(variance);

        /// <summary>
        /// Calculate the standard deviation of the <see cref="Decimal" /> values in the current collection given the provided
        /// variance.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Decimal" /> values.
        /// </param>
        /// <param name="variance">
        /// The variance of the values in the current <see cref="Decimal" /> collection.
        /// </param>
        /// <returns>
        /// The standard deviation of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Decimal StandardDeviation(this IEnumerable<Decimal> target, Decimal variance) => Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(variance)));

        /// <summary>
        /// Calculate the variance of the <see cref="Int32" /> values in the current collection given the provided mean.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int32" /> values.
        /// </param>
        /// <param name="mean">
        /// The mean of the values in the current <see cref="Int32" /> collection.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Variance(this IEnumerable<Int32?> target, Double mean) => target.Where(number => number.HasValue).Select(number => number.Value).Variance(mean);

        /// <summary>
        /// Calculate the variance of the <see cref="Int32" /> values in the current collection given the provided mean.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int32" /> values.
        /// </param>
        /// <param name="mean">
        /// The mean of the values in the current <see cref="Int32" /> collection.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Int32" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Variance(this IEnumerable<Int32> target, Double mean) => ((Convert.ToDouble(target.Sum(number => (number * number))) / Convert.ToDouble(target.Count())) - Math.Pow(mean, 2d));

        /// <summary>
        /// Calculate the variance of the <see cref="Int64" /> values in the current collection given the provided mean.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Int64" /> values.
        /// </param>
        /// <param name="mean">
        /// The mean of the values in the current <see cref="Int64" /> collection.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Variance(this IEnumerable<Int64?> target, Double mean) => target.Where(number => number.HasValue).Select(number => number.Value).Variance(mean);

        /// <summary>
        /// Calculate the variance of the <see cref="Int64" /> values in the current collection given the provided mean.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Int64" /> values.
        /// </param>
        /// <param name="mean">
        /// The mean of the values in the current <see cref="Int64" /> collection.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Int64" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Variance(this IEnumerable<Int64> target, Double mean) => ((Convert.ToDouble(target.Sum(number => (number * number))) / Convert.ToDouble(target.Count())) - Math.Pow(mean, 2d));

        /// <summary>
        /// Calculate the variance of the <see cref="Single" /> values in the current collection given the provided mean.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Single" /> values.
        /// </param>
        /// <param name="mean">
        /// The mean of the values in the current <see cref="Single" /> collection.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Variance(this IEnumerable<Single?> target, Double mean) => target.Where(number => number.HasValue).Select(number => number.Value).Variance(mean);

        /// <summary>
        /// Calculate the variance of the <see cref="Single" /> values in the current collection given the provided mean.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Single" /> values.
        /// </param>
        /// <param name="mean">
        /// The mean of the values in the current <see cref="Single" /> collection.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Single" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Variance(this IEnumerable<Single> target, Double mean) => ((Convert.ToDouble(target.Sum(number => (number * number))) / Convert.ToDouble(target.Count())) - Math.Pow(mean, 2d));

        /// <summary>
        /// Calculate the variance of the <see cref="Double" /> values in the current collection given the provided mean.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Double" /> values.
        /// </param>
        /// <param name="mean">
        /// The mean of the values in the current <see cref="Double" /> collection.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Variance(this IEnumerable<Double?> target, Double mean) => target.Where(number => number.HasValue).Select(number => number.Value).Variance(mean);

        /// <summary>
        /// Calculate the variance of the <see cref="Double" /> values in the current collection given the provided mean.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Double" /> values.
        /// </param>
        /// <param name="mean">
        /// The mean of the values in the current <see cref="Double" /> collection.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Double" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Double Variance(this IEnumerable<Double> target, Double mean) => ((target.Sum(number => (number * number)) / Convert.ToDouble(target.Count())) - Math.Pow(mean, 2d));

        /// <summary>
        /// Calculate the variance of the <see cref="Decimal" /> values in the current collection given the provided mean.
        /// </summary>
        /// <param name="target">
        /// The current collection of nullable <see cref="Decimal" /> values.
        /// </param>
        /// <param name="mean">
        /// The mean of the values in the current <see cref="Decimal" /> collection.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Decimal Variance(this IEnumerable<Decimal?> target, Decimal mean) => target.Where(number => number.HasValue).Select(number => number.Value).Variance(mean);

        /// <summary>
        /// Calculate the variance of the <see cref="Decimal" /> values in the current collection given the provided mean.
        /// </summary>
        /// <param name="target">
        /// The current collection of <see cref="Decimal" /> values.
        /// </param>
        /// <param name="mean">
        /// The mean of the values in the current <see cref="Decimal" /> collection.
        /// </param>
        /// <returns>
        /// The variance of the values in the current <see cref="Decimal" /> collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="OverflowException">
        /// A calculation produced a result that is outside the range of its data type.
        /// </exception>
        [DebuggerHidden]
        private static Decimal Variance(this IEnumerable<Decimal> target, Decimal mean) => ((target.Sum(number => (number * number)) / Convert.ToDecimal(target.Count())) - (mean * mean));
    }
}