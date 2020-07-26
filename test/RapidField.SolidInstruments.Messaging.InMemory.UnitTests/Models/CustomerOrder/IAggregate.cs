// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using CustomerModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Customer.DomainModel;
using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityAggregateDomainModel;
using ProductModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Product.DomainModel;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.CustomerOrder
{
    /// <summary>
    /// Represents an order placed by a customer.
    /// </summary>
    internal interface IAggregate : IBaseDomainModel, IValue
    {
        /// <summary>
        /// Gets the customer that placed the current <see cref="IAggregate" />.
        /// </summary>
        public CustomerModel Customer
        {
            get;
        }

        /// <summary>
        /// Gets or sets the date and time when the current <see cref="IAggregate" /> was placed.
        /// </summary>
        public new DateTime PlacementTimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the constituent products that are or were ordered by the associated <see cref="CustomerModel" />.
        /// </summary>
        public ICollection<ProductModel> Products
        {
            get;
        }

        /// <summary>
        /// Gets the total cost of the current <see cref="IAggregate" />.
        /// </summary>
        public Decimal TotalCost
        {
            get;
        }
    }
}