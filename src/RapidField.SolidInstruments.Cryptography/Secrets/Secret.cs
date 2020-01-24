// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
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
            SecureValueBuffer = null;
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
        public void Read(Action<IReadOnlyPinnedBuffer<Byte>> readAction)
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
        /// Converts the value of the current <see cref="Secret{TValue}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Secret{TValue}" />.
        /// </returns>
        public override String ToString() => Name;

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
        /// Creates a <typeparamref name="TValue" /> using the provided bytes.
        /// </summary>
        /// <param name="bytes">
        /// A pinned buffer representing a <typeparamref name="TValue" />.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The resulting <typeparamref name="TValue" />.
        /// </returns>
        protected abstract TValue ConvertBytesToValue(IReadOnlyPinnedBuffer<Byte> bytes, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Gets the bytes of <paramref name="value" />, pins them in memory and returns the resulting
        /// <see cref="IPinnedBuffer{T}" />.
        /// </summary>
        /// <param name="value">
        /// The secret value.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// <paramref name="value" /> as a pinned buffer.
        /// </returns>
        protected abstract IPinnedBuffer<Byte> ConvertValueToBytes(TValue value, ConcurrencyControlToken controlToken);

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
                    SecureValueBuffer?.Dispose();
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
        private void Read(Action<IReadOnlyPinnedBuffer<Byte>> readAction, ConcurrencyControlToken controlToken)
        {
            if (HasValue == false)
            {
                throw new InvalidOperationException($"The secret does not have a value. This exception can be avoided by evaluating {nameof(HasValue)} before performing a read operation.");
            }

            try
            {
                if (SecureValueBuffer is null)
                {
                    using (var pinnedBuffer = new ReadOnlyPinnedBuffer<Byte>(0))
                    {
                        // Because secure buffers cannot have length zero, this is here to handle cases in which write operations
                        // produce empty buffers.
                        readAction(pinnedBuffer);
                    }

                    return;
                }

                SecureValueBuffer.Access((buffer) =>
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
        private void Read(Action<TValue> readAction, ConcurrencyControlToken controlToken)
        {
            try
            {
                Read((pinnedBuffer) =>
                {
                    readAction(ConvertBytesToValue(pinnedBuffer, controlToken));
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
        private void Write(Func<TValue> writeFunction, ConcurrencyControlToken controlToken)
        {
            try
            {
                var value = writeFunction();

                if (ValueType.IsValueType == false && value is null)
                {
                    throw new SecretAccessException("The specified write function produced a null secret value.");
                }

                using (var valueBuffer = ConvertValueToBytes(value, controlToken))
                {
                    try
                    {
                        if (valueBuffer.IsEmpty)
                        {
                            // Secure buffers cannot be empty.
                            SecureValueBuffer?.Dispose();
                            SecureValueBuffer = null;
                            HasValue = true;
                            return;
                        }

                        if (SecureValueBuffer is null)
                        {
                            SecureValueBuffer = new SecureBuffer(valueBuffer.LengthInBytes);
                        }
                        else if (SecureValueBuffer.LengthInBytes != valueBuffer.LengthInBytes)
                        {
                            SecureValueBuffer.Dispose();
                            SecureValueBuffer = new SecureBuffer(valueBuffer.LengthInBytes);
                        }

                        SecureValueBuffer.Access(secureBuffer =>
                        {
                            valueBuffer.Span.CopyTo(secureBuffer.Span);
                        });

                        HasValue = true;
                    }
                    finally
                    {
                        valueBuffer.OverwriteWithZeros();
                    }
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
        private SecureBuffer SecureValueBuffer;
    }
}