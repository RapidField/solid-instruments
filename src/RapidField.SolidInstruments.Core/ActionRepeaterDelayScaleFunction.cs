// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Specifies the function that is used to scale successive delay durations during <see cref="IActionRepeater" /> operation.
    /// </summary>
    internal enum ActionRepeaterDelayScaleFunction : Int32
    {
        /// <summary>
        /// The delay scale function is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Successive delays do not scale or vary.
        /// </summary>
        Constant = 1,

        /// <summary>
        /// Successive delay durations increase at a constant rate, producing a linear, arithmetic-growth delay cadence.
        /// </summary>
        Arithmetic = 2,

        /// <summary>
        /// Successive delay durations increase by a semi-constant factor -- derived using the recursive Fibonacci generating
        /// function -- producing a geometric-growth delay cadence.
        /// </summary>
        Geometric = 3
    }
}