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
using UserRoleAssignmentModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.DomainModel;

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
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="EmailAddress" /> is too long.
        /// </exception>
        [DataMember]
        public String EmailAddress
        {
            get => EmailAddressValue;
            set => EmailAddressValue = value.RejectIf().IsNullOrEmpty(nameof(EmailAddress)).OrIf().LengthIsGreaterThan(EmailAddressValueMaximumLength, nameof(EmailAddress));
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="Name" /> is too long.
        /// </exception>
        [DataMember]
        public String Name
        {
            get => NameValue;
            set => NameValue = value.RejectIf().IsNullOrEmpty(nameof(Name)).OrIf().LengthIsGreaterThan(EmailAddressValueMaximumLength, nameof(Name));
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="PasswordHash" /> is too long.
        /// </exception>
        [DataMember]
        public String PasswordHash
        {
            get => PasswordHashValue;
            set => PasswordHashValue = value.RejectIf().IsNullOrEmpty(nameof(PasswordHash)).OrIf().LengthIsGreaterThan(PasswordHashValueMaximumLength, nameof(PasswordHash));
        }

        /// <summary>
        /// Gets a collection of user roles which are assigned to the current <see cref="DomainModel" />.
        /// </summary>
        [DataMember]
        public ICollection<UserRoleAssignmentModel> UserRoleAssignments
        {
            get
            {
                if (UserRoleAssignmentsValue is null)
                {
                    UserRoleAssignmentsValue = new List<UserRoleAssignmentModel>();
                }

                return UserRoleAssignmentsValue;
            }
        }

        /// <summary>
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public const String DataContractName = nameof(User);

        /// <summary>
        /// Represents the maximum email address string length for <see cref="DomainModel" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 EmailAddressValueMaximumLength = 320;

        /// <summary>
        /// Represents the maximum name string length for <see cref="DomainModel" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 NameValueMaximumLength = 89;

        /// <summary>
        /// Represents the maximum hashed password string length for <see cref="DomainModel" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 PasswordHashValueMaximumLength = 89;

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
        /// Represents a collection of user roles which are assigned to the current <see cref="DomainModel" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private List<UserRoleAssignmentModel> UserRoleAssignmentsValue;

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