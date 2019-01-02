// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Specifies a format to use for serialization and deserialization.
    /// </summary>
    public enum SerializationFormat : Int32
    {
        /// <summary>
        /// The serialization format is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Specifies binary serialization.
        /// </summary>
        Binary = 1,

        /// <summary>
        /// Specifies JSON serialization.
        /// </summary>
        Json = 2,

        /// <summary>
        /// Specifies XML serialization.
        /// </summary>
        Xml = 3
    }
}