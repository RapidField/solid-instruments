﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityValueDomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public interface IValueDomainModel : IBaseDomainModel, IValueModel
    {
    }
}