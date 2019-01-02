// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.TextEncoding
{
    /// <summary>
    /// Represents z-base-32 character encoding.
    /// </summary>
    public class ZBase32Encoding : Base32Encoding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZBase32Encoding" /> class.
        /// </summary>
        public ZBase32Encoding()
            : base(PermutedAlphabet)
        {
            return;
        }

        /// <summary>
        /// Represents the default z-base-32 encoding alphabet.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PermutedAlphabet = "ybndrfg8ejkmcpqxot1uwisza345h769";
    }
}