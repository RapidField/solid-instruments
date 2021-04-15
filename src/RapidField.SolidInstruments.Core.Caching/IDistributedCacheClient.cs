// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents a read-write client for accessing strongly-typed, remotely cached objects using textual keys.
    /// </summary>
    public interface IDistributedCacheClient : ICacheClient
    {
        /// <summary>
        /// Manually resets the sliding expiration for the specified object.
        /// </summary>
        /// <remarks>
        /// Under normal use scenarios it is not necessary to invoke <see cref="Refresh(String)" /> because implementations of
        /// <see cref="ICacheReader.TryRead{TValue}(String, out TValue)" /> are expected to reset the sliding expiration.
        /// </remarks>
        /// <param name="key">
        /// A textual key which uniquely identifies the cached object for which the sliding expiration is reset.
        /// </param>
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
        public void Refresh(String key);
    }
}