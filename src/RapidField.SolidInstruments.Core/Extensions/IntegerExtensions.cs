// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Int16" />, <see cref="Int32" />, <see cref="Int64" />, <see cref="UInt16" />, <see cref="UInt32" />
    /// and <see cref="UInt64" /> structures with general purpose features.
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Converts the current <see cref="Int16" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Int16" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Int16" />.
        /// </returns>
        public static Byte[] ToByteArray(this Int16 target) => BitConverter.GetBytes(target);

        /// <summary>
        /// Converts the current <see cref="Int32" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Int32" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Int32" />.
        /// </returns>
        public static Byte[] ToByteArray(this Int32 target) => BitConverter.GetBytes(target);

        /// <summary>
        /// Converts the current <see cref="Int64" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Int64" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Int64" />.
        /// </returns>
        public static Byte[] ToByteArray(this Int64 target) => BitConverter.GetBytes(target);

        /// <summary>
        /// Converts the current <see cref="UInt16" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="UInt16" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="UInt16" />.
        /// </returns>
        public static Byte[] ToByteArray(this UInt16 target) => BitConverter.GetBytes(target);

        /// <summary>
        /// Converts the current <see cref="UInt32" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="UInt32" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="UInt32" />.
        /// </returns>
        public static Byte[] ToByteArray(this UInt32 target) => BitConverter.GetBytes(target);

        /// <summary>
        /// Converts the current <see cref="UInt64" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="UInt64" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="UInt64" />.
        /// </returns>
        public static Byte[] ToByteArray(this UInt64 target) => BitConverter.GetBytes(target);
    }
}