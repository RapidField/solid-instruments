// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models
{
    /// <summary>
    /// Represents an item of merchandise.
    /// </summary>
    [DataContract]
    internal sealed class Product : GlobalIdentityDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product" /> class.
        /// </summary>
        public Product()
            : base()
        {
            Name = null;
            Price = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Guid" />.
        /// </param>
        /// <param name="name">
        /// The name of the product.
        /// </param>
        public Product(Guid identifier, String name)
            : this(identifier, name, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Guid" />.
        /// </param>
        /// <param name="name">
        /// The name of the product.
        /// </param>
        /// <param name="price">
        /// The price of the product, or <see langword="null" /> if the price is unknown.
        /// </param>
        public Product(Guid identifier, String name, Decimal? price)
            : base(identifier)
        {
            Name = name;
            Price = price;
        }

        /// <summary>
        /// Gets or sets the name of the current <see cref="Product" />.
        /// </summary>
        [DataMember]
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the price of the current <see cref="Product" />, or <see langword="null" /> if the price is unknown.
        /// </summary>
        [DataMember]
        public Decimal? Price
        {
            get;
            set;
        }
    }
}