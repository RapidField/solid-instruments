// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.CompilerServices;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="BitShiftDirection" /> enumeration with general purpose features.
    /// </summary>
    public static class BitShiftDirectionExtensions
    {
        /// <summary>
        /// Converts the current <see cref="BitShiftDirection" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BitShiftDirection" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="BitShiftDirection" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte[] ToByteArray(this BitShiftDirection target) => BitConverter.GetBytes((Int32)target);
    }
}