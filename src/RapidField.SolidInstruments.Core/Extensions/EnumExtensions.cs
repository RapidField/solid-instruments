// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends <see cref="Enum" /> types with general purpose features.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts the value of the current <see cref="Enum" /> to its equivalent integer and textual name pair.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Enum" />.
        /// </param>
        /// <returns>
        /// An integer and textual name pair representation of the current <see cref="Enum" />.
        /// </returns>
        public static KeyValuePair<Int32, String> ToKeyValuePair<T>(this T target)
            where T : Enum => new(Convert.ToInt32(target), Enum.GetName(typeof(T), target));
    }
}