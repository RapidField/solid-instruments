// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using BaseDomainModel = RapidField.SolidInstruments.Core.Domain.GlobalIdentityDomainModel;
using CustomerModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Customer.DomainModel;
using ProductModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Product.DomainModel;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.CustomerOrder
{
    /// <summary>
    /// Represents an order placed by a customer.
    /// </summary>
    [DataContract]
    internal sealed class DomainModel : BaseDomainModel, IAggregate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModel" /> class.
        /// </summary>
        [DebuggerHidden]
        internal DomainModel()
            : base()
        {
            Products = new List<ProductModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModel" /> class.
        /// </summary>
        /// <param name="model">
        /// The domain model to replicate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal DomainModel(IValue model)
            : this(model.RejectIf().IsNull(nameof(model)).TargetArgument.Identifier)
        {
            PlacementTimeStamp = model.PlacementTimeStamp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the domain model.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        private DomainModel(Guid identifier)
            : this()
        {
            Identifier = identifier.RejectIf().IsEqualToValue(default, nameof(identifier));
        }

        /// <summary>
        /// Gets or sets the customer that placed the current <see cref="DomainModel" />.
        /// </summary>
        [DataMember]
        public CustomerModel Customer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date and time when the current <see cref="DomainModel" /> was placed.
        /// </summary>
        [DataMember]
        public DateTime PlacementTimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the constituent products that are or were ordered by the associated <see cref="CustomerModel" />.
        /// </summary>
        [DataMember]
        public ICollection<ProductModel> Products
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the total cost of the current <see cref="DomainModel" />.
        /// </summary>
        [IgnoreDataMember]
        public Decimal TotalCost => Products.Select(product => product.Price ?? 0m).Sum();

        /// <summary>
        /// Contains a collection of known <see cref="DomainModel" /> instances.
        /// </summary>
        internal static class Named
        {
            /// <summary>
            /// Returns a collection of all known <see cref="DomainModel" /> instances.
            /// </summary>
            /// <returns>
            /// A collection of all known <see cref="DomainModel" /> instances.
            /// </returns>
            [DebuggerHidden]
            internal static IEnumerable<DomainModel> All() => Array.Empty<DomainModel>();
        }
    }
}