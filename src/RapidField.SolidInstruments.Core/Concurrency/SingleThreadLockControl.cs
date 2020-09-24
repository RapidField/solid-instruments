// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents a lock that is used to restrict access the target resource(s) by a maximum of one thread concurrently.
    /// </summary>
    public sealed class SingleThreadLockControl : SemaphoreControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleThreadLockControl" /> class.
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
        internal SingleThreadLockControl(TimeSpan blockTimeoutThreshold)
            : base(1, blockTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SingleThreadLockControl" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}