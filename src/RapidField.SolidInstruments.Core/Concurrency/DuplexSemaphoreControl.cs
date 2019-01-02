// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents a semaphore that is used to restrict access the target resource(s) by a maximum of two threads concurrently.
    /// </summary>
    public sealed class DuplexSemaphoreControl : SemaphoreControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplexSemaphoreControl" /> class.
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
        internal DuplexSemaphoreControl(TimeSpan blockTimeoutThreshold)
            : base(2, blockTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DuplexSemaphoreControl" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}