// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Hashing;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a named textual password that is pinned in memory and encrypted at rest.
    /// </summary>
    /// <remarks>
    /// <see cref="Password" /> is the default implementation of <see cref="IPassword" />.
    /// </remarks>
    public sealed class Password : StringSecret, IPassword
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Password" /> class.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies the secret.
        /// </param>
        /// <param name="encoding">
        /// The encoding that is use when converting the password string to and from bytes. The default value is
        /// <see cref="Encoding.Unicode" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="encoding" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private Password(String name, Encoding encoding)
            : base(name, encoding)
        {
            return;
        }

        /// <summary>
        /// Creates a new <see cref="Password" /> using the specified ASCII password string.
        /// </summary>
        /// <param name="password">
        /// The plaintext password.
        /// </param>
        /// <returns>
        /// A new <see cref="StringSecret" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="password" /> contains invalid ASCII characters.
        /// </exception>
        public static Password FromAsciiString(String password) => FromAsciiString(password, NewDefaultPasswordName());

        /// <summary>
        /// Creates a new <see cref="Password" /> using the specified name and ASCII password string.
        /// </summary>
        /// <param name="password">
        /// The plaintext password.
        /// </param>
        /// <param name="name">
        /// A textual name that uniquely identifies the password.
        /// </param>
        /// <returns>
        /// A new <see cref="StringSecret" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty -or- <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" /> -or- <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="password" /> contains invalid ASCII characters.
        /// </exception>
        public static Password FromAsciiString(String password, String name) => FromString(password, name, Encoding.ASCII);

        /// <summary>
        /// Creates a new <see cref="Password" /> using the specified Unicode password string.
        /// </summary>
        /// <param name="password">
        /// The plaintext password.
        /// </param>
        /// <returns>
        /// A new <see cref="StringSecret" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="password" /> contains invalid Unicode characters.
        /// </exception>
        public static Password FromUnicodeString(String password) => FromUnicodeString(password, NewDefaultPasswordName());

        /// <summary>
        /// Creates a new <see cref="Password" /> using the specified name and Unicode password string.
        /// </summary>
        /// <param name="password">
        /// The plaintext password.
        /// </param>
        /// <param name="name">
        /// A textual name that uniquely identifies the password.
        /// </param>
        /// <returns>
        /// A new <see cref="StringSecret" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty -or- <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" /> -or- <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="password" /> contains invalid Unicode characters.
        /// </exception>
        public static Password FromUnicodeString(String password, String name) => FromString(password, name, Encoding.Unicode);

        /// <summary>
        /// Creates a new <see cref="Password" /> using the specified UTF-8 password string.
        /// </summary>
        /// <param name="password">
        /// The plaintext password.
        /// </param>
        /// <returns>
        /// A new <see cref="StringSecret" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="password" /> contains invalid UTF-8 characters.
        /// </exception>
        public static Password FromUtf8String(String password) => FromUtf8String(password, NewDefaultPasswordName());

        /// <summary>
        /// Creates a new <see cref="Password" /> using the specified name and UTF-8 password string.
        /// </summary>
        /// <param name="password">
        /// The plaintext password.
        /// </param>
        /// <param name="name">
        /// A textual name that uniquely identifies the password.
        /// </param>
        /// <returns>
        /// A new <see cref="StringSecret" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty -or- <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" /> -or- <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="password" /> contains invalid UTF-8 characters.
        /// </exception>
        public static Password FromUtf8String(String password, String name) => FromString(password, name, Encoding.UTF8);

        /// <summary>
        /// Generates a strong password that complies with <see cref="PasswordCompositionRequirements.Strict" />.
        /// </summary>
        /// <returns>
        /// A new strong password.
        /// </returns>
        public static Password NewStrongPassword()
        {
            while (true)
            {
                var plaintextPassword = HardenedRandomNumberGenerator.Instance.GetString(RandomStrongPasswordLengthLowerBoundary, RandomStrongPasswordLengthUpperBoundary, false, true, true, true, true, false, false);
                var password = FromAsciiString(plaintextPassword);

                if (password.MeetsRequirements(PasswordCompositionRequirements.Strict))
                {
                    return password;
                }

                password.Dispose();
            }
        }

        /// <summary>
        /// Calculates a cryptographically secure salted hash value for the current <see cref="Password" /> using the Argon2id
        /// algorithm and returns it as a Base64 string.
        /// </summary>
        /// <returns>
        /// A Base64-encoded digest for the current <see cref="Password" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public String CalculateSecureHashString()
        {
            using (var hashValue = CalculateSecureHashValue())
            {
                return hashValue.ToBase64String();
            }
        }

        /// <summary>
        /// Calculates a cryptographically secure salted hash value for the current <see cref="Password" /> using the Argon2id
        /// algorithm.
        /// </summary>
        /// <returns>
        /// A digest for the current <see cref="Password" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IReadOnlyPinnedMemory<Byte> CalculateSecureHashValue()
        {
            var digest = (Byte[])null;

            Read((IReadOnlyPinnedMemory<Byte> plaintextPassword) =>
            {
                digest = HashingProcessor.Instance.CalculateHash(plaintextPassword.ToArray(), HashingAlgorithm, SaltingMode.Salted);
            });

            return new PinnedMemory(digest);
        }

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
        public Boolean EvaluateSecureHashString(String hashString) => EvaluateSecureHashValue(Convert.FromBase64String(hashString.RejectIf().IsNullOrEmpty(nameof(hashString))));

        /// <summary>
        /// Calculates a cryptographically secure salted hash value for the current <see cref="Password" /> using the Argon2id
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
        public Boolean EvaluateSecureHashValue(Byte[] hashValue)
        {
            hashValue = hashValue.RejectIf().IsNullOrEmpty(nameof(hashValue));
            var hashValueMatchesPassword = default(Boolean);

            Read((IReadOnlyPinnedMemory<Byte> plaintextPassword) =>
            {
                hashValueMatchesPassword = HashingProcessor.Instance.EvaluateHash(hashValue, plaintextPassword.ToArray(), HashingAlgorithm, SaltingMode.Salted);
            });

            return hashValueMatchesPassword;
        }

        /// <summary>
        /// Returns the number of characters comprising the current <see cref="Password" />.
        /// </summary>
        /// <returns>
        /// The character length of the current <see cref="Password" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int32 GetCharacterLength()
        {
            var characterLength = default(Int32);

            Read((String plaintextPassword) =>
            {
                characterLength = plaintextPassword.Length;
            });

            return characterLength;
        }

        /// <summary>
        /// Determines whether or not the current <see cref="Password" /> complies with the specified composition requirements.
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
        public Boolean MeetsRequirements(IPasswordCompositionRequirements requirements)
        {
            requirements = requirements.RejectIf().IsNull(nameof(requirements)).TargetArgument;
            var totalCharacterCount = 0;
            var lowercaseAlphabeticCharacterCount = 0;
            var uppercaseAlphabeticCharacterCount = 0;
            var numericCharacterCount = 0;
            var nonAlphanumericCharacterCount = 0;
            var matchesCaseInsensitiveCommonBreachedPassword = false;

            Read((String plaintextPassword) =>
            {
                totalCharacterCount = plaintextPassword.Length;

                if (totalCharacterCount == 0)
                {
                    return;
                }

                lowercaseAlphabeticCharacterCount = plaintextPassword.Count(character => character.IsLowercaseAlphabetic());
                uppercaseAlphabeticCharacterCount = plaintextPassword.Count(character => character.IsUppercaseAlphabetic());
                numericCharacterCount = plaintextPassword.Count(character => character.IsNumeric());
                nonAlphanumericCharacterCount = plaintextPassword.Count(character => character.IsPunctuation() || character.IsSymbolic() || character.IsWhiteSpaceCharacter());
                matchesCaseInsensitiveCommonBreachedPassword = PasswordCompositionRequirements.CommonBreachedPasswords.Contains(plaintextPassword.ToLower());
            });

            if (totalCharacterCount < requirements.MinimumTotalCharacterCount)
            {
                return false;
            }
            else if (lowercaseAlphabeticCharacterCount < requirements.MinimumLowercaseAlphabeticCharacterCount)
            {
                return false;
            }
            else if (uppercaseAlphabeticCharacterCount < requirements.MinimumUppercaseAlphabeticCharacterCount)
            {
                return false;
            }
            else if (numericCharacterCount < requirements.MinimumNumericCharacterCount)
            {
                return false;
            }
            else if (nonAlphanumericCharacterCount < requirements.MinimumNonAlphanumericCharacterCount)
            {
                return false;
            }
            else if (requirements.ForbidCommonBreachedPasswords && matchesCaseInsensitiveCommonBreachedPassword)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a new byte array containing the plaintext password bytes for the specified <see cref="IPassword" />, using the
        /// originally-specified text encoding.
        /// </summary>
        /// <returns>
        /// A new byte array containing the plaintext password bytes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="password" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        internal static Byte[] GetPasswordPlaintextBytes(IPassword password)
        {
            var byteArray = (Byte[])null;

            password.RejectIf().IsNull(nameof(password)).TargetArgument.Read((IReadOnlyPinnedMemory<Byte> plaintextBytes) =>
            {
                byteArray = plaintextBytes.ReadOnlySpan.ToArray();
            });

            return byteArray;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Password" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Creates a new <see cref="Password" /> using the specified name and value.
        /// </summary>
        /// <param name="password">
        /// The plaintext password.
        /// </param>
        /// <param name="name">
        /// A textual name that uniquely identifies the password.
        /// </param>
        /// <param name="encoding">
        /// The encoding that is use when converting the password string to and from bytes.
        /// </param>
        /// <returns>
        /// A new <see cref="StringSecret" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty -or- <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" /> -or- <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="password" /> contains invalid characters for <paramref name="encoding" />.
        /// </exception>
        [DebuggerHidden]
        private static Password FromString(String password, String name, Encoding encoding)
        {
            password = password.RejectIf().IsNullOrEmpty(nameof(password));
            var secret = new Password(name, encoding);
            secret.Write(() => password);
            return secret;
        }

        /// <summary>
        /// Generates a new, unique default password name.
        /// </summary>
        /// <returns>
        /// A new, unique default password name.
        /// </returns>
        [DebuggerHidden]
        private static String NewDefaultPasswordName() => Secret.GetPrefixedSemanticIdentifier(DefaultPasswordNamePrefix, NewRandomSemanticIdentifier());

        /// <summary>
        /// Represents the default textual prefix for <see cref="Password" /> names.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DefaultPasswordNamePrefix = "Password";

        /// <summary>
        /// Represents the hashing algorithm that is used to produce cryptographically secure digests for <see cref="Password" />
        /// instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const HashingAlgorithmSpecification HashingAlgorithm = HashingAlgorithmSpecification.Argon2idBalanced;

        /// <summary>
        /// Represents the lower boundary for the length of randomly-generated strong passwords.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Int32 RandomStrongPasswordLengthLowerBoundary = PasswordCompositionRequirements.Strict.MinimumTotalCharacterCount;

        /// <summary>
        /// Represents the upper boundary for the length of randomly-generated strong passwords.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Int32 RandomStrongPasswordLengthUpperBoundary = RandomStrongPasswordLengthLowerBoundary + 5;
    }
}