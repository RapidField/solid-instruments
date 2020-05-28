// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a named secret bit field that is pinned in memory and encrypted at rest.
    /// </summary>
    public sealed class Secret : Secret<IReadOnlyPinnedMemory<Byte>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Secret" /> class.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies the secret.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        public Secret(String name)
            : base(name)
        {
            return;
        }

        /// <summary>
        /// Creates a new <see cref="Secret" /> using the specified name and value.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies the secret.
        /// </param>
        /// <param name="value">
        /// The secret value.
        /// </param>
        /// <returns>
        /// A new <see cref="Secret" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        public static Secret FromValue(String name, Byte[] value)
        {
            value = value.RejectIf().IsNull(nameof(value));
            var secret = new Secret(name);
            secret.Write(() => new PinnedMemory<Byte>(value));
            return secret;
        }

        /// <summary>
        /// Creates a <see cref="IReadOnlyPinnedMemory{T}" /> using the provided bytes.
        /// </summary>
        /// <param name="bytes">
        /// Pinned memory.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IReadOnlyPinnedMemory{T}" />.
        /// </returns>
        protected sealed override IReadOnlyPinnedMemory<Byte> ConvertBytesToValue(IReadOnlyPinnedMemory<Byte> bytes, IConcurrencyControlToken controlToken) => bytes;

        /// <summary>
        /// Gets the bytes of <paramref name="value" />, pins them in memory and returns the resulting
        /// <see cref="IReadOnlyPinnedMemory{T}" />.
        /// </summary>
        /// <param name="value">
        /// The secret value.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// <paramref name="value" /> as pinned memory.
        /// </returns>
        protected sealed override IReadOnlyPinnedMemory<Byte> ConvertValueToBytes(IReadOnlyPinnedMemory<Byte> value, IConcurrencyControlToken controlToken) => value;

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Secret" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Represents a named secret value that is pinned in memory and encrypted at rest.
    /// </summary>
    /// <remarks>
    /// <see cref="Secret{TValue}" /> is the default implementation of <see cref="ISecret{TValue}" />.
    /// </remarks>
    /// <typeparam name="TValue">
    /// The type of the value.
    /// </typeparam>
    public abstract class Secret<TValue> : Instrument, ISecret<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Secret{TValue}" /> class.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies the secret.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        protected Secret(String name)
            : base()
        {
            HasValue = false;
            Name = name.RejectIf().IsNullOrEmpty(nameof(name));
            SecureValueMemory = null;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode()
        {
            var hashCode = (Name?.GetHashCode() ?? 0) ^ (ValueType?.FullName.GetHashCode() ?? 0);

            if (HasValue && SecureValueMemory is null == false)
            {
                hashCode ^= SecureValueMemory.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Decrypts the secret value, pins it in memory and performs the specified read operation against the resulting bytes as a
        /// thread-safe, atomic operation.
        /// </summary>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The secret does not have a value. This exception can be avoided by evaluating <see cref="HasValue" /> before invoking
        /// the method.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception.
        /// </exception>
        public void Read(Action<IReadOnlyPinnedMemory<Byte>> readAction)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                Read(readAction.RejectIf().IsNull(nameof(readAction)), controlToken);
            }
        }

        /// <summary>
        /// Decrypts the secret value, pins a copy of it in memory and performs the specified read operation against it as a
        /// thread-safe, atomic operation.
        /// </summary>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The secret does not have a value. This exception can be avoided by evaluating <see cref="HasValue" /> before invoking
        /// the method.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception.
        /// </exception>
        public void Read(Action<TValue> readAction)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                Read(readAction.RejectIf().IsNull(nameof(readAction)), controlToken);
            }
        }

        /// <summary>
        /// Asynchronously decrypts the secret value, pins it in memory and performs the specified read operation against the
        /// resulting bytes as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The secret does not have a value. This exception can be avoided by evaluating <see cref="HasValue" /> before invoking
        /// the method.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception.
        /// </exception>
        public Task ReadAsync(Action<IReadOnlyPinnedMemory<Byte>> readAction) => Task.Factory.StartNew(() => Read(readAction));

        /// <summary>
        /// Asynchronously decrypts the secret value, pins a copy of it in memory and performs the specified read operation against
        /// it as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The secret does not have a value. This exception can be avoided by evaluating <see cref="IReadOnlySecret.HasValue" />
        /// before invoking the method.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception.
        /// </exception>
        public Task ReadAsync(Action<TValue> readAction) => Task.Factory.StartNew(() => Read(readAction));

        /// <summary>
        /// Converts the value of the current <see cref="Secret{TValue}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Secret{TValue}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Name)}\": \"{Name}\" }}";

        /// <summary>
        /// Performs the specified write operation and encrypts the resulting value as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="writeFunction">
        /// The write operation to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writeFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="writeFunction" /> raised an exception or returned an invalid <typeparamref name="TValue" />.
        /// </exception>
        public void Write(Func<TValue> writeFunction)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                Write(writeFunction.RejectIf().IsNull(nameof(writeFunction)), controlToken);
            }
        }

        /// <summary>
        /// Asynchronously performs the specified write operation and encrypts the resulting value as a thread-safe, atomic
        /// operation.
        /// </summary>
        /// <param name="writeFunction">
        /// The write operation to perform.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writeFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="writeFunction" /> raised an exception or returned an invalid <typeparamref name="TValue" />.
        /// </exception>
        public Task WriteAsync(Func<TValue> writeFunction) => Task.Factory.StartNew(() => Write(writeFunction));

        /// <summary>
        /// Creates a <typeparamref name="TValue" /> using the provided bytes.
        /// </summary>
        /// <param name="bytes">
        /// Pinned memory representing a <typeparamref name="TValue" />.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The resulting <typeparamref name="TValue" />.
        /// </returns>
        protected abstract TValue ConvertBytesToValue(IReadOnlyPinnedMemory<Byte> bytes, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Gets the bytes of <paramref name="value" />, pins them in memory and returns the resulting
        /// <see cref="IReadOnlyPinnedMemory{T}" />.
        /// </summary>
        /// <param name="value">
        /// The secret value.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// <paramref name="value" /> as pinned memory.
        /// </returns>
        protected abstract IReadOnlyPinnedMemory<Byte> ConvertValueToBytes(TValue value, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Secret{TValue}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    SecureValueMemory?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Decrypts the secret value, pins it in memory and performs the specified read operation against the resulting bytes as a
        /// thread-safe, atomic operation.
        /// </summary>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception.
        /// </exception>
        [DebuggerHidden]
        private void Read(Action<IReadOnlyPinnedMemory<Byte>> readAction, IConcurrencyControlToken controlToken)
        {
            if (HasValue == false)
            {
                throw new InvalidOperationException($"The secret does not have a value. This exception can be avoided by evaluating {nameof(HasValue)} before performing a read operation.");
            }

            try
            {
                if (SecureValueMemory is null)
                {
                    using (var memory = new ReadOnlyPinnedMemory<Byte>(0))
                    {
                        // Because secure memory cannot have length zero, this is here to handle cases in which write operations
                        // produce empty memory.
                        readAction(memory);
                    }

                    return;
                }

                SecureValueMemory.Access(buffer =>
                {
                    readAction(buffer);
                });
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (SecretAccessException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new SecretAccessException(exception);
            }
        }

        /// <summary>
        /// Decrypts the secret value, pins a copy of it in memory and performs the specified read operation against it as a
        /// thread-safe, atomic operation.
        /// </summary>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception.
        /// </exception>
        [DebuggerHidden]
        private void Read(Action<TValue> readAction, IConcurrencyControlToken controlToken)
        {
            try
            {
                Read(memory =>
                {
                    readAction(ConvertBytesToValue(memory, controlToken));
                }, controlToken);
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (SecretAccessException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new SecretAccessException(exception);
            }
        }

        /// <summary>
        /// Performs the specified write operation and encrypts the resulting value as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="writeFunction">
        /// The write operation to perform.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="writeFunction" /> raised an exception or returned an invalid <typeparamref name="TValue" />.
        /// </exception>
        [DebuggerHidden]
        private void Write(Func<TValue> writeFunction, IConcurrencyControlToken controlToken)
        {
            try
            {
                var value = writeFunction();

                if (ValueType.IsValueType == false && value is null)
                {
                    throw new SecretAccessException("The specified write function produced a null secret value.");
                }

                using (var valueMemory = ConvertValueToBytes(value, controlToken))
                {
                    if (valueMemory.IsEmpty)
                    {
                        // Secure memory cannot be empty.
                        SecureValueMemory?.Dispose();
                        SecureValueMemory = null;
                        HasValue = true;
                        return;
                    }

                    if (SecureValueMemory is null)
                    {
                        SecureValueMemory = new SecureMemory(valueMemory.LengthInBytes);
                    }
                    else if (SecureValueMemory.LengthInBytes != valueMemory.LengthInBytes)
                    {
                        SecureValueMemory.Dispose();
                        SecureValueMemory = new SecureMemory(valueMemory.LengthInBytes);
                    }

                    SecureValueMemory.Access(memory =>
                    {
                        valueMemory.ReadOnlySpan.CopyTo(memory.Span);
                    });

                    HasValue = true;
                }
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (SecretAccessException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new SecretAccessException(exception);
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="Secret{TValue}" /> has a value.
        /// </summary>
        public Boolean HasValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a textual name that uniquely identifies the current <see cref="Secret{TValue}" />.
        /// </summary>
        public String Name
        {
            get;
        }

        /// <summary>
        /// Gets the type of the secret value.
        /// </summary>
        public Type ValueType => typeof(TValue);

        /// <summary>
        /// Represents the encrypted field in which the secure value is stored.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ISecureMemory SecureValueMemory;
    }
}