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
    public interface ICascadingSymmetricKey : IDisposable
    {
        /// <summary>
        /// Converts the value of the current <see cref="ICascadingSymmetricKey" /> to its equivalent binary representation.
        /// </summary>
        /// <returns>
        /// A binary representation of the current <see cref="ICascadingSymmetricKey" />.
        /// </returns>
        ISecureBuffer ToBuffer();

        /// <summary>
        /// Gets the number of layers of encryption that a resulting transform will apply.
        /// </summary>
        Int32 Depth
        {
            get;
        }

        /// <summary>
        /// Gets the ordered array of keys comprising the cascading key.
        /// </summary>
        IEnumerable<ISymmetricKey> Keys
        {
            get;
        }
    }
}