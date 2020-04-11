// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Mathematics.Data
{
    /// <summary>
    /// Represents a two-dimensional scalar line series.
    /// </summary>
    /// <typeparam name="TXAxis">
    /// The data type of the x-axis.
    /// </typeparam>
    /// <typeparam name="TYAxis">
    /// The data type of the y-axis.
    /// </typeparam>
    public interface IScalarLineSeries<TXAxis, TYAxis> : ILineSeries<TXAxis, TYAxis>
        where TXAxis : struct, IComparable<TXAxis>, IEquatable<TXAxis>
        where TYAxis : struct, IComparable<TYAxis>, IEquatable<TYAxis>
    {
        /// <summary>
        /// Attempts to get the y-axis value associated with the specified x-axis value.
        /// </summary>
        /// <param name="xAxisValue">
        /// The x-axis value to evaluate.
        /// </param>
        /// <param name="interpolationMode">
        /// A value specifying how missing data points are calculated.
        /// </param>
        /// <returns>
        /// The resulting y-axis value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="interpolationMode" /> is equal to <see cref="InterpolationMode.None" /> and the specified x-axis value
        /// does not exist within the data set.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="interpolationMode" /> is equal to <see cref="InterpolationMode.Unspecified" /> -or-
        /// <paramref name="xAxisValue" /> is outside of the series boundaries.
        /// </exception>
        /// <exception cref="InterpolationException">
        /// An exception was raised while attempting to interpolate a y-axis value for <paramref name="xAxisValue" />.
        /// </exception>
        public TYAxis GetYAxisValue(TXAxis xAxisValue, InterpolationMode interpolationMode);
    }
}