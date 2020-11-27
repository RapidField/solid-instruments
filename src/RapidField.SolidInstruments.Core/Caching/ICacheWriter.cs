// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents a write-only client for accessing strongly-typed cached objects using textual keys.
    /// </summary>
    public interface ICacheWriter : IDisposable
    {
        /// <summary>
        /// Adds or updates the specified object using the specified key.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the cached object.
        /// </typeparam>
        /// <param name="key">
        /// A textual key which uniquely identifies <paramref name="value" /> in the cache.
        /// </param>
        /// <param name="value">
        /// The object to add or update.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" /> -or- <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CacheAccessException">
        /// An exception was raised while attempting to access the cache.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Write<TValue>(String key, TValue value)
            where TValue : class;
    }
}