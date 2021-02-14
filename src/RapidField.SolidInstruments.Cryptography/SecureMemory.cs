// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using RapidField.SolidInstruments.Cryptography.Symmetric.Aes;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a fixed-length bit field that is pinned in bit field and encrypted at rest.
    /// </summary>
    public class SecureMemory : Instrument, ISecureMemory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureMemory" /> class.
        /// </summary>
        /// <param name="lengthInBytes">
        /// The length of the bit field, in bytes.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="lengthInBytes" /> is less than or equal to zero.
        /// </exception>
        public SecureMemory(Int32 lengthInBytes)
            : base()
        {
            Cipher = new Aes128CbcCipher(RandomnessProvider);
            LengthInBytes = lengthInBytes.RejectIf().IsLessThanOrEqualTo(0, nameof(lengthInBytes));
            PrivateKeySourceField = new(PrivateKeySourceLengthInBytes, PrivateKeySourceFieldMultiplier, RandomnessProvider);
            PrivateKeySourceBitShiftDirection = RandomnessProvider.GetBoolean() ? BitShiftDirection.Left : BitShiftDirection.Right;
            PrivateKeySourceBitShiftCount = BitConverter.GetBytes(RandomnessProvider.GetUInt16()).Max();
            PrivateKeyVersionValue = 0;

            using var initializationVector = new PinnedMemory(Cipher.BlockSizeInBytes, true);
            RandomnessProvider.GetBytes(initializationVector);

            using var plaintext = new PinnedMemory(lengthInBytes);
            using var privateKey = DerivePrivateKey(PrivateKeySource, PrivateKeySourceBitShiftDirection, PrivateKeySourceBitShiftCount, Cipher.KeySizeInBytes);
            using var ciphertext = Cipher.Encrypt(plaintext, privateKey, initializationVector);
            CiphertextField = new(ciphertext.Length, CiphertextFieldMultiplier, RandomnessProvider);
            ciphertext.ReadOnlySpan.CopyTo(Ciphertext);
        }

        /// <summary>
        /// Generates cryptographically secure pseudo-random bytes that are pinned in bit field and encrypted at rest.
        /// </summary>
        /// <remarks>
        /// This method is useful for generating keys and other sensitive random values.
        /// </remarks>
        /// <param name="lengthInBytes">
        /// The length of the random bit field, in bytes.
        /// </param>
        /// <returns>
        /// A cryptographically secure pseudo-random byte array of specified length.
        /// </returns>
        public static ISecureMemory GenerateHardenedRandomBytes(Int32 lengthInBytes)
        {
            var hardenedRandomBytes = new SecureMemory(lengthInBytes);

            hardenedRandomBytes.Access((memory) =>
            {
                RandomnessProvider.GetBytes(memory);
            });

            return hardenedRandomBytes;
        }

        /// <summary>
        /// Decrypts the bit field, performs the specified operation against the pinned plaintext and encrypts the bit field as a
        /// thread-safe, atomic operation.
        /// </summary>
        /// <param name="action">
        /// The operation to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Access(Action<PinnedMemory> action)
        {
            action = action.RejectIf().IsNull(nameof(action));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                using var plaintext = new PinnedMemory(LengthInBytes, true);
                DecryptField(plaintext);

                try
                {
                    action(plaintext);
                }
                finally
                {
                    EncryptField(plaintext);
                }
            }
        }

        /// <summary>
        /// Asynchronously decrypts the bit field, performs the specified operation against the pinned plaintext and encrypts the
        /// bit field as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="action">
        /// The operation to perform.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task AccessAsync(Action<PinnedMemory> action) => Task.Factory.StartNew(() => Access(action));

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode()
        {
            var ciphertext = Ciphertext;
            return ciphertext is null ? 0 : ciphertext.ComputeThirtyTwoBitHash() ^ 0x3a566a5c;
        }

        /// <summary>
        /// Regenerates and replaces the source bytes for the private key that is used to secure the current
        /// <see cref="SecureMemory" />.
        /// </summary>
        void ISecureMemory.RegeneratePrivateKey()
        {
            if (IsDisposedOrDisposing)
            {
                return;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return;
                }

                using (var plaintext = new PinnedMemory(LengthInBytes, true))
                {
                    DecryptField(plaintext);

                    try
                    {
                        CiphertextField.Scramble();
                        PrivateKeySourceField.Scramble();
                    }
                    finally
                    {
                        PrivateKeyVersionValue = PrivateKeyVersionValue == UInt32.MaxValue ? 0 : PrivateKeyVersionValue + 1;
                        EncryptField(plaintext);
                    }
                }
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="SecureMemory" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="SecureMemory" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(LengthInBytes)}\": {LengthInBytes} }}";

        /// <summary>
        /// Derives a private key from <see cref="PrivateKeySource" />.
        /// </summary>
        /// <returns>
        /// The resulting private key.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal PinnedMemory DerivePrivateKey()
        {
            RejectIfDisposed();
            return DerivePrivateKey(PrivateKeySource, PrivateKeySourceBitShiftDirection, PrivateKeySourceBitShiftCount, Cipher.KeySizeInBytes);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SecureMemory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (IsDisposed is false)
                {
                    Cipher?.Dispose();
                    CiphertextField?.Dispose();
                    PrivateKeySourceField?.Dispose();
                    CiphertextField = null;
                    PrivateKeySourceField = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Derives a private key from the specified source bytes.
        /// </summary>
        /// <remarks>
        /// This doesn't ensure security of the private key. It exists to complicate the engineering challenge for would-be
        /// attackers and to increase the computational expense of an attack to improve the chance that it will be evident to users.
        /// </remarks>
        /// <param name="privateKeySource">
        /// A field of random bits from which the private key is derived.
        /// </param>
        /// <param name="bitShiftDirection">
        /// The circular bit shift direction to use when deriving the key.
        /// </param>
        /// <param name="bitShiftCount">
        /// The circular bit shift count to use when deriving the key.
        /// </param>
        /// <param name="keyLengthInBytes">
        /// The length of the derived key, in bytes.
        /// </param>
        /// <returns>
        /// The resulting private key.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static PinnedMemory DerivePrivateKey(Byte[] privateKeySource, BitShiftDirection bitShiftDirection, Int32 bitShiftCount, Int32 keyLengthInBytes) => new(new Span<Byte>(privateKeySource.PerformCircularBitShift(bitShiftDirection, bitShiftCount)).Slice(0, keyLengthInBytes).ToArray(), true);

        /// <summary>
        /// Decrypts the current bit field and writes the result to the specified bit field.
        /// </summary>
        /// <param name="plaintext">
        /// The bit field to which the plaintext result is written.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DecryptField(PinnedMemory plaintext)
        {
            using var privateKey = DerivePrivateKey();
            using var buffer = Cipher.Decrypt(Ciphertext, privateKey);
            buffer.Span.CopyTo(plaintext);
        }

        /// <summary>
        /// Encrypts the specified plaintext and writes the result to the current bit field.
        /// </summary>
        /// <param name="plaintext">
        /// The bit field containing the plaintext to encrypt.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EncryptField(PinnedMemory plaintext)
        {
            RejectIfDisposed();
            using var initializationVector = new PinnedMemory(Cipher.BlockSizeInBytes, true);
            RandomnessProvider.GetBytes(initializationVector);

            using var privateKey = DerivePrivateKey();
            using var buffer = Cipher.Encrypt(plaintext, privateKey, initializationVector);
            buffer.Span.CopyTo(Ciphertext);
        }

        /// <summary>
        /// Gets a static, cryptographically secure pseudo-random number generator that hardens platform-generated bytes using
        /// 256-bit AES encryption.
        /// </summary>
        public static RandomNumberGenerator RandomnessProvider => HardenedRandomNumberGenerator.Instance;

        /// <summary>
        /// Gets the length of the bit field, in bytes.
        /// </summary>
        public Int32 LengthInBytes
        {
            get;
        }

        /// <summary>
        /// Gets the current zero-based ordinal version of the bits comprising the private key that is used to secure the current
        /// <see cref="SecureMemory" />.
        /// </summary>
        UInt32 ISecureMemory.PrivateKeyVersion => PrivateKeyVersionValue;

        /// <summary>
        /// Gets the ciphertext bits for the bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PinnedMemory Ciphertext => CiphertextField?.TargetField;

        /// <summary>
        /// Gets a field of random bits from which a private key is derived to encrypt and decrypt the secure bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PinnedMemory PrivateKeySource => PrivateKeySourceField?.TargetField;

        /// <summary>
        /// Gets the length, in bytes, of <see cref="PrivateKeySource" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 PrivateKeySourceLengthInBytes => Cipher.KeySizeInBytes + 16;

        /// <summary>
        /// Represents the length multiplier for <see cref="CiphertextField" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 CiphertextFieldMultiplier = 2;

        /// <summary>
        /// Represents the length multiplier for <see cref="PrivateKeySourceField" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PrivateKeySourceFieldMultiplier = 4;

        /// <summary>
        /// Represents a cipher that is used to encrypt and decrypt the bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SymmetricKeyCipher Cipher;

        /// <summary>
        /// Represents the circular bit shift count to use when deriving a private key from <see cref="PrivateKeySource" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Byte PrivateKeySourceBitShiftCount;

        /// <summary>
        /// Represents the circular bit shift direction to use when deriving a private key from <see cref="PrivateKeySource" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly BitShiftDirection PrivateKeySourceBitShiftDirection;

        /// <summary>
        /// Represents the ciphertext bits for the bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private InflatedField CiphertextField;

        /// <summary>
        /// Represents a field of random bits from which a private key is derived to encrypt and decrypt the secure bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private InflatedField PrivateKeySourceField;

        /// <summary>
        /// Represents the current zero-based ordinal version of the bits comprising <see cref="PrivateKeySource" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UInt32 PrivateKeyVersionValue;

        /// <summary>
        /// Represents a pinned memory field, the length of which is inflated by an integer multiplier.
        /// </summary>
        /// <remarks>
        /// <see cref="InflatedField" /> exposes diversionary bit fields in memory and increases the computational expense of memory
        /// attacks upon <see cref="SecureMemory" /> instances.
        /// </remarks>
        private sealed class InflatedField : Instrument
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InflatedField" /> class.
            /// </summary>
            /// <param name="length">
            /// The length, in bytes, of the target field.
            /// </param>
            /// <param name="multiplier">
            /// The total number of fields to allocate.
            /// </param>
            /// <param name="randomnessProvider">
            /// A random number generator that is used to scramble the inflated field.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="randomnessProvider" /> is <see langword="null" />.
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="length" /> is less than or equal to zero -or- <paramref name="multiplier" /> is less than or equal
            /// to one.
            /// </exception>
            [DebuggerHidden]
            internal InflatedField(Int32 length, Int32 multiplier, RandomNumberGenerator randomnessProvider)
                : base()
            {
                Fields = new PinnedMemory[multiplier.RejectIf().IsLessThanOrEqualTo(1, nameof(multiplier))];
                RandomnessProvider = randomnessProvider.RejectIf().IsNull(nameof(randomnessProvider));
                ReferenceKey = RandomnessProvider.GetUInt64();

                for (var i = 0; i < multiplier; i++)
                {
                    var field = new PinnedMemory(length, true);
                    RandomnessProvider.GetBytes(field);
                    Fields[i] = field;
                }
            }

            /// <summary>
            /// Fills the current <see cref="InflatedField" /> with random bits and generates a new, random reference key.
            /// </summary>
            [DebuggerHidden]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal void Scramble()
            {
                if (IsDisposedOrDisposing)
                {
                    return;
                }

                using (var controlToken = StateControl.Enter())
                {
                    if (IsDisposedOrDisposing)
                    {
                        return;
                    }

                    ReferenceKey = RandomnessProvider.GetUInt64();

                    foreach (var field in Fields)
                    {
                        RandomnessProvider.GetBytes(field);
                    }
                }
            }

            /// <summary>
            /// Releases all resources consumed by the current <see cref="InflatedField" />.
            /// </summary>
            /// <param name="disposing">
            /// A value indicating whether or not disposal was invoked by user code.
            /// </param>
            protected override void Dispose(Boolean disposing)
            {
                try
                {
                    if (IsDisposed is false)
                    {
                        ReferenceKey = default;

                        if (Fields?.Any() ?? false)
                        {
                            foreach (var field in Fields)
                            {
                                field?.Dispose();
                            }

                            Fields = null;
                        }
                    }
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }

            /// <summary>
            /// Gets the target field.
            /// </summary>
            /// <returns>
            /// The target field.
            /// </returns>
            [DebuggerHidden]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private PinnedMemory GetTargetField()
            {
                if (IsDisposed)
                {
                    return null;
                }

                using (var controlToken = StateControl.Enter())
                {
                    if (IsDisposed)
                    {
                        return null;
                    }

                    return Fields[TargetFieldIndex];
                }
            }

            /// <summary>
            /// Gets the target field.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal PinnedMemory TargetField => GetTargetField();

            /// <summary>
            /// Represents the total number of fields allocated by the current <see cref="InflatedField" />.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Int32 Multiplier => Fields.Length;

            /// <summary>
            /// Gets the zero-based index of the target field within <see cref="Fields" />.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Int32 TargetFieldIndex => Convert.ToInt32(ReferenceKey / (UInt64.MaxValue / Multiplier.ToUInt64()));

            /// <summary>
            /// Represents a random number generator that is used to scramble the inflated field.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly RandomNumberGenerator RandomnessProvider;

            /// <summary>
            /// Represents the underlying collection of pinned memory fields.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private PinnedMemory[] Fields;

            /// <summary>
            /// Represents a 64-bit key that defines the location of the target field within the current
            /// <see cref="InflatedField" />.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private UInt64 ReferenceKey;
        }
    }
}