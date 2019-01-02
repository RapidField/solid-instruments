// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Mathematics.Data
{
    /// <summary>
    /// Represents a two-dimensional data set.
    /// </summary>
    /// <typeparam name="TXAxis">
    /// The data type of the x-axis.
    /// </typeparam>
    /// <typeparam name="TYAxis">
    /// The data type of the y-axis.
    /// </typeparam>
    public interface ITwoDimensionalDataSet<TXAxis, TYAxis> : IEnumerable<TwoDimensionalCoordinates<TXAxis, TYAxis>>
        where TXAxis : struct, IComparable<TXAxis>, IEquatable<TXAxis>
    {
        /// <summary>
        /// Gets the collection of values comprising the x-axis.
        /// </summary>
        IEnumerable<TXAxis> XAxis
        {
            get;
        }

        /// <summary>
        /// Gets the collection of values comprising the y-axis.
        /// </summary>
        IEnumerable<TYAxis> YAxis
        {
            get;
        }
    }
}