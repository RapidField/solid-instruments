// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents a concurrency control mechanism.
    /// </summary>
    /// <remarks>
    /// <see cref="ConcurrencyControl" /> is the default implementation of <see cref="IConcurrencyControl" />.
    /// </remarks>
    public abstract class ConcurrencyControl : IConcurrencyControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyControl" /> class.
        /// </summary>
        /// <param name="blockTimeoutThreshold">
        /// The maximum length of time to block a thread before raising an exception, or <see cref="Timeout.InfiniteTimeSpan" /> if
        /// indefinite thread blocking is permitted.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="blockTimeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> and is not equal to
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </exception>
        protected ConcurrencyControl(TimeSpan blockTimeoutThreshold)
        {
            if (blockTimeoutThreshold == Timeout.InfiniteTimeSpan)
            {
                BlockTimeoutThreshold = blockTimeoutThreshold;
                BlockTimeoutThresholdIsInfinite = true;
                return;
            }

            BlockTimeoutThreshold = blockTimeoutThreshold.RejectIf().IsLessThanOrEqualTo(TimeSpan.Zero, nameof(blockTimeoutThreshold));
            BlockTimeoutThresholdIsInfinite = false;
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrencyControl" /> instance using the specified settings.
        /// </summary>
        /// <param name="mode">
        /// The logical concurrency control mode for the resulting control.
        /// </param>
        /// <returns>
        /// A new <see cref="ConcurrencyControl" /> instance.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="mode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" />.
        /// </exception>
        public static ConcurrencyControl New(ConcurrencyControlMode mode) => New(mode, Timeout.InfiniteTimeSpan);

        /// <summary>
        /// Creates a new <see cref="ConcurrencyControl" /> instance using the specified settings.
        /// </summary>
        /// <param name="mode">
        /// The logical concurrency control mode for the resulting control.
        /// </param>
        /// <param name="blockTimeoutThreshold">
        /// The maximum length of time that the resulting control may block a thread before raising an exception, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking is permitted. The default value is
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </param>
        /// <returns>
        /// A new <see cref="ConcurrencyControl" /> instance.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="blockTimeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> and is not equal to
        /// <see cref="Timeout.InfiniteTimeSpan" /> -or- <paramref name="mode" /> is equal to
        /// <see cref="ConcurrencyControlMode.Unspecified" />.
        /// </exception>
        public static ConcurrencyControl New(ConcurrencyControlMode mode, TimeSpan blockTimeoutThreshold)
        {
            switch (mode.RejectIf().IsEqualToValue(ConcurrencyControlMode.Unspecified).TargetArgument)
            {
                case ConcurrencyControlMode.DuplexSemaphore:

                    return new DuplexSemaphoreControl(blockTimeoutThreshold);

                case ConcurrencyControlMode.ProcessorCountSemaphore:

                    return new ProcessorCountSemaphoreControl(blockTimeoutThreshold);

                case ConcurrencyControlMode.SingleThreadLock:

                    return new SingleThreadLockControl(blockTimeoutThreshold);

                case ConcurrencyControlMode.SingleThreadSpinLock:

                    return new SingleThreadSpinLockControl(blockTimeoutThreshold);

                case ConcurrencyControlMode.Unconstrained:

                    return new UnconstrainedControl();

                default:

                    throw new InvalidOperationException($"The specified concurrency control mode, {mode}, is not supported.");
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ConcurrencyControl" />.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource.
        /// </summary>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public ConcurrencyControlToken Enter()
        {
            RejectIfDisposed();

            try
            {
                if (BlockTimeoutThresholdIsInfinite)
                {
                    EnterWithoutTimeout();
                    return GetNextToken(SynchronizationContext.Current, Thread.CurrentThread, Timeout.InfiniteTimeSpan, null);
                }

                var expirationStopwatch = Stopwatch.StartNew();
                EnterWithTimeout(BlockTimeoutThreshold);
                return GetNextToken(SynchronizationContext.Current, Thread.CurrentThread, BlockTimeoutThreshold, expirationStopwatch);
            }
            catch (ConcurrencyControlOperationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new ConcurrencyControlOperationException(exception);
            }
        }

        /// <summary>
        /// Informs the control that a thread is exiting a block of code or has finished consuming a resource.
        /// </summary>
        /// <param name="token">
        /// A token issued by the current <see cref="IConcurrencyControl" /> that is no longer in use.
        /// </param>
        /// <exception cref="ConcurrencyControlOperationException">
        /// <paramref name="token" /> was not issued by this control or the <see cref="IConcurrencyControl" /> is in an invalid
        /// state.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Exit(ConcurrencyControlToken token)
        {
            RejectIfDisposed();

            if (Tokens.TryRemove(token.Identifier, out var releasedToken))
            {
                var exitedSuccessfully = false;

                try
                {
                    Exit(ref exitedSuccessfully);
                }
                catch (ConcurrencyControlOperationException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new ConcurrencyControlOperationException(exception);
                }

                if (exitedSuccessfully)
                {
                    return;
                }
            }

            throw new ConcurrencyControlOperationException("The specified token is not valid for release by the control. It was issued by a different control or it was already released.");
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ConcurrencyControl" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (Tokens.Any())
                {
                    // Reject new entries and wait for pending exits.
                    IsDisposed = true;
                    SpinWait.SpinUntil(() => Tokens.Any() == false);
                }
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource.
        /// </summary>
        protected abstract void EnterWithoutTimeout();

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource and
        /// specifies a timeout threshold.
        /// </summary>
        /// <param name="blockTimeoutThreshold">
        /// The maximum length of time to block a thread before raising an exception.
        /// </param>
        protected abstract void EnterWithTimeout(TimeSpan blockTimeoutThreshold);

        /// <summary>
        /// Informs the control that a thread is exiting a block of code or has finished consuming a resource.
        /// </summary>
        /// <param name="exitedSuccessfully">
        /// A value indicating whether or not the exit operation was successful. The initial value is <see langword="false" />.
        /// </param>
        protected abstract void Exit(ref Boolean exitedSuccessfully);

        /// <summary>
        /// Creates a new, uniquely-identified <see cref="ConcurrencyControlToken" /> and adds it to <see cref="Tokens" /> in a
        /// thread-safe manner.
        /// </summary>
        /// <param name="context">
        /// The synchronization context for the thread that is being granted a control token, or <see langword="null" /> if the
        /// context is unspecified.
        /// </param>
        /// <param name="granteeThread">
        /// The thread that is being granted a control token.
        /// </param>
        /// <param name="expirationThreshold">
        /// The maximum length of time to honor the token before releasing control to other threads, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking is permitted.
        /// </param>
        /// <param name="expirationStopwatch">
        /// A stopwatch that measures the total wait time for the controlled operation, or <see langword="null" /> if indefinite
        /// thread blocking is permitted.
        /// </param>
        /// <returns>
        /// A new <see cref="ConcurrencyControlToken" />.
        /// </returns>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        [DebuggerHidden]
        private ConcurrencyControlToken GetNextToken(SynchronizationContext context, Thread granteeThread, TimeSpan expirationThreshold, Stopwatch expirationStopwatch)
        {
            var identifier = Interlocked.Increment(ref NextTokenIdentifier);
            Interlocked.CompareExchange(ref NextTokenIdentifier, 0, MaximumTokenIdentifier);
            var controlToken = new ConcurrencyControlToken(context, granteeThread, this, identifier, expirationThreshold, expirationStopwatch);

            if (Tokens.TryAdd(identifier, controlToken))
            {
                return controlToken;
            }

            throw new ConcurrencyControlOperationException("The control failed to create a new control token.");
        }

        /// <summary>
        /// Raises a new <see cref="ObjectDisposedException" /> if the current <see cref="ConcurrencyControl" /> is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private void RejectIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        /// <summary>
        /// Represents the highest assignable token identifier.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MaximumTokenIdentifier = (Int32.MaxValue - 256);

        /// <summary>
        /// Represents the maximum length of time to block a thread before raising an exception, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking is permitted.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TimeSpan BlockTimeoutThreshold;

        /// <summary>
        /// Represents a value indicating whether or not indefinite thread blocking is permitted.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean BlockTimeoutThresholdIsInfinite;

        /// <summary>
        /// Represents currently-in-use tokens that were issued by the current <see cref="ConcurrencyControl" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<Int32, ConcurrencyControlToken> Tokens = new ConcurrentDictionary<Int32, ConcurrencyControlToken>();

        /// <summary>
        /// Represents a value indicating whether or not the current <see cref="ConcurrencyControl" /> has been disposed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean IsDisposed = false;

        /// <summary>
        /// Represents the unique identifier for the next token issued by the current <see cref="ConcurrencyControl" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 NextTokenIdentifier = 0;
    }
}