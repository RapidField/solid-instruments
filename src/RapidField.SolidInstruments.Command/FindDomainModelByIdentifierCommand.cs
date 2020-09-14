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
    /// Represents a command to find and return an object that models a domain construct by its primary identifier.
    /// </summary>
    /// <remarks>
    /// <see cref="FindDomainModelByIdentifierCommand{TIdentifier, TModel}" /> is the default implementation of
    /// <see cref="IFindDomainModelByIdentifierCommand{TIdentifier, TModel}" />.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    [DataContract]
    public class FindDomainModelByIdentifierCommand<TIdentifier, TModel> : FindDomainModelCommand<TModel>, IFindDomainModelByIdentifierCommand<TIdentifier, TModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TModel : class, IDomainModel<TIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelByIdentifierCommand{TIdentifier, TModel}" /> class.
        /// </summary>
        public FindDomainModelByIdentifierCommand()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelByIdentifierCommand{TIdentifier, TModel}" /> class.
        /// </summary>
        /// <param name="modelIdentifier">
        /// A value that uniquely identifies the associated <see cref="IDomainModel{TIdentifier}" />.
        /// </param>
        public FindDomainModelByIdentifierCommand(TIdentifier modelIdentifier)
            : base()
        {
            ModelIdentifier = modelIdentifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelByIdentifierCommand{TIdentifier, TModel}" /> class.
        /// </summary>
        /// <param name="modelIdentifier">
        /// A value that uniquely identifies the associated <see cref="IDomainModel{TIdentifier}" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public FindDomainModelByIdentifierCommand(TIdentifier modelIdentifier, IEnumerable<String> labels)
            : base(labels)
        {
            ModelIdentifier = modelIdentifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelByIdentifierCommand{TIdentifier, TModel}" /> class.
        /// </summary>
        /// <param name="modelIdentifier">
        /// A value that uniquely identifies the associated <see cref="IDomainModel{TIdentifier}" />.
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
        public FindDomainModelByIdentifierCommand(TIdentifier modelIdentifier, IEnumerable<String> labels, Guid correlationIdentifier)
            : base(labels, correlationIdentifier)
        {
            ModelIdentifier = modelIdentifier;
        }

        /// <summary>
        /// Gets or sets a value that uniquely identifies the associated <see cref="IDomainModel{TIdentifier}" />.
        /// </summary>
        [DataMember]
        public TIdentifier ModelIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the standard preposition which is included in the name that is used when representing this type in
        /// serialization and transport contexts.
        /// </summary>
        protected internal const String DataContractNamePreposition = "ByIdentifier";
    }
}