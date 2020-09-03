// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityAggregateDomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRole
{
    /// <summary>
    /// Represents a user role.
    /// </summary>
    /// <remarks>
    /// This is the root declaration for an aggregate domain model interface. Aggregate models expose the full schema for a model
    /// group and are appropriate for use in any context in which detail-level information is needed. Domain models represent domain
    /// constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO specify interface implementation(s).
    /// - DO implement <see cref="IAggregateModel" />, <see cref="IBaseDomainModel" /> and <see cref="IValueDomainModel" />.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public partial interface IAggregateDomainModel : IAggregateModel, IBaseDomainModel, IValueDomainModel
    {
    }
}