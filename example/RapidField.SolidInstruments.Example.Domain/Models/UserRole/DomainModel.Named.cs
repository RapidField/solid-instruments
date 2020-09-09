// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRole
{
    /// <summary>
    /// Represents a user role.
    /// </summary>
    /// <remarks>
    /// This is the named entity declaration for a concrete aggregate domain model. Aggregate models expose the full schema for a
    /// model group and are appropriate for use in any context in which detail-level information is needed. Domain models represent
    /// domain constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO declare the public static class <see cref="Named" />.
    /// - DO declare static <see cref="DomainModel" /> properties with public getters which create and return known entities.
    /// - DO declare the public static method <see cref="Named.All" /> which returns all known entities.
    /// - DO NOT specify class inheritance or interface implementation(s).
    /// - DO NOT decorate the class with attributes.
    /// - DO NOT declare constructors.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public sealed partial class DomainModel
    {
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
                Name = nameof(EndUser)
            };

            /// <summary>
            /// Gets the system administrator user role.
            /// </summary>
            public static DomainModel SystemAdministrator => new DomainModel(Guid.Parse("b13c7a39-0c65-4515-a4af-2e3f60d289ba"))
            {
                Description = "A user with full administrative privileges.",
                Name = nameof(SystemAdministrator)
            };
        }
    }
}