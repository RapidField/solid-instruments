// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRole
{
    /// <summary>
    /// Represents a user role.
    /// </summary>
    /// <remarks>
    /// This is the root declaration for a shared aggregate model interface. Aggregate models expose the full schema for a model
    /// group and are appropriate for use in any context in which detail-level information is needed. Shared models define the
    /// characteristics which are shared between data access models and domain models. The following are guidelines for use of this
    /// declaration.
    /// - DO specify interface implementation(s).
    /// - DO implement <see cref="IValueModel" />.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public partial interface IAggregateModel : IValueModel
    {
    }
}