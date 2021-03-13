// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents a read-write client for accessing strongly typed, locally cached objects using textual keys.
    /// </summary>
    /// <remarks>
    /// <see cref="InMemoryCacheClient" /> is the default implementation of <see cref="IInMemoryCacheClient" />.
    /// </remarks>
    public sealed class InMemoryCacheClient : CacheClient, IInMemoryCacheClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCacheClient" /> class.
        /// </summary>
        public InMemoryCacheClient()
            : this(DefaultStrategy)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCacheClient" /> class.
        /// </summary>
        /// <param name="strategy">
        /// A value specifying the cache access and management behavior of the client. The default value is
        /// <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="strategy" /> is equal to <see cref="InMemoryCachingStrategy.Unspecified" />.
        /// </exception>
        public InMemoryCacheClient(InMemoryCachingStrategy strategy)
            : base(strategy == InMemoryCachingStrategy.NoCaching ? ConcurrencyControlMode.Unconstrained : ConcurrencyControlMode.ProcessorCountSemaphore)
        {
            Strategy = strategy.RejectIf().IsEqualToValue(InMemoryCachingStrategy.Unspecified, nameof(strategy));
            LazyCache = IsOperative ? new(InitializeCache, LazyThreadSafetyMode.ExecutionAndPublication) : null;
            LazyCacheOptions = IsOperative ? new(InitializeCacheOptions, LazyThreadSafetyMode.ExecutionAndPublication) : null;
        }

        /// <summary>
        /// Removes all cached objects.
        /// </summary>
        /// <exception cref="CacheAccessException">
        /// An exception was raised while attempting to access the cache.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Clear()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                try
                {
                    if (IsOperative)
                    {
                        Cache?.Compact(1d);
                    }
                }
                catch (Exception exception)
                {
                    throw new CacheAccessException("An exception was raised while clearing the cache.", exception);
                }
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="InMemoryCacheClient" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (IsOperative)
                {
                    LazyCache?.Dispose();
                }
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
            if (IsOperative)
            {
                Cache?.Remove(key);
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
            if (IsOperative)
            {
                return Cache.TryGetValue(key, out var value) ? value as TValue : null;
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
            if (IsOperative)
            {
                var options = Strategy.ToCacheEntryOptions();
                options.Size = value.CalculateSizeInBytes();
                _ = Cache?.Set(key, value, options);
            }
        }

        /// <summary>
        /// Initializes the underlying memory cache.
        /// </summary>
        /// <returns>
        /// A new <see cref="MemoryCache" />.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private MemoryCache InitializeCache() => new(CacheOptions);

        /// <summary>
        /// Initializes the configuration options for <see cref="Cache" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="MemoryCache" />.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IOptions<MemoryCacheOptions> InitializeCacheOptions() => Strategy.ToCacheOptions();

        /// <summary>
        /// Gets a value indicating whether or not the client is operative.
        /// </summary>
        public override Boolean IsOperative => Strategy != InMemoryCachingStrategy.NoCaching;

        /// <summary>
        /// Gets the underlying memory cache.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MemoryCache Cache => LazyCache?.Value;

        /// <summary>
        /// Gets the configuration options for <see cref="Cache" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IOptions<MemoryCacheOptions> CacheOptions => LazyCacheOptions?.Value;

        /// <summary>
        /// Represents the default value specifying the cache access and management behavior of the client.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const InMemoryCachingStrategy DefaultStrategy = InMemoryCachingStrategy.Moderate;

        /// <summary>
        /// Represents the underlying, lazily-initialized memory cache.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<MemoryCache> LazyCache;

        /// <summary>
        /// Represents lazily-initialized configuration options for <see cref="Cache" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IOptions<MemoryCacheOptions>> LazyCacheOptions;

        /// <summary>
        /// Represents a value specifying the cache access and management behavior of the client.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InMemoryCachingStrategy Strategy;
    }
}