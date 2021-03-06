﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Performs compressed XML serialization and deserialization for a given type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the serializable object.
    /// </typeparam>
    public sealed class CompressedXmlSerializer<T> : DynamicSerializer<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressedXmlSerializer{T}" /> class.
        /// </summary>
        public CompressedXmlSerializer()
            : base(SerializationFormat.CompressedXml)
        {
            return;
        }
    }
}