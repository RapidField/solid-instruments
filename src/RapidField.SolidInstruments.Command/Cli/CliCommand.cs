// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents a command that is described by textual command line arguments.
    /// </summary>
    /// <remarks>
    /// <see cref="CliCommand" /> is the default implementation of <see cref="ICliCommand" />.
    /// </remarks>
    [DataContract]
    public class CliCommand : Command, ICliCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommand" /> class.
        /// </summary>
        public CliCommand()
            : this(Array.Empty<String>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommand" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public CliCommand(Guid correlationIdentifier)
            : this(Array.Empty<String>(), correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommand" /> class.
        /// </summary>
        /// <param name="arguments">
        /// A collection of textual command line arguments constituting the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments" /> is <see langword="null" />.
        /// </exception>
        public CliCommand(IEnumerable<String> arguments)
            : base()
        {
            Arguments = new List<String>(arguments.RejectIf().IsNull(nameof(arguments)).TargetArgument);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommand" /> class.
        /// </summary>
        /// <param name="arguments">
        /// A collection of textual command line arguments constituting the command.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public CliCommand(IEnumerable<String> arguments, Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            Arguments = new List<String>(arguments.RejectIf().IsNull(nameof(arguments)).TargetArgument);
        }

        /// <summary>
        /// Interrogates <see cref="Arguments" /> and hydrates the derived properties of the current <see cref="ICliCommand" />.
        /// </summary>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the command line arguments.
        /// </exception>
        public void Hydrate()
        {
            try
            {
                Hydrate(Arguments ?? Array.Empty<String>());
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(GetType(), exception);
            }
        }

        /// <summary>
        /// Interrogates <see cref="Arguments" /> and hydrates the derived properties of the current <see cref="ICliCommand" />.
        /// </summary>
        /// <param name="arguments">
        /// A collection of textual command line arguments constituting the command.
        /// </param>
        protected virtual void Hydrate(IEnumerable<String> arguments)
        {
            return;
        }

        /// <summary>
        /// Gets or sets a collection of textual command line arguments constituting the current <see cref="CliCommand" />.
        /// </summary>
        [DataMember]
        public ICollection<String> Arguments
        {
            get;
            set;
        }
    }
}