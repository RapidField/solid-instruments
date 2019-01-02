// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Hashing
{
    /// <summary>
    /// Provides facilities for hashing typed objects and binary arrays.
    /// </summary>
    /// <remarks>
    /// <see cref="HashingProcessor{T}" /> is the default implementation of <see cref="IHashingProcessor{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the object that can be hashed.
    /// </typeparam>
    public class HashingProcessor<T> : IHashingProcessor<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashingProcessor{T}" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate salt values.
        /// </param>
        /// <param name="binarySerializer">
        /// A binary serializer that is used to transform plaintext.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" /> -or- <paramref name="binarySerializer" /> is
        /// <see langword="null" />.
        /// </exception>
        public HashingProcessor(RandomNumberGenerator randomnessProvider, ISerializer<T> binarySerializer)
            : this(randomnessProvider, binarySerializer, DefaultSaltLengthInBytes)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashingProcessor{T}" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate salt values.
        /// </param>
        /// <param name="binarySerializer">
        /// A binary serializer that is used to transform plaintext.
        /// </param>
        /// <param name="saltLengthInBytes">
        /// The salt length, in bytes, to use when calculating and evaluating hash values. The default value is 8.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" /> -or- <paramref name="binarySerializer" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="saltLengthInBytes" /> is less than one.
        /// </exception>
        public HashingProcessor(RandomNumberGenerator randomnessProvider, ISerializer<T> binarySerializer, Int32 saltLengthInBytes)
        {
            BinarySerializer = binarySerializer.RejectIf().IsNull(nameof(binarySerializer)).TargetArgument;
            RandomnessProvider = randomnessProvider.RejectIf().IsNull(nameof(randomnessProvider));
            SaltLengthInBytes = saltLengthInBytes.RejectIf().IsLessThan(1, nameof(saltLengthInBytes));
        }

        /// <summary>
        /// Calculates a hash value for the specified plaintext binary array.
        /// </summary>
        /// <param name="plaintextBinaryArray">
        /// The plaintext binary array to hash.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the plaintext.
        /// </param>
        /// <returns>
        /// The resulting hash value.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public Byte[] CalculateHash(Byte[] plaintextBinaryArray, HashingAlgorithmSpecification algorithm) => CalculateHash(plaintextBinaryArray, algorithm, null);

        /// <summary>
        /// Calculates a hash value for the specified plaintext binary array.
        /// </summary>
        /// <param name="plaintextBinaryArray">
        /// The plaintext binary array to hash.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the plaintext.
        /// </param>
        /// <param name="salt">
        /// The salt to apply to the plaintext, or <see langword="null" /> if the plaintext is unsalted. The default value is
        /// <see langword="null" />.
        /// </param>
        /// <returns>
        /// The resulting hash value.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public Byte[] CalculateHash(Byte[] plaintextBinaryArray, HashingAlgorithmSpecification algorithm, Byte[] salt)
        {
            try
            {
                var applySalt = (salt is null == false);
                var saltLengthInBytes = (applySalt ? salt.Length : 0);
                var plaintextLengthInBytes = plaintextBinaryArray.Length;
                var plaintextBufferLengthInBytes = (plaintextLengthInBytes + saltLengthInBytes);
                var digestLengthInBytes = (algorithm.ToDigestBitLength() / 8);
                var hashLengthInBytes = (digestLengthInBytes + saltLengthInBytes);
                var hashValue = new Byte[hashLengthInBytes];
                Byte[] plaintextBuffer;

                if (applySalt)
                {
                    plaintextBuffer = new Byte[plaintextBufferLengthInBytes];
                    Array.Copy(plaintextBinaryArray, plaintextBuffer, plaintextLengthInBytes);
                    Array.Copy(salt, 0, plaintextBuffer, plaintextLengthInBytes, saltLengthInBytes);
                }
                else
                {
                    plaintextBuffer = plaintextBinaryArray;
                }

                using (var hashAlgorithm = algorithm.ToHashAlgorithm())
                {
                    Array.Copy(hashAlgorithm.ComputeHash(plaintextBinaryArray), hashValue, digestLengthInBytes);
                }

                if (applySalt)
                {
                    Array.Copy(salt, 0, hashValue, digestLengthInBytes, saltLengthInBytes);
                }

                return hashValue;
            }
            catch
            {
                throw new SecurityException("The hashing operation failed.");
            }
        }

        /// <summary>
        /// Calculates a hash value for the specified plaintext object.
        /// </summary>
        /// <param name="plaintextObject">
        /// The plaintext object to hash.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the plaintext.
        /// </param>
        /// <param name="saltingMode">
        /// A value specifying whether or not salt is applied to the plaintext.
        /// </param>
        /// <returns>
        /// The resulting hash value.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public Byte[] CalculateHash(T plaintextObject, HashingAlgorithmSpecification algorithm, SaltingMode saltingMode)
        {
            try
            {
                switch (saltingMode)
                {
                    case SaltingMode.Salted:

                        var salt = new Byte[SaltLengthInBytes];
                        RandomnessProvider.GetBytes(salt);
                        return CalculateHash(plaintextObject, algorithm, salt);

                    default:

                        return CalculateHash(plaintextObject, algorithm, null);
                }
            }
            catch
            {
                throw new SecurityException("The hashing operation failed.");
            }
        }

        /// <summary>
        /// Calculates a hash value for the specified plaintext object and compares the result with the specified hash value.
        /// </summary>
        /// <param name="hash">
        /// The hash value to evaluate.
        /// </param>
        /// <param name="plaintextObject">
        /// The plaintext object to evaluate.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the plaintext.
        /// </param>
        /// <param name="saltingMode">
        /// A value specifying whether or not salt is applied to the plaintext.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the resulting hash value matches <paramref name="hash" />, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public Boolean EvaluateHash(Byte[] hash, T plaintextObject, HashingAlgorithmSpecification algorithm, SaltingMode saltingMode)
        {
            try
            {
                var digestLength = (algorithm.ToDigestBitLength() / 8);
                Byte[] processedHash;
                Byte[] calculatedHash;

                switch (saltingMode)
                {
                    case SaltingMode.Salted:

                        var salt = new Byte[SaltLengthInBytes];
                        processedHash = hash.Take(digestLength).ToArray();
                        salt = hash.Skip(digestLength).Take(SaltLengthInBytes).ToArray();
                        calculatedHash = CalculateHash(plaintextObject, algorithm, salt).Take(digestLength).ToArray();
                        break;

                    default:

                        processedHash = hash;
                        calculatedHash = CalculateHash(plaintextObject, algorithm, null);
                        break;
                }

                if (processedHash.Length != digestLength)
                {
                    return false;
                }
                else if (calculatedHash.Length != digestLength)
                {
                    return false;
                }

                for (var i = 0; i < digestLength; i++)
                {
                    if (processedHash[i] == calculatedHash[i])
                    {
                        continue;
                    }

                    return false;
                }

                return true;
            }
            catch
            {
                throw new SecurityException("The hashing operation failed.");
            }
        }

        /// <summary>
        /// Calculates a hash value for the specified plaintext object.
        /// </summary>
        /// <param name="plaintextObject">
        /// The plaintext object to hash.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the plaintext.
        /// </param>
        /// <param name="salt">
        /// The salt to apply to the plaintext, or <see langword="null" /> if the plaintext is unsalted.
        /// </param>
        /// <returns>
        /// The resulting hash value.
        /// </returns>
        [DebuggerHidden]
        private Byte[] CalculateHash(T plaintextObject, HashingAlgorithmSpecification algorithm, Byte[] salt) => CalculateHash(BinarySerializer.Serialize(plaintextObject), algorithm, salt);

        /// <summary>
        /// Gets the salt length, in bytes, to use when calculating and evaluating hash values.
        /// </summary>
        public Int32 SaltLengthInBytes
        {
            get;
        }

        /// <summary>
        /// Represents the default salt length, in bytes, to use when calculating and evaluating hash values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DefaultSaltLengthInBytes = 8;

        /// <summary>
        /// Represents a binary serializer that is used to transform plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ISerializer<T> BinarySerializer;

        /// <summary>
        /// Represents a random number generator that is used to generate salt values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly RandomNumberGenerator RandomnessProvider;
    }
}