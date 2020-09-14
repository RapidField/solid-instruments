// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command to find and return an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="FindDomainModelCommand{TModel}" /> is the default implementation of
    /// <see cref="IFindDomainModelCommand{TModel}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    [DataContract]
    public class FindDomainModelCommand<TModel> : DomainCommand, IFindDomainModelCommand<TModel>
        where TModel : class, IDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelCommand{TModel}" /> class.
        /// </summary>
        public FindDomainModelCommand()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelCommand{TModel}" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public FindDomainModelCommand(Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelCommand{TModel}" /> class.
        /// </summary>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public FindDomainModelCommand(IEnumerable<String> labels)
            : base(labels)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelCommand{TModel}" /> class.
        /// </summary>
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
        public FindDomainModelCommand(IEnumerable<String> labels, Guid correlationIdentifier)
            : base(labels, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Represents the standard verb which is prefixed to the name that is used when representing this type in serialization and
        /// transport contexts.
        /// </summary>
        protected internal const String DataContractNameVerb = "Find";
    }
}