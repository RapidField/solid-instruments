// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Specifies a format to use for serialization and deserialization.
    /// </summary>
    [DataContract]
    public enum SerializationFormat : Int32
    {
        /// <summary>
        /// The serialization format is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// Specifies no serialization format or, in some contexts, the default format.
        /// </summary>
        [EnumMember]
        Binary = 1,

        /// <summary>
        /// Specifies the compressed JSON serialization format.
        /// </summary>
        [EnumMember]
        CompressedJson = 2,

        /// <summary>
        /// Specifies the compressed XML serialization format.
        /// </summary>
        [EnumMember]
        CompressedXml = 3,

        /// <summary>
        /// Specifies the JSON serialization format.
        /// </summary>
        [EnumMember]
        Json = 4,

        /// <summary>
        /// Specifies the XML serialization format.
        /// </summary>
        [EnumMember]
        Xml = 5
    }
}