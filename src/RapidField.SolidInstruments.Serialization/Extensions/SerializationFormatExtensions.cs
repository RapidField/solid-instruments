// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Serialization.Extensions
{
    /// <summary>
    /// Extends the <see cref="SerializationFormat" /> enumeration with general purpose features.
    /// </summary>
    public static class SerializationFormatExtensions
    {
        /// <summary>
        /// Converts the current <see cref="SerializationFormat" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SerializationFormat" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="SerializationFormat" />.
        /// </returns>
        public static Byte[] ToByteArray(this SerializationFormat target) => BitConverter.GetBytes((Int32)target);

        /// <summary>
        /// Returns the MIME media type associated with the current <see cref="SerializationFormat" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SerializationFormat" />.
        /// </param>
        /// <returns>
        /// The MIME media type associated with the current <see cref="SerializationFormat" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="target" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        public static String ToMimeMediaType(this SerializationFormat target)
        {
            switch (target.RejectIf().IsEqualToValue(SerializationFormat.Unspecified, nameof(target)).TargetArgument)
            {
                case SerializationFormat.Binary:

                    return MimeMediaTypeForBinary;

                case SerializationFormat.CompressedJson:

                    return MimeMediaTypeForBinary;

                case SerializationFormat.CompressedXml:

                    return MimeMediaTypeForBinary;

                case SerializationFormat.Json:

                    return MimeMediaTypeForJson;

                case SerializationFormat.Xml:

                    return MimeMediaTypeForXml;

                default:

                    throw new UnsupportedSpecificationException($"The specified serialization type, {target}, is not supported.");
            }
        }

        /// <summary>
        /// Represents the MIME media type for binary content.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String MimeMediaTypeForBinary = "application/octet-stream";

        /// <summary>
        /// Represents the MIME media type for JSON content.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String MimeMediaTypeForJson = "application/json";

        /// <summary>
        /// Represents the MIME media type for XML content.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String MimeMediaTypeForXml = "application/xml";
    }
}