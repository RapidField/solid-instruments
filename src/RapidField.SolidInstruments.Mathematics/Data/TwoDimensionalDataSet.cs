// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Mathematics.Data
{
    /// <summary>
    /// Represents a two-dimensional data set.
    /// </summary>
    /// <remarks>
    /// <see cref="TwoDimensionalDataSet{TXAxis, TYAxis}" /> is the default implementation of
    /// <see cref="ITwoDimensionalDataSet{TXAxis, TYAxis}" />.
    /// </remarks>
    /// <typeparam name="TXAxis">
    /// The data type of the x-axis.
    /// </typeparam>
    /// <typeparam name="TYAxis">
    /// The data type of the y-axis.
    /// </typeparam>
    public abstract class TwoDimensionalDataSet<TXAxis, TYAxis> : ITwoDimensionalDataSet<TXAxis, TYAxis>
        where TXAxis : struct, IComparable<TXAxis>, IEquatable<TXAxis>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwoDimensionalDataSet{TXAxis, TYAxis}" /> class.
        /// </summary>
        /// <param name="data">
        /// The underlying data points.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="data" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data" /> is <see langword="null" />.
        /// </exception>
        protected TwoDimensionalDataSet(IEnumerable<TwoDimensionalCoordinates<TXAxis, TYAxis>> data)
        {
            Data = new List<TwoDimensionalCoordinates<TXAxis, TYAxis>>(data.RejectIf().IsNullOrEmpty(nameof(data)).TargetArgument);
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The index of the element to access.
        /// </param>
        /// <returns>
        /// The element at the specified index if one exists.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// The specified index is out of range.
        /// </exception>
        public TwoDimensionalCoordinates<TXAxis, TYAxis> this[Int32 index] => this.ElementAt(index);

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current
        /// <see cref="TwoDimensionalDataSet{TXAxis, TYAxis}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="TwoDimensionalDataSet{TXAxis, TYAxis}" />.
        /// </returns>
        public IEnumerator<TwoDimensionalCoordinates<TXAxis, TYAxis>> GetEnumerator() => Data.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current
        /// <see cref="TwoDimensionalDataSet{TXAxis, TYAxis}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="TwoDimensionalDataSet{TXAxis, TYAxis}" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets the collection of values comprising the x-axis.
        /// </summary>
        public IEnumerable<TXAxis> XAxis => Data.Select(element => element.XValue);

        /// <summary>
        /// Gets the collection of values comprising the y-axis.
        /// </summary>
        public IEnumerable<TYAxis> YAxis => Data.Select(element => element.YValue);

        /// <summary>
        /// Represents the underlying data points.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly ICollection<TwoDimensionalCoordinates<TXAxis, TYAxis>> Data;
    }
}