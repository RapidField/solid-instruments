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

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Customer
{
    /// <summary>
    /// Represents a customer.
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
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the domain model.
        /// </param>
        [DebuggerHidden]
        internal DomainModel(Guid identifier)
            : base(identifier)
        {
            return;
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
        /// Represents the name of the current <see cref="DomainModel" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String NameValue;

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
                AcmeCo,
                SmithIndustries
            };

            /// <summary>
            /// Gets the ACME Co. customer.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal static DomainModel AcmeCo => new DomainModel(Guid.Parse("8e953a24-2d7f-4be1-96aa-c5288ad380ad"))
            {
                Name = "ACME Co."
            };

            /// <summary>
            /// Gets the Smith Industries customer.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal static DomainModel SmithIndustries => new DomainModel(Guid.Parse("c2e497e2-ab27-457c-808d-8dfd74c9748d"))
            {
                Name = "Smith Industries"
            };
        }
    }
}