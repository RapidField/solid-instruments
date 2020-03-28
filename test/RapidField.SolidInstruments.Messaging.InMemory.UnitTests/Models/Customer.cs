// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models
{
    /// <summary>
    /// Represents a customer.
    /// </summary>
    [DataContract]
    internal sealed class Customer : GlobalIdentityDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Customer" /> class.
        /// </summary>
        public Customer()
            : base()
        {
            Name = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Guid" />.
        /// </param>
        /// <param name="name">
        /// The name of the customer.
        /// </param>
        public Customer(Guid identifier, String name)
            : base(identifier)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the current <see cref="Customer" />.
        /// </summary>
        [DataMember]
        public String Name
        {
            get;
            set;
        }
    }
}