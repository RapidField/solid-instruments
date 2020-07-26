// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Serialization;
using System;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Hashing
{
    /// <summary>
    /// Provides facilities for hashing Unicode strings.
    /// </summary>
    public sealed class HashingStringProcessor : HashingProcessor<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashingStringProcessor" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate salt values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        public HashingStringProcessor(RandomNumberGenerator randomnessProvider)
            : base(randomnessProvider, new UnicodeSerializer())
        {
            return;
        }

        /// <summary>
        /// Represents a singleton instance of the <see cref="HashingStringProcessor" /> class.
        /// </summary>
        public static readonly IHashingProcessor<String> Instance = new HashingStringProcessor(HardenedRandomNumberGenerator.Instance);
    }
}