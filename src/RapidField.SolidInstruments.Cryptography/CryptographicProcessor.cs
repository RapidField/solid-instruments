// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Provides facilities for performing cryptographic operations upon typed objects.
    /// </summary>
    /// <remarks>
    /// <see cref="CryptographicProcessor{T}" /> is the default implementation of <see cref="ICryptographicProcessor{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the object upon which cryptographic operations are performed.
    /// </typeparam>
    public abstract class CryptographicProcessor<T> : CryptographicProcessor, ICryptographicProcessor<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographicProcessor{T}" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate initialization vectors.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        protected CryptographicProcessor(RandomNumberGenerator randomnessProvider)
            : this(randomnessProvider, DefaultSerializer)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographicProcessor{T}" /> class.
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
        protected CryptographicProcessor(RandomNumberGenerator randomnessProvider, ISerializer<T> serializer)
            : base(randomnessProvider)
        {
            Serializer = serializer.RejectIf().IsNull(nameof(serializer)).TargetArgument;
        }

        /// <summary>
        /// Gets the type of the object upon which cryptographic operations are performed.
        /// </summary>
        public Type TargetObjectType => typeof(T);

        /// <summary>
        /// Represents the default serializer that is used to transform plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly ISerializer<T> DefaultSerializer = new CompressedJsonSerializer<T>();

        /// <summary>
        /// Represents a serializer that is used to transform plaintext.
        /// </summary>
        protected readonly ISerializer<T> Serializer;
    }

    /// <summary>
    /// Provides facilities for performing cryptographic operations upon typed objects.
    /// </summary>
    /// <remarks>
    /// <see cref="CryptographicProcessor" /> is the default implementation of <see cref="ICryptographicProcessor" />.
    /// </remarks>
    public abstract class CryptographicProcessor : ICryptographicProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographicProcessor" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate initialization vectors.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        protected CryptographicProcessor(RandomNumberGenerator randomnessProvider)
        {
            RandomnessProvider = randomnessProvider.RejectIf().IsNull(nameof(randomnessProvider));
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="CryptographicProcessor" /> can be used to digitally sign
        /// information using asymmetric key cryptography.
        /// </summary>
        public Boolean SupportsDigitalSignature => Usage.HasFlag(CryptographicComponentUsage.DigitalSignature);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="CryptographicProcessor" /> can be used to produce hash
        /// values for plaintext information.
        /// </summary>
        public Boolean SupportsHashing => Usage.HasFlag(CryptographicComponentUsage.Hashing);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="CryptographicProcessor" /> can be used to securely
        /// exchange symmetric keys with remote parties.
        /// </summary>
        public Boolean SupportsKeyExchange => Usage.HasFlag(CryptographicComponentUsage.KeyExchange);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="CryptographicProcessor" /> can be used to encrypt or
        /// decrypt information using symmetric key cryptography.
        /// </summary>
        public Boolean SupportsSymmetricKeyEncryption => Usage.HasFlag(CryptographicComponentUsage.SymmetricKeyEncryption);

        /// <summary>
        /// Gets a value specifying the valid purposes and uses of the current <see cref="CryptographicProcessor" />.
        /// </summary>
        public abstract CryptographicComponentUsage Usage
        {
            get;
        }

        /// <summary>
        /// Represents a random number generator that is used to generate initialization vectors.
        /// </summary>
        protected readonly RandomNumberGenerator RandomnessProvider;
    }
}