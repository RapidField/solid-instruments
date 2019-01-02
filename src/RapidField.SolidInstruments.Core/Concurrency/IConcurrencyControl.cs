// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents a concurrency control mechanism.
    /// </summary>
    public interface IConcurrencyControl : IDisposable
    {
        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consuming a resource.
        /// </summary>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        ConcurrencyControlToken Enter();

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
        void Exit(ConcurrencyControlToken token);
    }
}