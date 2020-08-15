// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Cryptography.Secrets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using BaseDomainModel = RapidField.SolidInstruments.Core.Domain.GlobalIdentityDomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    [DataContract(Name = DataContractName)]
    public sealed class DomainModel : BaseDomainModel, IAggregateDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModel" /> class.
        /// </summary>
        public DomainModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the domain model.
        /// </param>
        [DebuggerHidden]
        internal DomainModel(Guid identifier)
            : base(identifier)
        {
            return;
        }

        /// <summary>
        /// Gets or sets the email address of the current <see cref="DomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="EmailAddress" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="EmailAddress" /> is <see langword="null" />.
        /// </exception>
        [DataMember]
        public String EmailAddress
        {
            get => EmailAddressValue;
            set => EmailAddressValue = value.RejectIf().IsNullOrEmpty(nameof(EmailAddress));
        }

        /// <summary>
        /// Gets or sets the name of the current <see cref="DomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="Name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Name" /> is <see langword="null" />.
        /// </exception>
        [DataMember]
        public String Name
        {
            get => NameValue;
            set => NameValue = value.RejectIf().IsNullOrEmpty(nameof(Name));
        }

        /// <summary>
        /// Gets or sets the hashed password for the current <see cref="DomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="PasswordHash" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="PasswordHash" /> is <see langword="null" />.
        /// </exception>
        [DataMember]
        public String PasswordHash
        {
            get => PasswordHashValue;
            set => PasswordHashValue = value.RejectIf().IsNullOrEmpty(nameof(PasswordHash));
        }

        /// <summary>
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DataContractName = "User";

        /// <summary>
        /// Represents the email address for the current <see cref="DomainModel" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String EmailAddressValue;

        /// <summary>
        /// Represents the name of the current <see cref="DomainModel" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String NameValue;

        /// <summary>
        /// Represents the hashed password for the current <see cref="DomainModel" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String PasswordHashValue;

        /// <summary>
        /// Contains a collection of known <see cref="DomainModel" /> instances.
        /// </summary>
        internal static class Named
        {
            /// <summary>
            /// Returns a collection of all known <see cref="DomainModel" /> instances.
            /// </summary>
            /// <returns>
            /// A collection of all known <see cref="DomainModel" /> instances.
            /// </returns>
            [DebuggerHidden]
            internal static IEnumerable<DomainModel> All() => new DomainModel[]
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
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal static DomainModel StevenCallahan => new DomainModel(Guid.Parse("a04fc5b0-1a67-43ff-89af-750128398d8a"))
            {
                EmailAddress = "steven.callahan@example.com",
                Name = "Steven Callahan",
                PasswordHash = GetPasswordHash("My name is Steven. 321")
            };

            /// <summary>
            /// Gets the Tom Smith user.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal static DomainModel TomSmith => new DomainModel(Guid.Parse("01b7e51c-95e1-4eb1-aba6-1db0657a0fa3"))
            {
                EmailAddress = "tom.smith@example.com",
                Name = "Tom Smith",
                PasswordHash = GetPasswordHash("My name is Tom. 321")
            };
        }
    }
}