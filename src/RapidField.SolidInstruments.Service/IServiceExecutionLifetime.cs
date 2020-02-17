// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Service
{
    /// <summary>
    /// Provides control over the lifetime of execution for a service.
    /// </summary>
    public interface IServiceExecutionLifetime : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Unblocks waiting threads and ends the execution lifetime.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        void End();

        /// <summary>
        /// Blocks the current thread until <see cref="End" /> or <see cref="IDisposable.Dispose()" /> is invoked.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The service execution lifetime has ended.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        void KeepAlive();

        /// <summary>
        /// Gets a value that indicates whether or not the service is operational.
        /// </summary>
        Boolean IsAlive
        {
            get;
        }
    }
}