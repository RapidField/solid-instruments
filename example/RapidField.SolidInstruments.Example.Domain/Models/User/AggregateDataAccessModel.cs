// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
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
        /// Gets or sets the hashed password for the current <see cref="AggregateDataAccessModel" />.
        /// </summary>
        [Column(Order = 3)]
        [DataMember]
        [Required]
        [StringLength(89)]
        public String PasswordHash
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