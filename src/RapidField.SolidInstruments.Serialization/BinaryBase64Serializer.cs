// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Text;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Performs Base64 string-to-binary serialization and deserialization.
    /// </summary>
    public sealed class BinaryBase64Serializer : BinaryStringSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryBase64Serializer" /> class.
        /// </summary>
        public BinaryBase64Serializer()
            : base()
        {
            return;
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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer" /> is invalid.
        /// </exception>
        protected sealed override String DeserializeFromBinary(Byte[] buffer, Encoding characterEncoding) => Convert.ToBase64String(buffer);

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
        /// <exception cref="FormatException">
        /// <paramref name="target" /> is not a valid Base64 string.
        /// </exception>
        protected sealed override Byte[] SerializeToBinary(String target, Encoding characterEncoding) => Convert.FromBase64String(target);
    }
}