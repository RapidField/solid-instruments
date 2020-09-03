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
    /// <remarks>
    /// This is the root declaration for a concrete value data access model. Value models expose the foundational schema for a model
    /// group and are appropriate for use in any context in which basic or identifying information is needed. Data access models are
    /// in-memory representations of data entities and are used to perform data access operations. The following are guidelines for
    /// use of this declaration.
    /// - DO specify class inheritance and interface implementation(s).
    /// - DO derive this class (inherit) from <see cref="BaseDataAccessModel" />.
    /// - DO implement <see cref="IValueModel" />.
    /// - DO decorate the class with <see cref="DataContractAttribute" /> and <see cref="TableAttribute" />.
    /// - DO specify a custom, unique data contract name and custom, unique table name.
    /// - DO declare a public, parameterless constructor.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    [DataContract(Name = DataContractName)]
    [Table(TableName)]
    public partial class ValueDataAccessModel : BaseDataAccessModel, IValueModel
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