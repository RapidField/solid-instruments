﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using RapidField.SolidInstruments.Cryptography.Symmetric.Aes;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a cryptographically secure pseudo-random number generator that hardens platform-generated bytes using 256-bit AES
    /// encryption.
    /// </summary>
    internal sealed class HardenedRandomNumberGenerator : RandomNumberGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HardenedRandomNumberGenerator" /> class.
        /// </summary>
        [DebuggerHidden]
        private HardenedRandomNumberGenerator()
        {
            LazyCipher = new(InitializeCipher, LazyThreadSafetyMode.PublicationOnly);
            LazySourceRandomnessProvider = new(InitializeSourceRandomnessProvider, LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>
        /// Fills an array of bytes with a cryptographically strong random sequence of values.
        /// </summary>
        /// <param name="data">
        /// The array to fill with cryptographically strong random bytes.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        public override void GetBytes(Byte[] data)
        {
            data.RejectIf().IsNull(nameof(data));
            var count = data.Length;

            if (count == 0)
            {
                return;
            }

            var startIndex = 0;

            lock (SyncRoot)
            {
                while (true)
                {
                    var result = Buffer.TryPopRange(data, startIndex, count);
                    startIndex += result;
                    count -= result;

                    if (count == 0)
                    {
                        break;
                    }

                    PermuteBuffer();
                }
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="HardenedRandomNumberGenerator" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        [DebuggerHidden]
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                lock (SyncRoot ?? new())
                {
                    LazyCipher?.Dispose();
                    LazySourceRandomnessProvider?.Dispose();
                    Buffer?.Clear();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Finalizes static members of the <see cref="HardenedRandomNumberGenerator" /> class.
        /// </summary>
        [DebuggerHidden]
        private static void FinalizeStaticMembers() => Instance.Dispose();

        /// <summary>
        /// Initializes a random number generator that is used to provide source random material for the current
        /// <see cref="HardenedRandomNumberGenerator" />.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="RandomNumberGenerator" /> class.
        /// </returns>
        [DebuggerHidden]
        private static RandomNumberGenerator InitializeSourceRandomnessProvider() => Create();

        /// <summary>
        /// Initializes a symmetric-key cipher that is used to harden platform-generated bytes.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="Aes256EcbCipher" /> class.
        /// </returns>
        [DebuggerHidden]
        private SymmetricKeyCipher InitializeCipher() => new Aes256EcbCipher(SourceRandomnessProvider);

        /// <summary>
        /// Generates and processes (encrypts) a fixed number of pseudo-random bytes and pushes them onto <see cref="Buffer" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private void PermuteBuffer()
        {
            using (var sourceBytes = new PinnedMemory(BufferPermutationSourceLengthInBytes, true))
            {
                SourceRandomnessProvider.GetBytes(sourceBytes);

                using (var privateKey = new PinnedMemory(CipherKeyLengthInBytes, true))
                {
                    SourceRandomnessProvider.GetBytes(privateKey);

                    using (var initializationVector = new PinnedMemory(CipherBlockLengthInBytes, true))
                    {
                        SourceRandomnessProvider.GetBytes(initializationVector);

                        using (var encryptedBytes = Cipher.Encrypt(sourceBytes, privateKey, initializationVector))
                        {
                            Buffer.PushRange(encryptedBytes);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current length, in bytes, of the random byte buffer.
        /// </summary>
        [DebuggerHidden]
        public Int32 BufferLengthInBytes => Buffer.Count;

        /// <summary>
        /// Gets a symmetric-key cipher that is used to harden platform-generated bytes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SymmetricKeyCipher Cipher => LazyCipher.Value;

        /// <summary>
        /// Gets a random number generator that is used to provide source random material for the current
        /// <see cref="HardenedRandomNumberGenerator" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RandomNumberGenerator SourceRandomnessProvider => LazySourceRandomnessProvider.Value;

        /// <summary>
        /// Represents a singleton instance of the <see cref="HardenedRandomNumberGenerator" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly HardenedRandomNumberGenerator Instance = new();

        /// <summary>
        /// Represents the number of source bytes generated for a single round of <see cref="PermuteBuffer" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 BufferPermutationSourceLengthInBytes = CipherBlockLengthInBytes * 32;

        /// <summary>
        /// Represents the bit-length of a single block for <see cref="Cipher" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 CipherBlockLengthInBits = 128;

        /// <summary>
        /// Represents the length of a single block for <see cref="Cipher" />, in bytes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 CipherBlockLengthInBytes = CipherBlockLengthInBits / 8;

        /// <summary>
        /// Represents the bit-length of the private key for <see cref="Cipher" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 CipherKeyLengthInBits = 256;

        /// <summary>
        /// Represents the length of the private key for <see cref="Cipher" />, in bytes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 CipherKeyLengthInBytes = CipherKeyLengthInBits / 8;

        /// <summary>
        /// Represents a finalizer for static members of the <see cref="HardenedRandomNumberGenerator" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly StaticMemberFinalizer StaticMemberFinalizer = new(FinalizeStaticMembers);

        /// <summary>
        /// Represents a synchronized stack of processed (encrypted) bytes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentStack<Byte> Buffer = new();

        /// <summary>
        /// Represents a lazily-initialized symmetric-key cipher that is used to harden platform-generated bytes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<SymmetricKeyCipher> LazyCipher;

        /// <summary>
        /// Represents a lazily-initialized random number generator that is used to provide source random material for the current
        /// <see cref="HardenedRandomNumberGenerator" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<RandomNumberGenerator> LazySourceRandomnessProvider;

        /// <summary>
        /// Represents an object that is used to synchronize access to the generator.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Object SyncRoot = new();
    }
}