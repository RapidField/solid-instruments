// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.Serialization;
using AssociatedDomainModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    using BaseDataAccessModel = DataAccess.EntityFramework.EntityFrameworkGlobalIdentityDataAccessModel<AssociatedDomainModel>;

    /// <summary>
    /// Represents the assignment of a single user role to a single user.
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