// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public abstract class ScalarLineSeries<TXAxis, TYAxis> : LineSeries<TXAxis, TYAxis>
        where TXAxis : struct, IComparable<TXAxis>, IEquatable<TXAxis>
        where TYAxis : struct, IComparable<TYAxis>, IEquatable<TYAxis>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarLineSeries{TXAxis, TYAxis}" /> class.
        /// </summary>
        /// <param name="data">
        /// The dictionary whose elements are copied to the series.
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
        protected ScalarLineSeries(IDictionary<TXAxis, TYAxis> data)
            : base(data)
        {
            return;
        }

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
        public TYAxis GetYAxisValue(TXAxis xAxisValue, InterpolationMode interpolationMode)
        {
            var seriesLength = this.Count();

            if (seriesLength == 0)
            {
                throw new InvalidOperationException("The data set is empty.");
            }
            else if (TryGetYAxisValue(xAxisValue, out var yAxisValue))
            {
                return yAxisValue;
            }
            else if (seriesLength == 1 || Data.First().XValue.CompareTo(xAxisValue) > 0 || Data.Last().XValue.CompareTo(xAxisValue) < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(xAxisValue), "The specified x-axis value is outside of the series boundaries.");
            }

            var downwardDataPoint = Data.Last(element => element.XValue.CompareTo(xAxisValue) < 0);
            var upwardDataPoint = Data.First(element => element.XValue.CompareTo(xAxisValue) > 0);

            switch (interpolationMode.RejectIf().IsEqualToValue(InterpolationMode.Unspecified, nameof(interpolationMode)).TargetArgument)
            {
                case InterpolationMode.Linear:

                    try
                    {
                        return InterpolateLinear(xAxisValue, downwardDataPoint.XValue, downwardDataPoint.YValue, upwardDataPoint.XValue, upwardDataPoint.YValue);
                    }
                    catch (InterpolationException)
                    {
                        throw;
                    }
                    catch (Exception exception)
                    {
                        throw new InterpolationException(exception);
                    }

                case InterpolationMode.NearestDataPoint:

                    try
                    {
                        return InterpolateNearest(xAxisValue, downwardDataPoint.XValue, downwardDataPoint.YValue, upwardDataPoint.XValue, upwardDataPoint.YValue);
                    }
                    catch (InterpolationException)
                    {
                        throw;
                    }
                    catch (Exception exception)
                    {
                        throw new InterpolationException(exception);
                    }

                case InterpolationMode.None:

                    throw new ArgumentException("The specified x-axis value does not exist in the data set.");

                default:

                    throw new UnsupportedSpecificationException($"The specified interpolation mode, {interpolationMode}, is not supported.");
            }
        }

        /// <summary>
        /// Returns a y-axis value using <see cref="InterpolationMode.Linear" /> interpolation.
        /// </summary>
        /// <param name="xAxisValue">
        /// The x-axis value to evaluate.
        /// </param>
        /// <param name="downwardXAxisValue">
        /// The x-axis value for the data point that is immediately downward of <paramref name="xAxisValue" />.
        /// </param>
        /// <param name="downwardYAxisValue">
        /// The y-axis value for the data point that is immediately downward of <paramref name="xAxisValue" />.
        /// </param>
        /// <param name="upwardXAxisValue">
        /// The x-axis value for the data point that is immediately upward of <paramref name="xAxisValue" />.
        /// </param>
        /// <param name="upwardYAxisValue">
        /// The y-axis value for the data point that is immediately upward of <paramref name="xAxisValue" />.
        /// </param>
        /// <returns>
        /// The resulting y-axis value.
        /// </returns>
        protected abstract TYAxis InterpolateLinear(TXAxis xAxisValue, TXAxis downwardXAxisValue, TYAxis downwardYAxisValue, TXAxis upwardXAxisValue, TYAxis upwardYAxisValue);

        /// <summary>
        /// Returns a y-axis value using <see cref="InterpolationMode.NearestDataPoint" /> interpolation.
        /// </summary>
        /// <param name="xAxisValue">
        /// The x-axis value to evaluate.
        /// </param>
        /// <param name="downwardXAxisValue">
        /// The x-axis value for the data point that is immediately downward of <paramref name="xAxisValue" />.
        /// </param>
        /// <param name="downwardYAxisValue">
        /// The y-axis value for the data point that is immediately downward of <paramref name="xAxisValue" />.
        /// </param>
        /// <param name="upwardXAxisValue">
        /// The x-axis value for the data point that is immediately upward of <paramref name="xAxisValue" />.
        /// </param>
        /// <param name="upwardYAxisValue">
        /// The y-axis value for the data point that is immediately upward of <paramref name="xAxisValue" />.
        /// </param>
        /// <returns>
        /// The resulting y-axis value.
        /// </returns>
        protected abstract TYAxis InterpolateNearest(TXAxis xAxisValue, TXAxis downwardXAxisValue, TYAxis downwardYAxisValue, TXAxis upwardXAxisValue, TYAxis upwardYAxisValue);
    }
}