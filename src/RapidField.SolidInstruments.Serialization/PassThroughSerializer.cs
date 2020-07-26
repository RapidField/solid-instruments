// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Stands in place of a serializer when the input and output are both unformatted byte arrays.
    /// </summary>
    public sealed class PassThroughSerializer : DynamicSerializer<Byte[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PassThroughSerializer" /> class.
        /// </summary>
        public PassThroughSerializer()
            : base(SerializationFormat.Binary)
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
        protected sealed override Byte[] Deserialize(Byte[] serializedObject, SerializationFormat format) => serializedObject;

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
        protected sealed override Byte[] Serialize(Byte[] target, SerializationFormat format) => target;
    }
}