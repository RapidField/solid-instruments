// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents a read-write client for accessing strongly-typed cached objects using textual keys.
    /// </summary>
    /// <remarks>
    /// <see cref="CacheClient" /> is the default implementation of <see cref="ICacheClient" />.
    /// </remarks>
    public abstract class CacheClient : Instrument, ICacheClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheClient" /> class.
        /// </summary>
        protected CacheClient()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheClient" /> class.
        /// </summary>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.SingleThreadLock" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" />.
        /// </exception>
        protected CacheClient(ConcurrencyControlMode stateControlMode)
            : base(stateControlMode)
        {
            return;
        }

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
        public void Invalidate(String key)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                _ = key.RejectIf().IsNullOrEmpty(nameof(key));

                try
                {
                    Invalidate(key, controlToken);
                }
                catch (CacheAccessException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new CacheAccessException("An exception was raised while invalidating the cached object.", exception);
                }
            }
        }

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
            where TValue : class
        {
            _ = produceValueFunction.RejectIf().IsNull(nameof(produceValueFunction));

            if (TryRead<TValue>(key, out var value))
            {
                return value;
            }

            try
            {
                value = produceValueFunction();

                if (value is not null)
                {
                    Write(key, value);
                }

                return value;
            }
            catch (CacheAccessException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CacheAccessException(typeof(TValue), exception);
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
            where TValue : class => Task.Factory.StartNew(() => Process(key, produceValueFunction));

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
            where TValue : class
        {
            value = null;
            RejectIfDisposed();
            _ = key.RejectIf().IsNullOrEmpty(nameof(key));

            try
            {
                value = TryRead<TValue>(key);
            }
            catch (CacheAccessException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CacheAccessException(typeof(TValue), exception);
            }

            return value is null ? false : true;
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
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" /> -or- <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CacheAccessException">
        /// An exception was raised while attempting to access the cache.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Write<TValue>(String key, TValue value)
            where TValue : class
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                _ = key.RejectIf().IsNullOrEmpty(nameof(key));
                _ = value.RejectIf().IsNull(nameof(value));

                try
                {
                    Write(key, value, controlToken);
                }
                catch (CacheAccessException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new CacheAccessException(typeof(TValue), exception);
                }
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CacheClient" />.
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
        protected abstract void Invalidate(String key, IConcurrencyControlToken controlToken);

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
        protected abstract TValue TryRead<TValue>(String key)
            where TValue : class;

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
        protected abstract void Write<TValue>(String key, TValue value, IConcurrencyControlToken controlToken)
            where TValue : class;

        /// <summary>
        /// Gets a value indicating whether or not the client is operative.
        /// </summary>
        public virtual Boolean IsOperative => true;
    }
}