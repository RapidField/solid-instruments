// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents a semaphore that is used to restrict access the target resource(s) by a number of threads at a time equal to
    /// <see cref="Environment.ProcessorCount" />.
    /// </summary>
    public sealed class ProcessorCountSemaphoreControl : SemaphoreControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorCountSemaphoreControl" /> class.
        /// </summary>
        /// <param name="blockTimeoutThreshold">
        /// The maximum length of time to block a thread before raising an exception, or <see cref="Timeout.InfiniteTimeSpan" /> if
        /// indefinite thread blocking is permitted.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="blockTimeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> and is not equal to
        /// <see cref="Timeout.InfiniteTimeSpan" />
        /// </exception>
        [DebuggerHidden]
        internal ProcessorCountSemaphoreControl(TimeSpan blockTimeoutThreshold)
            : base(Environment.ProcessorCount, blockTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ProcessorCountSemaphoreControl" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}