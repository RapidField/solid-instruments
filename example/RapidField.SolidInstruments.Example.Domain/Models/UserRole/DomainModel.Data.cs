﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRole
{
    /// <summary>
    /// Represents a user role.
    /// </summary>
    /// <remarks>
    /// This is the data declaration for a concrete aggregate domain model. Aggregate models expose the full schema for a model
    /// group and are appropriate for use in any context in which detail-level information is needed. Domain models represent domain
    /// constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO declare data properties with public getters and setters.
    /// - DO decorate data properties with <see cref="DataMemberAttribute" />.
    /// - DO add validation logic to data property setters to enforce state validity, as needed.
    /// - DO NOT specify class inheritance or interface implementation(s).
    /// - DO NOT decorate the class with attributes.
    /// - DO NOT declare constructors.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public sealed partial class DomainModel
    {
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
    }
}