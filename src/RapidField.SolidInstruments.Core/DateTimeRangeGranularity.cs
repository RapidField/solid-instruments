// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Specifies the granularity of a <see cref="DateTimeRange" />.
    /// </summary>
    public enum DateTimeRangeGranularity : Int32
    {
        /// <summary>
        /// The accuracy of the start and end points of the range is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The start and end points of the range are exactly accurate and are not quantized.
        /// </summary>
        Exact = 1,

        /// <summary>
        /// The millisecond values expressed by the start and end points of the range are accurate and the tick counts are quantized
        /// to the midpoints of the millisecond.
        /// </summary>
        AccurateToTheMillisecond = 2,

        /// <summary>
        /// The second values expressed by the start and end points of the range are accurate and the millisecond values are
        /// quantized to the midpoints of the second.
        /// </summary>
        AccurateToTheSecond = 3,

        /// <summary>
        /// The minute values expressed by the start and end points of the range are accurate and the second values are quantized to
        /// the midpoints of the minute.
        /// </summary>
        AccurateToTheMinute = 4,

        /// <summary>
        /// The hour values expressed by the start and end points of the range are accurate and the minute values are quantized to
        /// the midpoints of the hour.
        /// </summary>
        AccurateToTheHour = 5,

        /// <summary>
        /// The day values expressed by the start and end points of the range are accurate and the hour values are quantized to the
        /// midpoints of the day.
        /// </summary>
        AccurateToTheDay = 6,

        /// <summary>
        /// The month values expressed by the start and end points of the range are accurate and the day values are quantized to the
        /// midpoints of the month.
        /// </summary>
        AccurateToTheMonth = 7,

        /// <summary>
        /// The year values expressed by the start and end points of the range are accurate and the day values are quantized to the
        /// midpoints of the year.
        /// </summary>
        AccurateToTheYear = 8
    }
}