// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Commands.ModelState.UserRole
{
    /// <summary>
    /// Represents a command to find and return a <see cref="DomainModel" /> by its primary identifier.
    /// </summary>
    [DataContract(Name = DataContractName)]
    public sealed class FindDomainModelByIdentifierCommand : FindDomainModelByIdentifierCommand<Guid, DomainModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelByIdentifierCommand" /> class.
        /// </summary>
        public FindDomainModelByIdentifierCommand()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelByIdentifierCommand" /> class.
        /// </summary>
        /// <param name="modelIdentifier">
        /// A value that uniquely identifies the associated <see cref="DomainModel" />.
        /// </param>
        public FindDomainModelByIdentifierCommand(Guid modelIdentifier)
            : base(modelIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelByIdentifierCommand" /> class.
        /// </summary>
        /// <param name="modelIdentifier">
        /// A value that uniquely identifies the associated <see cref="DomainModel" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public FindDomainModelByIdentifierCommand(Guid modelIdentifier, IEnumerable<String> labels)
            : base(modelIdentifier, labels)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelByIdentifierCommand" /> class.
        /// </summary>
        /// <param name="modelIdentifier">
        /// A value that uniquely identifies the associated <see cref="DomainModel" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public FindDomainModelByIdentifierCommand(Guid modelIdentifier, IEnumerable<String> labels, Guid correlationIdentifier)
            : base(modelIdentifier, labels, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DataContractName = DataContractNameVerb + DomainModel.DataContractName + DataContractNamePreposition + DataContractNameSuffix;
    }
}