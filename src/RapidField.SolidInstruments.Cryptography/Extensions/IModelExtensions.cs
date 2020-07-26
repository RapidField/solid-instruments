// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Extensions
{
    /// <summary>
    /// Extends the <see cref="IModel" /> interface with cryptographic features.
    /// </summary>
    public static class IModelExtensions
    {
        /// <summary>
        /// Encrypts the current <see cref="IModel" /> using the specified key.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the current <see cref="IModel" />.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IModel" />.
        /// </param>
        /// <param name="key">
        /// The symmetric key to use when encrypting the model.
        /// </param>
        /// <returns>
        /// The symmetrically encrypted model.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public static IEncryptedModel<TModel> Encrypt<TModel>(this TModel target, ISymmetricKey key)
            where TModel : class, IModel => EncryptedModel.FromPlaintextModel(target, key);

        /// <summary>
        /// Encrypts the current <see cref="IModel" /> using the specified key.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the current <see cref="IModel" />.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IModel" />.
        /// </param>
        /// <param name="key">
        /// The symmetric key to use when encrypting the model.
        /// </param>
        /// <returns>
        /// The symmetrically encrypted model.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public static IEncryptedModel<TModel> Encrypt<TModel>(this TModel target, ICascadingSymmetricKey key)
            where TModel : class, IModel => EncryptedModel.FromPlaintextModel(target, key);
    }
}