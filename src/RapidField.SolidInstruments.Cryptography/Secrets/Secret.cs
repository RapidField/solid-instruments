// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Hashing;
using System;
using System.Diagnostics;
using System.Text;
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
        /// Returns a standard-format, prefixed, unique textual identifier.
        /// </summary>
        /// <param name="prefix">
        /// An alphanumeric prefix for the textual identifier.
        /// </param>
        /// <param name="uniqueSemanticIdentity">
        /// The unique portion of the textual identifier.
        /// </param>
        /// <returns>
        /// A standard-format, prefixed, unique textual identifier.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="prefix" /> is empty -or- <paramref name="uniqueSemanticIdentity" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="prefix" /> is <see langword="null" /> -or- <paramref name="uniqueSemanticIdentity" /> is
        /// <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal static String GetPrefixedSemanticIdentifier(String prefix, String uniqueSemanticIdentity) => $"__{prefix.RejectIf().IsNullOrEmpty(nameof(prefix)).TargetArgument}-{uniqueSemanticIdentity.RejectIf().IsNullOrEmpty(nameof(uniqueSemanticIdentity)).TargetArgument}";

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
        /// A value indicating whether or not disposal was invoked by user code.
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
            InMemoryKeyTimeStampValue = TimeStamp.Current;
            Name = name.RejectIf().IsNullOrEmpty(nameof(name));
            SecureValueMemory = null;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => IsDisposedOrDisposing ? 0 : GetDerivedIdentity().GetHashCode();

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
        public void Read(Action<IReadOnlyPinnedMemory<Byte>> readAction) => WithStateControl(controlToken =>
        {
            Read(readAction.RejectIf().IsNull(nameof(readAction)), controlToken);
        });

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
        public void Read(Action<TValue> readAction) => WithStateControl(controlToken =>
        {
            Read(readAction.RejectIf().IsNull(nameof(readAction)), controlToken);
        });

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
        /// Regenerates and replaces the in-memory key that is used to secure the current <see cref="Secret{TValue}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        void IReadOnlySecret.RegenerateInMemoryKey() => WithStateControl(controlToken =>
        {
            try
            {
                SecureValueMemory.RegeneratePrivateKey();
            }
            finally
            {
                InMemoryKeyTimeStampValue = TimeStamp.Current;
            }
        });

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
        public void Write(Func<TValue> writeFunction) => WithStateControl(controlToken =>
        {
            Write(writeFunction.RejectIf().IsNull(nameof(writeFunction)), controlToken);
        });

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
        /// Generates a new, unique textual identifier.
        /// </summary>
        /// <returns>
        /// A new, unique textual identifier.
        /// </returns>
        [DebuggerHidden]
        internal static String NewRandomSemanticIdentifier() => HardenedRandomNumberGenerator.Instance.GetString(8, false, true, false, true, false, false, false);

        /// <summary>
        /// Writes and encrypts the specified value.
        /// </summary>
        /// <param name="value">
        /// The value to write and encrypt.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal void Write(Byte[] value)
        {
            using var valueMemory = new ReadOnlyPinnedMemory(value);
            Write(valueMemory: valueMemory);
        }

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
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                SecureValueMemory?.Dispose();
                SecureValueMemory = null;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets a globally unique identifier that is derived from a cryptographically secure hash of the secret value and secret
        /// name.
        /// </summary>
        /// <returns>
        /// A <see cref="Guid" /> that is derived from a cryptographically secure hash of the secret value and secret name.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private Guid GetDerivedIdentity()
        {
            RejectIfDisposed();
            var algorithm = DerivedIdentityHashingAlgorithm;
            var hashLengthInBytes = algorithm.ToDigestBitLength() / 8;
            var hashPairLengthInBytes = hashLengthInBytes * 2;

            using var hashPair = new PinnedMemory(hashPairLengthInBytes, true);

            if (Name.IsNullOrEmpty())
            {
                DerivedIdentityEmptyValueHashBytes.CopyTo(hashPair, 0);
            }
            else
            {
                HashingProcessor.Instance.CalculateHash(Name.ToByteArray(Encoding.Unicode), algorithm).CopyTo(hashPair, 0);
            }

            if (HasValue)
            {
                Read((IReadOnlyPinnedMemory<Byte> value) =>
                {
                    if (value.IsNullOrEmpty())
                    {
                        DerivedIdentityEmptyValueHashBytes.CopyTo(hashPair, hashLengthInBytes);
                        return;
                    }

                    HashingProcessor.Instance.CalculateHash(value.ReadOnlySpan.ToArray(), algorithm).CopyTo(hashPair, hashLengthInBytes);
                });
            }
            else
            {
                DerivedIdentityEmptyValueHashBytes.CopyTo(hashPair, hashLengthInBytes);
            }

            return hashPair.GenerateChecksumIdentity();
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
            if (HasValue is false)
            {
                throw new InvalidOperationException($"The secret does not have a value. This exception can be avoided by evaluating {nameof(HasValue)} before performing a read operation.");
            }

            try
            {
                if (SecureValueMemory is null)
                {
                    // Because secure memory cannot have length zero, this is here to handle cases in which write operations produce
                    // empty memory.
                    using var memory = new ReadOnlyPinnedMemory<Byte>(0);
                    readAction(memory);
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
        /// Writes and encrypts the specified value memory.
        /// </summary>
        /// <param name="valueMemory">
        /// The memory to write and encrypt.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueMemory" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private void Write(IReadOnlyPinnedMemory<Byte> valueMemory)
        {
            if (valueMemory.RejectIf().IsNull(nameof(valueMemory)).TargetArgument.IsEmpty)
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

                if (ValueType.IsValueType is false && value is null)
                {
                    throw new SecretAccessException("The specified write function produced a null secret value.");
                }

                using var valueMemory = ConvertValueToBytes(value, controlToken);
                Write(valueMemory);
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
        /// Gets a globally unique identifier that is derived from a cryptographically secure hash of the secret value and secret
        /// name.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Guid DerivedIdentity => GetDerivedIdentity();

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="Secret{TValue}" /> has a value.
        /// </summary>
        public Boolean HasValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the date and time when the in-memory key that is used to secure the current <see cref="Secret{TValue}" /> was
        /// generated.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime IReadOnlySecret.InMemoryKeyTimeStamp => InMemoryKeyTimeStampValue;

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
        /// Represents the hashing algorithm that is used to produce <see cref="DerivedIdentity" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const HashingAlgorithmSpecification DerivedIdentityHashingAlgorithm = HashingAlgorithmSpecification.Pbkdf2;

        /// <summary>
        /// Represents an array of bytes that are used in place of plaintext bytes for producing <see cref="DerivedIdentity" /> when
        /// the plaintext is empty.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Byte[] DerivedIdentityEmptyValueBytes = { 0xf0, 0x55, 0xcc, 0x99, 0x0f, 0xaa, 0x33, 0x66 };

        /// <summary>
        /// Represents an array of bytes that are used in place of ciphertext bytes for producing <see cref="DerivedIdentity" />
        /// when the plaintext is empty.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Byte[] DerivedIdentityEmptyValueHashBytes = HashingProcessor.Instance.CalculateHash(DerivedIdentityEmptyValueBytes, DerivedIdentityHashingAlgorithm);

        /// <summary>
        /// Represents the date and time when the in-memory key that is used to secure the current <see cref="Secret{TValue}" /> was
        /// generated.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime InMemoryKeyTimeStampValue;

        /// <summary>
        /// Represents the encrypted field in which the secure value is stored.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ISecureMemory SecureValueMemory;
    }
}