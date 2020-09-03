// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityValueDomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    /// <remarks>
    /// This is the root declaration for a value domain model interface. Value models expose the foundational schema for a model
    /// group and are appropriate for use in any context in which basic or identifying information is needed. Domain models
    /// represent domain constructs and define their characteristics and behavior. The following are guidelines for use of this
    /// declaration.
    /// - DO specify interface implementation(s).
    /// - DO implement <see cref="IBaseDomainModel" /> and <see cref="IValueModel" />.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public partial interface IValueDomainModel : IBaseDomainModel, IValueModel
    {
    }
}