// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Service
{
    /// <summary>
    /// Provides control over the lifetime of execution for a service.
    /// </summary>
    /// <remarks>
    /// <see cref="ServiceExecutionLifetime" /> is the default implementation of <see cref="IServiceExecutionLifetime" />.
    /// </remarks>
    public sealed class ServiceExecutionLifetime : Instrument, IServiceExecutionLifetime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExecutionLifetime" /> class.
        /// </summary>
        [DebuggerHidden]
        internal ServiceExecutionLifetime()
            : base()
        {
            IsAlive = true;
        }

        /// <summary>
        /// Unblocks waiting threads and ends the execution lifetime.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void End()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (IsAlive)
                {
                    EndOfLifeEvent.Set();
                    IsAlive = false;
                }
            }
        }

        /// <summary>
        /// Blocks the current thread until <see cref="End" /> or <see cref="Dispose" /> is invoked.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The service execution lifetime has ended.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void KeepAlive()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (IsAlive == false)
                {
                    throw new InvalidOperationException("The service execution lifetime has ended.");
                }
            }

            EndOfLifeEvent.WaitOne();
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ServiceExecutionLifetime" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (StateControl is null)
                {
                    return;
                }

                using (var controlToken = StateControl.Enter())
                {
                    if (IsAlive)
                    {
                        EndOfLifeEvent?.Set();
                        IsAlive = false;
                    }

                    EndOfLifeEvent?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether or not the service is operational.
        /// </summary>
        public Boolean IsAlive
        {
            get;
            private set;
        }

        /// <summary>
        /// Represents an event that can be triggered to signal the end of an associated service's lifetime.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ManualResetEvent EndOfLifeEvent = new ManualResetEvent(false);
    }
}