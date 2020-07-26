// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a symmetric key encryption facility that uses managed keys to perform cryptographic operations.
    /// </summary>
    public interface IManagedKeyCipher : IAsyncDisposable, ICryptographicComponent, IDisposable
    {
        /// <summary>
        /// Decrypts the specified model using the master key.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the model to decrypt.
        /// </typeparam>
        /// <param name="model">
        /// The model to decrypt.
        /// </param>
        /// <returns>
        /// The decrypted model.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public TModel Decrypt<TModel>(IEncryptedModel<TModel> model)
            where TModel : class, IModel;

        /// <summary>
        /// Decrypts the specified model using the specified key.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the model to decrypt.
        /// </typeparam>
        /// <param name="model">
        /// The model to decrypt.
        /// </param>
        /// <param name="keyName">
        /// The name of the key to use when decrypting the model, or <see langword="null" /> to use the master key.
        /// </param>
        /// <returns>
        /// The decrypted model.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret store does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public TModel Decrypt<TModel>(IEncryptedModel<TModel> model, String keyName)
            where TModel : class, IModel;

        /// <summary>
        /// Decrypts the specified Base-64 encoded ciphertext string using the master key.
        /// </summary>
        /// <param name="ciphertext">
        /// The Base-64 encoded ciphertext string to decrypt.
        /// </param>
        /// <returns>
        /// The resulting plaintext string.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="ciphertext" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ciphertext" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption.
        /// </exception>
        public String Decrypt(String ciphertext);

        /// <summary>
        /// Decrypts the specified Base-64 encoded ciphertext string using the specified key.
        /// </summary>
        /// <param name="ciphertext">
        /// The Base-64 encoded ciphertext string to decrypt.
        /// </param>
        /// <param name="keyName">
        /// The name of the key to use when decrypting the model, or <see langword="null" /> to use the master key.
        /// </param>
        /// <returns>
        /// The resulting plaintext string.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="ciphertext" /> is empty -or- <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret store does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ciphertext" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption.
        /// </exception>
        public String Decrypt(String ciphertext, String keyName);

        /// <summary>
        /// Encrypts the specified model using the master key.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the model to encrypt.
        /// </typeparam>
        /// <param name="model">
        /// The model to encrypt.
        /// </param>
        /// <returns>
        /// The encrypted model.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public IEncryptedModel<TModel> Encrypt<TModel>(TModel model)
            where TModel : class, IModel;

        /// <summary>
        /// Encrypts the specified model using the specified key.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the model to encrypt.
        /// </typeparam>
        /// <param name="model">
        /// The model to encrypt.
        /// </param>
        /// <param name="keyName">
        /// The name of the key to use when encrypting the model, or <see langword="null" /> to use the master key.
        /// </param>
        /// <returns>
        /// The encrypted model.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret store does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public IEncryptedModel<TModel> Encrypt<TModel>(TModel model, String keyName)
            where TModel : class, IModel;

        /// <summary>
        /// Encrypts the specified plaintext string using the master key.
        /// </summary>
        /// <param name="plaintext">
        /// The plaintext string to encrypt.
        /// </param>
        /// <returns>
        /// The resulting Base-64 encoded, encrypted string.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="plaintext" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="plaintext" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption.
        /// </exception>
        public String Encrypt(String plaintext);

        /// <summary>
        /// Encrypts the specified plaintext string using the specified key.
        /// </summary>
        /// <param name="plaintext">
        /// The plaintext string to encrypt.
        /// </param>
        /// <param name="keyName">
        /// The name of the key to use when encrypting the plaintext string, or <see langword="null" /> to use the master key.
        /// </param>
        /// <returns>
        /// The resulting Base-64 encoded, encrypted string.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="plaintext" /> is empty -or- <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret store does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="plaintext" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption.
        /// </exception>
        public String Encrypt(String plaintext, String keyName);
    }
}