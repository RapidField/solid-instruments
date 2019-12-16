// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Text.RegularExpressions;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Char" /> structure with general purpose features.
    /// </summary>
    public static class CharacterExtensions
    {
        /// <summary>
        /// Determines whether or not the current <see cref="Char" /> represents a Unicode alphabetic character.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Char" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Char" /> represents a Unicode alphabetic character, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsAlphabetic(this Char target) => Char.IsLetter(target);

        /// <summary>
        /// Determines whether or not the current <see cref="Char" /> represents a Unicode basic Latin character (0x00 - 0x7f).
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Char" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Char" /> represents a Unicode basic Latin character, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsBasicLatin(this Char target) => Regex.IsMatch(new String(target, 1), "[\x00-\x7f]");

        /// <summary>
        /// Determines whether or not the current <see cref="Char" /> represents a Unicode control character.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Char" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Char" /> represents a Unicode control character, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsControlCharacter(this Char target) => Char.IsControl(target);

        /// <summary>
        /// Determines whether or not the current <see cref="Char" /> represents a Unicode lowercase alphabetic character.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Char" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Char" /> represents a Unicode lowercase alphabetic character,
        /// otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsLowercaseAlphabetic(this Char target) => Char.IsLower(target);

        /// <summary>
        /// Determines whether or not the current <see cref="Char" /> represents a Unicode numeric character (0 - 9).
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Char" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Char" /> represents a Unicode numeric character, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsNumeric(this Char target) => Char.IsDigit(target);

        /// <summary>
        /// Determines whether or not the current <see cref="Char" /> represents a Unicode symbolic character.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Char" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Char" /> represents a Unicode symbolic character, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsSymbolic(this Char target) => Char.IsSymbol(target);

        /// <summary>
        /// Determines whether or not the current <see cref="Char" /> represents a Unicode uppercase alphabetic character.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Char" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Char" /> represents a Unicode uppercase alphabetic character,
        /// otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsUppercaseAlphabetic(this Char target) => Char.IsUpper(target);

        /// <summary>
        /// Determines whether or not the current <see cref="Char" /> represents a Unicode white space character.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Char" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Char" /> represents a Unicode white space character, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsWhiteSpaceCharacter(this Char target) => Char.IsWhiteSpace(target);

        /// <summary>
        /// Converts the current <see cref="Char" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Char" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Char" />.
        /// </returns>
        public static Byte[] ToByteArray(this Char target) => BitConverter.GetBytes(target);
    }
}