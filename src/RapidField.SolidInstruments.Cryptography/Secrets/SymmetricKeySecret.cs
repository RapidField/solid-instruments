// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a named secret <see cref="SymmetricKey" /> value that is pinned in memory and encrypted at rest.
    /// </summary>
    public sealed class SymmetricKeySecret : Secret<SymmetricKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricKeySecret" /> class.
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
        public SymmetricKeySecret(String name)
            : base(name)
        {
            return;
        }

        /// <summary>
        /// Creates a new <see cref="SymmetricKeySecret" /> using the specified name and value.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies the secret.
        /// </param>
        /// <param name="value">
        /// The secret value.
        /// </param>
        /// <returns>
        /// A new <see cref="StringSecret" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        public static SymmetricKeySecret FromValue(String name, SymmetricKey value)
        {
            value = value.RejectIf().IsNull(nameof(value)).TargetArgument;
            var secret = new SymmetricKeySecret(name);
            secret.Write(() => value);
            return secret;
        }

        /// <summary>
        /// Generates a new named <see cref="SymmetricKey" />.
        /// </summary>
        /// <returns>
        /// A new named <see cref="SymmetricKey" />.
        /// </returns>
        [DebuggerHidden]
        internal static SymmetricKeySecret New() => FromValue(NewDefaultSymmetricKeySecretName(), SymmetricKey.New());

        /// <summary>
        /// Creates a <see cref="SymmetricKey" /> using the provided bytes.
        /// </summary>
        /// <param name="bytes">
        /// A pinned bit field representing a <see cref="SymmetricKey" />.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The resulting <see cref="SymmetricKey" />.
        /// </returns>
        protected sealed override SymmetricKey ConvertBytesToValue(IReadOnlyPinnedMemory<Byte> bytes, IConcurrencyControlToken controlToken)
        {
            var result = (SymmetricKey)null;

            using (var secureMemory = new SecureMemory(bytes.Length))
            {
                secureMemory.Access(memory =>
                {
                    bytes.ReadOnlySpan.CopyTo(memory);
                });

                result = SymmetricKey.FromSecureMemory(secureMemory);
            }

            return result;
        }

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
        /// <paramref name="value" /> as a pinned memory.
        /// </returns>
        protected sealed override IReadOnlyPinnedMemory<Byte> ConvertValueToBytes(SymmetricKey value, IConcurrencyControlToken controlToken)
        {
            var result = (ReadOnlyPinnedMemory)null;

            using (var secureMemory = value.ToSecureMemory())
            {
                secureMemory.Access(memory =>
                {
                    result = new ReadOnlyPinnedMemory(memory.ToArray());
                });
            }

            return result;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SymmetricKeySecret" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Generates a new, unique default symmetric key name.
        /// </summary>
        /// <returns>
        /// A new, unique default symmetric key name.
        /// </returns>
        [DebuggerHidden]
        private static String NewDefaultSymmetricKeySecretName() => Secret.GetPrefixedSemanticIdentifier(DefaultSymmetricKeySecretNamePrefix, NewRandomSemanticIdentifier());

        /// <summary>
        /// Represents the default textual prefix for <see cref="SymmetricKeySecret" /> names.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DefaultSymmetricKeySecretNamePrefix = "SymmetricKey";
    }
}