// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents a read-write client for accessing strongly-typed cached objects using textual keys.
    /// </summary>
    public interface ICacheClient : ICacheReader, ICacheWriter
    {
        /// <summary>
        /// Removes the cached object using the specified textual key.
        /// </summary>
        /// <param name="key">
        /// A textual key which uniquely identifies the cached object to remove.
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
        public void Invalidate(String key);

        /// <summary>
        /// Attempts to retrieve the cached object using the specified textual key and, failing that, invokes the specified function
        /// to produce the object and adds it to the cache.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the cached object.
        /// </typeparam>
        /// <param name="key">
        /// A textual key which uniquely identifies the object in the cache.
        /// </param>
        /// <param name="produceValueFunction">
        /// A function that produces the object if it is not found in the cache.
        /// </param>
        /// <returns>
        /// The resulting cached object.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" /> -or- <paramref name="produceValueFunction" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="CacheAccessException">
        /// An exception was raised while attempting to access the cache or produce the value.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TValue Process<TValue>(String key, Func<TValue> produceValueFunction)
            where TValue : class;

        /// <summary>
        /// Attempts to retrieve the cached object using the specified textual key and, failing that, invokes the specified function
        /// to produce the object and adds it to the cache.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the cached object.
        /// </typeparam>
        /// <param name="compositeKey">
        /// An ordered collection of objects from which to construct a unique textual key.
        /// </param>
        /// <param name="produceValueFunction">
        /// A function that produces the object if it is not found in the cache.
        /// </param>
        /// <returns>
        /// The resulting cached object.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="compositeKey" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="compositeKey" /> is <see langword="null" /> -or- <paramref name="produceValueFunction" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="CacheAccessException">
        /// An exception was raised while attempting to access the cache or produce the value.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TValue Process<TValue>(IEnumerable<Object> compositeKey, Func<TValue> produceValueFunction)
            where TValue : class
        {
            _ = compositeKey.RejectIf().IsNullOrEmpty(nameof(compositeKey));
            var valueType = typeof(TValue);

            try
            {
                var keyMaterial = new StringBuilder($"{valueType}{CompositeKeyElementDelimitingCharacter}");

                foreach (var keyElement in compositeKey)
                {
                    var keyElementString = keyElement?.ToString() ?? CompositeKeyNullElementValue;
                    keyMaterial.Append($"{keyElementString}{CompositeKeyElementDelimitingCharacter}");
                }

                var key = Encoding.Unicode.GetBytes(keyMaterial.ToString()).GenerateChecksumIdentity().ToSerializedString();
                return Process($"{CompositeKeyPrependedCharacter}{key}", produceValueFunction);
            }
            catch (CacheAccessException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CacheAccessException(valueType, exception);
            }
        }

        /// <summary>
        /// Asynchronously attempts to retrieve the cached object using the specified textual key and, failing that, invokes the
        /// specified function to produce the object and adds it to the cache.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the cached object.
        /// </typeparam>
        /// <param name="key">
        /// A textual key which uniquely identifies the object in the cache.
        /// </param>
        /// <param name="produceValueFunction">
        /// A function that produces the object if it is not found in the cache.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the resulting cached object.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" /> -or- <paramref name="produceValueFunction" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="CacheAccessException">
        /// An exception was raised while attempting to access the cache or produce the value.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task<TValue> ProcessAsync<TValue>(String key, Func<TValue> produceValueFunction)
            where TValue : class;

        /// <summary>
        /// Asynchronously attempts to retrieve the cached object using the specified textual key and, failing that, invokes the
        /// specified function to produce the object and adds it to the cache.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the cached object.
        /// </typeparam>
        /// <param name="compositeKey">
        /// An ordered collection of objects from which to construct a unique textual key.
        /// </param>
        /// <param name="produceValueFunction">
        /// A function that produces the object if it is not found in the cache.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the resulting cached object.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="compositeKey" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="compositeKey" /> is <see langword="null" /> -or- <paramref name="produceValueFunction" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="CacheAccessException">
        /// An exception was raised while attempting to access the cache or produce the value.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task<TValue> ProcessAsync<TValue>(IEnumerable<Object> compositeKey, Func<TValue> produceValueFunction)
            where TValue : class => Task.Factory.StartNew(() => Process(compositeKey, produceValueFunction));

        /// <summary>
        /// Gets a value indicating whether or not the client is operative.
        /// </summary>
        public Boolean IsOperative
        {
            get;
        }

        /// <summary>
        /// Represents a character that is used as a delimiter between composite key element strings.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char CompositeKeyElementDelimitingCharacter = ':';

        /// <summary>
        /// Represents a textual representation of <see langword="null" /> composite key elements.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CompositeKeyNullElementValue = "_null_";

        /// <summary>
        /// Represents a character that is prepended to textual composite keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char CompositeKeyPrependedCharacter = '_';
    }
}