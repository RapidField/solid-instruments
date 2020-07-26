// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Performs Base64 encoding and decoding in place of a serializer.
    /// </summary>
    public sealed class Base64Serializer : TextEncodingSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Serializer" /> class.
        /// </summary>
        public Base64Serializer()
            : base()
        {
            return;
        }

        /// <summary>
        /// Converts the specified bit field to its typed equivalent.
        /// </summary>
        /// <param name="serializedObject">
        /// A serialized object.
        /// </param>
        /// <param name="format">
        /// The format to use for deserialization.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="serializedObject" /> is invalid or an error occurred during deserialization.
        /// </exception>
        protected sealed override String Deserialize(Byte[] serializedObject, SerializationFormat format)
        {
            try
            {
                return Convert.ToBase64String(serializedObject);
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during deserialization. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Converts the specified object to a bit field.
        /// </summary>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <param name="format">
        /// The format to use for serialization.
        /// </param>
        /// <returns>
        /// The serialized bit field.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        protected sealed override Byte[] Serialize(String target, SerializationFormat format)
        {
            try
            {
                return Convert.FromBase64String(target);
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during serialization. See inner exception.", exception);
            }
        }
    }
}