// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents a spin lock that is used to restrict access the target resource(s) by a maximum of one thread concurrently.
    /// </summary>
    public sealed class SingleThreadSpinLockControl : ConcurrencyControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleThreadSpinLockControl" /> class.
        /// </summary>
        /// <param name="blockTimeoutThreshold">
        /// The maximum length of time to block a thread before raising an exception, or <see cref="Timeout.InfiniteTimeSpan" /> if
        /// indefinite thread blocking is permitted.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="blockTimeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> and is not equal to
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </exception>
        [DebuggerHidden]
        internal SingleThreadSpinLockControl(TimeSpan blockTimeoutThreshold)
            : base(blockTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SingleThreadSpinLockControl" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource.
        /// </summary>
        /// <returns>
        /// The resulting consumption state of the current <see cref="SingleThreadSpinLockControl" />.
        /// </returns>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The lock was not acquired.
        /// </exception>
        /// <exception cref="LockRecursionException">
        /// The current thread already owns the lock.
        /// </exception>
        protected sealed override ConcurrencyControlConsumptionState EnterWithoutTimeout()
        {
            var lockTaken = false;
            Spin.TryEnter(ref lockTaken);

            if (lockTaken)
            {
                return ConcurrencyControlConsumptionState.FullyClaimed;
            }

            throw new ConcurrencyControlOperationException("The operation failed to acquire a spin lock.");
        }

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource and
        /// specifies a timeout threshold.
        /// </summary>
        /// <param name="blockTimeoutThreshold">
        /// The maximum length of time to block a thread before raising an exception.
        /// </param>
        /// <returns>
        /// The resulting consumption state of the current <see cref="SingleThreadSpinLockControl" />.
        /// </returns>
        /// <exception cref="LockRecursionException">
        /// The current thread already owns the lock.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        protected sealed override ConcurrencyControlConsumptionState EnterWithTimeout(TimeSpan blockTimeoutThreshold)
        {
            var lockTaken = false;
            Spin.TryEnter(blockTimeoutThreshold, ref lockTaken);

            if (lockTaken)
            {
                return ConcurrencyControlConsumptionState.FullyClaimed;
            }

            throw new TimeoutException($"The operation failed to acquire a spin lock after {blockTimeoutThreshold.ToSerializedString()}.");
        }

        /// <summary>
        /// Informs the control that a thread is exiting a block of code or has finished consuming a resource.
        /// </summary>
        /// <param name="exitedSuccessfully">
        /// A value indicating whether or not the exit operation was successful. The initial value is <see langword="false" />.
        /// </param>
        /// <returns>
        /// The resulting consumption state of the current <see cref="SingleThreadSpinLockControl" />.
        /// </returns>
        /// <exception cref="SynchronizationLockException">
        /// The current thread does not own the lock.
        /// </exception>
        protected sealed override ConcurrencyControlConsumptionState Exit(ref Boolean exitedSuccessfully)
        {
            try
            {
                Spin.Exit();
                exitedSuccessfully = true;
                return ConcurrencyControlConsumptionState.Unclaimed;
            }
            catch
            {
                exitedSuccessfully = false;
                throw;
            }
        }

        /// <summary>
        /// Represents the underlying concurrency control mechanism.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SpinLock Spin = new SpinLock(false);
    }
}