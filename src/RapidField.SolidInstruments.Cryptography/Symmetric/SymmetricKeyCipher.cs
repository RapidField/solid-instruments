// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Represents a symmetric-key encryption cipher.
    /// </summary>
    internal abstract class SymmetricKeyCipher : Instrument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricKeyCipher" /> class.
        /// </summary>
        /// <param name="blockSize">
        /// The bit-length of a single block for the cipher.
        /// </param>
        /// <param name="keySize">
        /// The bit-length of the key for the cipher.
        /// </param>
        /// <param name="mode">
        /// The encryption mode for the cipher.
        /// </param>
        /// <param name="paddingMode">
        /// The padding setting for the cipher.
        /// </param>
        /// <param name="randomnessProvider">
        /// A random number generator used to generate initialization vectors.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        protected SymmetricKeyCipher(Int32 blockSize, Int32 keySize, CipherMode mode, PaddingMode paddingMode, RandomNumberGenerator randomnessProvider)
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            BlockSizeInBits = blockSize;
            KeySizeInBits = keySize;
            Mode = mode;
            PaddingMode = paddingMode;
            RandomnessProvider = randomnessProvider.RejectIf().IsNull(nameof(randomnessProvider));
        }

        /// <summary>
        /// Decrypts the specified ciphertext.
        /// </summary>
        /// <param name="ciphertext">
        /// Cipher text that was encrypted using the specified private key.
        /// </param>
        /// <param name="privateKey">
        /// A secure private key that was used to encrypt the specified ciphertext.
        /// </param>
        /// <returns>
        /// The plaintext result of the algorithm.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The binary length of <paramref name="ciphertext" /> is invalid -or- the binary length of <paramref name="privateKey" />
        /// is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ciphertext" /> is <see langword="null" /> -or- <paramref name="privateKey" /> is
        /// <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal PinnedBuffer Decrypt(PinnedBuffer<Byte> ciphertext, PinnedBuffer<Byte> privateKey)
        {
            ciphertext.RejectIf().IsNull(nameof(ciphertext)).OrIf(argument => argument.Count() < BlockSizeInBytes, nameof(ciphertext), "The length of the specified ciphertext is invalid for the algorithm.");
            privateKey.RejectIf().IsNull(nameof(privateKey)).OrIf(argument => argument.Count() != KeySizeInBytes, nameof(privateKey), "The length of the specified key is invalid for the algorithm.");

            return Mode switch
            {
                CryptographicTransform.CipherModeCbc => DecryptInCbcMode(ciphertext, privateKey),
                CryptographicTransform.CipherModeEcb => DecryptInEcbMode(ciphertext, privateKey),
                _ => throw new UnsupportedSpecificationException($"The specified cipher mode, {Mode}, is not supported.")
            };
        }

        /// <summary>
        /// Encrypts the specified plaintext.
        /// </summary>
        /// <param name="plaintext">
        /// Plaintext data to be encrypted using the specified private key.
        /// </param>
        /// <param name="privateKey">
        /// A secure private key that will be used to encrypt the specified plaintext.
        /// </param>
        /// <param name="initializationVector">
        /// A fixed-size input to the algorithm.
        /// </param>
        /// <returns>
        /// The ciphertext result of the algorithm.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The binary length of <paramref name="privateKey" /> is invalid -or- the cipher mode is <see cref="CipherMode.CBC" /> and
        /// the binary length of <paramref name="initializationVector" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="plaintext" /> is <see langword="null" /> -or- <paramref name="privateKey" /> is <see langword="null" />
        /// -or- the cipher mode is <see cref="CipherMode.CBC" /> and <paramref name="initializationVector" /> is
        ///  <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal PinnedBuffer Encrypt(PinnedBuffer<Byte> plaintext, PinnedBuffer<Byte> privateKey, PinnedBuffer<Byte> initializationVector)
        {
            plaintext.RejectIf().IsNull(nameof(plaintext));
            privateKey.RejectIf().IsNull(nameof(privateKey)).OrIf(argument => argument.Count() != KeySizeInBytes, nameof(privateKey), "The length of the specified key is invalid for the algorithm.");

            return Mode switch
            {
                CryptographicTransform.CipherModeCbc => EncryptInCbcMode(plaintext, privateKey, initializationVector),
                CryptographicTransform.CipherModeEcb => EncryptInEcbMode(plaintext, privateKey),
                _ => throw new UnsupportedSpecificationException($"The specified cipher mode, {Mode}, is not supported.")
            };
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SymmetricKeyCipher" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        [DebuggerHidden]
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Initializes the underlying symmetric-key algorithm provider for this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="SymmetricAlgorithm" />.
        /// </returns>
        protected abstract SymmetricAlgorithm InitializeProvider();

        /// <summary>
        /// Decrypts the specified ciphertext in Cipher Block Chaining (CBC) mode.
        /// </summary>
        /// <param name="ciphertext">
        /// Cipher text that was encrypted using the specified private key.
        /// </param>
        /// <param name="privateKey">
        /// A secure private key that was used to encrypt the specified ciphertext.
        /// </param>
        /// <returns>
        /// The plaintext result of the algorithm.
        /// </returns>
        [DebuggerHidden]
        private PinnedBuffer DecryptInCbcMode(PinnedBuffer<Byte> ciphertext, PinnedBuffer<Byte> privateKey)
        {
            using (var initializationVector = new PinnedBuffer(BlockSizeInBytes, true))
            {
                Array.Copy(ciphertext, 0, initializationVector, 0, BlockSizeInBytes);

                using (var cipherTextSansInitializationVector = new PinnedBuffer((ciphertext.Length - BlockSizeInBytes), true))
                {
                    Array.Copy(ciphertext, BlockSizeInBytes, cipherTextSansInitializationVector, 0, cipherTextSansInitializationVector.Length);

                    using (var encryptionProvider = InitializeProvider())
                    {
                        encryptionProvider.BlockSize = BlockSizeInBits;
                        encryptionProvider.KeySize = KeySizeInBits;
                        encryptionProvider.Mode = Mode;
                        encryptionProvider.Padding = PaddingMode;
                        encryptionProvider.Key = privateKey;
                        encryptionProvider.IV = initializationVector;

                        using (var decryptor = encryptionProvider.CreateDecryptor(privateKey, initializationVector))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptographicStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                                {
                                    cryptographicStream.Write(cipherTextSansInitializationVector, 0, cipherTextSansInitializationVector.Length);
                                    cryptographicStream.FlushFinalBlock();
                                    return new PinnedBuffer(memoryStream.ToArray(), false);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts the specified ciphertext in Electronic Codebook (ECB) mode.
        /// </summary>
        /// <param name="ciphertext">
        /// Cipher text that was encrypted using the specified private key.
        /// </param>
        /// <param name="privateKey">
        /// A secure private key that was used to encrypt the specified ciphertext.
        /// </param>
        /// <returns>
        /// The plaintext result of the algorithm.
        /// </returns>
        [DebuggerHidden]
        private PinnedBuffer DecryptInEcbMode(PinnedBuffer<Byte> ciphertext, PinnedBuffer<Byte> privateKey)
        {
            using (var encryptionProvider = InitializeProvider())
            {
                encryptionProvider.BlockSize = BlockSizeInBits;
                encryptionProvider.KeySize = KeySizeInBits;
                encryptionProvider.Mode = Mode;
                encryptionProvider.Padding = PaddingMode;
                encryptionProvider.Key = privateKey;

                using (var decryptor = encryptionProvider.CreateDecryptor(privateKey, UnusedInitializationVector))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptographicStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                        {
                            cryptographicStream.Write(ciphertext, 0, ciphertext.Length);
                            cryptographicStream.FlushFinalBlock();
                            return new PinnedBuffer(memoryStream.ToArray(), false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts the specified ciphertext in Cipher Block Chaining (CBC) mode.
        /// </summary>
        /// <param name="plaintext">
        /// Plaintext data to be encrypted using the specified private key.
        /// </param>
        /// <param name="privateKey">
        /// A secure private key that will be used to encrypt the specified plaintext.
        /// </param>
        /// <param name="initializationVector">
        /// A fixed-size input to the algorithm.
        /// </param>
        /// <returns>
        /// The ciphertext result of the algorithm.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The binary length of <paramref name="initializationVector" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="initializationVector" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private PinnedBuffer EncryptInCbcMode(PinnedBuffer<Byte> plaintext, PinnedBuffer<Byte> privateKey, PinnedBuffer<Byte> initializationVector)
        {
            initializationVector.RejectIf().IsNull(nameof(initializationVector)).OrIf(argument => argument.Count() != BlockSizeInBytes, nameof(privateKey), "The length of the specified initialization vector is invalid for the algorithm.");

            using (var encryptionProvider = InitializeProvider())
            {
                encryptionProvider.BlockSize = BlockSizeInBits;
                encryptionProvider.KeySize = KeySizeInBits;
                encryptionProvider.Mode = Mode;
                encryptionProvider.Padding = PaddingMode;
                encryptionProvider.Key = privateKey;
                encryptionProvider.IV = initializationVector;

                using (var encryptor = encryptionProvider.CreateEncryptor(privateKey, initializationVector))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptographicStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptographicStream.Write(plaintext, 0, plaintext.Length);
                            cryptographicStream.FlushFinalBlock();
                            return new PinnedBuffer(initializationVector.Concat(memoryStream.ToArray()).ToArray(), false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts the specified ciphertext in Electronic Codebook (ECB) mode.
        /// </summary>
        /// <param name="plaintext">
        /// Plaintext data to be encrypted using the specified private key.
        /// </param>
        /// <param name="privateKey">
        /// A secure private key that will be used to encrypt the specified plaintext.
        /// </param>
        /// <returns>
        /// The ciphertext result of the algorithm.
        /// </returns>
        [DebuggerHidden]
        private PinnedBuffer EncryptInEcbMode(PinnedBuffer<Byte> plaintext, PinnedBuffer<Byte> privateKey)
        {
            using (var encryptionProvider = InitializeProvider())
            {
                encryptionProvider.BlockSize = BlockSizeInBits;
                encryptionProvider.KeySize = KeySizeInBits;
                encryptionProvider.Mode = Mode;
                encryptionProvider.Padding = PaddingMode;
                encryptionProvider.Key = privateKey;

                using (var encryptor = encryptionProvider.CreateEncryptor(privateKey, UnusedInitializationVector))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptographicStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptographicStream.Write(plaintext, 0, plaintext.Length);
                            cryptographicStream.FlushFinalBlock();
                            return new PinnedBuffer(memoryStream.ToArray(), false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of bytes per block for the cipher represented by this <see cref="SymmetricKeyCipher" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Int32 BlockSizeInBytes => (BlockSizeInBits / 8);

        /// <summary>
        /// Gets the number of bits per block for the cipher represented by this <see cref="SymmetricKeyCipher" />.
        /// </summary>
        protected internal Int32 BlockSizeInBits
        {
            get;
        }

        /// <summary>
        /// Gets the number of key bits for the cipher represented by this <see cref="SymmetricKeyCipher" />.
        /// </summary>
        protected internal Int32 KeySizeInBits
        {
            get;
        }

        /// <summary>
        /// Gets the encryption mode for the cipher represented by this <see cref="SymmetricKeyCipher" />.
        /// </summary>
        protected internal CipherMode Mode
        {
            get;
        }

        /// <summary>
        /// Gets the padding setting for the cipher represented by this <see cref="SymmetricKeyCipher" />.
        /// </summary>
        protected internal PaddingMode PaddingMode
        {
            get;
        }

        /// <summary>
        /// Gets a random number generator that is used to generate initialization vectors.
        /// </summary>
        protected RandomNumberGenerator RandomnessProvider
        {
            get;
        }

        /// <summary>
        /// Gets the number of key bytes for the cipher represented by this <see cref="SymmetricKeyCipher" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 KeySizeInBytes => (KeySizeInBits / 8);

        /// <summary>
        /// Gets an unused initialization vector with correct byte length for Electronic Codebook (ECB) mode transformations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Byte[] UnusedInitializationVector => new Byte[BlockSizeInBytes];
    }
}