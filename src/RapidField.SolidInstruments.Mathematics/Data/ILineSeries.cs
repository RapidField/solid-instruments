// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Mathematics.Data
{
    /// <summary>
    /// Represents a two-dimensional line series.
    /// </summary>
    /// <typeparam name="TXAxis">
    /// The data type of the x-axis.
    /// </typeparam>
    /// <typeparam name="TYAxis">
    /// The data type of the y-axis.
    /// </typeparam>
    public interface ILineSeries<TXAxis, TYAxis> : ITwoDimensionalDataSet<TXAxis, TYAxis>
        where TXAxis : struct, IComparable<TXAxis>, IEquatable<TXAxis>
    {
        /// <summary>
        /// Attempts to get the y-axis value associated with the specified x-axis value.
        /// </summary>
        /// <param name="xAxisValue">
        /// The x-axis value to evaluate.
        /// </param>
        /// <returns>
        /// The resulting y-axis value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The specified x-axis value does not exist within the data set.
        /// </exception>
        TYAxis GetYAxisValue(TXAxis xAxisValue);

        /// <summary>
        /// Attempts to get the y-axis value associated with the specified x-axis value.
        /// </summary>
        /// <param name="xAxisValue">
        /// The x-axis value to evaluate.
        /// </param>
        /// <param name="yAxisValue">
        /// The resulting y-axis value, if one exists.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified x-axis value exists, otherwise <see langword="false" />.
        /// </returns>
        Boolean TryGetYAxisValue(TXAxis xAxisValue, out TYAxis yAxisValue);
    }
}