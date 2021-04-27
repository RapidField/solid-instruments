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
    /// Represents an attribute that maps a CLI command parameter to a <see cref="CliCommand" /> field or property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class CliCommandParameterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        public CliCommandParameterAttribute()
            : base()
        {
            Alias = DefaultAlias;
            IsRequired = DefaultIsRequiredValue;
            Kind = DefaultKind;
            Name = DefaultName;
            Position = DefaultPosition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="position">
        /// The zero-based index of the position of the command line parameter.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="position" /> is less than zero (0).
        /// </exception>
        public CliCommandParameterAttribute(Int32 position)
            : this(DefaultKind, position)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="name">
        /// The alphanumeric name of the command line parameter.
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
        public CliCommandParameterAttribute(String name)
            : this(name, DefaultAlias)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="name">
        /// The alphanumeric name of the command line parameter.
        /// </param>
        /// <param name="alias">
        /// An abbreviated alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter does not
        /// have an abbreviated form. The default value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters -or- <paramref name="alias" /> contains non-alphanumeric
        /// characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        public CliCommandParameterAttribute(String name, String alias)
            : this(DefaultKind, name, alias)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="kind">
        /// A value specifying whether the command line parameter is a boolean switch or defines a value. The default value is
        /// <see cref="CliCommandParameterKind.Value" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="kind" /> is equal to <see cref="CliCommandParameterKind.Unspecified" />.
        /// </exception>
        public CliCommandParameterAttribute(CliCommandParameterKind kind)
            : this(kind, DefaultName)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="kind">
        /// A value specifying whether the command line parameter is a boolean switch or defines a value. The default value is
        /// <see cref="CliCommandParameterKind.Value" />.
        /// </param>
        /// <param name="name">
        /// The alphanumeric name of the command line parameter.
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="kind" /> is equal to <see cref="CliCommandParameterKind.Unspecified" />.
        /// </exception>
        public CliCommandParameterAttribute(CliCommandParameterKind kind, String name)
            : this(kind, name, DefaultAlias)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="kind">
        /// A value specifying whether the command line parameter is a boolean switch or defines a value. The default value is
        /// <see cref="CliCommandParameterKind.Value" />.
        /// </param>
        /// <param name="name">
        /// The alphanumeric name of the command line parameter.
        /// </param>
        /// <param name="alias">
        /// An abbreviated alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter does not
        /// have an abbreviated form. The default value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters -or- <paramref name="alias" /> contains non-alphanumeric
        /// characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="kind" /> is equal to <see cref="CliCommandParameterKind.Unspecified" />.
        /// </exception>
        public CliCommandParameterAttribute(CliCommandParameterKind kind, String name, String alias)
            : this(kind, name, alias, DefaultIsRequiredValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="kind">
        /// A value specifying whether the command line parameter is a boolean switch or defines a value. The default value is
        /// <see cref="CliCommandParameterKind.Value" />.
        /// </param>
        /// <param name="position">
        /// The zero-based index of the position of the command line parameter.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="kind" /> is equal to <see cref="CliCommandParameterKind.Unspecified" /> -or-
        /// <paramref name="position" /> is less than zero (0).
        /// </exception>
        public CliCommandParameterAttribute(CliCommandParameterKind kind, Int32 position)
            : this(kind, position, DefaultIsRequiredValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="kind">
        /// A value specifying whether the command line parameter is a boolean switch or defines a value. The default value is
        /// <see cref="CliCommandParameterKind.Value" />.
        /// </param>
        /// <param name="name">
        /// The alphanumeric name of the command line parameter.
        /// </param>
        /// <param name="isRequired">
        /// A value indicating whether or not the command line parameter is required. The default value is <see langword="false" />.
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="kind" /> is equal to <see cref="CliCommandParameterKind.Unspecified" />.
        /// </exception>
        public CliCommandParameterAttribute(CliCommandParameterKind kind, String name, Boolean isRequired)
            : this(kind, name, DefaultAlias, isRequired)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="kind">
        /// A value specifying whether the command line parameter is a boolean switch or defines a value. The default value is
        /// <see cref="CliCommandParameterKind.Value" />.
        /// </param>
        /// <param name="name">
        /// The alphanumeric name of the command line parameter.
        /// </param>
        /// <param name="alias">
        /// An abbreviated alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter does not
        /// have an abbreviated form. The default value is <see langword="null" />.
        /// </param>
        /// <param name="isRequired">
        /// A value indicating whether or not the command line parameter is required. The default value is <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters -or- <paramref name="alias" /> contains non-alphanumeric
        /// characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="kind" /> is equal to <see cref="CliCommandParameterKind.Unspecified" />.
        /// </exception>
        public CliCommandParameterAttribute(CliCommandParameterKind kind, String name, String alias, Boolean isRequired)
            : this(kind, name.RejectIf().IsNullOrEmpty(nameof(name)), alias, DefaultPosition, isRequired)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="kind">
        /// A value specifying whether the command line parameter is a boolean switch or defines a value. The default value is
        /// <see cref="CliCommandParameterKind.Value" />.
        /// </param>
        /// <param name="position">
        /// The zero-based index of the position of the command line parameter.
        /// </param>
        /// <param name="isRequired">
        /// A value indicating whether or not the command line parameter is required. The default value is <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="kind" /> is equal to <see cref="CliCommandParameterKind.Unspecified" /> -or-
        /// <paramref name="position" /> is less than zero (0).
        /// </exception>
        public CliCommandParameterAttribute(CliCommandParameterKind kind, Int32 position, Boolean isRequired)
            : this(kind, DefaultName, DefaultAlias, position.RejectIf().IsLessThan(0, nameof(position)), isRequired)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandParameterAttribute" /> class.
        /// </summary>
        /// <param name="kind">
        /// A value specifying whether the command line parameter is a boolean switch or defines a value. The default value is
        /// <see cref="CliCommandParameterKind.Value" />.
        /// </param>
        /// <param name="name">
        /// The alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter is positional and
        /// unnamed. The default value is <see langword="null" />.
        /// </param>
        /// <param name="alias">
        /// An abbreviated alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter does not
        /// have an abbreviated form. The default value is <see langword="null" />.
        /// </param>
        /// <param name="position">
        /// The zero-based index of the position of the command line parameter, or <see langword="null" /> if the parameter is named
        /// and non-positional. The default value is <see langword="null" />.
        /// </param>
        /// <param name="isRequired">
        /// A value indicating whether or not the command line parameter is required. The default value is <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> contains non-alphanumeric characters -or- <paramref name="alias" /> contains non-alphanumeric
        /// characters -or- <paramref name="name" /> is <see langword="null" /> and <paramref name="position" /> is
        /// <see langword="null" /> -or- <paramref name="position" /> is less than zero (0).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="kind" /> is equal to <see cref="CliCommandParameterKind.Unspecified" />.
        /// </exception>
        public CliCommandParameterAttribute(CliCommandParameterKind kind, String name, String alias, Int32? position, Boolean isRequired)
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
            }, nameof(alias), $"The specified CLI command parameter alias \"{processedAlias}\" contains non-alphanumeric characters.");
            IsRequired = isRequired;
            Kind = kind.RejectIf().IsEqualToValue(CliCommandParameterKind.Unspecified, nameof(kind));
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
            }, nameof(name), $"The specified CLI command parameter name \"{processedName}\" contains non-alphanumeric characters.");
            Position = position.RejectIf(argument =>
            {
                return Name is null && argument is null;
            }, nameof(position), "The specified command line parameter is neither named nor positional.").OrIf(argument =>
            {
                return argument.HasValue && argument.Value < 0;
            }, nameof(position), "The specified command line parameter's position is less than zero (0).");
        }

        /// <summary>
        /// Gets an abbreviated alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter does
        /// not have an abbreviated form.
        /// </summary>
        public String Alias
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the command line parameter is required.
        /// </summary>
        public Boolean IsRequired
        {
            get;
        }

        /// <summary>
        /// Gets a value specifying whether the command line parameter is a boolean switch or defines a value.
        /// </summary>
        public CliCommandParameterKind Kind
        {
            get;
        }

        /// <summary>
        /// Gets the alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter is positional and
        /// unnamed.
        /// </summary>
        public String Name
        {
            get;
        }

        /// <summary>
        /// Gets the zero-based index of the position of the command line parameter, or <see langword="null" /> if the parameter is
        /// named and non-positional.
        /// </summary>
        public Int32? Position
        {
            get;
        }

        /// <summary>
        /// Represents the default abbreviated alphanumeric name of the command line parameter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultAlias = null;

        /// <summary>
        /// Represents the value indicating whether or not a command line parameter is required.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultIsRequiredValue = false;

        /// <summary>
        /// Represents the default value specifying whether a command line parameter is a boolean switch or defines a value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const CliCommandParameterKind DefaultKind = CliCommandParameterKind.Value;

        /// <summary>
        /// Represents the default alphanumeric name of the command line parameter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultName = null;

        /// <summary>
        /// Represents the default zero-based index of the position of the command line parameter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Int32? DefaultPosition = null;
    }
}