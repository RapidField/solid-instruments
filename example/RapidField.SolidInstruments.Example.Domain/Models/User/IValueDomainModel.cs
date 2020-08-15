// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityValueDomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public interface IValueDomainModel : IBaseDomainModel
    {
        /// <summary>
        /// Gets the email address for the current <see cref="IValueDomainModel" />.
        /// </summary>
        public String EmailAddress
        {
            get;
        }

        /// <summary>
        /// Gets the name of the current <see cref="IValueDomainModel" />.
        /// </summary>
        public String Name
        {
            get;
        }
    }
}