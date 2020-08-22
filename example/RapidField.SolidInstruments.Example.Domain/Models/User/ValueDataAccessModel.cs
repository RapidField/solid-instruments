// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.Serialization;
using AssociatedDomainModel = RapidField.SolidInstruments.Example.Domain.Models.User.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    using BaseDataAccessModel = DataAccess.EntityFramework.EntityFrameworkGlobalIdentityDataAccessModel<AssociatedDomainModel>;

    /// <summary>
    /// Represents a user.
    /// </summary>
    [DataContract(Name = DataContractName)]
    [Table(TableName)]
    public class ValueDataAccessModel : BaseDataAccessModel, IValueModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueDataAccessModel" /> class.
        /// </summary>
        public ValueDataAccessModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Gets or sets the email address of the current <see cref="ValueDataAccessModel" />.
        /// </summary>
        [Column(Order = 1)]
        [DataMember]
        [Required]
        [StringLength(320)]
        public String EmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the current <see cref="ValueDataAccessModel" />.
        /// </summary>
        [Column(Order = 2)]
        [DataMember]
        [Required]
        [StringLength(89)]
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the name that is used when representing this type as a database entity.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String TableName = AssociatedDomainModel.DataContractName;

        /// <summary>
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DataContractName = TableName + nameof(ValueDataAccessModel);
    }
}