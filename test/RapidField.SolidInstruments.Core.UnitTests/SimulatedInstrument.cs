// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Threading;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    /// <summary>
    /// Represents an <see cref="Instrument" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedInstrument : Instrument
    {
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
        public SimulatedInstrument(ConcurrencyControlMode stateControlMode)
            : base(stateControlMode)
        {
            NullableIntegerValue = null;
        }

        /// <summary>
        /// Simulates a thread-safe operation.
        /// </summary>
        public void SimulateThreadSafeOperation()
        {
            using (var controlToken = StateControl.Enter())
            {
                lock (SyncRoot)
                {
                    ConcurrencyCount++;
                }

                Thread.Sleep(ThreadSafeOperationDelayDuration);

                lock (SyncRoot)
                {
                    ConcurrencyCount--;
                }
            }
        }

        /// <summary>
        /// Sets the value of <see cref="NullableIntegerValue" /> equal to the specified value.
        /// </summary>
        /// <param name="value">
        /// An <see cref="Int32" /> value.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void StoreIntegerValue(Int32 value)
        {
            RejectIfDisposed();
            NullableIntegerValue = value;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedInstrument" />.
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
                    NullableIntegerValue = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets a nullable <see cref="Int32" /> value.
        /// </summary>
        public Int32? NullableIntegerValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not <see cref="SimulateThreadSafeOperation" /> produced a volatile state.
        /// </summary>
        public Boolean StateIsVolatile
        {
            get
            {
                var result = false;

                using (var controlToken = StateControl.Enter())
                {
                    lock (SyncRoot)
                    {
                        result = ConcurrencyCount != 0;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Represents the amount of time <see cref="SimulateThreadSafeOperation" /> delays for each invocation.
        /// </summary>
        private static readonly TimeSpan ThreadSafeOperationDelayDuration = TimeSpan.FromTicks(34);

        /// <summary>
        /// Represents an object that is used to synchronize access to the associated resource(s).
        /// </summary>
        private readonly Object SyncRoot = new Object();

        /// <summary>
        /// Represents the present concurrency for invocations of <see cref="SimulateThreadSafeOperation" />.
        /// </summary>
        private Int32 ConcurrencyCount = 0;
    }
}