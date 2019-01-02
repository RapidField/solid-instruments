// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Supports string-to-binary serialization and deserialization.
    /// </summary>
    public abstract class BinaryStringSerializer : BinarySerializer<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStringSerializer" /> class.
        /// </summary>
        [DebuggerHidden]
        internal BinaryStringSerializer()
            : base()
        {
            CharacterEncoding = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStringSerializer" /> class.
        /// </summary>
        /// <param name="characterEncoding">
        /// The character encoding that is used for serialization and deserialization.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="characterEncoding" /> is <see langword="null" />.
        /// </exception>
        protected BinaryStringSerializer(Encoding characterEncoding)
            : base()
        {
            CharacterEncoding = characterEncoding.RejectIf().IsNull(nameof(characterEncoding));
        }

        /// <summary>
        /// Converts the specified binary buffer to its typed equivalent.
        /// </summary>
        /// <param name="buffer">
        /// A serialized object.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="buffer" /> is invalid or an error occurred during deserialization.
        /// </exception>
        protected sealed override String DeserializeFromBinary(Byte[] buffer)
        {
            try
            {
                return DeserializeFromBinary(buffer, CharacterEncoding);
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during deserialization. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Converts the specified binary buffer to its typed equivalent.
        /// </summary>
        /// <param name="buffer">
        /// A serialized object.
        /// </param>
        /// <param name="characterEncoding">
        /// The character encoding that is used for deserialization.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="buffer" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer" /> is invalid.
        /// </exception>
        /// <exception cref="DecoderFallbackException">
        /// A fallback occurred.
        /// </exception>
        protected virtual String DeserializeFromBinary(Byte[] buffer, Encoding characterEncoding) => characterEncoding.GetString(buffer);

        /// <summary>
        /// Converts the specified object to a binary buffer.
        /// </summary>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <returns>
        /// The serialized buffer.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        protected sealed override Byte[] SerializeToBinary(String target)
        {
            try
            {
                return SerializeToBinary(target, CharacterEncoding);
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during serialization. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Converts the specified object to a binary buffer.
        /// </summary>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <param name="characterEncoding">
        /// The character encoding that is used for serialization.
        /// </param>
        /// <returns>
        /// The serialized output.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="EncoderFallbackException">
        /// A fallback occurred.
        /// </exception>
        protected virtual Byte[] SerializeToBinary(String target, Encoding characterEncoding) => characterEncoding.GetBytes(target);

        /// <summary>
        /// Represents the character encoding that is used for serialization and deserialization.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Encoding CharacterEncoding;
    }
}