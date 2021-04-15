// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Caching.Distributed;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents a dual layer (local and remote) read-write client for accessing strongly typed cached objects using textual keys.
    /// </summary>
    public sealed class DualLayerCacheClient : CacheClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DualLayerCacheClient" /> class.
        /// </summary>
        /// <param name="remoteCache">
        /// The underlying remote (distributed) cache.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="remoteCache" /> is <see langword="null" />.
        /// </exception>
        public DualLayerCacheClient(IDistributedCache remoteCache)
            : this(remoteCache, InMemoryCachingStrategy.Moderate)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DualLayerCacheClient" /> class.
        /// </summary>
        /// <param name="remoteCache">
        /// The underlying remote (distributed) cache.
        /// </param>
        /// <param name="localCachingStrategy">
        /// A value specifying the cache access and management behavior of the local (in-memory) client. The default value is
        /// <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="remoteCache" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="localCachingStrategy" /> is equal to <see cref="InMemoryCachingStrategy.Unspecified" />.
        /// </exception>
        public DualLayerCacheClient(IDistributedCache remoteCache, InMemoryCachingStrategy localCachingStrategy)
            : base(ConcurrencyControlMode.Unconstrained)
        {
            LocalCache = new InMemoryCacheClient(localCachingStrategy);
            RemoteCache = new DistributedCacheClient(remoteCache.RejectIf().IsNull(nameof(remoteCache)).TargetArgument);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DualLayerCacheClient" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                LocalCache?.Dispose();
                RemoteCache?.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Removes the cached object using the specified textual key.
        /// </summary>
        /// <param name="key">
        /// A textual key which uniquely identifies the cached object to remove.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
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
        protected override void Invalidate(String key, IConcurrencyControlToken controlToken)
        {
            try
            {
                RemoteCache.Invalidate(key);
            }
            finally
            {
                LocalCache.Invalidate(key);
            }
        }

        /// <summary>
        /// Attempts to retrieve the cached object using the specified textual key.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the cached object.
        /// </typeparam>
        /// <param name="key">
        /// A textual key which uniquely identifies a value in the cache.
        /// </param>
        /// <returns>
        /// The object retrieved from the cache, or <see langword="null" /> if the object was not successfully retrieved.
        /// </returns>
        protected override TValue TryRead<TValue>(String key)
            where TValue : class
        {
            if (IsDisposedOrDisposing)
            {
                return null;
            }
            else if (LocalCache.TryRead<TValue>(key, out var localValue))
            {
                return localValue;
            }
            else if (RemoteCache.TryRead<TValue>(key, out var remoteValue))
            {
                if (IsDisposedOrDisposing)
                {
                    return remoteValue;
                }

                try
                {
                    LocalCache.Write(key, remoteValue);
                }
                catch
                {
                    return remoteValue;
                }

                return remoteValue;
            }

            return null;
        }

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
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void Write<TValue>(String key, TValue value, IConcurrencyControlToken controlToken)
            where TValue : class
        {
            try
            {
                RemoteCache.Write(key, value);
            }
            finally
            {
                LocalCache.Write(key, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the client is operative.
        /// </summary>
        public override Boolean IsOperative => LocalCache.IsOperative || RemoteCache.IsOperative;

        /// <summary>
        /// Represents the underlying local (in-memory) cache client.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IInMemoryCacheClient LocalCache;

        /// <summary>
        /// Represents the underlying remote (distributed) cache client.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDistributedCacheClient RemoteCache;
    }
}