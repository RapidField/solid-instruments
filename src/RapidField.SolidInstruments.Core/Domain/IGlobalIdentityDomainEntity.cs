// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Domain
{
    /// <summary>
    /// Represents an object that models a domain entity and is identified primarily by a <see cref="Guid" /> value.
    /// </summary>
    public interface IGlobalIdentityDomainEntity : IDomainEntity<Guid>
    {
    }
}