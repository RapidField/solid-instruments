// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Provides facilities for performing asymmetric key operations upon byte arrays.
    /// </summary>
    /// <remarks>
    /// <see cref="AsymmetricProcessor" /> is the default implementation of <see cref="IAsymmetricProcessor" />.
    /// </remarks>
    public abstract class AsymmetricProcessor : AsymmetricProcessor<Byte[]>, IAsymmetricProcessor
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
            : base(randomnessProvider, new PassThroughSerializer())
        {
            return;
        }
    }

    /// <summary>
    /// Provides facilities for performing asymmetric key operations upon typed objects.
    /// </summary>
    /// <remarks>
    /// <see cref="AsymmetricProcessor{T}" /> is the default implementation of <see cref="IAsymmetricProcessor{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the object upon which operations are performed.
    /// </typeparam>
    public abstract class AsymmetricProcessor<T> : CryptographicProcessor<T>, IAsymmetricProcessor<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricProcessor{T}" /> class.
        /// </summary>
        [DebuggerHidden]
        internal AsymmetricProcessor()
            : this(HardenedRandomNumberGenerator.Instance)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricProcessor{T}" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate initialization vectors.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        protected AsymmetricProcessor(RandomNumberGenerator randomnessProvider)
            : base(randomnessProvider)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricProcessor{T}" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate initialization vectors.
        /// </param>
        /// <param name="serializer">
        /// A serializer that is used to transform plaintext.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" /> -or- <paramref name="serializer" /> is
        /// <see langword="null" />.
        /// </exception>
        protected AsymmetricProcessor(RandomNumberGenerator randomnessProvider, ISerializer<T> serializer)
            : base(randomnessProvider, serializer)
        {
            return;
        }
    }
}