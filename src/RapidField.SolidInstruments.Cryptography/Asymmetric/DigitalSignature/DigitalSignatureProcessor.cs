// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature
{
    /// <summary>
    /// Provides facilities for digitally signing byte arrays.
    /// </summary>
    /// <remarks>
    /// <see cref="DigitalSignatureProcessor" /> is the default implementation of <see cref="IDigitalSignatureProcessor" />.
    /// </remarks>
    public class DigitalSignatureProcessor : DigitalSignatureProcessor<Byte[]>, IDigitalSignatureProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSignatureProcessor" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate initialization vectors.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        public DigitalSignatureProcessor(RandomNumberGenerator randomnessProvider)
            : base(randomnessProvider, new PassThroughSerializer())
        {
            return;
        }

        /// <summary>
        /// Creates a new <see cref="IDigitalSignatureProcessor{T}" /> for the specified serializable type.
        /// </summary>
        /// <typeparam name="T">
        /// The serializable object type that the processor can encrypt or decrypt.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="IDigitalSignatureProcessor{T}" /> for the specified serializable type.
        /// </returns>
        public static IDigitalSignatureProcessor<T> ForType<T>()
            where T : class => new DigitalSignatureProcessor<T>();

        /// <summary>
        /// Represents a singleton instance of the <see cref="DigitalSignatureProcessor" /> class.
        /// </summary>
        public static readonly IDigitalSignatureProcessor Instance = new DigitalSignatureProcessor(HardenedRandomNumberGenerator.Instance);
    }

    /// <summary>
    /// Provides facilities for digitally signing typed objects.
    /// </summary>
    /// <remarks>
    /// <see cref="DigitalSignatureProcessor{T}" /> is the default implementation of <see cref="IDigitalSignatureProcessor{T}" />.
    /// </remarks>
    public class DigitalSignatureProcessor<T> : AsymmetricProcessor, IDigitalSignatureProcessor<T>
        where T : class
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
        public DigitalSignatureProcessor(RandomNumberGenerator randomnessProvider)
            : this(randomnessProvider, DefaultSerializer)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricProcessor" /> class.
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
        public DigitalSignatureProcessor(RandomNumberGenerator randomnessProvider, ISerializer<T> serializer)
            : base(randomnessProvider)
        {
            Serializer = serializer.RejectIf().IsNull(nameof(serializer)).TargetArgument;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSignatureProcessor{T}" /> class.
        /// </summary>
        [DebuggerHidden]
        internal DigitalSignatureProcessor()
            : this(HardenedRandomNumberGenerator.Instance)
        {
            return;
        }

        /// <summary>
        /// Represents the default serializer that is used to transform plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly ISerializer<T> DefaultSerializer = new CompressedJsonSerializer<T>();

        /// <summary>
        /// Represents a serializer that is used to transform plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ISerializer<T> Serializer;
    }
}