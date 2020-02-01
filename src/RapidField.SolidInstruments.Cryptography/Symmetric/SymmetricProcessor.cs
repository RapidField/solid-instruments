// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Provides facilities for encrypting and decrypting typed objects.
    /// </summary>
    /// <remarks>
    /// <see cref="SymmetricProcessor{T}" /> is the default implementation of <see cref="ISymmetricProcessor{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the object that can be encrypted and decrypted.
    /// </typeparam>
    public class SymmetricProcessor<T> : ISymmetricProcessor<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricProcessor{T}" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate initialization vectors.
        /// </param>
        /// <param name="binarySerializer">
        /// A binary serializer that is used to transform plaintext.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" /> -or- <paramref name="binarySerializer" /> is
        /// <see langword="null" />.
        /// </exception>
        public SymmetricProcessor(RandomNumberGenerator randomnessProvider, ISerializer<T> binarySerializer)
        {
            BinarySerializer = binarySerializer.RejectIf().IsNull(nameof(binarySerializer)).TargetArgument;
            RandomnessProvider = randomnessProvider.RejectIf().IsNull(nameof(randomnessProvider));
        }

        /// <summary>
        /// Decrypts the specified ciphertext.
        /// </summary>
        /// <param name="ciphertext">
        /// The ciphertext to decrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits and algorithm specification used to transform the ciphertext.
        /// </param>
        /// <returns>
        /// The resulting plaintext object.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public T Decrypt(Byte[] ciphertext, ISymmetricKey key)
        {
            try
            {
                using (var keyBuffer = key.ToDerivedKeyBytes())
                {
                    return Decrypt(ciphertext, keyBuffer, key.Algorithm);
                }
            }
            catch
            {
                throw new SecurityException("The decryption operation failed.");
            }
        }

        /// <summary>
        /// Decrypts the specified ciphertext.
        /// </summary>
        /// <param name="ciphertext">
        /// The ciphertext to decrypt.
        /// </param>
        /// <param name="key">
        /// The cascading key derivation bits and algorithm specifications used to transform the ciphertext.
        /// </param>
        /// <returns>
        /// The resulting plaintext object.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public T Decrypt(Byte[] ciphertext, ICascadingSymmetricKey key)
        {
            try
            {
                var keys = key.Keys;
                var binaryDecryptor = new SymmetricBinaryProcessor(RandomnessProvider);
                var buffer = ciphertext;

                for (var i = (key.Depth - 1); i > 0; i--)
                {
                    buffer = binaryDecryptor.Decrypt(buffer, keys.ElementAt(i));
                }

                return Decrypt(buffer, keys.First());
            }
            catch
            {
                throw new SecurityException("The decryption operation failed.");
            }
        }

        /// <summary>
        /// Decrypts the specified ciphertext.
        /// </summary>
        /// <param name="ciphertext">
        /// The ciphertext to decrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits used to transform the ciphertext.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the ciphertext.
        /// </param>
        /// <returns>
        /// The resulting plaintext object.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public T Decrypt(Byte[] ciphertext, ISecureBuffer key, SymmetricAlgorithmSpecification algorithm)
        {
            try
            {
                var plaintext = default(T);

                key.Access(keyBuffer =>
                {
                    plaintext = Decrypt(ciphertext, keyBuffer, algorithm);
                });

                return plaintext;
            }
            catch
            {
                throw new SecurityException("The decryption operation failed.");
            }
        }

        /// <summary>
        /// Encrypts the specified plaintext object.
        /// </summary>
        /// <param name="plaintextObject">
        /// The plaintext object to encrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits and algorithm specification used to transform the object.
        /// </param>
        /// <returns>
        /// The resulting ciphertext.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public Byte[] Encrypt(T plaintextObject, ISymmetricKey key) => Encrypt(plaintextObject, key, null);

        /// <summary>
        /// Encrypts the specified plaintext object.
        /// </summary>
        /// <param name="plaintextObject">
        /// The plaintext object to encrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits and algorithm specification used to transform the object.
        /// </param>
        /// <param name="initializationVector">
        /// An initialization vector with length greater than or equal to the block size for the specified cipher (extra bytes are
        /// ignored), or <see langword="null" /> to generate a random initialization vector.
        /// </param>
        /// <returns>
        /// The resulting ciphertext.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public Byte[] Encrypt(T plaintextObject, ISymmetricKey key, Byte[] initializationVector)
        {
            try
            {
                using (var keyBuffer = key.ToDerivedKeyBytes())
                {
                    return Encrypt(plaintextObject, keyBuffer, key.Algorithm, initializationVector);
                }
            }
            catch
            {
                throw new SecurityException("The encryption operation failed.");
            }
        }

        /// <summary>
        /// Encrypts the specified plaintext object.
        /// </summary>
        /// <param name="plaintextObject">
        /// The plaintext object to encrypt.
        /// </param>
        /// <param name="key">
        /// The cascading key derivation bits and algorithm specifications used to transform the object.
        /// </param>
        /// <returns>
        /// The resulting ciphertext.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public Byte[] Encrypt(T plaintextObject, ICascadingSymmetricKey key)
        {
            try
            {
                var keys = key.Keys;
                var binaryEncryptor = new SymmetricBinaryProcessor(RandomnessProvider);
                var buffer = Encrypt(plaintextObject, keys.First());

                for (var i = 1; i < key.Depth; i++)
                {
                    buffer = binaryEncryptor.Encrypt(buffer, keys.ElementAt(i));
                }

                return buffer;
            }
            catch
            {
                throw new SecurityException("The encryption operation failed.");
            }
        }

        /// <summary>
        /// Encrypts the specified plaintext object.
        /// </summary>
        /// <param name="plaintextObject">
        /// The plaintext object to encrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits used to transform the object.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the object.
        /// </param>
        /// <returns>
        /// The resulting ciphertext.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public Byte[] Encrypt(T plaintextObject, ISecureBuffer key, SymmetricAlgorithmSpecification algorithm) => Encrypt(plaintextObject, key, algorithm, null);

        /// <summary>
        /// Encrypts the specified plaintext object.
        /// </summary>
        /// <param name="plaintextObject">
        /// The plaintext object to encrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits used to transform the object.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the object.
        /// </param>
        /// <param name="initializationVector">
        /// An initialization vector with length greater than or equal to the block size for the specified cipher (extra bytes are
        /// ignored), or <see langword="null" /> to generate a random initialization vector.
        /// </param>
        /// <returns>
        /// The resulting ciphertext.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public Byte[] Encrypt(T plaintextObject, ISecureBuffer key, SymmetricAlgorithmSpecification algorithm, Byte[] initializationVector)
        {
            try
            {
                var ciphertext = (Byte[])null;

                key.Access(keyBuffer =>
                {
                    ciphertext = Encrypt(plaintextObject, keyBuffer, algorithm, initializationVector);
                });

                return ciphertext;
            }
            catch
            {
                throw new SecurityException("The encryption operation failed.");
            }
        }

        /// <summary>
        /// Decrypts the specified ciphertext.
        /// </summary>
        /// <param name="ciphertext">
        /// The ciphertext to decrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits used to transform the ciphertext.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the ciphertext.
        /// </param>
        /// <returns>
        /// The resulting plaintext object.
        /// </returns>
        [DebuggerHidden]
        private T Decrypt(Byte[] ciphertext, PinnedBuffer key, SymmetricAlgorithmSpecification algorithm)
        {
            using (var cipher = algorithm.ToCipher(RandomnessProvider))
            {
                using (var pinnedCiphertext = new PinnedBuffer(ciphertext, false))
                {
                    using (var plaintext = cipher.Decrypt(pinnedCiphertext, key))
                    {
                        return BinarySerializer.Deserialize(plaintext);
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts the specified plaintext object.
        /// </summary>
        /// <param name="plaintextObject">
        /// The plaintext object to encrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits used to transform the object.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the object.
        /// </param>
        /// <param name="initializationVector">
        /// An initialization vector with length greater than or equal to the block size for the specified cipher (extra bytes are
        /// ignored), or <see langword="null" /> to generate a random initialization vector.
        /// </param>
        /// <returns>
        /// The resulting ciphertext.
        /// </returns>
        [DebuggerHidden]
        private Byte[] Encrypt(T plaintextObject, PinnedBuffer key, SymmetricAlgorithmSpecification algorithm, Byte[] initializationVector)
        {
            var plaintext = BinarySerializer.Serialize(plaintextObject);
            var plaintextLength = plaintext.Length;

            using (var pinnedPlaintext = new PinnedBuffer(plaintextLength, true))
            {
                Array.Copy(plaintext, pinnedPlaintext, plaintextLength);

                using (var cipher = algorithm.ToCipher(RandomnessProvider))
                {
                    switch (cipher.Mode)
                    {
                        case CryptographicTransform.CipherModeCbc:

                            using (var processedInitializationVector = new PinnedBuffer(cipher.BlockSizeInBytes, true))
                            {
                                if (initializationVector is null)
                                {
                                    RandomnessProvider.GetBytes(processedInitializationVector);
                                }
                                else
                                {
                                    Array.Copy(initializationVector.RejectIf(argument => argument.Length < cipher.BlockSizeInBytes, nameof(initializationVector)), processedInitializationVector, cipher.BlockSizeInBytes);
                                }

                                return cipher.Encrypt(pinnedPlaintext, key, processedInitializationVector);
                            }

                        case CryptographicTransform.CipherModeEcb:

                            return cipher.Encrypt(pinnedPlaintext, key, null);

                        default:

                            throw new UnsupportedSpecificationException($"The specified cipher mode, {cipher.Mode}, is not supported.");
                    }
                }
            }
        }

        /// <summary>
        /// Represents a binary serializer that is used to transform plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ISerializer<T> BinarySerializer;

        /// <summary>
        /// Represents a random number generator that is used to generate initialization vectors.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly RandomNumberGenerator RandomnessProvider;
    }
}