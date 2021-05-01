// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents an attribute that maps a CLI command to a class derived from <see cref="CliCommand" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CliCommandAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandAttribute" /> class.
        /// </summary>
        public CliCommandAttribute()
            : base()
        {
            Alias = DefaultAlias;
            Name = DefaultName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandAttribute" /> class.
        /// </summary>
        /// <param name="name">
        /// The alphanumeric name of the CLI command.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        public CliCommandAttribute(String name)
            : this(name, DefaultAlias)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandAttribute" /> class.
        /// </summary>
        /// <param name="name">
        /// The alphanumeric name of the CLI command, or <see langword="null" /> if the command is unnamed. The default value is
        /// <see langword="null" />.
        /// </param>
        /// <param name="alias">
        /// An abbreviated alphanumeric name of the CLI command, or <see langword="null" /> if the command does not have an
        /// abbreviated form. The default value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters -or- <paramref name="alias" /> contains non-alphanumeric
        /// characters.
        /// </exception>
        public CliCommandAttribute(String name, String alias)
            : base()
        {
            var processedAlias = alias?.Trim();
            var processedName = name?.Trim();
            Alias = processedAlias.IsNullOrEmpty() ? null : processedAlias.RejectIf(argument =>
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
            }, nameof(alias), $"The specified CLI command alias \"{processedAlias}\" contains non-alphanumeric characters.");
            Name = processedName.IsNullOrEmpty() ? null : processedName.RejectIf(argument =>
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
            }, nameof(name), $"The specified CLI command name \"{processedName}\" contains non-alphanumeric characters.");
        }

        /// <summary>
        /// Gets an abbreviated alphanumeric name of the CLI command, or <see langword="null" /> if the command does not have an
        /// abbreviated form.
        /// </summary>
        public String Alias
        {
            get;
        }

        /// <summary>
        /// Gets the alphanumeric name of the CLI command, or <see langword="null" /> if the parameter is positional and unnamed.
        /// </summary>
        public String Name
        {
            get;
        }

        /// <summary>
        /// Represents the default abbreviated alphanumeric name of the CLI command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultAlias = null;

        /// <summary>
        /// Represents the default alphanumeric name of the CLI command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultName = null;
    }
}