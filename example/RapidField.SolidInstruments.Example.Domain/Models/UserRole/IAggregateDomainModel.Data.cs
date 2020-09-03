// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRole
{
    /// <summary>
    /// Represents a user role.
    /// </summary>
    /// <remarks>
    /// This is the data declaration for an aggregate domain model interface. Aggregate models expose the full schema for a model
    /// group and are appropriate for use in any context in which detail-level information is needed. Domain models represent domain
    /// constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO declare data properties with public getters and setters, using the <see langword="new" /> keyword as needed.
    /// - DO NOT specify interface implementation(s).
    /// - DO NOT decorate the interface with attributes.
    /// - DO NOT decorate data properties with attributes.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public partial interface IAggregateDomainModel
    {
        /// <summary>
        /// Gets or sets the description for the current <see cref="IAggregateDomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="Description" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Description" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Description" /> is too long.
        /// </exception>
        public new String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the current <see cref="IAggregateDomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="Name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Name" /> is too long.
        /// </exception>
        public new String Name
        {
            get;
            set;
        }
    }
}