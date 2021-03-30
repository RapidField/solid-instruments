// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents an exception that is raised when a caching operation fails.
    /// </summary>
    public class CacheAccessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheAccessException" /> class.
        /// </summary>
        public CacheAccessException()
            : this(cacheValueType: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheAccessException" />
        /// </summary>
        /// <param name="cacheValueType">
        /// The type of the cache value that was being processed when the exception was raised.
        /// </param>
        public CacheAccessException(Type cacheValueType)
            : this(cacheValueType: cacheValueType, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheAccessException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public CacheAccessException(String message)
            : this(message: message, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheAccessException" />
        /// </summary>
        /// <param name="cacheValueType">
        /// The type of the cache value that was being processed when the exception was raised.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public CacheAccessException(Type cacheValueType, Exception innerException)
            : this(cacheValueType is null ? "An exception was raised while accessing the cache or processing the cached object." : $"An exception was raised while processing a cached instance of type {cacheValueType}.", innerException)
        {
            CacheValueType = cacheValueType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheAccessException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public CacheAccessException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }

        /// <summary>
        /// Gets the type of the cache value that was being processed when the exception was raised.
        /// </summary>
        public Type CacheValueType
        {
            get;
        }
    }
}