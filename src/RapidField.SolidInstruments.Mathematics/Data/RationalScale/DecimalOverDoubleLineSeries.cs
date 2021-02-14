// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Mathematics.Extensions;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Mathematics.Data.RationalScale
{
    /// <summary>
    /// Represents a line series that is comprised of decimal number values measured over a rational number scale.
    /// </summary>
    public class DecimalOverDoubleLineSeries : ScalarLineSeries<Double, Decimal>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalOverDoubleLineSeries" /> class.
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
        public DecimalOverDoubleLineSeries(IDictionary<Double, Decimal> data)
            : base(data)
        {
            return;
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
        protected sealed override Decimal InterpolateLinear(Double xAxisValue, Double downwardXAxisValue, Decimal downwardYAxisValue, Double upwardXAxisValue, Decimal upwardYAxisValue)
        {
            var yAxisRange = upwardYAxisValue - downwardYAxisValue;
            var positionInXAxisRange = xAxisValue.PositionInRange(downwardXAxisValue, upwardXAxisValue).ToDecimal();
            var adjustment = yAxisRange * positionInXAxisRange;
            return downwardYAxisValue + adjustment;
        }

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
        protected sealed override Decimal InterpolateNearest(Double xAxisValue, Double downwardXAxisValue, Decimal downwardYAxisValue, Double upwardXAxisValue, Decimal upwardYAxisValue)
        {
            var positionInXAxisRange = xAxisValue.PositionInRange(downwardXAxisValue, upwardXAxisValue);
            return positionInXAxisRange < 0.5d ? downwardYAxisValue : upwardYAxisValue;
        }
    }
}