// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange
{
    /// <summary>
    /// Provides facilities for exchanging symmetric keys using asymmetric key pairs.
    /// </summary>
    /// <remarks>
    /// <see cref="KeyExchangeProcessor" /> is the default implementation of <see cref="IKeyExchangeProcessor" />.
    /// </remarks>
    public sealed class KeyExchangeProcessor : AsymmetricProcessor, IKeyExchangeProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricProcessor" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate initialization vectors.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        public KeyExchangeProcessor(RandomNumberGenerator randomnessProvider)
            : base(randomnessProvider)
        {
            return;
        }

        /// <summary>
        /// Represents a singleton instance of the <see cref="KeyExchangeProcessor" /> class.
        /// </summary>
        public static readonly IKeyExchangeProcessor Instance = new KeyExchangeProcessor(HardenedRandomNumberGenerator.Instance);
    }
}