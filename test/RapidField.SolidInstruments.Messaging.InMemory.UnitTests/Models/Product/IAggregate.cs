// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityAggregateDomainModel;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Product
{
    /// <summary>
    /// Represents an item of merchandise.
    /// </summary>
    internal interface IAggregate : IBaseDomainModel, IValue
    {
        /// <summary>
        /// Gets or sets the name of the current <see cref="IAggregate" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="Name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Name" /> is <see langword="null" />.
        /// </exception>
        public new String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the price of the current <see cref="IAggregate" />, or <see langword="null" /> if the price is unknown.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="Price" /> is less than zero.
        /// </exception>
        public new Decimal? Price
        {
            get;
            set;
        }
    }
}