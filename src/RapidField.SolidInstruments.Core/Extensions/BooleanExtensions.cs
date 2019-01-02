// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Boolean" /> structure with general purpose features.
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// Converts the current <see cref="Boolean" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Boolean" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Boolean" />.
        /// </returns>
        public static Byte[] ToByteArray(this Boolean target) => BitConverter.GetBytes(target);

        /// <summary>
        /// Converts the value of the current <see cref="Boolean" /> to its equivalent serialized string representation.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Boolean" />.
        /// </param>
        /// <returns>
        /// A serialized string representation of the current <see cref="Boolean" />.
        /// </returns>
        public static String ToSerializedString(this Boolean target) => target ? "true" : "false";
    }
}