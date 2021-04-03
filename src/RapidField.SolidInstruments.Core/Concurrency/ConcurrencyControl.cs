// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            ConsumptionState = ConcurrencyControlConsumptionState.Unclaimed;
        }

        /// <summary>
        /// Finalizes the current <see cref="ConcurrencyControl" />.
        /// </summary>
        [DebuggerHidden]
        ~ConcurrencyControl()
        {
            Dispose(false);
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
        public static ConcurrencyControl New(ConcurrencyControlMode mode, TimeSpan blockTimeoutThreshold) => mode.RejectIf().IsEqualToValue(ConcurrencyControlMode.Unspecified).TargetArgument switch
        {
            ConcurrencyControlMode.DuplexSemaphore => new DuplexSemaphoreControl(blockTimeoutThreshold),
            ConcurrencyControlMode.ProcessorCountSemaphore => new ProcessorCountSemaphoreControl(blockTimeoutThreshold),
            ConcurrencyControlMode.SingleThreadLock => new SingleThreadLockControl(blockTimeoutThreshold),
            ConcurrencyControlMode.SingleThreadSpinLock => new SingleThreadSpinLockControl(blockTimeoutThreshold),
            ConcurrencyControlMode.Unconstrained => new UnconstrainedControl(),
            _ => throw new UnsupportedSpecificationException($"The specified concurrency control mode, {mode}, is not supported.")
        };

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ConcurrencyControl" />.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Asynchronously releases all resources consumed by the current <see cref="ConcurrencyControl" />.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public ValueTask DisposeAsync() => new(Task.Factory.StartNew(Dispose));

        /// <summary>
        /// Enqueues the specified operation for controlled execution and returns without waiting.
        /// </summary>
        /// <param name="action">
        /// An action to execute asynchronously.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public void Enqueue(Action action) => _ = Task.Factory.StartNew(() => EnqueueAndWait(action)).ContinueWith(enqueueTask => enqueueTask?.Dispose());

        /// <summary>
        /// Enqueues the specified operation for controlled execution and returns without waiting.
        /// </summary>
        /// <param name="task">
        /// A task to execute asynchronously.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        public void Enqueue(Task task) => _ = Task.Factory.StartNew(() => EnqueueAndWait(task)).ContinueWith(enqueueTask => enqueueTask?.Dispose());

        /// <summary>
        /// Enqueues the specified operation for controlled execution and returns without waiting.
        /// </summary>
        /// <param name="action">
        /// An action to execute asynchronously.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public void Enqueue(Action<IConcurrencyControlToken> action) => _ = Task.Factory.StartNew(() => EnqueueAndWait(action)).ContinueWith(enqueueTask => enqueueTask?.Dispose());

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <param name="action">
        /// An action to execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        public void EnqueueAndWait(Action action)
        {
            _ = action.RejectIf().IsNull(nameof(action));
            using var controlToken = Enter();
            action();
        }

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <param name="task">
        /// A task to execute.
        /// </param>
        /// <exception cref="AggregateException">
        /// An exception was raised while executing <paramref name="task" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public void EnqueueAndWait(Task task)
        {
            using var enqueueTask = EnqueueAsync(task);
            enqueueTask.Wait();
        }

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <param name="action">
        /// An action to execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        public void EnqueueAndWait(Action<IConcurrencyControlToken> action)
        {
            _ = action.RejectIf().IsNull(nameof(action));
            using var controlToken = Enter();
            action(controlToken);
        }

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="function" />.
        /// </typeparam>
        /// <param name="function">
        /// A function to execute.
        /// </param>
        /// <returns>
        /// The result of <paramref name="function" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        public T EnqueueAndWait<T>(Func<T> function)
        {
            _ = function.RejectIf().IsNull(nameof(function));
            using var controlToken = Enter();
            return function();
        }

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="task" />.
        /// </typeparam>
        /// <param name="task">
        /// A task to execute.
        /// </param>
        /// <returns>
        /// The result of <paramref name="task" />.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while executing <paramref name="task" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public T EnqueueAndWait<T>(Task<T> task)
        {
            using var enqueueTask = EnqueueAsync(task);
            enqueueTask.Wait();
            return enqueueTask.Result;
        }

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="function" />.
        /// </typeparam>
        /// <param name="function">
        /// A function to execute.
        /// </param>
        /// <returns>
        /// The result of <paramref name="function" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        public T EnqueueAndWait<T>(Func<IConcurrencyControlToken, T> function)
        {
            _ = function.RejectIf().IsNull(nameof(function));
            using var controlToken = Enter();
            return function(controlToken);
        }

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution and returns a task that completes when the
        /// action is finished running.
        /// </summary>
        /// <param name="action">
        /// An action to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        public Task EnqueueAsync(Action action)
        {
            _ = action.RejectIf().IsNull(nameof(action));
            return EnqueueAsync(controlToken => action());
        }

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution.
        /// </summary>
        /// <param name="task">
        /// A task to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while executing <paramref name="task" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public Task EnqueueAsync(Task task)
        {
            _ = task.RejectIf().IsNull(nameof(task));
            return Task.Factory.StartNew(() =>
            {
                using var controlToken = Enter();

                try
                {
                    task.Wait();
                }
                finally
                {
                    task.Dispose();
                }
            });
        }

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution and returns a task that completes when the
        /// action is finished running.
        /// </summary>
        /// <param name="action">
        /// An action to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        public Task EnqueueAsync(Action<IConcurrencyControlToken> action)
        {
            _ = action.RejectIf().IsNull(nameof(action));
            return Task.Factory.StartNew(() =>
            {
                using var controlToken = Enter();
                action(controlToken);
            });
        }

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution and returns a task that completes when the
        /// function is finished running.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="function" />.
        /// </typeparam>
        /// <param name="function">
        /// A function to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the result of <paramref name="function" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        public Task<T> EnqueueAsync<T>(Func<T> function)
        {
            _ = function.RejectIf().IsNull(nameof(function));
            return EnqueueAsync(controlToken => function());
        }

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="task" />.
        /// </typeparam>
        /// <param name="task">
        /// A task to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the result of <paramref name="task" />.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while executing <paramref name="task" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public Task<T> EnqueueAsync<T>(Task<T> task)
        {
            _ = task.RejectIf().IsNull(nameof(task));
            return Task.Factory.StartNew(() =>
            {
                using var controlToken = Enter();

                try
                {
                    task.Wait();
                    return task.Result;
                }
                finally
                {
                    task.Dispose();
                }
            });
        }

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution and returns a task that completes when the
        /// function is finished running.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="function" />.
        /// </typeparam>
        /// <param name="function">
        /// A function to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the result of <paramref name="function" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        public Task<T> EnqueueAsync<T>(Func<IConcurrencyControlToken, T> function)
        {
            _ = function.RejectIf().IsNull(nameof(function));
            return Task.Factory.StartNew(() =>
            {
                using var controlToken = Enter();
                return function(controlToken);
            });
        }

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource.
        /// </summary>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public IConcurrencyControlToken Enter()
        {
            try
            {
                if (IsDisposed)
                {
                    ConsumptionState = ConcurrencyControlConsumptionState.Unclaimed;
                    return GetNextToken(SynchronizationContext.Current, Thread.CurrentThread, Timeout.InfiniteTimeSpan, null);
                }
                else if (BlockTimeoutThresholdIsInfinite)
                {
                    ConsumptionState = EnterWithoutTimeout();
                    return GetNextToken(SynchronizationContext.Current, Thread.CurrentThread, Timeout.InfiniteTimeSpan, null);
                }

                var expirationStopwatch = Stopwatch.StartNew();
                ConsumptionState = EnterWithTimeout(BlockTimeoutThreshold);
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
        public void Exit(IConcurrencyControlToken token)
        {
            if (IsDisposed)
            {
                ConsumptionState = ConcurrencyControlConsumptionState.Unclaimed;
                Tokens?.Clear();
            }
            else if (Tokens?.TryRemove(token.Identifier, out _) ?? false)
            {
                var exitedSuccessfully = false;

                try
                {
                    ConsumptionState = Exit(ref exitedSuccessfully);
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

#if DEBUG
                if (Debugger.IsAttached)
                {
                    // Attaching a debugger during test execution breaks the expected behavior of Exit implementations. Leave this
                    // here as a conditional exception to the safeguard defined above.
                    return;
                }
#endif
            }

            throw new ConcurrencyControlOperationException("The specified token is not valid for release by the control. It was issued by a different control or it was already released.");
        }

        /// <summary>
        /// Converts the value of the current <see cref="ConcurrencyControl" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="ConcurrencyControl" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(ConsumptionState)}\": \"{ConsumptionState}\" }}";

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ConcurrencyControl" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            lock (DisposalSyncRoot ?? new())
            {
                if (IsDisposed)
                {
                    return;
                }

                try
                {
                    var waitDurationInMilliseconds = 1;

                    while ((Tokens?.Any() ?? false) && waitDurationInMilliseconds <= 1024)
                    {
                        if (IsDisposed)
                        {
                            break;
                        }

                        Thread.Sleep(waitDurationInMilliseconds);
                        waitDurationInMilliseconds *= 2;
                    }
                }
                finally
                {
                    IsDisposed = true;
                }
            }
        }

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource.
        /// </summary>
        /// <returns>
        /// The resulting consumption state of the current <see cref="ConcurrencyControl" />.
        /// </returns>
        protected abstract ConcurrencyControlConsumptionState EnterWithoutTimeout();

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource and
        /// specifies a timeout threshold.
        /// </summary>
        /// <param name="blockTimeoutThreshold">
        /// The maximum length of time to block a thread before raising an exception.
        /// </param>
        /// <returns>
        /// The resulting consumption state of the current <see cref="ConcurrencyControl" />.
        /// </returns>
        protected abstract ConcurrencyControlConsumptionState EnterWithTimeout(TimeSpan blockTimeoutThreshold);

        /// <summary>
        /// Informs the control that a thread is exiting a block of code or has finished consuming a resource.
        /// </summary>
        /// <param name="exitedSuccessfully">
        /// A value indicating whether or not the exit operation was successful. The initial value is <see langword="false" />.
        /// </param>
        /// <returns>
        /// The resulting consumption state of the current <see cref="ConcurrencyControl" />.
        /// </returns>
        protected abstract ConcurrencyControlConsumptionState Exit(ref Boolean exitedSuccessfully);

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
        private IConcurrencyControlToken GetNextToken(SynchronizationContext context, Thread granteeThread, TimeSpan expirationThreshold, Stopwatch expirationStopwatch)
        {
            var identifier = Interlocked.Increment(ref NextTokenIdentifier);
            Interlocked.CompareExchange(ref NextTokenIdentifier, 0, MaximumTokenIdentifier);
            var controlToken = new ConcurrencyControlToken(context, granteeThread, this, identifier, expirationThreshold, expirationStopwatch);

            if (Tokens?.TryAdd(identifier, controlToken) ?? true)
            {
                return controlToken;
            }

            throw new ConcurrencyControlOperationException("The control failed to create a new control token.");
        }

        /// <summary>
        /// Gets the consumption state of the current <see cref="ConcurrencyControl" />.
        /// </summary>
        public ConcurrencyControlConsumptionState ConsumptionState
        {
            get => (ConcurrencyControlConsumptionState)Convert.ToInt32(Interlocked.Read(ref ConsumptionStateValue));
            private set => Interlocked.Exchange(ref ConsumptionStateValue, Convert.ToInt64((Int32)value));
        }

        /// <summary>
        /// Represents the highest assignable token identifier.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MaximumTokenIdentifier = Int32.MaxValue - 256;

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
        /// Represents an object that can be used to synchronize object disposal.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Object DisposalSyncRoot = new();

        /// <summary>
        /// Represents currently-in-use tokens that were issued by the current <see cref="ConcurrencyControl" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<Int32, IConcurrencyControlToken> Tokens = new();

        /// <summary>
        /// Represents the consumption state of the current <see cref="ConcurrencyControl" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int64 ConsumptionStateValue;

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