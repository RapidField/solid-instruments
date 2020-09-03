// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AssociatedDomainModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRole
{
    /// <summary>
    /// Represents a user role.
    /// </summary>
    /// <remarks>
    /// This is the data declaration for a concrete value data access model. Value models expose the foundational schema for a model group
    /// and are appropriate for use in any context in which basic or identifying information is needed. Data access models are
    /// in-memory representations of data entities and are used to perform data access operations. The following are guidelines for
    /// use of this declaration.
    /// - DO declare data properties with public getters and setters.
    /// - DO decorate data properties with <see cref="ColumnAttribute" />, <see cref="DataMemberAttribute" />.
    /// - DO decorate data properties with other data annotations (eg. <see cref="RequiredAttribute" />), as needed.
    /// - DO NOT specify class inheritance or interface implementation(s).
    /// - DO NOT decorate the class with attributes.
    /// - DO NOT declare constructors.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public partial class ValueDataAccessModel
    {
        /// <summary>
        /// Gets or sets the description of the current <see cref="ValueDataAccessModel" />.
        /// </summary>
        [Column]
        [DataMember]
        [Required]
        [StringLength(AssociatedDomainModel.DescriptionValueMaximumLength)]
        public String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the current <see cref="ValueDataAccessModel" />.
        /// </summary>
        [Column]
        [DataMember]
        [Required]
        [StringLength(AssociatedDomainModel.NameValueMaximumLength)]
        public String Name
        {
            get;
            set;
        }
    }
}