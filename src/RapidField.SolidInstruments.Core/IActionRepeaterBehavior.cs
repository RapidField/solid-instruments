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
    public interface IActionRepeaterBehavior
    {
        /// <summary>
        /// Gets or sets the constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="DelayScaleConstant" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        public TimeSpan DelayScaleConstant
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the function that is used to scale successive delays during <see cref="IActionRepeater" /> operation.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="DelayScaleFunction" /> is equal to <see cref="ActionRepeaterDelayScaleFunction.Unspecified" />.
        /// </exception>
        public ActionRepeaterDelayScaleFunction DelayScaleFunction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum number of times that the associated <see cref="IActionRepeater" /> will perform an action, or
        /// negative one (-1) if repetition count is unlimited.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="MaximumRepititionCount" /> is less than <see cref="ActionRepeaterBehavior.InfiniteRepititionCount" />
        /// (negative one).
        /// </exception>
        public Int32 MaximumRepititionCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the behavior of the associated <see cref="IActionRepeater" /> after it has exhausted its timeout threshold
        /// or maximum repetition count.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="TerminalBehavior" /> is equal to <see cref="ActionRepeaterTerminalBehavior.Unspecified" />.
        /// </exception>
        public ActionRepeaterTerminalBehavior TerminalBehavior
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum length of time that the associated <see cref="IActionRepeater" /> will repeat an action, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if repetition duration is unlimited.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <see cref="TimeoutThreshold" /> is less than <see cref="TimeSpan.Zero" /> and not equal to
        /// <see cref="ActionRepeaterBehavior.InfiniteTimeoutThreshold" />.
        /// </exception>
        public TimeSpan TimeoutThreshold
        {
            get;
            set;
        }
    }
}