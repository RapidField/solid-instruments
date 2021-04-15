// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents a read-write client for accessing strongly typed, locally cached objects using textual keys.
    /// </summary>
    public interface IInMemoryCacheClient : ICacheClient
    {
        /// <summary>
        /// Removes all cached objects.
        /// </summary>
        /// <exception cref="CacheAccessException">
        /// An exception was raised while attempting to access the cache.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Clear();
    }
}