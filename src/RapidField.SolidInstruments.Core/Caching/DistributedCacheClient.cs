// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents a read-write client for accessing strongly typed, remotely cached objects using textual keys.
    /// </summary>
    /// <remarks>
    /// <see cref="DistributedCacheClient" /> is the default implementation of <see cref="IDistributedCacheClient" />.
    /// </remarks>
    public sealed class DistributedCacheClient : CacheClient, IDistributedCacheClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedCacheClient" /> class.
        /// </summary>
        /// <param name="cache">
        /// The underlying distributed cache, or <see langword="null" /> if the distributed cache client is non-operative.
        /// </param>
        public DistributedCacheClient(IDistributedCache cache)
            : base(cache is null ? ConcurrencyControlMode.Unconstrained : ConcurrencyControlMode.ProcessorCountSemaphore)
        {
            Cache = cache;
        }

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
        public void Refresh(String key)
        {
            RejectIfDisposed();
            _ = key.RejectIf().IsNullOrEmpty(nameof(key));

            try
            {
                if (IsOperative)
                {
                    Cache?.Refresh(key);
                }
            }
            catch (CacheAccessException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CacheAccessException("An exception was raised while refreshing the cached object.", exception);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DistributedCacheClient" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

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
                var serializer = new DataContractJsonSerializer(typeof(TValue), SerializerSettings);
                var serializedValue = Cache?.Get(key);

                try
                {
                    return serializedValue is null ? null : Deserialize(serializer, serializedValue) as TValue;
                }
                catch (SerializationException)
                {
                    return null;
                }
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
        /// <exception cref="SerializationException">
        /// <paramref name="value" /> is invalid or an error occurred during serialization.
        /// </exception>
        protected override void Write<TValue>(String key, TValue value, IConcurrencyControlToken controlToken)
            where TValue : class
        {
            if (IsOperative)
            {
                var serializer = new DataContractJsonSerializer(typeof(TValue), SerializerSettings);
                var serializedValue = Serialize(serializer, value);

                if (serializedValue is not null)
                {
                    Cache?.Set(key, serializedValue);
                }
            }
        }

        /// <summary>
        /// Converts the specified serialized object to its typed equivalent.
        /// </summary>
        /// <param name="serializer">
        /// A serializer that deserializes <paramref name="serializedObject" />.
        /// </param>
        /// <param name="serializedObject">
        /// A serialized object.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="serializedObject" /> is invalid or an error occurred during deserialization.
        /// </exception>
        [DebuggerHidden]
        private static Object Deserialize(DataContractJsonSerializer serializer, Byte[] serializedObject)
        {
            using (var stream = new MemoryStream(serializedObject))
            {
                try
                {
                    return serializer.ReadObject(stream) ?? throw new SerializationException("The specified serialized object is invalid.");
                }
                catch (SerializationException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new SerializationException("An error occurred during deserialization. See inner exception.", exception);
                }
            }
        }

        /// <summary>
        /// Converts the specified object to a serialized byte array.
        /// </summary>
        /// <param name="serializer">
        /// A serializer that serializes <paramref name="target" />.
        /// </param>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <returns>
        /// The serialized byte array.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        [DebuggerHidden]
        private static Byte[] Serialize(DataContractJsonSerializer serializer, Object target)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    serializer.WriteObject(stream, target);
                }
                catch (SerializationException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new SerializationException("An error occurred during serialization. See inner exception.", exception);
                }

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the client is operative.
        /// </summary>
        public override Boolean IsOperative => Cache is not null;

        /// <summary>
        /// Gets the underlying distributed cache, or <see langword="null" /> if the distributed cache client is non-operative.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDistributedCache Cache
        {
            get;
        }

        /// <summary>
        /// Represents the <see cref="DateTime" /> format string that is used by
        /// <see cref="Serialize(DataContractJsonSerializer, Object)" /> to serialize objects.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DateTimeSerializationFormatString = "o";

        /// <summary>
        /// Represents settings used by <see cref="Serialize(DataContractJsonSerializer, Object)" /> to serialize objects.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly DataContractJsonSerializerSettings SerializerSettings = new DataContractJsonSerializerSettings
        {
            DateTimeFormat = new DateTimeFormat(DateTimeSerializationFormatString),
            EmitTypeInformation = EmitTypeInformation.AsNeeded,
            SerializeReadOnlyTypes = false
        };
    }
}