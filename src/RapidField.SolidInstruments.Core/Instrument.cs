// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a utility with disposable resources and exposes a lazily-loaded concurrency control mechanism.
    /// </summary>
    public abstract class Instrument : IDisposable
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
            LazyStateControl = new Lazy<IConcurrencyControl>(InitializeStateControl, LazyThreadSafetyMode.ExecutionAndPublication);
        }

#pragma warning disable CA1063

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Instrument" />.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposedOrDisposing)
            {
                return;
            }

            IsDisposing = true;

            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            finally
            {
                IsDisposed = true;
                IsDisposing = false;
            }
        }

#pragma warning restore CA1063

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
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                LazyStateControl.Dispose();
            }
        }

        /// <summary>
        /// Raises a new <see cref="ObjectDisposedException" /> if the current <see cref="Instrument" /> is disposed or is currently
        /// in the process of being disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected void RejectIfDisposed()
        {
            if (IsDisposedOrDisposing)
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        /// <summary>
        /// Gets a concurrency control mechanism that is used to manage state for the current <see cref="Instrument" />.
        /// </summary>
        protected internal IConcurrencyControl StateControl => LazyStateControl.Value;

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
        protected Boolean IsDisposedOrDisposing => (IsDisposed || IsDisposing);

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
        /// Represents a lazily-initialized concurrency control mechanism that is used to manage state for the current
        /// <see cref="Instrument" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IConcurrencyControl> LazyStateControl;
    }
}