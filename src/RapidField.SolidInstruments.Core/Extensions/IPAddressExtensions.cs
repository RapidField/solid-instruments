// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Net;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="IPAddress" /> class with general purpose features.
    /// </summary>
    public static class IPAddressExtensions
    {
        /// <summary>
        /// Converts the current <see cref="IPAddress" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="IPAddress" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="IPAddress" />.
        /// </returns>
        public static Byte[] ToByteArray(this IPAddress target) => target.GetAddressBytes();
    }
}