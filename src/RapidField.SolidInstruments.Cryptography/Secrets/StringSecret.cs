// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;
using System.Text;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a named secret <see cref="String" /> value that is pinned in memory and encrypted at rest.
    /// </summary>
    public class StringSecret : Secret<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSecret" /> class.
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
        public StringSecret(String name)
            : this(name, DefaultEncoding)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSecret" /> class.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies the secret.
        /// </param>
        /// <param name="encoding">
        /// The encoding that is use when converting the secret string to and from bytes. The default value is
        /// <see cref="Encoding.Unicode" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="encoding" /> is <see langword="null" />.
        /// </exception>
        protected StringSecret(String name, Encoding encoding)
            : base(name)
        {
            Encoding = encoding.RejectIf().IsNull(nameof(encoding));
        }

        /// <summary>
        /// Creates a new <see cref="StringSecret" /> using the specified name and value.
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
        public static StringSecret FromValue(String name, String value)
        {
            value = value.RejectIf().IsNull(nameof(value));
            var secret = new StringSecret(name);
            secret.Write(() => value);
            return secret;
        }

        /// <summary>
        /// Creates a <see cref="String" /> using the provided bytes.
        /// </summary>
        /// <param name="bytes">
        /// Pinned memory representing a <see cref="String" />.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The resulting <see cref="String" />.
        /// </returns>
        protected sealed override String ConvertBytesToValue(IReadOnlyPinnedMemory<Byte> bytes, IConcurrencyControlToken controlToken) => Encoding.GetString(bytes.ReadOnlySpan);

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
        /// <exception cref="FormatException">
        /// <paramref name="value" /> does not conform to the specified encoding.
        /// </exception>
        protected sealed override IReadOnlyPinnedMemory<Byte> ConvertValueToBytes(String value, IConcurrencyControlToken controlToken)
        {
            try
            {
                return new PinnedMemory(Encoding.GetBytes(value), true);
            }
            catch (EncoderFallbackException exception)
            {
                throw new FormatException($"The specified secret string value contains characters that are invalid for the encoding format: {Encoding.EncodingName}", exception);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="StringSecret" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the encoding that is use when converting the secret string to and from bytes.
        /// </summary>
        protected Encoding Encoding
        {
            get;
        }

        /// <summary>
        /// Gets the default encoding that is use when converting the secret string to and from bytes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Encoding DefaultEncoding => Encoding.Unicode;
    }
}