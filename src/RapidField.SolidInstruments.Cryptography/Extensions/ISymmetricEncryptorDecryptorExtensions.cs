// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Extensions
{
    /// <summary>
    /// Extends the <see cref="ISymmetricProcessor{T}" /> interface with cryptographic features.
    /// </summary>
    public static class ISymmetricProcessorExtensions
    {
        /// <summary>
        /// Decrypts the specified Base64 string ciphertext.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object that is decrypted.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ISymmetricProcessor{T}" />.
        /// </param>
        /// <param name="ciphertext">
        /// The Base64 string ciphertext to decrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits and algorithm specification used to transform the ciphertext.
        /// </param>
        /// <returns>
        /// The resulting plaintext object.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="ciphertext" /> is not a valid Base64 string.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public static T DecryptFromBase64String<T>(this ISymmetricProcessor<T> target, String ciphertext, SecureSymmetricKey key) => target.Decrypt(Convert.FromBase64String(ciphertext), key);

        /// <summary>
        /// Decrypts the specified Base64 string ciphertext.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object that is decrypted.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ISymmetricProcessor{T}" />.
        /// </param>
        /// <param name="ciphertext">
        /// The Base64 string ciphertext to decrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits used to transform the ciphertext.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the ciphertext.
        /// </param>
        /// <returns>
        /// The resulting plaintext object.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="ciphertext" /> is not a valid Base64 string.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public static T DecryptFromBase64String<T>(this ISymmetricProcessor<T> target, String ciphertext, SecureBuffer key, SymmetricAlgorithmSpecification algorithm) => target.Decrypt(Convert.FromBase64String(ciphertext), key, algorithm);

        /// <summary>
        /// Encrypts the specified plaintext object to a Base64 string.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object that is encrypted.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ISymmetricProcessor{T}" />.
        /// </param>
        /// <param name="plaintextObject">
        /// The plaintext object to encrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits and algorithm specification used to transform the object.
        /// </param>
        /// <returns>
        /// The resulting ciphertext as a Base64 string.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public static String EncryptToBase64String<T>(this ISymmetricProcessor<T> target, T plaintextObject, SecureSymmetricKey key) => Convert.ToBase64String(target.Encrypt(plaintextObject, key));

        /// <summary>
        /// Encrypts the specified plaintext object to a Base64 string.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object that is encrypted.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ISymmetricProcessor{T}" />.
        /// </param>
        /// <param name="plaintextObject">
        /// The plaintext object to encrypt.
        /// </param>
        /// <param name="key">
        /// The key derivation bits used to transform the object.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm specification used to transform the object.
        /// </param>
        /// <returns>
        /// The resulting ciphertext as a Base64 string.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public static String EncryptToBase64String<T>(this ISymmetricProcessor<T> target, T plaintextObject, SecureBuffer key, SymmetricAlgorithmSpecification algorithm) => Convert.ToBase64String(target.Encrypt(plaintextObject, key, algorithm));
    }
}