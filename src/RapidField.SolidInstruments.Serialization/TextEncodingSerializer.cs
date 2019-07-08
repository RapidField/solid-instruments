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
    /// Supports text encoding and decoding in place of a serializer.
    /// </summary>
    public abstract class TextEncodingSerializer : Serializer<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextEncodingSerializer" /> class.
        /// </summary>
        [DebuggerHidden]
        internal TextEncodingSerializer()
            : base()
        {
            CharacterEncoding = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEncodingSerializer" /> class.
        /// </summary>
        /// <param name="characterEncoding">
        /// The character encoding that is used for serialization and deserialization.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="characterEncoding" /> is <see langword="null" />.
        /// </exception>
        protected TextEncodingSerializer(Encoding characterEncoding)
            : base()
        {
            CharacterEncoding = characterEncoding.RejectIf().IsNull(nameof(characterEncoding));
        }

        /// <summary>
        /// Converts the specified buffer to its typed equivalent.
        /// </summary>
        /// <param name="buffer">
        /// A serialized object.
        /// </param>
        /// <param name="format">
        /// The format to use for deserialization.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="buffer" /> is invalid or an error occurred during deserialization.
        /// </exception>
        protected override String Deserialize(Byte[] buffer, SerializationFormat format)
        {
            try
            {
                return Deserialize(buffer, CharacterEncoding);
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during deserialization. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Converts the specified object to a buffer.
        /// </summary>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <param name="format">
        /// The format to use for serialization.
        /// </param>
        /// <returns>
        /// The serialized buffer.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        protected override Byte[] Serialize(String target, SerializationFormat format)
        {
            try
            {
                return Serialize(target, CharacterEncoding);
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during serialization. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Converts the specified buffer to its typed equivalent.
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
        [DebuggerHidden]
        private static String Deserialize(Byte[] buffer, Encoding characterEncoding) => characterEncoding.GetString(buffer);

        /// <summary>
        /// Converts the specified object to a buffer.
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
        [DebuggerHidden]
        private static Byte[] Serialize(String target, Encoding characterEncoding) => characterEncoding.GetBytes(target);

        /// <summary>
        /// Represents the character encoding that is used for serialization and deserialization.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Encoding CharacterEncoding;
    }
}