// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
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
    }
}