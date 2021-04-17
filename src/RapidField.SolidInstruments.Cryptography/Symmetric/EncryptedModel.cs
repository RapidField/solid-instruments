// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Represents a symmetrically encrypted <see cref="IModel" />.
    /// </summary>
    /// <remarks>
    /// <see cref="EncryptedModel{TModel}" /> is the default implementation of <see cref="IEncryptedModel{TModel}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the plaintext model.
    /// </typeparam>
    [DataContract]
    public class EncryptedModel<TModel> : EncryptedModel, IEncryptedModel<TModel>
        where TModel : class, IModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedModel{TModel}" /> class.
        /// </summary>
        /// <param name="ciphertext">
        /// The ciphertext of the model.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="ciphertext" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ciphertext" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal EncryptedModel(Byte[] ciphertext)
            : base(ciphertext, typeof(TModel))
        {
            return;
        }

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
        public TModel ToPlaintextModel(ISymmetricKey key) => ToPlaintextModel<TModel>(key);

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
        public TModel ToPlaintextModel(ICascadingSymmetricKey key) => ToPlaintextModel<TModel>(key);

        /// <summary>
        /// Converts the current <see cref="EncryptedModel{TModel}" /> to a serializable <see cref="EncryptedModel" />.
        /// </summary>
        /// <returns>
        /// A serializable <see cref="EncryptedModel" /> representation of the current <see cref="EncryptedModel{TModel}" />.
        /// </returns>
        public EncryptedModel ToSerializableModel() => new(CiphertextBytes, ModelType);
    }

    /// <summary>
    /// Represents a symmetrically encrypted <see cref="IModel" />.
    /// </summary>
    /// <remarks>
    /// <see cref="EncryptedModel" /> is the default implementation of <see cref="IEncryptedModel" />.
    /// </remarks>
    [DataContract]
    public class EncryptedModel : Model, IEncryptedModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedModel" /> class.
        /// </summary>
        /// <param name="ciphertext">
        /// The ciphertext of the model.
        /// </param>
        /// <param name="modelType">
        /// The type of the model.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="ciphertext" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ciphertext" /> is <see langword="null" /> -or- <paramref name="modelType" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal EncryptedModel(Byte[] ciphertext, Type modelType)
            : base()
        {
            CiphertextBytes = ciphertext.RejectIf().IsNullOrEmpty(nameof(ciphertext));
            ModelTypeAssemblyQualifiedName = modelType.RejectIf().IsNull(nameof(modelType)).TargetArgument.AssemblyQualifiedName;
        }

        /// <summary>
        /// Encrypts the specified <see cref="IModel" /> and returns a new <see cref="EncryptedModel{TModel}" />.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the plaintext model.
        /// </typeparam>
        /// <param name="model">
        /// The plaintext model to encrypt.
        /// </param>
        /// <param name="key">
        /// The symmetric key that is used to encrypt the model.
        /// </param>
        /// <returns>
        /// A new <see cref="EncryptedModel{TModel}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public static IEncryptedModel<TModel> FromPlaintextModel<TModel>(TModel model, ISymmetricKey key)
            where TModel : class, IModel => new EncryptedModel<TModel>(SymmetricProcessor.ForType<TModel>().Encrypt(model.RejectIf().IsNull(nameof(model)), key.RejectIf().IsNull(nameof(key)).TargetArgument));

        /// <summary>
        /// Encrypts the specified <see cref="IModel" /> and returns a new <see cref="EncryptedModel{TModel}" />.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the plaintext model.
        /// </typeparam>
        /// <param name="model">
        /// The plaintext model to encrypt.
        /// </param>
        /// <param name="key">
        /// The symmetric key that is used to encrypt the model.
        /// </param>
        /// <returns>
        /// A new <see cref="EncryptedModel{TModel}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public static IEncryptedModel<TModel> FromPlaintextModel<TModel>(TModel model, ICascadingSymmetricKey key)
            where TModel : class, IModel => new EncryptedModel<TModel>(SymmetricProcessor.ForType<TModel>().Encrypt(model.RejectIf().IsNull(nameof(model)), key.RejectIf().IsNull(nameof(key)).TargetArgument));

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
            where TModel : class, IModel
        {
            var modelType = typeof(TModel);

            if (ModelType == modelType)
            {
                return SymmetricProcessor.ForType<TModel>().Decrypt(CiphertextBytes, key.RejectIf().IsNull(nameof(key)).TargetArgument);
            }

            throw new ArgumentException($"The specified type, {modelType}, does not match the model type of the encrypted model, {ModelType?.ToString() ?? "[unknown]"}.", nameof(TModel));
        }

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
            where TModel : class, IModel
        {
            var modelType = typeof(TModel);

            if (ModelType == modelType)
            {
                return SymmetricProcessor.ForType<TModel>().Decrypt(CiphertextBytes, key.RejectIf().IsNull(nameof(key)).TargetArgument);
            }

            throw new ArgumentException($"The specified type, {modelType}, does not match the model type of the encrypted model, {ModelType?.ToString() ?? "[unknown]"}.", nameof(TModel));
        }

        /// <summary>
        /// Gets or sets the ciphertext of the model as a Base64 string.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// The specified string is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The specified string is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// The specified string is not a valid Base64 string.
        /// </exception>
        [DataMember]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public String Ciphertext
        {
            [DebuggerHidden]
            get => CiphertextBytes.IsNullOrEmpty() ? null : Convert.ToBase64String(CiphertextBytes);
            [DebuggerHidden]
            set => CiphertextBytes = Convert.FromBase64String(value.RejectIf().IsNullOrEmpty(nameof(Ciphertext)));
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
        [IgnoreDataMember]
        public Type ModelType => ModelTypeAssemblyQualifiedName.IsNullOrEmpty() ? null : Type.GetType(ModelTypeAssemblyQualifiedName, false, true);

        /// <summary>
        /// Gets or sets the assembly-qualified name of the type of the plaintext model.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// The specified string is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The specified string is <see langword="null" />.
        /// </exception>
        [DataMember]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public String ModelTypeAssemblyQualifiedName
        {
            [DebuggerHidden]
            get => ModelTypeAssemblyQualifiedNameReference;
            [DebuggerHidden]
            set => ModelTypeAssemblyQualifiedNameReference = value.RejectIf().IsNullOrEmpty(nameof(ModelTypeAssemblyQualifiedName));
        }

        /// <summary>
        /// Represents the ciphertext of the model.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        internal Byte[] CiphertextBytes;

        /// <summary>
        /// Represents the assembly-qualified name of the type of the plaintext model.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String ModelTypeAssemblyQualifiedNameReference;
    }
}