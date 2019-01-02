// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Text;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Performs UTF-16 string-to-binary serialization and deserialization.
    /// </summary>
    public sealed class BinaryUnicodeSerializer : BinaryStringSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryUnicodeSerializer" /> class.
        /// </summary>
        public BinaryUnicodeSerializer()
            : base(Encoding.Unicode)
        {
            return;
        }
    }
}