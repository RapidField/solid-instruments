// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring.Extensions
{
    /// <summary>
    /// Extends the <see cref="ApplicationEventCategory" /> enumeration with general purpose features.
    /// </summary>
    public static class ApplicationEventCategoryExtensions
    {
        /// <summary>
        /// Converts the current <see cref="ApplicationEventCategory" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="ApplicationEventCategory" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="ApplicationEventCategory" />.
        /// </returns>
        public static Byte[] ToByteArray(this ApplicationEventCategory target) => BitConverter.GetBytes((Int32)target);
    }
}