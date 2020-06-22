// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Provides facilities for performing asymmetric key operations.
    /// </summary>
    /// <remarks>
    /// <see cref="AsymmetricProcessor" /> is the default implementation of <see cref="IAsymmetricProcessor" />.
    /// </remarks>
    public abstract class AsymmetricProcessor : IAsymmetricProcessor
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
        protected AsymmetricProcessor(RandomNumberGenerator randomnessProvider)
        {
            RandomnessProvider = randomnessProvider.RejectIf().IsNull(nameof(randomnessProvider));
        }

        /// <summary>
        /// Represents a random number generator that is used to generate initialization vectors.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly RandomNumberGenerator RandomnessProvider;
    }
}