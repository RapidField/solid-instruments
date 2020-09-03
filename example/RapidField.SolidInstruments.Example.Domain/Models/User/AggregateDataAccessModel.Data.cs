// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AssociatedDomainModel = RapidField.SolidInstruments.Example.Domain.Models.User.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    /// <remarks>
    /// This is the data declaration for a concrete aggregate data access model. Aggregate models expose the full schema for a model
    /// group and are appropriate for use in a system of record or in any context in which detail-level information is needed. Data
    /// access models are in-memory representations of data entities and are used to perform data access operations. The following
    /// are guidelines for use of this declaration.
    /// - DO declare data properties with public getters and setters.
    /// - DO decorate data properties with <see cref="ColumnAttribute" />, <see cref="DataMemberAttribute" />.
    /// - DO decorate data properties with other data annotations (eg. <see cref="RequiredAttribute" />), as needed.
    /// - DO NOT specify class inheritance or interface implementation(s).
    /// - DO NOT decorate the class with attributes.
    /// - DO NOT declare constructors.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public sealed partial class AggregateDataAccessModel
    {
        /// <summary>
        /// Gets or sets the hashed password for the current <see cref="AggregateDataAccessModel" />.
        /// </summary>
        [Column]
        [DataMember]
        [Required]
        [StringLength(AssociatedDomainModel.PasswordHashValueMaximumLength)]
        public String PasswordHash
        {
            get;
            set;
        }
    }
}