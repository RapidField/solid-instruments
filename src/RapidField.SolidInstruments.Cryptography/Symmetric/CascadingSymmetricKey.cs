// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Represents a series of <see cref="ISymmetricKey" /> instances that constitute instructions for applying cascading encryption
    /// and decryption.
    /// </summary>
    /// <remarks>
    /// <see cref="CascadingSymmetricKey" /> is the default implementation of <see cref="ICascadingSymmetricKey" />.
    /// </remarks>
    public sealed class CascadingSymmetricKey : Instrument, ICascadingSymmetricKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascadingSymmetricKey" /> class.
        /// </summary>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys.
        /// </param>
        /// <param name="algorithms">
        /// The layered algorithm specifications.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="SymmetricKeyDerivationMode.Unspecified" /> -or- one or more
        /// algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private CascadingSymmetricKey(SymmetricKeyDerivationMode derivationMode, params SymmetricAlgorithmSpecification[] algorithms)
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            var keys = new SymmetricKey[algorithms.Length];

            for (var i = 0; i < algorithms.Length; i++)
            {
                keys[i] = SymmetricKey.New(algorithms[i], derivationMode);
            }

            Keys = keys;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascadingSymmetricKey" /> class.
        /// </summary>
        /// <param name="keys">
        /// An ordered array of keys comprising the cascading key.
        /// </param>
        [DebuggerHidden]
        private CascadingSymmetricKey(ISymmetricKey[] keys)
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            Keys = keys;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="CascadingSymmetricKey" /> using the specified buffer.
        /// </summary>
        /// <param name="buffer">
        /// A binary representation of a <see cref="CascadingSymmetricKey" />.
        /// </param>
        /// <returns>
        /// A new instance of a <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="buffer" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer" /> is <see langword="null" />.
        /// </exception>
        public static CascadingSymmetricKey FromBuffer(ISecureMemory buffer)
        {
            buffer.RejectIf().IsNull(nameof(buffer)).OrIf(argument => argument.LengthInBytes != SerializedLength, nameof(buffer), "The specified buffer is invalid.");

            try
            {
                var keys = (ISymmetricKey[])null;

                buffer.Access(memory =>
                {
                    // Interrogate the final 16 bits to determine the depth.
                    var keyLength = SymmetricKey.SerializedLength;
                    var depth = BitConverter.ToUInt16(memory, (SerializedLength - sizeof(UInt16)));
                    keys = new ISymmetricKey[depth];

                    for (var i = 0; i < depth; i++)
                    {
                        using (var secureBuffer = new SecureMemory(keyLength))
                        {
                            secureBuffer.Access(key =>
                            {
                                // Copy out the key buffers.
                                Array.Copy(memory, (keyLength * i), key, 0, keyLength);
                            });

                            keys[i] = SymmetricKey.FromBuffer(secureBuffer);
                        }
                    }
                });

                return new CascadingSymmetricKey(keys);
            }
            catch
            {
                throw new ArgumentException("The specified buffer is invalid.", nameof(buffer));
            }
        }

        /// <summary>
        /// Derives a new <see cref="CascadingSymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        public static CascadingSymmetricKey FromPassword(String password) => FromPassword(password, DefaultDerivationMode, DefaultFirstLayerAlgorithm, DefaultSecondLayerAlgorithm);

        /// <summary>
        /// Derives a new <see cref="CascadingSymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="SymmetricKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner) layer of encryption. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second (outer) layer of encryption. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes128Ecb" />.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="SymmetricKeyDerivationMode.Unspecified" /> -or- one or more
        /// algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        public static CascadingSymmetricKey FromPassword(String password, SymmetricKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm) => FromPassword(password, derivationMode, new SymmetricAlgorithmSpecification[] { firstLayerAlgorithm, secondLayerAlgorithm });

        /// <summary>
        /// Derives a new <see cref="CascadingSymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="SymmetricKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner-most) layer of encryption.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second layer of encryption.
        /// </param>
        /// <param name="thirdLayerAlgorithm">
        /// The algorithm for the third (outer-most) layer of encryption.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="SymmetricKeyDerivationMode.Unspecified" /> -or- one or more
        /// algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        public static CascadingSymmetricKey FromPassword(String password, SymmetricKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm, SymmetricAlgorithmSpecification thirdLayerAlgorithm) => FromPassword(password, derivationMode, new SymmetricAlgorithmSpecification[] { firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm });

        /// <summary>
        /// Derives a new <see cref="CascadingSymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="SymmetricKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner-most) layer of encryption.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second layer of encryption.
        /// </param>
        /// <param name="thirdLayerAlgorithm">
        /// The algorithm for the third layer of encryption.
        /// </param>
        /// <param name="fourthLayerAlgorithm">
        /// The algorithm for the fourth (outer-most) layer of encryption.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="SymmetricKeyDerivationMode.Unspecified" /> -or- one or more
        /// algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        public static CascadingSymmetricKey FromPassword(String password, SymmetricKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm, SymmetricAlgorithmSpecification thirdLayerAlgorithm, SymmetricAlgorithmSpecification fourthLayerAlgorithm) => FromPassword(password, derivationMode, new SymmetricAlgorithmSpecification[] { firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm });

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKey" />.
        /// </summary>
        /// <remarks>
        /// Keys generated by this method employ <see cref="SymmetricKeyDerivationMode.XorLayeringWithSubstitution" /> for key
        /// derivation, <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" /> for first (inner) layer transformation and
        /// <see cref="SymmetricAlgorithmSpecification.Aes128Ecb" /> for second (outer) layer transformation.
        /// </remarks>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        public static CascadingSymmetricKey New() => new CascadingSymmetricKey(DefaultDerivationMode, DefaultFirstLayerAlgorithm, DefaultSecondLayerAlgorithm);

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKey" />.
        /// </summary>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="SymmetricKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner) layer of encryption. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second (outer) layer of encryption. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes128Ecb" />.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="SymmetricKeyDerivationMode.Unspecified" /> -or- one or more
        /// algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        public static CascadingSymmetricKey New(SymmetricKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm) => new CascadingSymmetricKey(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKey" />.
        /// </summary>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="SymmetricKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner-most) layer of encryption.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second layer of encryption.
        /// </param>
        /// <param name="thirdLayerAlgorithm">
        /// The algorithm for the third (outer-most) layer of encryption.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="SymmetricKeyDerivationMode.Unspecified" /> -or- one or more
        /// algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        public static CascadingSymmetricKey New(SymmetricKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm, SymmetricAlgorithmSpecification thirdLayerAlgorithm) => new CascadingSymmetricKey(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKey" />.
        /// </summary>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="SymmetricKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner-most) layer of encryption.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second layer of encryption.
        /// </param>
        /// <param name="thirdLayerAlgorithm">
        /// The algorithm for the third layer of encryption.
        /// </param>
        /// <param name="fourthLayerAlgorithm">
        /// The algorithm for the fourth (outer-most) layer of encryption.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="SymmetricKeyDerivationMode.Unspecified" /> -or- one or more
        /// algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        public static CascadingSymmetricKey New(SymmetricKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm, SymmetricAlgorithmSpecification thirdLayerAlgorithm, SymmetricAlgorithmSpecification fourthLayerAlgorithm) => new CascadingSymmetricKey(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);

        /// <summary>
        /// Converts the value of the current <see cref="CascadingSymmetricKey" /> to its equivalent binary representation.
        /// </summary>
        /// <returns>
        /// A binary representation of the current <see cref="CascadingSymmetricKey" />.
        /// </returns>
        public ISecureMemory ToBuffer()
        {
            var result = new SecureMemory(SerializedLength);

            try
            {
                using (var controlToken = StateControl.Enter())
                {
                    result.Access(pinnedResultBuffer =>
                    {
                        var keyLength = SymmetricKey.SerializedLength;

                        for (var i = 0; i < MaximumDepth; i++)
                        {
                            if (i < Depth)
                            {
                                using (var keyBuffer = Keys.ElementAt(i).ToBuffer())
                                {
                                    keyBuffer.Access(key =>
                                    {
                                        // Copy the key buffers out to the result buffer.
                                        Array.Copy(key, 0, pinnedResultBuffer, (keyLength * i), keyLength);
                                    });
                                }

                                continue;
                            }

                            // Fill the unused segments with random bytes.
                            var randomBytes = new Byte[keyLength];
                            HardenedRandomNumberGenerator.Instance.GetBytes(randomBytes);
                            Array.Copy(randomBytes, 0, pinnedResultBuffer, (keyLength * i), keyLength);
                        }

                        // Append the depth as a 16-bit integer.
                        Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToUInt16(Depth)), 0, pinnedResultBuffer, (SerializedLength - sizeof(UInt16)), sizeof(UInt16));
                    });

                    return result;
                }
            }
            catch
            {
                result.Dispose();
                throw new SecurityException("Key serialization failed.");
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CascadingSymmetricKey" />.
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
                    foreach (var key in Keys)
                    {
                        key.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Derives a new <see cref="CascadingSymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="SymmetricKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="algorithms">
        /// An ordered array of algorithms.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty -or- <paramref name="algorithms" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" /> -or- <paramref name="algorithms" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="SymmetricKeyDerivationMode.Unspecified" /> -or- one or more
        /// algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private static CascadingSymmetricKey FromPassword(String password, SymmetricKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification[] algorithms)
        {
            var keyDepth = algorithms.RejectIf().IsNullOrEmpty(nameof(algorithms)).TargetArgument.Length;
            var singleKeySourceLengthInBytes = SymmetricKey.KeySourceLengthInBytes;
            var totalSourceLengthInBytes = singleKeySourceLengthInBytes * keyDepth;
            var keys = new ISymmetricKey[keyDepth];

            using (var keySourceBytes = SymmetricKey.DeriveKeySourceBytesFromPassword(password, totalSourceLengthInBytes))
            {
                for (var i = 0; i < keyDepth; i++)
                {
                    var algorithm = algorithms[i];

                    using (var keySource = new PinnedMemory(keySourceBytes.Span.Slice(i * singleKeySourceLengthInBytes, singleKeySourceLengthInBytes).ToArray()))
                    {
                        keys[i] = SymmetricKey.New(algorithm, derivationMode, keySource);
                    }
                }
            }

            return new CascadingSymmetricKey(keys);
        }

        /// <summary>
        /// Gets the number of layers of encryption that a resulting transform will apply.
        /// </summary>
        public Int32 Depth => Keys.Count();

        /// <summary>
        /// Gets the ordered array of keys comprising the cascading key.
        /// </summary>
        public IEnumerable<ISymmetricKey> Keys
        {
            get;
        }

        /// <summary>
        /// Represents the number of bytes comprising a serialized representation of a <see cref="CascadingSymmetricKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 SerializedLength = ((SymmetricKey.SerializedLength * MaximumDepth) + sizeof(UInt16));

        /// <summary>
        /// Represents the default derivation mode for new keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SymmetricKeyDerivationMode DefaultDerivationMode = SymmetricKeyDerivationMode.Pbkdf2;

        /// <summary>
        /// Represents the default inner-layer symmetric-key algorithm specification for new keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SymmetricAlgorithmSpecification DefaultFirstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

        /// <summary>
        /// Represents the default outer-layer symmetric-key algorithm specification for new keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SymmetricAlgorithmSpecification DefaultSecondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

        /// <summary>
        /// Represents the maximum number of layers of encryption that a resulting transform can apply.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MaximumDepth = 4;
    }
}