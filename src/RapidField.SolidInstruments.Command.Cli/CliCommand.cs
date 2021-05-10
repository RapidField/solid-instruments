// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            : this(DefaultArguments)
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
            : this(DefaultArguments, correlationIdentifier)
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
            : this(arguments, DefaultCorrelationIdentifier)
        {
            return;
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
            : this(arguments, correlationIdentifier, DefaultName, DefaultAlias)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommand" /> class.
        /// </summary>
        /// <param name="arguments">
        /// A collection of textual command line arguments constituting the command.
        /// </param>
        /// <param name="name">
        /// The alphanumeric name of the CLI command, or <see langword="null" /> if the parameter is positional and unnamed. The
        /// default argument is <see langword="null" />.
        /// </param>
        /// <param name="alias">
        /// An abbreviated or alternate alphanumeric name of the CLI command, or <see langword="null" /> if the command does not
        /// have an abbreviated form. The default argument is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters -or- <paramref name="alias" /> contains non-alphanumeric
        /// characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal CliCommand(IEnumerable<String> arguments, String name, String alias)
            : this(arguments, DefaultCorrelationIdentifier, name, alias)
        {
            return;
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
        /// <param name="name">
        /// The alphanumeric name of the CLI command, or <see langword="null" /> if the parameter is positional and unnamed. The
        /// default argument is <see langword="null" />.
        /// </param>
        /// <param name="alias">
        /// An abbreviated or alternate alphanumeric name of the CLI command, or <see langword="null" /> if the command does not
        /// have an abbreviated form. The default argument is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters -or- <paramref name="alias" /> contains non-alphanumeric
        /// characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        private CliCommand(IEnumerable<String> arguments, Guid correlationIdentifier, String name, String alias)
            : base(correlationIdentifier)
        {
            Alias = alias;
            Arguments = new List<String>(arguments.RejectIf().IsNull(nameof(arguments)).TargetArgument);
            Name = name;
        }

        /// <summary>
        /// Interrogates <see cref="Arguments" /> and hydrates the derived properties of the current <see cref="CliCommand" />.
        /// </summary>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the command line arguments.
        /// </exception>
        public void Hydrate()
        {
            try
            {
                Hydrate(Arguments ?? DefaultArguments);
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
        /// Interrogates <see cref="Arguments" /> and hydrates the derived properties of the current <see cref="CliCommand" />.
        /// </summary>
        /// <param name="arguments">
        /// A collection of textual command line arguments constituting the command.
        /// </param>
        protected virtual void Hydrate(IEnumerable<String> arguments)
        {
            return;
        }

        /// <summary>
        /// Gets or sets an abbreviated or alternate alphanumeric name of the CLI command, or <see langword="null" /> if the command
        /// does not have an abbreviated form.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The specified value contains non-alphanumeric characters.
        /// </exception>
        [DataMember]
        public String Alias
        {
            [DebuggerHidden]
            get => AliasReference;
            [DebuggerHidden]
            set
            {
                var processedAlias = value?.Trim();
                AliasReference = processedAlias.IsNullOrEmpty() ? null : processedAlias.RejectIf(argument =>
                {
                    foreach (var character in argument)
                    {
                        if (character.IsAlphabetic() || character.IsNumeric())
                        {
                            continue;
                        }

                        return true;
                    }

                    return false;
                }, nameof(Alias), $"The specified CLI command alias \"{processedAlias}\" contains non-alphanumeric characters.");
            }
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

        /// <summary>
        /// Gets or sets the alphanumeric name of the CLI command, or <see langword="null" /> if the parameter is positional and
        /// unnamed.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The specified value contains non-alphanumeric characters.
        /// </exception>
        [DataMember]
        public String Name
        {
            [DebuggerHidden]
            get => NameReference;
            [DebuggerHidden]
            set
            {
                var processedName = value?.Trim();
                NameReference = processedName.IsNullOrEmpty() ? null : processedName.RejectIf(argument =>
                {
                    foreach (var character in argument)
                    {
                        if (character.IsAlphabetic() || character.IsNumeric())
                        {
                            continue;
                        }

                        return true;
                    }

                    return false;
                }, nameof(Name), $"The specified CLI command name \"{processedName}\" contains non-alphanumeric characters.");
            }
        }

        /// <summary>
        /// Gets the type of the current command.
        /// </summary>
        [IgnoreDataMember]
        public Type Type => GetType();

        /// <summary>
        /// Gets a default collection of command line arguments.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static IEnumerable<String> DefaultArguments => Array.Empty<String>();

        /// <summary>
        /// Gets a default unique identifier that is assigned to related commands.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Guid DefaultCorrelationIdentifier => Guid.NewGuid();

        /// <summary>
        /// Represents the default abbreviated or alternate alphanumeric name of the CLI command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly String DefaultAlias = CliCommandAttribute.DefaultAlias;

        /// <summary>
        /// Represents the default alphanumeric name of the CLI command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly String DefaultName = CliCommandAttribute.DefaultName;

        /// <summary>
        /// Represents an abbreviated or alternate alphanumeric name of the CLI command, or <see langword="null" /> if the command
        /// does not have an abbreviated or alternate form.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String AliasReference;

        /// <summary>
        /// Represents the alphanumeric name of the CLI command, or <see langword="null" /> if the command is positional and
        /// unnamed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String NameReference;
    }
}