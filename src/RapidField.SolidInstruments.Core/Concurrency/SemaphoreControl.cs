// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents a semaphore that is used to restrict access the target resource(s) by a maximum number of threads concurrently.
    /// </summary>
    public abstract class SemaphoreControl : ConcurrencyControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreControl" /> class.
        /// </summary>
        /// <param name="maximumConcurrencyLimit">
        /// The maximum number of threads that may enter the semaphore concurrently.
        /// </param>
        /// <param name="blockTimeoutThreshold">
        /// The maximum length of time to block a thread before raising an exception, or <see cref="Timeout.InfiniteTimeSpan" /> if
        /// indefinite thread blocking is permitted.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maximumConcurrencyLimit" /> is less than one -or- <paramref name="blockTimeoutThreshold" /> is less than
        /// or equal to <see cref="TimeSpan.Zero" /> and is not equal to <see cref="Timeout.InfiniteTimeSpan" />
        /// </exception>
        protected SemaphoreControl(Int32 maximumConcurrencyLimit, TimeSpan blockTimeoutThreshold)
            : base(blockTimeoutThreshold)
        {
            MaximumConcurrencyLimit = maximumConcurrencyLimit.RejectIf().IsLessThan(1, nameof(maximumConcurrencyLimit));
            Semaphore = new SemaphoreSlim(MaximumConcurrencyLimit);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SemaphoreControl" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    Semaphore.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource.
        /// </summary>
        /// <returns>
        /// The resulting consumption state of the current <see cref="SemaphoreControl" />.
        /// </returns>
        protected sealed override ConcurrencyControlConsumptionState EnterWithoutTimeout()
        {
            Semaphore.Wait();
            return Semaphore.CurrentCount == 0 ? ConcurrencyControlConsumptionState.FullyClaimed : ConcurrencyControlConsumptionState.PartiallyClaimed;
        }

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource and
        /// specifies a timeout threshold.
        /// </summary>
        /// <param name="blockTimeoutThreshold">
        /// The maximum length of time to block a thread before raising an exception.
        /// </param>
        /// <returns>
        /// The resulting consumption state of the current <see cref="SemaphoreControl" />.
        /// </returns>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        protected sealed override ConcurrencyControlConsumptionState EnterWithTimeout(TimeSpan blockTimeoutThreshold)
        {
            if (Semaphore.Wait(blockTimeoutThreshold))
            {
                return Semaphore.CurrentCount == 0 ? ConcurrencyControlConsumptionState.FullyClaimed : ConcurrencyControlConsumptionState.PartiallyClaimed;
            }

            throw new TimeoutException($"The operation failed to enter the semaphore after {blockTimeoutThreshold.ToSerializedString()}.");
        }

        /// <summary>
        /// Informs the control that a thread is exiting a block of code or has finished consuming a resource.
        /// </summary>
        /// <param name="exitedSuccessfully">
        /// A value indicating whether or not the exit operation was successful. The initial value is <see langword="false" />.
        /// </param>
        /// <returns>
        /// The resulting consumption state of the current <see cref="SemaphoreControl" />.
        /// </returns>
        /// <exception cref="SemaphoreFullException">
        /// The semaphore has already reached its maximum size.
        /// </exception>
        protected sealed override ConcurrencyControlConsumptionState Exit(ref Boolean exitedSuccessfully)
        {
            Semaphore.Release();
            return Semaphore.CurrentCount == MaximumConcurrencyLimit ? ConcurrencyControlConsumptionState.Unclaimed : ConcurrencyControlConsumptionState.PartiallyClaimed;
        }

        /// <summary>
        /// Represents the maximum number of threads that may enter the semaphore concurrently.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 MaximumConcurrencyLimit;

        /// <summary>
        /// Represents the underlying concurrency control mechanism.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SemaphoreSlim Semaphore;
    }
}