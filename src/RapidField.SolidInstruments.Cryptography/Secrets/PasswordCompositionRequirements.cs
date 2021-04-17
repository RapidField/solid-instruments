// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a definition for the composition requirements of an <see cref="IPassword" />.
    /// </summary>
    /// <remarks>
    /// <see cref="PasswordCompositionRequirements" /> is the default implementation of
    /// <see cref="IPasswordCompositionRequirements" />.
    /// </remarks>
    [DataContract]
    public sealed class PasswordCompositionRequirements : Model, IPasswordCompositionRequirements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordCompositionRequirements" /> class.
        /// </summary>
        public PasswordCompositionRequirements()
            : base()
        {
            return;
        }

        /// <summary>
        /// Gets a <see cref="PasswordCompositionRequirements" /> that matches the requirements specified by NIST Special
        /// Publication 800-63-3.
        /// </summary>
        [IgnoreDataMember]
        public static PasswordCompositionRequirements Nist800633 => new()
        {
            ForbidCommonBreachedPasswords = true,
            MinimumLowercaseAlphabeticCharacterCount = 0,
            MinimumNonAlphanumericCharacterCount = 0,
            MinimumNumericCharacterCount = 0,
            MinimumTotalCharacterCount = 8,
            MinimumUppercaseAlphabeticCharacterCount = 0
        };

        /// <summary>
        /// Gets a <see cref="PasswordCompositionRequirements" /> that specifies no requirements.
        /// </summary>
        [IgnoreDataMember]
        public static PasswordCompositionRequirements None => new()
        {
            ForbidCommonBreachedPasswords = false,
            MinimumLowercaseAlphabeticCharacterCount = 0,
            MinimumNonAlphanumericCharacterCount = 0,
            MinimumNumericCharacterCount = 0,
            MinimumTotalCharacterCount = 0,
            MinimumUppercaseAlphabeticCharacterCount = 0
        };

        /// <summary>
        /// Gets a <see cref="PasswordCompositionRequirements" /> that requires 13 or more total characters with at least one
        /// character in every category and forbids commonly breached passwords.
        /// </summary>
        [IgnoreDataMember]
        public static PasswordCompositionRequirements Strict => new()
        {
            ForbidCommonBreachedPasswords = true,
            MinimumLowercaseAlphabeticCharacterCount = 1,
            MinimumNonAlphanumericCharacterCount = 1,
            MinimumNumericCharacterCount = 1,
            MinimumTotalCharacterCount = 13,
            MinimumUppercaseAlphabeticCharacterCount = 1
        };

        /// <summary>
        /// Gets or sets a value indicating whether or not the use of passwords which frequently appear in breach publications
        /// should be forbidden.
        /// </summary>
        [DataMember]
        public Boolean ForbidCommonBreachedPasswords
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum number of lowercase Latin alphabetic characters which an <see cref="IPassword" /> must contain,
        /// or zero if no such characters are required.
        /// </summary>
        [DataMember]
        public Int32 MinimumLowercaseAlphabeticCharacterCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum number of non-alphanumeric characters which an <see cref="IPassword" /> must contain, or zero
        /// if no such characters are required.
        /// </summary>
        [DataMember]
        public Int32 MinimumNonAlphanumericCharacterCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum number of numeric characters which an <see cref="IPassword" /> must contain, or zero if no such
        /// characters are required.
        /// </summary>
        [DataMember]
        public Int32 MinimumNumericCharacterCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum number of total characters which an <see cref="IPassword" /> must contain, or zero if no total
        /// length requirement is imposed.
        /// </summary>
        [DataMember]
        public Int32 MinimumTotalCharacterCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum number of uppercase Latin alphabetic characters which an <see cref="IPassword" /> must contain,
        /// or zero if no such characters are required.
        /// </summary>
        [DataMember]
        public Int32 MinimumUppercaseAlphabeticCharacterCount
        {
            get;
            set;
        }

        /// <summary>
        /// Represents an ordered collection of the 20 most common lowercase passwords that appeared in breached password lists in
        /// 2019, per the National Cyber Security Centre (NCSC).
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [IgnoreDataMember]
        public static readonly IEnumerable<String> CommonBreachedPasswords = new[]
        {
            "123456",
            "123456789",
            "qwerty",
            "password",
            "1111111",
            "12345678",
            "abc123",
            "1234567",
            "password1",
            "12345",
            "1234567890",
            "123123",
            "0",
            "iloveyou",
            "1234",
            "1q2w3e4r5t",
            "qwertyuiop",
            "123",
            "monkey",
            "dragon"
        };
    }
}