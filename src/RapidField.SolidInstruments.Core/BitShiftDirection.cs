// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Specifies a direction (left or right) to shift bits in a bit field.
    /// </summary>
    public enum BitShiftDirection : Int32
    {
        /// <summary>
        /// The bit shift direction is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Bits in the field are shifted to the left.
        /// </summary>
        Left = 1,

        /// <summary>
        /// Bits in the field are shifted to the right.
        /// </summary>
        Right = 2
    }
}