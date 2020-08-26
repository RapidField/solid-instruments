// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityAggregateDomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRole
{
    /// <summary>
    /// Represents a user role.
    /// </summary>
    public interface IAggregateDomainModel : IAggregateModel, IBaseDomainModel, IValueDomainModel
    {
        /// <summary>
        /// Gets or sets the description for the current <see cref="IAggregateDomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="Description" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Description" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Description" /> is too long.
        /// </exception>
        public new String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the current <see cref="IAggregateDomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="Name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Name" /> is too long.
        /// </exception>
        public new String Name
        {
            get;
            set;
        }
    }
}