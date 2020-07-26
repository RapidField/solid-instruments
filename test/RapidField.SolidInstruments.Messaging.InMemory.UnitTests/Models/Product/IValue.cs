// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityValueDomainModel;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Product
{
    /// <summary>
    /// Represents an item of merchandise.
    /// </summary>
    internal interface IValue : IBaseDomainModel
    {
        /// <summary>
        /// Gets the name of the current <see cref="IValue" />.
        /// </summary>
        public String Name
        {
            get;
        }

        /// <summary>
        /// Gets the price of the current <see cref="IValue" />, or <see langword="null" /> if the price is unknown.
        /// </summary>
        public Decimal? Price
        {
            get;
        }
    }
}