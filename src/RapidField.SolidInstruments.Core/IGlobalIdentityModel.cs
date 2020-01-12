// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that models a general construct and that is identified primarily by a <see cref="Guid" /> value.
    /// </summary>
    public interface IGlobalIdentityModel : IModel<Guid>
    {
    }
}