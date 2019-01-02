// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Mathematics.Physics
{
    /// <summary>
    /// Specifies a related collection of units of measurement.
    /// </summary>
    public enum SystemOfMeasurement : Int32
    {
        /// <summary>
        /// The system of measurement is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Specifies the Metric system of measurement.
        /// </summary>
        Metric = 1,

        /// <summary>
        /// Specifies the Imperial system of measurement.
        /// </summary>
        Imperial = 2
    }
}