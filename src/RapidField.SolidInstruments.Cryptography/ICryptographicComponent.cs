// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a cryptographic key or instrument.
    /// </summary>
    public interface ICryptographicComponent
    {
        /// <summary>
        /// Gets a value specifying the valid purposes and uses of the current <see cref="ICryptographicComponent" />.
        /// </summary>
        public CryptographicComponentUsage Usage
        {
            get;
        }
    }
}