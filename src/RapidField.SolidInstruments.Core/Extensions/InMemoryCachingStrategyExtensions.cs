// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="InMemoryCachingStrategy" /> enumeration with general purpose features.
    /// </summary>
    public static class InMemoryCachingStrategyExtensions
    {
        /// <summary>
        /// Converts the current <see cref="InMemoryCachingStrategy" /> to a memory cache entry options.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="InMemoryCachingStrategy" />.
        /// </param>
        /// <returns>
        /// Memory cache entry options representing the current <see cref="InMemoryCachingStrategy" />.
        /// </returns>
        [DebuggerHidden]
        internal static MemoryCacheEntryOptions ToCacheEntryOptions(this InMemoryCachingStrategy target) => target switch
        {
            InMemoryCachingStrategy.Aggressive => CacheEntryOptionsForAggressiveStrategy,
            InMemoryCachingStrategy.Conservative => CacheEntryOptionsForConservativeStrategy,
            InMemoryCachingStrategy.Moderate => CacheEntryOptionsForModerateStrategy,
            _ => throw new UnsupportedSpecificationException($"The specified caching strategy, {target}, is not supported."),
        };

        /// <summary>
        /// Converts the current <see cref="InMemoryCachingStrategy" /> to a memory cache options.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="InMemoryCachingStrategy" />.
        /// </param>
        /// <returns>
        /// Memory cache options representing the current <see cref="InMemoryCachingStrategy" />.
        /// </returns>
        [DebuggerHidden]
        internal static IOptions<MemoryCacheOptions> ToCacheOptions(this InMemoryCachingStrategy target) => target switch
        {
            InMemoryCachingStrategy.Aggressive => CacheOptionsForAggressiveStrategy,
            InMemoryCachingStrategy.Conservative => CacheOptionsForConservativeStrategy,
            InMemoryCachingStrategy.Moderate => CacheOptionsForModerateStrategy,
            _ => throw new UnsupportedSpecificationException($"The specified caching strategy, {target}, is not supported."),
        };

        /// <summary>
        /// Gets the amount of physical memory, in bytes, allocated for the current process.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Int64 AvailableProcessMemoryInBytes => Process.GetCurrentProcess().WorkingSet64;

        /// <summary>
        /// Gets the <see cref="MemoryCacheEntryOptions" /> settings associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static MemoryCacheEntryOptions CacheEntryOptionsForAggressiveStrategy => new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = AbsoluteExpirationRelativeToNowForAggressiveStrategy,
            SlidingExpiration = SlidingExpirationForAggressiveStrategy
        };

        /// <summary>
        /// Gets the <see cref="MemoryCacheEntryOptions" /> settings associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static MemoryCacheEntryOptions CacheEntryOptionsForConservativeStrategy => new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = AbsoluteExpirationRelativeToNowForConservativeStrategy,
            SlidingExpiration = SlidingExpirationForConservativeStrategy
        };

        /// <summary>
        /// Gets the <see cref="MemoryCacheEntryOptions" /> settings associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static MemoryCacheEntryOptions CacheEntryOptionsForModerateStrategy => new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = AbsoluteExpirationRelativeToNowForModerateStrategy,
            SlidingExpiration = SlidingExpirationForModerateStrategy
        };

        /// <summary>
        /// Gets the <see cref="MemoryCacheOptions" /> settings associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static IOptions<MemoryCacheOptions> CacheOptionsForAggressiveStrategy => new MemoryCacheOptions()
        {
            CompactionPercentage = CompactionPercentageForAggressiveStrategy,
            ExpirationScanFrequency = ExpirationScanFrequencyForAggressiveStrategy,
            SizeLimit = SizeLimitForAggressiveStrategy
        };

        /// <summary>
        /// Gets the <see cref="MemoryCacheOptions" /> settings associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static IOptions<MemoryCacheOptions> CacheOptionsForConservativeStrategy => new MemoryCacheOptions()
        {
            CompactionPercentage = CompactionPercentageForConservativeStrategy,
            ExpirationScanFrequency = ExpirationScanFrequencyForConservativeStrategy,
            SizeLimit = SizeLimitForConservativeStrategy
        };

        /// <summary>
        /// Gets the <see cref="MemoryCacheOptions" /> settings associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static IOptions<MemoryCacheOptions> CacheOptionsForModerateStrategy => new MemoryCacheOptions()
        {
            CompactionPercentage = CompactionPercentageForModerateStrategy,
            ExpirationScanFrequency = ExpirationScanFrequencyForModerateStrategy,
            SizeLimit = SizeLimitForModerateStrategy
        };

        /// <summary>
        /// Gets the <see cref="MemoryCacheOptions.SizeLimit" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Int64 SizeLimitForAggressiveStrategy => Convert.ToInt64(Math.Round(AvailableProcessMemoryInBytes * MemoryUsePercentageForAggressiveStrategy, 0));

        /// <summary>
        /// Gets the <see cref="MemoryCacheOptions.SizeLimit" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Int64 SizeLimitForConservativeStrategy => Convert.ToInt64(Math.Round(AvailableProcessMemoryInBytes * MemoryUsePercentageForConservativeStrategy, 0));

        /// <summary>
        /// Gets the <see cref="MemoryCacheOptions.SizeLimit" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Int64 SizeLimitForModerateStrategy => Convert.ToInt64(Math.Round(AvailableProcessMemoryInBytes * MemoryUsePercentageForModerateStrategy, 0));

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.AbsoluteExpirationRelativeToNow" /> setting, in minutes, associated
        /// with the strategy <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 AbsoluteExpirationRelativeToNowInMinutesForAggressiveStrategy = 89;

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.AbsoluteExpirationRelativeToNow" /> setting, in minutes, associated
        /// with the strategy <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 AbsoluteExpirationRelativeToNowInMinutesForConservativeStrategy = 13;

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.AbsoluteExpirationRelativeToNow" /> setting, in minutes, associated
        /// with the strategy <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 AbsoluteExpirationRelativeToNowInMinutesForModerateStrategy = 34;

        /// <summary>
        /// Represents the <see cref="MemoryCacheOptions.CompactionPercentage" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double CompactionPercentageForAggressiveStrategy = 0.08d;

        /// <summary>
        /// Represents the <see cref="MemoryCacheOptions.CompactionPercentage" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double CompactionPercentageForConservativeStrategy = 0.21d;

        /// <summary>
        /// Represents the <see cref="MemoryCacheOptions.CompactionPercentage" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double CompactionPercentageForModerateStrategy = 0.13d;

        /// <summary>
        /// Represents the <see cref="MemoryCacheOptions.ExpirationScanFrequency" /> setting, in minutes, associated with the
        /// strategy <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 ExpirationScanFrequencyInMinutesForAggressiveStrategy = 1;

        /// <summary>
        /// Represents the <see cref="MemoryCacheOptions.ExpirationScanFrequency" /> setting, in minutes, associated with the
        /// strategy <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 ExpirationScanFrequencyInMinutesForConservativeStrategy = 3;

        /// <summary>
        /// Represents the <see cref="MemoryCacheOptions.ExpirationScanFrequency" /> setting, in minutes, associated with the
        /// strategy <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 ExpirationScanFrequencyInMinutesForModerateStrategy = 2;

        /// <summary>
        /// Represents the memory use percentage setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double MemoryUsePercentageForAggressiveStrategy = 0.55d;

        /// <summary>
        /// Represents the memory use percentage setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double MemoryUsePercentageForConservativeStrategy = 0.08d;

        /// <summary>
        /// Represents the memory use percentage setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double MemoryUsePercentageForModerateStrategy = 0.21d;

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.SlidingExpiration" /> setting, in minutes, associated with the
        /// strategy <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 SlidingExpirationInMinutesForAggressiveStrategy = 34;

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.SlidingExpiration" /> setting, in minutes, associated with the
        /// strategy <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 SlidingExpirationInMinutesForConservativeStrategy = 5;

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.SlidingExpiration" /> setting, in minutes, associated with the
        /// strategy <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 SlidingExpirationInMinutesForModerateStrategy = 13;

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.AbsoluteExpirationRelativeToNow" /> setting associated with the
        /// strategy <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan AbsoluteExpirationRelativeToNowForAggressiveStrategy = TimeSpan.FromMinutes(AbsoluteExpirationRelativeToNowInMinutesForAggressiveStrategy);

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.AbsoluteExpirationRelativeToNow" /> setting associated with the
        /// strategy <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan AbsoluteExpirationRelativeToNowForConservativeStrategy = TimeSpan.FromMinutes(AbsoluteExpirationRelativeToNowInMinutesForConservativeStrategy);

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.AbsoluteExpirationRelativeToNow" /> setting associated with the
        /// strategy <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan AbsoluteExpirationRelativeToNowForModerateStrategy = TimeSpan.FromMinutes(AbsoluteExpirationRelativeToNowInMinutesForModerateStrategy);

        /// <summary>
        /// Represents the <see cref="MemoryCacheOptions.ExpirationScanFrequency" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan ExpirationScanFrequencyForAggressiveStrategy = TimeSpan.FromMinutes(ExpirationScanFrequencyInMinutesForAggressiveStrategy);

        /// <summary>
        /// Represents the <see cref="MemoryCacheOptions.ExpirationScanFrequency" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan ExpirationScanFrequencyForConservativeStrategy = TimeSpan.FromMinutes(ExpirationScanFrequencyInMinutesForConservativeStrategy);

        /// <summary>
        /// Represents the <see cref="MemoryCacheOptions.ExpirationScanFrequency" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan ExpirationScanFrequencyForModerateStrategy = TimeSpan.FromMinutes(ExpirationScanFrequencyInMinutesForModerateStrategy);

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.SlidingExpiration" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Aggressive" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan SlidingExpirationForAggressiveStrategy = TimeSpan.FromMinutes(SlidingExpirationInMinutesForAggressiveStrategy);

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.SlidingExpiration" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Conservative" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan SlidingExpirationForConservativeStrategy = TimeSpan.FromMinutes(SlidingExpirationInMinutesForConservativeStrategy);

        /// <summary>
        /// Represents the <see cref="MemoryCacheEntryOptions.SlidingExpiration" /> setting associated with the strategy
        /// <see cref="InMemoryCachingStrategy.Moderate" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan SlidingExpirationForModerateStrategy = TimeSpan.FromMinutes(SlidingExpirationInMinutesForModerateStrategy);
    }
}