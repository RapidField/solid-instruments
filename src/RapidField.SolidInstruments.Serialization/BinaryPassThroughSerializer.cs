// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Stands in place of a serializer when the input and output are both binary arrays.
    /// </summary>
    public class BinaryPassThroughSerializer : DynamicSerializer<Byte[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryPassThroughSerializer" /> class.
        /// </summary>
        public BinaryPassThroughSerializer()
            : base(SerializationFormat.Binary)
        {
            return;
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
        protected override Byte[] DeserializeFromBinary(Byte[] buffer)
        {
            try
            {
                return buffer.RejectIf().IsNull(nameof(buffer));
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during deserialization. See inner exception.", exception);
            }
        }

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
        protected override Byte[] SerializeToBinary(Byte[] target)
        {
            try
            {
                return target.RejectIf().IsNull(nameof(target));
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during serialization. See inner exception.", exception);
            }
        }
    }
}