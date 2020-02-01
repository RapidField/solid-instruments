// =================================================================================================================================
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
    /// Represents a named secret <see cref="CascadingSymmetricKey" /> value that is pinned in memory and encrypted at rest.
    /// </summary>
    public sealed class CascadingSymmetricKeySecret : Secret<CascadingSymmetricKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascadingSymmetricKeySecret" /> class.
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
        public CascadingSymmetricKeySecret(String name)
            : base(name)
        {
            return;
        }

        /// <summary>
        /// Creates a new <see cref="CascadingSymmetricKeySecret" /> using the specified name and value.
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
        public static CascadingSymmetricKeySecret FromValue(String name, CascadingSymmetricKey value)
        {
            value = value.RejectIf().IsNull(nameof(value)).TargetArgument;
            var secret = new CascadingSymmetricKeySecret(name);
            secret.Write(() => value);
            return secret;
        }

        /// <summary>
        /// Creates a <see cref="CascadingSymmetricKey" /> using the provided bytes.
        /// </summary>
        /// <param name="bytes">
        /// A pinned buffer representing a <see cref="CascadingSymmetricKey" />.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The resulting <see cref="CascadingSymmetricKey" />.
        /// </returns>
        protected sealed override CascadingSymmetricKey ConvertBytesToValue(IReadOnlyPinnedBuffer<Byte> bytes, ConcurrencyControlToken controlToken)
        {
            var result = (CascadingSymmetricKey)null;

            using (var secureBuffer = new SecureBuffer(bytes.Length))
            {
                secureBuffer.Access(buffer =>
                {
                    bytes.ReadOnlySpan.CopyTo(buffer);
                });

                result = CascadingSymmetricKey.FromBuffer(secureBuffer);
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
        protected sealed override IReadOnlyPinnedBuffer<Byte> ConvertValueToBytes(CascadingSymmetricKey value, ConcurrencyControlToken controlToken)
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
        /// Releases all resources consumed by the current <see cref="CascadingSymmetricKeySecret" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}