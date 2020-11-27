﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    /// <remarks>
    /// This is the logic declaration for a value domain model interface. Value models expose the foundational schema for a model
    /// group and are appropriate for use in any context in which basic or identifying information is needed. Domain models
    /// represent domain constructs and define their characteristics and behavior. The following are guidelines for use of this
    /// declaration.
    /// - DO declare computed properties and domain logic methods.
    /// - DO NOT specify interface implementation(s).
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public partial interface IValueDomainModel
    {
    }
}