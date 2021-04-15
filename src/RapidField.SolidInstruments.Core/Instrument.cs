// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a utility with disposable resources and exposes a lazily-loaded concurrency control mechanism.
    /// </summary>
    /// <remarks>
    /// <see cref="Instrument" /> is the default implementation of <see cref="IInstrument" />.
    /// </remarks>
    public abstract class Instrument : IInstrument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Instrument" /> class.
        /// </summary>
        protected Instrument()
            : this(DefaultStateControlMode)
        {
            IsDisposed = false;
            IsDisposing = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Instrument" /> class.
        /// </summary>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.SingleThreadLock" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" />.
        /// </exception>
        protected Instrument(ConcurrencyControlMode stateControlMode)
            : this(stateControlMode, DefaultStateControlTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Instrument" /> class.
        /// </summary>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.SingleThreadLock" />.
        /// </param>
        /// <param name="stateControlTimeoutThreshold">
        /// The maximum length of time that the instrument's state control may block a thread before raising an exception, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking is permitted. The default value is
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" /> -or-
        /// <paramref name="stateControlTimeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> and is not equal
        /// to <see cref="Timeout.InfiniteTimeSpan" />.
        /// </exception>
        protected Instrument(ConcurrencyControlMode stateControlMode, TimeSpan stateControlTimeoutThreshold)
        {
            StateControlMode = stateControlMode.RejectIf().IsEqualToValue(ConcurrencyControlMode.Unspecified, nameof(stateControlMode));
            StateControlTimeoutThreshold = (stateControlTimeoutThreshold == Timeout.InfiniteTimeSpan) ? stateControlTimeoutThreshold : stateControlTimeoutThreshold.RejectIf().IsLessThanOrEqualTo(TimeSpan.Zero, nameof(stateControlTimeoutThreshold));
            LazyStateControl = new(InitializeStateControl, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Finalizes the current <see cref="Instrument" />.
        /// </summary>
        [DebuggerHidden]
        ~Instrument()
        {
            Dispose();
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Instrument" />.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposedOrDisposing)
            {
                return;
            }

            lock (DisposalSyncRoot ?? new())
            {
                if (IsDisposedOrDisposing)
                {
                    return;
                }

                try
                {
                    WithStateControl(() => { IsDisposing = true; });
                }
                catch
                {
                    IsDisposing = true;
                    throw;
                }
                finally
                {
                    try
                    {
                        Dispose(true);
                    }
                    finally
                    {
                        GC.SuppressFinalize(this);
                        IsDisposed = true;
                        IsDisposing = false;
                    }
                }
            }
        }

        /// <summary>
        /// Asynchronously releases all resources consumed by the current <see cref="Instrument" />.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public ValueTask DisposeAsync() => new(Task.Factory.StartNew(Dispose));

        /// <summary>
        /// Initializes a concurrency control mechanism that is used to manage state for the current <see cref="Instrument" />.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="SemaphoreSlim" /> class.
        /// </returns>
        [DebuggerHidden]
        internal virtual IConcurrencyControl InitializeStateControl() => ConcurrencyControl.New(StateControlMode, StateControlTimeoutThreshold);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Instrument" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected virtual void Dispose(Boolean disposing) => LazyStateControl?.Dispose();

        /// <summary>
        /// Raises a new <see cref="ObjectDisposedException" /> if the current <see cref="Instrument" /> is disposed or is currently
        /// in the process of being disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RejectIfDisposed()
        {
            if (IsDisposedOrDisposing)
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        /// <summary>
        /// Enqueues the specified operation for state controlled execution and waits for it to complete.
        /// </summary>
        /// <param name="action">
        /// An action to execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        protected void WithStateControl(Action action)
        {
            _ = action.RejectIf().IsNull(nameof(action));
            RejectIfDisposed();
            StateControl.EnqueueAndWait(() =>
            {
                RejectIfDisposed();
                action();
            });
        }

        /// <summary>
        /// Enqueues the specified operation for state controlled execution and waits for it to complete.
        /// </summary>
        /// <param name="action">
        /// An action to execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected void WithStateControl(Action<IConcurrencyControlToken> action)
        {
            _ = action.RejectIf().IsNull(nameof(action));
            RejectIfDisposed();
            StateControl.EnqueueAndWait(controlToken =>
            {
                RejectIfDisposed();
                action(controlToken);
            });
        }

        /// <summary>
        /// Enqueues the specified operation for state controlled execution and waits for it to complete.
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
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected T WithStateControl<T>(Func<T> function)
        {
            _ = function.RejectIf().IsNull(nameof(function));
            RejectIfDisposed();
            return StateControl.EnqueueAndWait(() =>
            {
                RejectIfDisposed();
                return function();
            });
        }

        /// <summary>
        /// Enqueues the specified operation for state controlled execution and waits for it to complete.
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
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected T WithStateControl<T>(Func<IConcurrencyControlToken, T> function)
        {
            _ = function.RejectIf().IsNull(nameof(function));
            RejectIfDisposed();
            return StateControl.EnqueueAndWait(controlToken =>
            {
                RejectIfDisposed();
                return function(controlToken);
            });
        }

        /// <summary>
        /// Asynchronously enqueues the specified operation for state controlled execution and returns a task that completes when
        /// the action is finished running.
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
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected Task WithStateControlAsync(Action action)
        {
            _ = action.RejectIf().IsNull(nameof(action));
            RejectIfDisposed();
            return StateControl.EnqueueAsync(() =>
            {
                RejectIfDisposed();
                action();
            });
        }

        /// <summary>
        /// Asynchronously enqueues the specified operation for state controlled execution and returns a task that completes when
        /// the action is finished running.
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
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected Task WithStateControlAsync(Action<IConcurrencyControlToken> action)
        {
            _ = action.RejectIf().IsNull(nameof(action));
            RejectIfDisposed();
            return StateControl.EnqueueAsync(controlToken =>
            {
                RejectIfDisposed();
                action(controlToken);
            });
        }

        /// <summary>
        /// Asynchronously enqueues the specified operation for state controlled execution and returns a task that completes when
        /// the function is finished running.
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
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected Task<T> WithStateControlAsync<T>(Func<T> function)
        {
            _ = function.RejectIf().IsNull(nameof(function));
            RejectIfDisposed();
            return StateControl.EnqueueAsync(() =>
            {
                RejectIfDisposed();
                return function();
            });
        }

        /// <summary>
        /// Asynchronously enqueues the specified operation for state controlled execution and returns a task that completes when
        /// the function is finished running.
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
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected Task<T> WithStateControlAsync<T>(Func<IConcurrencyControlToken, T> function)
        {
            _ = function.RejectIf().IsNull(nameof(function));
            RejectIfDisposed();
            return StateControl.EnqueueAsync(controlToken =>
            {
                RejectIfDisposed();
                return function(controlToken);
            });
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="IInstrument" /> is fully occupied, as measured by thread
        /// saturation for state-controlling operations.
        /// </summary>
        /// <remarks>
        /// Interrogate this property to determine if the instrument is immediately available to perform an operation that reserves
        /// state control. This is useful for cases in which another resource may be utilized to perform the same operation.
        /// </remarks>
        public Boolean IsBusy => StateControl.ConsumptionState == ConcurrencyControlConsumptionState.FullyClaimed;

        /// <summary>
        /// Gets a concurrency control mechanism that is used to manage state for the current <see cref="Instrument" />.
        /// </summary>
        protected internal IConcurrencyControl StateControl => LazyStateControl?.Value ?? new UnconstrainedControl();

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="Instrument" /> has been disposed.
        /// </summary>
        protected Boolean IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="Instrument" /> has been disposed or is currently in the
        /// process of being disposed.
        /// </summary>
        protected Boolean IsDisposedOrDisposing => IsDisposed || IsDisposing;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="Instrument" /> is currently in the process of being
        /// disposed.
        /// </summary>
        protected Boolean IsDisposing
        {
            get;
            private set;
        }

        /// <summary>
        /// Represents the default concurrency control mode that is used to manage state.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const ConcurrencyControlMode DefaultStateControlMode = ConcurrencyControlMode.SingleThreadLock;

        /// <summary>
        /// Represents the default maximum length of time that <see cref="StateControl" /> may block a thread before raising an
        /// exception, or <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking is permitted.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly TimeSpan DefaultStateControlTimeoutThreshold = Timeout.InfiniteTimeSpan;

        /// <summary>
        /// Represents the concurrency control mode that is used to manage state for the current <see cref="Instrument" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal ConcurrencyControlMode StateControlMode;

        /// <summary>
        /// Represents maximum length of time that <see cref="StateControl" /> may block a thread before raising an exception, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking is permitted.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal TimeSpan StateControlTimeoutThreshold;

        /// <summary>
        /// Represents an object that can be used to synchronize object disposal.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Object DisposalSyncRoot = new();

        /// <summary>
        /// Represents a lazily-initialized concurrency control mechanism that is used to manage state for the current
        /// <see cref="Instrument" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IConcurrencyControl> LazyStateControl;
    }
}