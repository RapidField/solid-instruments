// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using BaseDomainModel = RapidField.SolidInstruments.Core.Domain.GlobalIdentityDomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
    /// </summary>
    /// <remarks>
    /// This is the root declaration for a concrete aggregate domain model. Aggregate models expose the full schema for a model
    /// group and are appropriate for use in any context in which detail-level information is needed. Domain models represent domain
    /// constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO specify class inheritance and interface implementation(s).
    /// - DO derive this class (inherit) from <see cref="BaseDomainModel" />.
    /// - DO implement <see cref="IAggregateDomainModel" />.
    /// - DO decorate the class with <see cref="DataContractAttribute" />.
    /// - DO specify a custom, unique data contract name.
    /// - DO declare a public, parameterless constructor.
    /// - DO declare an internal constructor which accepts an identifier value.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    [DataContract(Name = DataContractName)]
    public sealed partial class DomainModel : BaseDomainModel, IAggregateDomainModel
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
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public const String DataContractName = nameof(UserRoleAssignment);
    }
}