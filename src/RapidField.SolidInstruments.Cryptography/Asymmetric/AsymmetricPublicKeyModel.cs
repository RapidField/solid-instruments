// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature;
using RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents a serializable asymmetric-key algorithm and the public key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="AsymmetricPublicKeyModel" /> is the default implementation of <see cref="IAsymmetricPublicKeyModel" />.
    /// </remarks>
    [DataContract]
    public sealed class AsymmetricPublicKeyModel : Model<Guid>, IAsymmetricPublicKeyModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricPublicKeyModel" /> class.
        /// </summary>
        /// <param name="model">
        /// A model from which to copy information to create a new model.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// The model key is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- the associated key is <see langword="null" />.
        /// </exception>
        public AsymmetricPublicKeyModel(IAsymmetricPublicKeyModel model)
            : this(model.RejectIf().IsNull(nameof(model)).TargetArgument.Identifier, model.Key, model.ExpirationTimeStamp)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricPublicKeyModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the model.
        /// </param>
        /// <param name="key">
        /// A textual, Base64-encoded representation of the public key.
        /// </param>
        /// <param name="expirationTimeStamp">
        /// The date and time when the associated key expires and is no longer valid for use.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal AsymmetricPublicKeyModel(Guid identifier, String key, DateTime expirationTimeStamp)
            : base(identifier)
        {
            ExpirationTimeStamp = expirationTimeStamp;
            Key = key.RejectIf().IsNullOrEmpty(nameof(key));
        }

        /// <summary>
        /// Extracts the digital signature algorithm specification from <see cref="Key" />, if the key is purposed for digital
        /// signature.
        /// </summary>
        /// <returns>
        /// A value that specifies the distinct algorithm for the key.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The key is not purposed for digital signature.
        /// </exception>
        /// <exception cref="SecurityException">
        /// <see cref="Key" /> is invalid.
        /// </exception>
        [DebuggerHidden]
        DigitalSignatureAlgorithmSpecification IAsymmetricPublicKeyModel.ExtractDigitalSignatureAlgorithm()
        {
            try
            {
                var keyPurpose = KeyPurpose;

                return keyPurpose switch
                {
                    AsymmetricKeyPurpose.DigitalSignature => DigitalSignaturePublicKey.ExtractAlgorithm(Key),
                    AsymmetricKeyPurpose.KeyExchange => throw new InvalidOperationException("The key is not purposed for digital signature."),
                    _ => throw new UnsupportedSpecificationException($"The specified asymmetric key purpose, {keyPurpose}, is not supported."),
                };
            }
            catch (Exception exception)
            {
                throw new SecurityException("An exception was raised while extracting the algorithm from the key model. The key is invalid.", exception);
            }
        }

        /// <summary>
        /// Extracts the key exchange algorithm specification from <see cref="Key" />, if the key is purposed for key exchange.
        /// </summary>
        /// <returns>
        /// A value that specifies the distinct algorithm for the key.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The key is not purposed for key exchange.
        /// </exception>
        /// <exception cref="SecurityException">
        /// <see cref="Key" /> is invalid.
        /// </exception>
        [DebuggerHidden]
        KeyExchangeAlgorithmSpecification IAsymmetricPublicKeyModel.ExtractKeyExchangeAlgorithm()
        {
            try
            {
                var keyPurpose = KeyPurpose;

                return keyPurpose switch
                {
                    AsymmetricKeyPurpose.DigitalSignature => throw new InvalidOperationException("The key is not purposed for key exchange."),
                    AsymmetricKeyPurpose.KeyExchange => KeyExchangePublicKey.ExtractAlgorithm(Key),
                    _ => throw new UnsupportedSpecificationException($"The specified asymmetric key purpose, {keyPurpose}, is not supported."),
                };
            }
            catch (Exception exception)
            {
                throw new SecurityException("An exception was raised while extracting the algorithm from the key model. The key is invalid.", exception);
            }
        }

        /// <summary>
        /// Extracts the key memory bytes from <see cref="Key" />.
        /// </summary>
        /// <returns>
        /// The public key memory bytes.
        /// </returns>
        /// <exception cref="SecurityException">
        /// <see cref="Key" /> is invalid.
        /// </exception>
        [DebuggerHidden]
        Span<Byte> IAsymmetricPublicKeyModel.ExtractKeyMemory()
        {
            try
            {
                var keyPurpose = KeyPurpose;

                return keyPurpose switch
                {
                    AsymmetricKeyPurpose.DigitalSignature => DigitalSignaturePublicKey.ExtractKeyMemory(Key),
                    AsymmetricKeyPurpose.KeyExchange => KeyExchangePublicKey.ExtractKeyMemory(Key),
                    _ => throw new UnsupportedSpecificationException($"The specified asymmetric key purpose, {keyPurpose}, is not supported."),
                };
            }
            catch (Exception exception)
            {
                throw new SecurityException("An exception was raised while extracting the key memory from the key model. The key is invalid.", exception);
            }
        }

        /// <summary>
        /// Extracts the purpose of the key from <see cref="Key" />.
        /// </summary>
        /// <returns>
        /// A value that specifies the purpose of the asymmetric key.
        /// </returns>
        /// <exception cref="SecurityException">
        /// <see cref="Key" /> is invalid.
        /// </exception>
        [DebuggerHidden]
        AsymmetricKeyPurpose IAsymmetricPublicKeyModel.ExtractKeyPurpose() => KeyPurpose;

        /// <summary>
        /// Converts the value of the current <see cref="AsymmetricPublicKeyModel" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="AsymmetricPublicKeyModel" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Identifier)}\": {Identifier.ToSerializedString()}, \"{nameof(ExpirationTimeStamp)}\": {ExpirationTimeStamp.ToSerializedString()} }}";

        /// <summary>
        /// Gets or sets the date and time when the associated key expires and is no longer valid for use.
        /// </summary>
        [DataMember]
        public DateTime ExpirationTimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a textual, Base64-encoded representation of the public key.
        /// </summary>
        [DataMember]
        public String Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value that specifies the purpose of the asymmetric key.
        /// </summary>
        /// <exception cref="SecurityException">
        /// <see cref="Key" /> is invalid.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AsymmetricKeyPurpose KeyPurpose
        {
            get
            {
                try
                {
                    return (AsymmetricKeyPurpose)Convert.FromBase64String(Key)[AsymmetricPublicKeyCompositionInformation.PurposeStartIndex];
                }
                catch (Exception exception)
                {
                    throw new SecurityException("An exception was raised while extracting the key purpose from the key model. The key is invalid.", exception);
                }
            }
        }
    }
}