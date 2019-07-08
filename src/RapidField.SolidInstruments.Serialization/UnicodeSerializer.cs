// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Text;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Performs UTF-16 encoding and decoding in place of a serializer.
    /// </summary>
    public sealed class UnicodeSerializer : TextEncodingSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnicodeSerializer" /> class.
        /// </summary>
        public UnicodeSerializer()
            : base(Encoding.Unicode)
        {
            return;
        }
    }
}