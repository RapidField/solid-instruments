// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a definition for the minimum permissible complexity of an <see cref="IPassword" />.
    /// </summary>
    public interface IPasswordCompositionRequirements : IModel
    {
        /// <summary>
        /// Gets a value indicating whether or not the use of passwords which frequently appear in breach publications should be
        /// forbidden.
        /// </summary>
        public Boolean ForbidCommonBreachedPasswords
        {
            get;
        }

        /// <summary>
        /// Gets the minimum number of lowercase Latin alphabetic characters which an <see cref="IPassword" /> must contain, or zero
        /// if no such characters are required.
        /// </summary>
        public Int32 MinimumLowercaseAlphabeticCharacterCount
        {
            get;
        }

        /// <summary>
        /// Gets the minimum number of non-alphanumeric characters which an <see cref="IPassword" /> must contain, or zero if no
        /// such characters are required.
        /// </summary>
        public Int32 MinimumNonAlphanumericCharacterCount
        {
            get;
        }

        /// <summary>
        /// Gets the minimum number of numeric characters which an <see cref="IPassword" /> must contain, or zero if no such
        /// characters are required.
        /// </summary>
        public Int32 MinimumNumericCharacterCount
        {
            get;
        }

        /// <summary>
        /// Gets the minimum number of total characters which an <see cref="IPassword" /> must contain, or zero if no total length
        /// requirement is imposed.
        /// </summary>
        public Int32 MinimumTotalCharacterCount
        {
            get;
        }

        /// <summary>
        /// Gets the minimum number of uppercase Latin alphabetic characters which an <see cref="IPassword" /> must contain, or zero
        /// if no such characters are required.
        /// </summary>
        public Int32 MinimumUppercaseAlphabeticCharacterCount
        {
            get;
        }
    }
}