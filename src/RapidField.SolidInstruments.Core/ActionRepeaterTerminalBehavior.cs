// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Specifies the behavior of an <see cref="IActionRepeater" /> after it has exhausted its timeout threshold or maximum
    /// repetition count.
    /// </summary>
    public enum ActionRepeaterTerminalBehavior : Int32
    {
        /// <summary>
        /// The terminal behavior is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The <see cref="IActionRepeater" /> stops performing the repeating action and does not raise an exception.
        /// </summary>
        Desist = 1,

        /// <summary>
        /// The <see cref="IActionRepeater" /> raises a new <see cref="TimeoutException" /> when its timeout threshold is exceeded
        /// or when its maximum repetition count is reached.
        /// </summary>
        RaiseException = 2
    }
}