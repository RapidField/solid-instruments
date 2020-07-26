// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Represents a series of <see cref="ISymmetricKey" /> instances that constitute instructions for applying cascading encryption
    /// and decryption.
    /// </summary>
    public interface ICascadingSymmetricKey : ICryptographicKey
    {
        /// <summary>
        /// Gets the number of layers of encryption that a resulting transform will apply.
        /// </summary>
        public Int32 Depth
        {
            get;
        }

        /// <summary>
        /// Gets the ordered array of keys comprising the cascading key.
        /// </summary>
        public IEnumerable<ISymmetricKey> Keys
        {
            get;
        }
    }
}