// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Cryptography.Secrets;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    /// <remarks>
    /// This is the named entity declaration for a concrete aggregate domain model. Aggregate models expose the full schema for a
    /// model group and are appropriate for use in any context in which detail-level information is needed. Domain models represent
    /// domain constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO declare the public static class <see cref="Named" />.
    /// - DO declare static <see cref="DomainModel" /> properties with public getters which create and return known entities.
    /// - DO declare the public static method <see cref="Named.All" /> which returns all known entities.
    /// - DO NOT specify class inheritance or interface implementation(s).
    /// - DO NOT decorate the class with attributes.
    /// - DO NOT declare constructors.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public sealed partial class DomainModel
    {
        /// <summary>
        /// Contains a collection of known <see cref="DomainModel" /> instances.
        /// </summary>
        public static class Named
        {
            /// <summary>
            /// Returns a collection of all known <see cref="DomainModel" /> instances.
            /// </summary>
            /// <returns>
            /// A collection of all known <see cref="DomainModel" /> instances.
            /// </returns>
            public static IEnumerable<DomainModel> All() => new DomainModel[]
            {
                StevenCallahan,
                TomSmith
            };

            /// <summary>
            /// Calculates a secure, Base64-encoded hash value for the specified password.
            /// </summary>
            /// <param name="plaintextPassword">
            /// The plaintext password.
            /// </param>
            /// <returns>
            /// The resulting hash value.
            /// </returns>
            /// <exception cref="ArgumentEmptyException">
            /// <paramref name="plaintextPassword" /> is empty.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="plaintextPassword" /> is <see langword="null" />.
            /// </exception>
            [DebuggerHidden]
            private static String GetPasswordHash(String plaintextPassword)
            {
                using var password = Password.FromAsciiString(plaintextPassword);
                return password.CalculateSecureHashString();
            }

            /// <summary>
            /// Gets the Steven Callahan user.
            /// </summary>
            public static DomainModel StevenCallahan => new DomainModel(Guid.Parse("a04fc5b0-1a67-43ff-89af-750128398d8a"))
            {
                EmailAddress = "steven.callahan@example.com",
                Name = "Steven Callahan",
                PasswordHash = GetPasswordHash("My name is Steven. 321")
            };

            /// <summary>
            /// Gets the Tom Smith user.
            /// </summary>
            public static DomainModel TomSmith => new DomainModel(Guid.Parse("3d46470a-1c90-4e94-bc5c-3cbde44ba6ac"))
            {
                EmailAddress = "tom.smith@example.com",
                Name = "Tom Smith",
                PasswordHash = GetPasswordHash("My name is Tom. 321")
            };
        }
    }
}