// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public interface IAggregateModel : IValueModel
    {
        /// <summary>
        /// Gets the hashed password for the current <see cref="IAggregateModel" />.
        /// </summary>
        public String PasswordHash
        {
            get;
        }
    }
}