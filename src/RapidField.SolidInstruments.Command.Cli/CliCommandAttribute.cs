// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents an attribute that maps a CLI command to a class derived from <see cref="CliCommand" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CliCommandAttribute : Attribute, ICliCommandDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandAttribute" /> class.
        /// </summary>
        public CliCommandAttribute()
            : this(DefaultName)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandAttribute" /> class.
        /// </summary>
        /// <param name="name">
        /// The alphanumeric name of the CLI command.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters.
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
        /// The alphanumeric name of the CLI command, or <see langword="null" /> if the command is unnamed. The default argument is
        /// <see langword="null" />.
        /// </param>
        /// <param name="alias">
        /// An abbreviated or alternate alphanumeric name of the CLI command, or <see langword="null" /> if the command does not
        /// have an abbreviated or alternate form. The default argument is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters -or- <paramref name="alias" /> contains non-alphanumeric
        /// characters.
        /// </exception>
        public CliCommandAttribute(String name, String alias)
            : this(name, alias, DefaultType)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandAttribute" /> class.
        /// </summary>
        /// <param name="name">
        /// The alphanumeric name of the CLI command, or <see langword="null" /> if the command is unnamed. The default argument is
        /// <see langword="null" />.
        /// </param>
        /// <param name="alias">
        /// An abbreviated or alternate alphanumeric name of the CLI command, or <see langword="null" /> if the command does not
        /// have an abbreviated or alternate form. The default argument is <see langword="null" />.
        /// </param>
        /// <param name="type">
        /// The type of the associated <see cref="ICliCommand" />, or <see langword="null" /> if the command attribute is not
        /// matched with a command type.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters -or- <paramref name="alias" /> contains non-alphanumeric
        /// characters.
        /// </exception>
        [DebuggerHidden]
        internal CliCommandAttribute(String name, String alias, Type type)
            : base()
        {
            Alias = alias;
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Gets or sets an abbreviated or alternate alphanumeric name of the CLI command, or <see langword="null" /> if the command
        /// does not have an abbreviated form.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The specified value contains non-alphanumeric characters.
        /// </exception>
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
        /// Gets or sets the alphanumeric name of the CLI command, or <see langword="null" /> if the parameter is positional and
        /// unnamed.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The specified value contains non-alphanumeric characters.
        /// </exception>
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
        /// Gets the type of the associated <see cref="ICliCommand" />, or <see langword="null" /> if the command attribute is not
        /// matched with a command type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Type Type
        {
            get;
            internal set;
        }

        /// <summary>
        /// Represents the default abbreviated or alternate alphanumeric name of the CLI command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DefaultAlias = null;

        /// <summary>
        /// Represents the default alphanumeric name of the CLI command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DefaultName = null;

        /// <summary>
        /// Represents the default type of the associated <see cref="ICliCommand" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Type DefaultType = null;

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