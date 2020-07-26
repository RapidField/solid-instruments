// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Collections.Generic;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests
{
    /// <summary>
    /// Represents the state of a service for testing purposes.
    /// </summary>
    internal static class SimulatedServiceState
    {
        internal static readonly List<Models.CustomerOrder.DomainModel> CustomerOrders = new List<Models.CustomerOrder.DomainModel>();
        internal static readonly List<Models.Customer.DomainModel> Customers = new List<Models.Customer.DomainModel>();
        internal static readonly List<Models.Product.DomainModel> Products = new List<Models.Product.DomainModel>();
    }
}