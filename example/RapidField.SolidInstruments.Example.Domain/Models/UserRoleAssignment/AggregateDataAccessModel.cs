// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.Serialization;
using UserModel = RapidField.SolidInstruments.Example.Domain.Models.User.AggregateDataAccessModel;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.AggregateDataAccessModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
    /// </summary>
    [DataContract(Name = DataContractName)]
    [Table(TableName)]
    public sealed class AggregateDataAccessModel : ValueDataAccessModel, IAggregateModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateDataAccessModel" /> class.
        /// </summary>
        public AggregateDataAccessModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Gets or sets the user to which a user role is assigned.
        /// </summary>
        [ForeignKey(nameof(UserIdentifier))]
        [IgnoreDataMember]
        public UserModel User
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that uniquely identifies the user to which a user role is assigned.
        /// </summary>
        [Column]
        [DataMember]
        [Required]
        public Guid UserIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user role that is a assigned to a user.
        /// </summary>
        [ForeignKey(nameof(UserRoleIdentifier))]
        [IgnoreDataMember]
        public UserRoleModel UserRole
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that uniquely identifies the user role that is assigned to a user.
        /// </summary>
        [Column]
        [DataMember]
        [Required]
        public Guid UserRoleIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the name that is used when representing this type as a database entity.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal new const String TableName = ValueDataAccessModel.TableName;

        /// <summary>
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DataContractName = TableName + nameof(AggregateDataAccessModel);
    }
}