﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Mathematics.Extensions;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Mathematics.Data.DateTimeScale
{
    /// <summary>
    /// Represents a line series that is comprised of 16-bit integer values measured over date and time.
    /// </summary>
    public class Int16OverDateTimeLineSeries : ScalarLineSeries<DateTime, Int16>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Int16OverDateTimeLineSeries" /> class.
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
        public Int16OverDateTimeLineSeries(IDictionary<DateTime, Int16> data)
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
        protected sealed override Int16 InterpolateLinear(DateTime xAxisValue, DateTime downwardXAxisValue, Int16 downwardYAxisValue, DateTime upwardXAxisValue, Int16 upwardYAxisValue)
        {
            var yAxisRange = Convert.ToDouble(upwardYAxisValue - downwardYAxisValue);
            var positionInXAxisRange = xAxisValue.PositionInRange(downwardXAxisValue, upwardXAxisValue);
            var adjustment = yAxisRange * positionInXAxisRange;
            return Convert.ToInt16((downwardYAxisValue.ToDouble() + adjustment).RoundedTo(0, MidpointRounding.AwayFromZero));
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
        protected sealed override Int16 InterpolateNearest(DateTime xAxisValue, DateTime downwardXAxisValue, Int16 downwardYAxisValue, DateTime upwardXAxisValue, Int16 upwardYAxisValue)
        {
            var positionInXAxisRange = xAxisValue.PositionInRange(downwardXAxisValue, upwardXAxisValue);
            return positionInXAxisRange < 0.5d ? downwardYAxisValue : upwardYAxisValue;
        }
    }
}