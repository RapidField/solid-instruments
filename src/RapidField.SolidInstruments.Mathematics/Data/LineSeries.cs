// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Mathematics.Data
{
    /// <summary>
    /// Represents a two-dimensional line series.
    /// </summary>
    /// <remarks>
    /// <see cref="LineSeries{TXAxis, TYAxis}" /> is the default implementation of <see cref="ILineSeries{TXAxis, TYAxis}" />.
    /// </remarks>
    /// <typeparam name="TXAxis">
    /// The data type of the x-axis.
    /// </typeparam>
    /// <typeparam name="TYAxis">
    /// The data type of the y-axis.
    /// </typeparam>
    public abstract class LineSeries<TXAxis, TYAxis> : TwoDimensionalDataSet<TXAxis, TYAxis>, ILineSeries<TXAxis, TYAxis>
        where TXAxis : struct, IComparable<TXAxis>, IEquatable<TXAxis>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries{TXAxis, TYAxis}" /> class.
        /// </summary>
        /// <param name="data">
        /// The dictionary whose elements are copied to the data set.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="data" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="data" /> contains one or more duplicate keys.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data" /> is <see langword="null" />.
        /// </exception>
        protected LineSeries(IDictionary<TXAxis, TYAxis> data)
            : base(data.Select(element => new TwoDimensionalCoordinates<TXAxis, TYAxis>(element.Key, element.Value)).OrderBy(element => element.XValue))
        {
            return;
        }

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
        public TYAxis GetYAxisValue(TXAxis xAxisValue)
        {
            if (TryGetYAxisValue(xAxisValue, out var yAxisValue))
            {
                return yAxisValue;
            }

            throw new ArgumentException("The specified x-axis value does not exist within the data set.");
        }

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
        public Boolean TryGetYAxisValue(TXAxis xAxisValue, out TYAxis yAxisValue)
        {
            var matchingElements = Data.Where(element => element.XValue.Equals(xAxisValue));

            switch (matchingElements.Count())
            {
                case 1:

                    yAxisValue = matchingElements.First().YValue;
                    return true;

                default:

                    yAxisValue = default;
                    return false;
            }
        }
    }
}