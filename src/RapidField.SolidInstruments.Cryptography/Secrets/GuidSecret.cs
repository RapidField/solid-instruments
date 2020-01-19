// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a named secret <see cref="Guid" /> value that is pinned in memory and encrypted at rest.
    /// </summary>

    public class GuidSecret : Secret<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidSecret" /> class.
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
        public GuidSecret(String name)
            : base(name)
        {
            return;
        }

        /// <summary>
        /// Creates a <see cref="Guid" /> using the provided bytes.
        /// </summary>
        /// <param name="bytes">
        /// A pinned buffer representing a <see cref="Guid" />.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Guid" />.
        /// </returns>
        protected sealed override Guid ConvertBytesToValue(IReadOnlyPinnedBuffer<Byte> bytes, ConcurrencyControlToken controlToken) => new Guid(bytes.ReadOnlySpan);

        /// <summary>
        /// Gets the bytes of <paramref name="value" />, pins them in memory and returns the resulting
        /// <see cref="IPinnedBuffer{T}" />.
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
        protected sealed override IPinnedBuffer<Byte> ConvertValueToBytes(Guid value, ConcurrencyControlToken controlToken) => new PinnedBuffer(value.ToByteArray());

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Secret{TValue}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}