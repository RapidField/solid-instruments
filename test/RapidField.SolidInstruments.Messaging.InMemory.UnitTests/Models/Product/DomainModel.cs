// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using BaseDomainModel = RapidField.SolidInstruments.Core.Domain.GlobalIdentityDomainModel;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Product
{
    /// <summary>
    /// Represents an item of merchandise.
    /// </summary>
    [DataContract(Name = DataContractName)]
    internal sealed class DomainModel : BaseDomainModel, IAggregate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModel" /> class.
        /// </summary>
        [DebuggerHidden]
        internal DomainModel()
            : base()
        {
            return;
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
        internal DomainModel(Guid identifier)
            : this()
        {
            Identifier = identifier.RejectIf().IsEqualToValue(default, nameof(identifier));
        }

        /// <summary>
        /// Gets or sets the name of the current <see cref="DomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="Name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Name" /> is <see langword="null" />.
        /// </exception>
        [DataMember]
        public String Name
        {
            get => NameValue;
            set => NameValue = value.RejectIf().IsNullOrEmpty(nameof(Name));
        }

        /// <summary>
        /// Gets or sets the price of the current <see cref="DomainModel" />, or <see langword="null" /> if the price is unknown.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="Price" /> is less than zero.
        /// </exception>
        [DataMember]
        public Decimal? Price
        {
            get => PriceValue;
            set => PriceValue = value?.RejectIf().IsLessThan(0, nameof(Price));
        }

        /// <summary>
        /// Represents the name that is used when representing this current type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DataContractName = "Product";

        /// <summary>
        /// Represents the name of the current <see cref="DomainModel" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String NameValue;

        /// <summary>
        /// Represents the price of the current <see cref="DomainModel" />, or <see langword="null" /> if the price is unknown.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Decimal? PriceValue;

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
            internal static IEnumerable<DomainModel> All() => new DomainModel[]
            {
                Fidget,
                Widget
            };

            /// <summary>
            /// Gets the Fidget product.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal static DomainModel Fidget => new DomainModel(Guid.Parse("dc177df0-2b54-44e6-a166-55c3b4403d5f"))
            {
                Name = "Fidget",
                Price = 1.99m
            };

            /// <summary>
            /// Gets the Widget product.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal static DomainModel Widget => new DomainModel(Guid.Parse("acec6baf-66e2-49c1-92e5-37f1b9722e73"))
            {
                Name = "Widget",
                Price = 13.69m
            };
        }
    }
}