// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Symmetric.Twofish
{
    /// <summary>
    /// Performs symmetric-key encryption and decryption using the Cryptographic Application Programming Interfaces (CAPI)
    /// implementation of the Twofish algorithm.
    /// </summary>
    internal sealed class TwofishCryptoServiceProvider : SymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwofishCryptoServiceProvider" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator used to generate initialization vectors.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        public TwofishCryptoServiceProvider(RandomNumberGenerator randomnessProvider)
            : base()
        {
            RandomnessProvider = randomnessProvider.RejectIf().IsNull(nameof(randomnessProvider));
        }

        /// <summary>
        /// Creates a symmetric decryptor object using the current key and initialization vector (IV).
        /// </summary>
        /// <returns>
        /// A new instance of a <see cref="TwofishTransform" /> decryptor.
        /// </returns>
        [DebuggerHidden]
        public override ICryptoTransform CreateDecryptor() => new TwofishTransform(KeySize, KeyValue, IVValue, BlockSize, ModeValue, PaddingValue, EncryptionDirection.Decryption);

        /// <summary>
        /// Creates a symmetric decryptor object using the specified key and initialization vector (IV).
        /// </summary>
        /// <param name="rgbKey">
        /// The secret key to use for the symmetric algorithm.
        /// </param>
        /// <param name="rgbIV">
        /// The initialization vector to use for the symmetric algorithm.
        /// </param>
        /// <returns>
        /// A new instance of a <see cref="TwofishTransform" /> decryptor.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rgbKey" /> is null or <paramref name="rgbIV" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        public override ICryptoTransform CreateDecryptor(Byte[] rgbKey, Byte[] rgbIV)
        {
            IV = rgbIV.RejectIf().IsNull(nameof(rgbIV));
            Key = rgbKey.RejectIf().IsNull(nameof(rgbKey));
            return CreateDecryptor();
        }

        /// <summary>
        /// Creates a symmetric encryptor object using the current key and initialization vector (IV).
        /// </summary>
        /// <returns>
        /// A new instance of a <see cref="TwofishTransform" /> encryptor.
        /// </returns>
        [DebuggerHidden]
        public override ICryptoTransform CreateEncryptor() => new TwofishTransform(KeySize, KeyValue, IVValue, BlockSize, ModeValue, PaddingValue, EncryptionDirection.Encryption);

        /// <summary>
        /// Creates a symmetric encryptor object using the specified key and initialization vector (IV).
        /// </summary>
        /// <param name="rgbKey">
        /// The secret key to use for the symmetric algorithm.
        /// </param>
        /// <param name="rgbIV">
        /// The initialization vector to use for the symmetric algorithm.
        /// </param>
        /// <returns>
        /// A new instance of a <see cref="TwofishTransform" /> encryptor.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rgbKey" /> is null or <paramref name="rgbIV" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        public override ICryptoTransform CreateEncryptor(Byte[] rgbKey, Byte[] rgbIV)
        {
            IV = rgbIV.RejectIf().IsNull(nameof(rgbIV));
            Key = rgbKey.RejectIf().IsNull(nameof(rgbKey));
            return CreateEncryptor();
        }

        /// <summary>
        /// Generates a random initialization vector (IV) to use for the algorithm.
        /// </summary>
        [DebuggerHidden]
        public override void GenerateIV()
        {
            IV = new Byte[(BlockSize / 8)];
            RandomnessProvider.GetBytes(IV);
        }

        /// <summary>
        /// Generates a random key to use for the algorithm.
        /// </summary>
        [DebuggerHidden]
        public override void GenerateKey()
        {
            Key = new Byte[(KeySize / 8)];
            RandomnessProvider.GetBytes(Key);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="TwofishCryptoServiceProvider" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        [DebuggerHidden]
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the block sizes, in bits, that are supported by the symmetric algorithm.
        /// </summary>
        [DebuggerHidden]
        public override KeySizes[] LegalBlockSizes => new KeySizes[] { new KeySizes(MinimumBlockSize, MaximumBlockSize, IntervalForBlockSize) };

        /// <summary>
        /// Gets the key sizes, in bits, that are supported by the symmetric algorithm.
        /// </summary>
        [DebuggerHidden]
        public override KeySizes[] LegalKeySizes => new KeySizes[] { new KeySizes(MinimumKeySize, MaximumKeySize, IntervalForKeySize) };

        /// <summary>
        /// Represents the interval between valid block sizes for the algorithm.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 IntervalForBlockSize = 0;

        /// <summary>
        /// Represents the interval between valid key sizes for the algorithm.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 IntervalForKeySize = 64;

        /// <summary>
        /// Represents the maximum block size for the algorithm.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MaximumBlockSize = 128;

        /// <summary>
        /// Represents the maximum key size for the algorithm.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MaximumKeySize = 256;

        /// <summary>
        /// Represents the minimum block size for the algorithm.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MinimumBlockSize = 128;

        /// <summary>
        /// Represents the minimum key size for the algorithm.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MinimumKeySize = 128;

        /// <summary>
        /// Gets or sets a random number generator used to generate initialization vectors.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly RandomNumberGenerator RandomnessProvider;
    }
}