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
    public interface IValueModel : IGlobalIdentityModel
    {
        /// <summary>
        /// Gets the description of the current <see cref="IValueModel" />.
        /// </summary>
        public String Description
        {
            get;
        }

        /// <summary>
        /// Gets the name of the current <see cref="IValueModel" />.
        /// </summary>
        public String Name
        {
            get;
        }
    }
}