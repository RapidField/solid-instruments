// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Mathematics.Data
{
    /// <summary>
    /// Represents a pair of values comprising coordinates on a two-dimensional scale.
    /// </summary>
    /// <typeparam name="TXValue">
    /// The type of the value on the x-axis.
    /// </typeparam>
    /// <typeparam name="TYValue">
    /// The type of the value on the y-axis.
    /// </typeparam>
    public class TwoDimensionalCoordinates<TXValue, TYValue>
        where TXValue : struct, IComparable<TXValue>, IEquatable<TXValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwoDimensionalCoordinates{TXValue, TYValue}" /> class.
        /// </summary>
        /// <param name="xValue">
        /// The value on the x-axis.
        /// </param>
        /// <param name="yValue">
        /// The value on the y-axis.
        /// </param>
        public TwoDimensionalCoordinates(TXValue xValue, TYValue yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }

        /// <summary>
        /// Gets the value on the x-axis.
        /// </summary>
        public TXValue XValue
        {
            get;
        }

        /// <summary>
        /// Gets the value on the y-axis.
        /// </summary>
        public TYValue YValue
        {
            get;
        }
    }
}