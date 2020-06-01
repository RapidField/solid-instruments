// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.IO;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Represents a symmetrically encrypted <see cref="IModel" />.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the plaintext model.
    /// </typeparam>
    public interface IEncryptedModel<TModel> : IEncryptedModel
        where TModel : class, IModel
    {
        /// <summary>
        /// Decrypts and returns the plaintext <see cref="IModel" />.
        /// </summary>
        /// <param name="key">
        /// The symmetric key that was used to encrypt the model.
        /// </param>
        /// <returns>
        /// The plaintext <see cref="IModel" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="BadImageFormatException">
        /// The type assembly or one of its dependencies is invalid.
        /// </exception>
        /// <exception cref="FileLoadException">
        /// The type assembly could not be loaded.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public TModel ToPlaintextModel(ISymmetricKey key);

        /// <summary>
        /// Decrypts and returns the plaintext <see cref="IModel" />.
        /// </summary>
        /// <param name="key">
        /// The symmetric key that was used to encrypt the model.
        /// </param>
        /// <returns>
        /// The plaintext <see cref="IModel" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="BadImageFormatException">
        /// The type assembly or one of its dependencies is invalid.
        /// </exception>
        /// <exception cref="FileLoadException">
        /// The type assembly could not be loaded.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public TModel ToPlaintextModel(ICascadingSymmetricKey key);

        /// <summary>
        /// Converts the current <see cref="IEncryptedModel{TModel}" /> to a serializable <see cref="EncryptedModel" />.
        /// </summary>
        /// <returns>
        /// A serializable <see cref="EncryptedModel" /> representation of the current <see cref="IEncryptedModel{TModel}" />.
        /// </returns>
        public EncryptedModel ToSerializableModel();
    }

    /// <summary>
    /// Represents a symmetrically encrypted <see cref="IModel" />.
    /// </summary>
    public interface IEncryptedModel : IModel
    {
        /// <summary>
        /// Decrypts and returns the plaintext <see cref="IModel" />.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the plaintext model.
        /// </typeparam>
        /// <param name="key">
        /// The symmetric key that was used to encrypt the model.
        /// </param>
        /// <returns>
        /// The plaintext <see cref="IModel" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="TModel" /> does not match the model type of the encrypted model.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="BadImageFormatException">
        /// The type assembly or one of its dependencies is invalid.
        /// </exception>
        /// <exception cref="FileLoadException">
        /// The type assembly could not be loaded.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public TModel ToPlaintextModel<TModel>(ISymmetricKey key)
            where TModel : class, IModel;

        /// <summary>
        /// Decrypts and returns the plaintext <see cref="IModel" />.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the plaintext model.
        /// </typeparam>
        /// <param name="key">
        /// The symmetric key that was used to encrypt the model.
        /// </param>
        /// <returns>
        /// The plaintext <see cref="IModel" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="TModel" /> does not match the model type of the encrypted model.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="BadImageFormatException">
        /// The type assembly or one of its dependencies is invalid.
        /// </exception>
        /// <exception cref="FileLoadException">
        /// The type assembly could not be loaded.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public TModel ToPlaintextModel<TModel>(ICascadingSymmetricKey key)
            where TModel : class, IModel;

        /// <summary>
        /// Gets the ciphertext of the model as a Base64 string.
        /// </summary>
        public String Ciphertext
        {
            get;
        }

        /// <summary>
        /// Gets the type of the plaintext model, or <see langword="null" /> if <see cref="ModelTypeAssemblyQualifiedName" /> is not
        /// a valid, assembly-qualified type name.
        /// </summary>
        /// <exception cref="BadImageFormatException">
        /// The type assembly or one of its dependencies is invalid.
        /// </exception>
        /// <exception cref="FileLoadException">
        /// The type assembly could not be loaded.
        /// </exception>
        public Type ModelType
        {
            get;
        }

        /// <summary>
        /// Gets the assembly-qualified name of the type of the plaintext model.
        /// </summary>
        public String ModelTypeAssemblyQualifiedName
        {
            get;
        }
    }
}