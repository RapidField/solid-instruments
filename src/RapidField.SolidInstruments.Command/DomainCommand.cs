// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command to perform a domain action.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainCommand" /> is the default implementation of <see cref="IDomainCommand" />.
    /// </remarks>
    [DataContract]
    public class DomainCommand : Command, IDomainCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommand" /> class.
        /// </summary>
        public DomainCommand()
            : this(Array.Empty<String>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommand" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainCommand(Guid correlationIdentifier)
            : this(Array.Empty<String>(), correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommand" /> class.
        /// </summary>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public DomainCommand(IEnumerable<String> labels)
            : base()
        {
            Labels = new List<String>(labels.RejectIf().IsNull(nameof(labels)).TargetArgument);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommand" /> class.
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
        public DomainCommand(IEnumerable<String> labels, Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            Labels = new List<String>(labels.RejectIf().IsNull(nameof(labels)).TargetArgument);
        }

        /// <summary>
        /// Gets or sets a collection of textual labels that provide categorical and/or contextual information about the current
        /// <see cref="DomainCommand" />.
        /// </summary>
        [DataMember]
        public ICollection<String> Labels
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents a command to perform a domain action.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainCommand{TResult}" /> is the default implementation of <see cref="IDomainCommand{TResult}" />.
    /// </remarks>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted when processing the command.
    /// </typeparam>
    [DataContract]
    public class DomainCommand<TResult> : Command<TResult>, IDomainCommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommand{TResult}" /> class.
        /// </summary>
        public DomainCommand()
            : this(Array.Empty<String>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommand{TResult}" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainCommand(Guid correlationIdentifier)
            : this(Array.Empty<String>(), correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommand{TResult}" /> class.
        /// </summary>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public DomainCommand(IEnumerable<String> labels)
            : base()
        {
            Labels = new List<String>(labels.RejectIf().IsNull(nameof(labels)).TargetArgument);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommand{TResult}" /> class.
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
        public DomainCommand(IEnumerable<String> labels, Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            Labels = new List<String>(labels.RejectIf().IsNull(nameof(labels)).TargetArgument);
        }

        /// <summary>
        /// Gets or sets a collection of textual labels that provide categorical and/or contextual information about the current
        /// <see cref="DomainCommand{TResult}" />.
        /// </summary>
        [DataMember]
        public ICollection<String> Labels
        {
            get;
            set;
        }
    }
}