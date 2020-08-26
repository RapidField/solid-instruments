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

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRole
{
    /// <summary>
    /// Represents a user role.
    /// </summary>
    [DataContract(Name = DataContractName)]
    public sealed class DomainModel : BaseDomainModel, IAggregateDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModel" /> class.
        /// </summary>
        public DomainModel()
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
        /// Gets or sets the description of the current <see cref="DomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="Description" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Description" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Description" /> is too long.
        /// </exception>
        [DataMember]
        public String Description
        {
            get => DescriptionValue;
            set => DescriptionValue = value.RejectIf().IsNullOrEmpty(nameof(Description)).OrIf().LengthIsGreaterThan(DescriptionValueMaximumLength, nameof(Description));
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
        /// <exception cref="ArgumentNullException">
        /// <see cref="Name" /> is too long.
        /// </exception>
        [DataMember]
        public String Name
        {
            get => NameValue;
            set => NameValue = value.RejectIf().IsNullOrEmpty(nameof(Name)).OrIf().LengthIsGreaterThan(NameValueMaximumLength, nameof(Name));
        }

        /// <summary>
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DataContractName = nameof(UserRole);

        /// <summary>
        /// Represents the maximum description string length for <see cref="DomainModel" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 DescriptionValueMaximumLength = 89;

        /// <summary>
        /// Represents the maximum name string length for <see cref="DomainModel" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 NameValueMaximumLength = 55;

        /// <summary>
        /// Represents the description for the current <see cref="DomainModel" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String DescriptionValue;

        /// <summary>
        /// Represents the name of the current <see cref="DomainModel" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String NameValue;

        /// <summary>
        /// Contains a collection of known <see cref="DomainModel" /> instances.
        /// </summary>
        public static class Named
        {
            /// <summary>
            /// Returns a collection of all known <see cref="DomainModel" /> instances.
            /// </summary>
            /// <returns>
            /// A collection of all known <see cref="DomainModel" /> instances.
            /// </returns>
            public static IEnumerable<DomainModel> All() => new DomainModel[]
            {
                EndUser,
                SystemAdministrator
            };

            /// <summary>
            /// Gets the end user role.
            /// </summary>
            public static DomainModel EndUser => new DomainModel(Guid.Parse("816e7126-2034-49b3-af08-d07cab150d93"))
            {
                Description = "A standard end user.",
                Name = "End User"
            };

            /// <summary>
            /// Gets the system administrator user role.
            /// </summary>
            public static DomainModel SystemAdministrator => new DomainModel(Guid.Parse("b13c7a39-0c65-4515-a4af-2e3f60d289ba"))
            {
                Description = "A user with full administrative privileges.",
                Name = "System Administrator"
            };
        }
    }
}