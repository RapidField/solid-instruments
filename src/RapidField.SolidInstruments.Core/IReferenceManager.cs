// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Tracks a collection of related object references and manages disposal of them.
    /// </summary>
    public interface IReferenceManager : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Instructs the current <see cref="IReferenceManager" /> to manage the specified object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the managed object.
        /// </typeparam>
        /// <param name="reference">
        /// The managed object.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddObject<T>(T reference)
            where T : class;

        /// <summary>
        /// Instructs the current <see cref="IReferenceManager" /> to manage the specified object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the managed object.
        /// </typeparam>
        /// <param name="reference">
        /// The managed object.
        /// </param>
        /// <param name="strongReferenceMinimumLifeSpan">
        /// The minimum length of time to preserve a strong reference to the managed object. The observed length of time may be
        /// shorter if the reference manager is disposed.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="strongReferenceMinimumLifeSpan" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddObject<T>(T reference, TimeSpan strongReferenceMinimumLifeSpan)
            where T : class;

        /// <summary>
        /// Gets the number of objects that are managed by the current <see cref="IReferenceManager" />.
        /// </summary>
        public Int32 ObjectCount
        {
            get;
        }
    }
}