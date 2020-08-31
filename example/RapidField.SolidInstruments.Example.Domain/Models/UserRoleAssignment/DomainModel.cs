// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using BaseDomainModel = RapidField.SolidInstruments.Core.Domain.GlobalIdentityDomainModel;
using UserModel = RapidField.SolidInstruments.Example.Domain.Models.User.DomainModel;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
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
        /// Gets the user to which a user role is assigned.
        /// </summary>
        [IgnoreDataMember]
        public UserModel User
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets a value that uniquely identifies the user to which a user role is assigned.
        /// </summary>
        [DataMember]
        public Guid UserIdentifier
        {
            get => User?.Identifier ?? Guid.Empty;
            set => User = new UserModel(value);
        }

        /// <summary>
        /// Gets the user role that is a assigned to a user.
        /// </summary>
        [IgnoreDataMember]
        public UserRoleModel UserRole
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets a value that uniquely identifies the user role that is assigned to a user.
        /// </summary>
        [DataMember]
        public Guid UserRoleIdentifier
        {
            get => UserRole?.Identifier ?? Guid.Empty;
            set => UserRole = new UserRoleModel(value);
        }

        /// <summary>
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public const String DataContractName = nameof(UserRoleAssignment);

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
                StevenCallahanSystemAdministrator,
                TomSmithEndUser
            };

            /// <summary>
            /// Gets the system administrator role assignment for Steven Callahan.
            /// </summary>
            public static DomainModel StevenCallahanSystemAdministrator => new DomainModel(Guid.Parse("3af59674-efe9-46ad-b8b3-847e1a74c53c"))
            {
                User = UserModel.Named.StevenCallahan,
                UserRole = UserRoleModel.Named.SystemAdministrator
            };

            /// <summary>
            /// Gets the end user role assignment for Tom Smith.
            /// </summary>
            public static DomainModel TomSmithEndUser => new DomainModel(Guid.Parse("472ceca5-e06b-4ae7-8022-ecb160822137"))
            {
                User = UserModel.Named.TomSmith,
                UserRole = UserRoleModel.Named.EndUser
            };
        }
    }
}