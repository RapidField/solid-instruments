// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for one key in an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="AsymmetricKey" /> is the default implementation of <see cref="IAsymmetricKey" />.
    /// </remarks>
    public abstract class AsymmetricKey : Instrument, IAsymmetricKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricKey" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="AsymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        protected AsymmetricKey(AsymmetricAlgorithmSpecification algorithm)
            : base()
        {
            Algorithm = algorithm.RejectIf().IsEqualToValue(AsymmetricAlgorithmSpecification.Unspecified, nameof(algorithm));
        }

        /// <summary>
        /// Converts the value of the current <see cref="AsymmetricKey" /> to a secure bit field.
        /// </summary>
        /// <returns>
        /// A secure bit field containing a representation of the current <see cref="AsymmetricKey" />.
        /// </returns>
        public ISecureMemory ToSecureMemory() => throw new NotImplementedException();

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AsymmetricKey" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    throw new NotImplementedException();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets the asymmetric-key algorithm for which the key is used.
        /// </summary>
        public AsymmetricAlgorithmSpecification Algorithm
        {
            get;
        }
    }
}