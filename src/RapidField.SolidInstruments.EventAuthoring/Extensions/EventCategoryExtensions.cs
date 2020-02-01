// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring.Extensions
{
    /// <summary>
    /// Extends the <see cref="EventCategory" /> enumeration with general purpose features.
    /// </summary>
    public static class EventCategoryExtensions
    {
        /// <summary>
        /// Converts the current <see cref="EventCategory" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="EventCategory" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="EventCategory" />.
        /// </returns>
        public static Byte[] ToByteArray(this EventCategory target) => BitConverter.GetBytes((Int32)target);
    }
}