﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
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
        /// Creates a <see cref="SymmetricKey" /> using the provided bytes.
        /// </summary>
        /// <param name="bytes">
        /// A pinned buffer representing a <see cref="SymmetricKey" />.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The resulting <see cref="SymmetricKey" />.
        /// </returns>
        protected sealed override SymmetricKey ConvertBytesToValue(IReadOnlyPinnedBuffer<Byte> bytes, ConcurrencyControlToken controlToken)
        {
            var result = (SymmetricKey)null;

            using (var secureBuffer = new SecureBuffer(bytes.Length))
            {
                secureBuffer.Access(buffer =>
                {
                    bytes.ReadOnlySpan.CopyTo(buffer);
                });

                result = SymmetricKey.FromBuffer(secureBuffer);
            }

            return result;
        }

        /// <summary>
        /// Gets the bytes of <paramref name="value" />, pins them in memory and returns the resulting
        /// <see cref="IReadOnlyPinnedBuffer{T}" />.
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
        protected sealed override IReadOnlyPinnedBuffer<Byte> ConvertValueToBytes(SymmetricKey value, ConcurrencyControlToken controlToken)
        {
            var result = (ReadOnlyPinnedBuffer)null;

            using (var secureBuffer = value.ToBuffer())
            {
                secureBuffer.Access(buffer =>
                {
                    result = new ReadOnlyPinnedBuffer(buffer.ToArray());
                });
            }

            return result;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SymmetricKeySecret" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}