// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a named secret <see cref="Double" /> value that is pinned in memory and encrypted at rest.
    /// </summary>
    public sealed class NumericSecret : Secret<Double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericSecret" /> class.
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
        public NumericSecret(String name)
            : base(name)
        {
            return;
        }

        /// <summary>
        /// Creates a new <see cref="NumericSecret" /> using the specified name and value.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies the secret.
        /// </param>
        /// <param name="value">
        /// The secret value.
        /// </param>
        /// <returns>
        /// A new <see cref="NumericSecret" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        public static NumericSecret FromValue(String name, Double value)
        {
            var secret = new NumericSecret(name);
            secret.Write(() => value);
            return secret;
        }

        /// <summary>
        /// Creates a <see cref="Double" /> using the provided bytes.
        /// </summary>
        /// <param name="bytes">
        /// Pinned memory representing a <see cref="Double" />.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Guid" />.
        /// </returns>
        protected sealed override Double ConvertBytesToValue(IReadOnlyPinnedMemory<Byte> bytes, IConcurrencyControlToken controlToken) => BitConverter.ToDouble(bytes.ReadOnlySpan);

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
        /// <paramref name="value" /> as pinned memory.
        /// </returns>
        protected sealed override IReadOnlyPinnedMemory<Byte> ConvertValueToBytes(Double value, IConcurrencyControlToken controlToken) => new PinnedMemory(value.ToByteArray(), true);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="NumericSecret" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}