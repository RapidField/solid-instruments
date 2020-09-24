// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents a concurrency control that does not constrain thread access.
    /// </summary>
    public sealed class UnconstrainedControl : ConcurrencyControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnconstrainedControl" /> class.
        /// </summary>
        [DebuggerHidden]
        internal UnconstrainedControl()
            : base(Timeout.InfiniteTimeSpan)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="UnconstrainedControl" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource.
        /// </summary>
        /// <returns>
        /// The resulting consumption state of the current <see cref="UnconstrainedControl" />.
        /// </returns>
        protected sealed override ConcurrencyControlConsumptionState EnterWithoutTimeout() => ConcurrencyControlConsumptionState.Unclaimed;

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource and
        /// specifies a timeout threshold.
        /// </summary>
        /// <param name="blockTimeoutThreshold">
        /// The maximum length of time to block a thread before raising an exception.
        /// </param>
        /// <returns>
        /// The resulting consumption state of the current <see cref="UnconstrainedControl" />.
        /// </returns>
        protected sealed override ConcurrencyControlConsumptionState EnterWithTimeout(TimeSpan blockTimeoutThreshold) => ConcurrencyControlConsumptionState.Unclaimed;

        /// <summary>
        /// Informs the control that a thread is exiting a block of code or has finished consuming a resource.
        /// </summary>
        /// <param name="exitedSuccessfully">
        /// A value indicating whether or not the exit operation was successful. The initial value is <see langword="false" />.
        /// </param>
        /// <returns>
        /// The resulting consumption state of the current <see cref="UnconstrainedControl" />.
        /// </returns>
        protected sealed override ConcurrencyControlConsumptionState Exit(ref Boolean exitedSuccessfully)
        {
            exitedSuccessfully = true;
            return ConcurrencyControlConsumptionState.Unclaimed;
        }
    }
}