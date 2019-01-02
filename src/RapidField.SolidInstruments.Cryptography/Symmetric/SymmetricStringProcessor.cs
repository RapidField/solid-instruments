// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Serialization;
using System;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Provides facilities for encrypting and decrypting Unicode strings.
    /// </summary>
    public sealed class SymmetricStringProcessor : SymmetricProcessor<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricStringProcessor" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate initialization vectors.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        public SymmetricStringProcessor(RandomNumberGenerator randomnessProvider)
            : base(randomnessProvider, new BinaryUnicodeSerializer())
        {
            return;
        }
    }
}