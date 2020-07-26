// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityValueDomainModel;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.CustomerOrder
{
    /// <summary>
    /// Represents an order placed by a customer.
    /// </summary>
    internal interface IValue : IBaseDomainModel
    {
        /// <summary>
        /// Gets the date and time when the current <see cref="IValue" /> was placed.
        /// </summary>
        public DateTime PlacementTimeStamp
        {
            get;
        }
    }
}