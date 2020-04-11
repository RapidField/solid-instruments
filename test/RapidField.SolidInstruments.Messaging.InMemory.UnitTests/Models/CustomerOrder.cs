// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models
{
    /// <summary>
    /// Represents an order placed by a customer.
    /// </summary>
    [DataContract]
    internal sealed class CustomerOrder : GlobalIdentityDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerOrder" /> class.
        /// </summary>
        public CustomerOrder()
            : base()
        {
            Customer = null;
            PlacementTimeStamp = default;
            Products = new List<Product>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerOrder" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Guid" />.
        /// </param>
        /// <param name="name">
        /// The customer that placed the order.
        /// </param>
        public CustomerOrder(Guid identifier, Customer customer)
            : this(identifier, customer, TimeStamp.Current)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerOrder" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Guid" />.
        /// </param>
        /// <param name="name">
        /// The customer that placed the order.
        /// </param>
        /// <param name="placementTimeStamp">
        /// The date and time when the order was placed.
        /// </param>
        public CustomerOrder(Guid identifier, Customer customer, DateTime placementTimeStamp)
            : this(identifier, customer, placementTimeStamp, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerOrder" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Guid" />.
        /// </param>
        /// <param name="name">
        /// The customer that placed the order.
        /// </param>
        /// <param name="placementTimeStamp">
        /// The date and time when the order was placed.
        /// </param>
        /// <param name="products">
        /// The constituent produces that are or were ordered by <paramref name="customer" />.
        /// </param>
        public CustomerOrder(Guid identifier, Customer customer, DateTime placementTimeStamp, IEnumerable<Product> products)
            : base(identifier)
        {
            Customer = customer;
            PlacementTimeStamp = placementTimeStamp;
            Products = new List<Product>(products ?? Array.Empty<Product>());
        }

        /// <summary>
        /// Gets or sets the customer that placed the current <see cref="CustomerOrder" />.
        /// </summary>
        [DataMember]
        public Customer Customer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date and time when the current <see cref="CustomerOrder" /> was placed.
        /// </summary>
        [DataMember]
        public DateTime PlacementTimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the constituent products that are or were ordered by the associated <see cref="Customer" />.
        /// </summary>
        [DataMember]
        public ICollection<Product> Products
        {
            get;
            set;
        }
    }
}