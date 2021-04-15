// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents information that defines the general behavior of an <see cref="IActionRepeater" />.
    /// </summary>
    internal interface IActionRepeaterBehavior
    {
        /// <summary>
        /// Gets the constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation.
        /// </summary>
        public TimeSpan DelayScaleConstant
        {
            get;
        }

        /// <summary>
        /// Gets the function that is used to scale successive delays during <see cref="IActionRepeater" /> operation.
        /// </summary>
        public ActionRepeaterDelayScaleFunction DelayScaleFunction
        {
            get;
        }

        /// <summary>
        /// Gets the maximum number of times that the associated <see cref="IActionRepeater" /> will perform an action, or negative
        /// one (-1) if repetition count is unlimited.
        /// </summary>
        public Int32 MaximumRepititionCount
        {
            get;
        }

        /// <summary>
        /// Gets the behavior of the associated <see cref="IActionRepeater" /> after it has exhausted its timeout threshold or
        /// maximum repetition count.
        /// </summary>
        public ActionRepeaterTerminalBehavior TerminalBehavior
        {
            get;
        }

        /// <summary>
        /// Gets the maximum length of time that the associated <see cref="IActionRepeater" /> will repeat an action, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if repetition duration is unlimited.
        /// </summary>
        public TimeSpan TimeoutThreshold
        {
            get;
        }
    }
}