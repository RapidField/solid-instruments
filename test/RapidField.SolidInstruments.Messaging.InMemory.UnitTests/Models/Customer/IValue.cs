// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityValueDomainModel;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Customer
{
    /// <summary>
    /// Represents a customer.
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
    }
}