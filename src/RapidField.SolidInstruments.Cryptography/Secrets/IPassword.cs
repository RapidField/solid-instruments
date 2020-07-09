// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a named textual password that is pinned in memory and encrypted at rest.
    /// </summary>
    public interface IPassword : IReadOnlySecret<String>
    {
        /// <summary>
        /// Calculates a cryptographically secure salted hash value for the current <see cref="IPassword" /> using the Argon2id
        /// algorithm and returns it as a Base64 string.
        /// </summary>
        /// <returns>
        /// A Base64-encoded digest for the current <see cref="IPassword" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public String CalculateSecureHashString();

        /// <summary>
        /// Calculates a cryptographically secure salted hash value for the current <see cref="IPassword" /> using the Argon2id
        /// algorithm.
        /// </summary>
        /// <returns>
        /// A digest for the current <see cref="IPassword" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IReadOnlyPinnedMemory<Byte> CalculateSecureHashValue();

        /// <summary>
        /// Calculates a cryptographically secure salted hash value for the current <see cref="IPassword" /> using the Argon2id
        /// algorithm and compares the result with the specified Base64-encoded digest.
        /// </summary>
        /// <param name="hashString">
        /// The salted hash string to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the resulting hash value matches <paramref name="hashString" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="hashString" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hashString" /> is <see langword="null" />.
        /// </exception>
        public Boolean EvaluateSecureHashString(String hashString);

        /// <summary>
        /// Calculates a cryptographically secure salted hash value for the current <see cref="IPassword" /> using the Argon2id
        /// algorithm and compares the result with the specified salted hash value.
        /// </summary>
        /// <param name="hashValue">
        /// The salted hash value to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the resulting hash value matches <paramref name="hashValue" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="hashValue" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hashValue" /> is <see langword="null" />.
        /// </exception>
        public Boolean EvaluateSecureHashValue(Byte[] hashValue);

        /// <summary>
        /// Returns the number of characters comprising the current <see cref="IPassword" />.
        /// </summary>
        /// <returns>
        /// The character length of the current <see cref="IPassword" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int32 GetCharacterLength();

        /// <summary>
        /// Determines whether or not the current <see cref="IPassword" /> complies with the specified composition requirements.
        /// </summary>
        /// <param name="requirements">
        /// The password composition requirements against which the password is evaluated.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the password is in compliance, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requirements" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean MeetsRequirements(IPasswordCompositionRequirements requirements);
    }
}