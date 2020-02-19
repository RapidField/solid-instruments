// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Mathematics.Statistics
{
    /// <summary>
    /// Represents a quantitative summary of the size and distribution of a data collection.
    /// </summary>
    [DataContract]
    public sealed class DescriptiveStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptiveStatistics" /> class.
        /// </summary>
        /// <param name="size">
        /// The size of the numeric collection.
        /// </param>
        /// <param name="minimum">
        /// The lowest value of the numeric collection.
        /// </param>
        /// <param name="maximum">
        /// The highest value of the numeric collection.
        /// </param>
        /// <param name="sum">
        /// The sum of the numeric collection.
        /// </param>
        /// <param name="median">
        /// The median of the numeric collection.
        /// </param>
        /// <param name="mean">
        /// The mean of the numeric collection.
        /// </param>
        /// <param name="variance">
        /// The variance of the numeric collection.
        /// </param>
        /// <param name="standardDeviation">
        /// The standard deviation of the numeric collection.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="size" /> is less than one or <paramref name="maximum" /> is less than <paramref name="minimum" />.
        /// </exception>
        [DebuggerHidden]
        internal DescriptiveStatistics(Int32 size, Decimal minimum, Decimal maximum, Decimal sum, Decimal median, Decimal mean, Decimal variance, Decimal standardDeviation)
            : base()
        {
            Maximum = maximum.RejectIf().IsLessThan(minimum, nameof(maximum), nameof(minimum));
            Mean = mean;
            Median = median;
            Midrange = ((maximum + minimum) / 2m);
            Minimum = minimum;
            Range = (maximum - minimum);
            Size = size.RejectIf().IsLessThan(1, nameof(size));
            StandardDeviation = standardDeviation;
            Sum = sum;
            Variance = variance;
        }

        /// <summary>
        /// Converts the value of the current <see cref="DescriptiveStatistics" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="DescriptiveStatistics" />.
        /// </returns>
        public override String ToString() => $"{{ {nameof(Size)}: {Size}, {nameof(Mean)}: {Mean.RoundedTo(3)}, {nameof(StandardDeviation)}: {StandardDeviation.RoundedTo(3)} }}";

        /// <summary>
        /// Represents the highest value of the numeric collection represented by the current <see cref="DescriptiveStatistics" />.
        /// </summary>
        [DataMember]
        public readonly Decimal Maximum;

        /// <summary>
        /// Represents the mean of the numeric collection represented by the current <see cref="DescriptiveStatistics" />.
        /// </summary>
        [DataMember]
        public readonly Decimal Mean;

        /// <summary>
        /// Represents the median of the numeric collection represented by the current <see cref="DescriptiveStatistics" />.
        /// </summary>
        [DataMember]
        public readonly Decimal Median;

        /// <summary>
        /// Represents the midpoint of the range of the numeric collection represented by the current
        /// <see cref="DescriptiveStatistics" />.
        /// </summary>
        [DataMember]
        public readonly Decimal Midrange;

        /// <summary>
        /// Represents the lowest value of the numeric collection represented by the current <see cref="DescriptiveStatistics" />.
        /// </summary>
        [DataMember]
        public readonly Decimal Minimum;

        /// <summary>
        /// Represents the range of the numeric collection represented by the current <see cref="DescriptiveStatistics" />.
        /// </summary>
        [DataMember]
        public readonly Decimal Range;

        /// <summary>
        /// Represents the size of the numeric collection represented by the current <see cref="DescriptiveStatistics" />.
        /// </summary>
        [DataMember]
        public readonly Int32 Size;

        /// <summary>
        /// Represents the standard deviation of the numeric collection represented by the current
        /// <see cref="DescriptiveStatistics" />.
        /// </summary>
        [DataMember]
        public readonly Decimal StandardDeviation;

        /// <summary>
        /// Represents the sum of the numeric collection represented by the current <see cref="DescriptiveStatistics" />.
        /// </summary>
        [DataMember]
        public readonly Decimal Sum;

        /// <summary>
        /// Represents the variance of the numeric collection represented by the current <see cref="DescriptiveStatistics" />.
        /// </summary>
        [DataMember]
        public readonly Decimal Variance;
    }
}