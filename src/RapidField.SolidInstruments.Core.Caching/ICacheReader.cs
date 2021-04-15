// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents a read-only client for accessing strongly-typed cached objects using textual keys.
    /// </summary>
    public interface ICacheReader : IDisposable
    {
        /// <summary>
        /// Attempts to retrieve the cached object using the specified textual key.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the cached object.
        /// </typeparam>
        /// <param name="key">
        /// A textual key which uniquely identifies <paramref name="value" /> in the cache.
        /// </param>
        /// <param name="value">
        /// The object to retrieve from the cache, or <see langword="null" /> if the object was not successfully retrieved.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the object was successfully retrieved, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CacheAccessException">
        /// An exception was raised while attempting to access the cache.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean TryRead<TValue>(String key, out TValue value)
            where TValue : class;
    }
}