// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Mathematics
{
    /// <summary>
    /// Specifies the mode for factor rounding operations.
    /// </summary>
    public enum FactorRoundingMode : Int32
    {
        /// <summary>
        /// The mode is unspecified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The value will be rounded to the nearest multiple that is equidistant to, closer to, or further from zero.
        /// </summary>
        InwardOrOutward = 1,

        /// <summary>
        /// The value will be rounded to the nearest multiple that is equidistant or closer to zero.
        /// </summary>
        InwardOnly = 2,

        /// <summary>
        /// The value will be rounded to the nearest multiple that is equidistant to or further from zero.
        /// </summary>
        OutwardOnly = 3
    }
}