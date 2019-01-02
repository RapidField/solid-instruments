// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Mathematics
{
    /// <summary>
    /// Specifies the mode for interpolating missing data points in a data series.
    /// </summary>
    public enum InterpolationMode : Int32
    {
        /// <summary>
        /// The mode is unspecified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Interpolation is not performed. Exceptions may be raised when attempts are made to access missing data points.
        /// </summary>
        None = 1,

        /// <summary>
        /// Missing data points are replaced with the nearest extant data point in the series, rounding outward when necessary.
        /// </summary>
        NearestDataPoint = 2,

        /// <summary>
        /// Missing data points are calculated linearly. The behavior is undefined for non-scalar data points.
        /// </summary>
        Linear = 3
    }
}