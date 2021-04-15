// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Caching;
using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Specifies the cache access and management behavior of an <see cref="InMemoryCacheClient" />.
    /// </summary>
    public enum InMemoryCachingStrategy : Int32
    {
        /// <summary>
        /// The behavior of the in-memory cache client is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The client will not perform caching. This value can be used to create non-operative cache clients.
        /// </summary>
        NoCaching = 1,

        /// <summary>
        /// The client will perform lightweight caching, exercising minimal consumption of memory and processing resources.
        /// </summary>
        Conservative = 2,

        /// <summary>
        /// The client will perform caching exercising moderate consumption of memory and processing resources.
        /// </summary>
        Moderate = 3,

        /// <summary>
        /// The client will perform heavy caching, exercising high consumption of memory and processing resources.
        /// </summary>
        Aggressive = 4
    }
}