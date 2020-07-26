// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Hashing
{
    /// <summary>
    /// Provides facilities for hashing byte arrays.
    /// </summary>
    public interface IHashingProcessor : IHashingProcessor<Byte[]>
    {
    }

    /// <summary>
    /// Provides facilities for hashing typed objects and byte arrays.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object that can be hashed.
    /// </typeparam>
    public interface IHashingProcessor<T> : ICryptographicProcessor<T>
        where T : class
    {
        /// <summary>
        /// Calculates a hash value for the specified plaintext byte array.
        /// </summary>
        /// <param name="plaintext">
        /// The plaintext byte array to hash.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the plaintext.
        /// </param>
        /// <returns>
        /// The resulting hash value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="algorithm" /> is equal to <see cref="HashingAlgorithmSpecification.Unspecified" />.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="plaintext" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="plaintext" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public Byte[] CalculateHash(Byte[] plaintext, HashingAlgorithmSpecification algorithm);

        /// <summary>
        /// Calculates a hash value for the specified plaintext byte array.
        /// </summary>
        /// <param name="plaintext">
        /// The plaintext byte array to hash.
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
        /// <exception cref="ArgumentException">
        /// <paramref name="algorithm" /> is equal to <see cref="HashingAlgorithmSpecification.Unspecified" />.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="plaintext" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="plaintext" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public Byte[] CalculateHash(Byte[] plaintext, HashingAlgorithmSpecification algorithm, Byte[] salt);

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
        /// <exception cref="ArgumentException">
        /// <paramref name="algorithm" /> is equal to <see cref="HashingAlgorithmSpecification.Unspecified" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="plaintextObject" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public Byte[] CalculateHash(T plaintextObject, HashingAlgorithmSpecification algorithm, SaltingMode saltingMode);

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
        /// <see langword="true" /> if the resulting hash value matches <paramref name="hash" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="algorithm" /> is equal to <see cref="HashingAlgorithmSpecification.Unspecified" />.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="hash" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hash" /> is <see langword="null" /> -or- <paramref name="plaintextObject" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public Boolean EvaluateHash(Byte[] hash, T plaintextObject, HashingAlgorithmSpecification algorithm, SaltingMode saltingMode);
    }
}