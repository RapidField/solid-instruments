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
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class CliCommandParameterAttribute : Attribute, ICliCommandParameter
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
            MemberName = DefaultMemberName;
            MemberType = DefaultMemberType;
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
        /// An abbreviated or alternate alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter
        /// does not have an abbreviated or alternate form. The default value is <see langword="null" />.
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
        /// An abbreviated or alternate alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter
        /// does not have an abbreviated or alternate form. The default value is <see langword="null" />.
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
        /// An abbreviated or alternate alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter
        /// does not have an abbreviated or alternate form. The default value is <see langword="null" />.
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
        /// An abbreviated or alternate alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter
        /// does not have an abbreviated or alternate form. The default value is <see langword="null" />.
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
            MemberName = DefaultMemberName;
            MemberType = DefaultMemberType;
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
                return NameReference is null && AliasReference is null && argument is null;
            }, nameof(position), "The specified command line parameter is neither named nor positional.").OrIf(argument =>
            {
                return argument.HasValue && argument.Value < 0;
            }, nameof(position), "The specified command line parameter's position is less than zero (0).");
        }

        /// <summary>
        /// Gets an abbreviated or alternate alphanumeric name of the command line parameter, or <see langword="null" /> if the
        /// parameter does not have an abbreviated or alternate form.
        /// </summary>
        public String Alias
        {
            [DebuggerHidden]
            get => AliasReference ?? (MemberName.IsNullOrEmpty() ? DefaultAlias : (Name == MemberName ? DefaultAlias : MemberName));
            [DebuggerHidden]
            private set => AliasReference = value;
        }

        /// <summary>
        /// Gets a value indicating whether or not the command line parameter is required.
        /// </summary>
        public Boolean IsRequired
        {
            [DebuggerHidden]
            get => IsRequiredValue;
            [DebuggerHidden]
            private set => IsRequiredValue = value;
        }

        /// <summary>
        /// Gets a value specifying whether the command line parameter is a boolean switch or defines a value.
        /// </summary>
        public CliCommandParameterKind Kind
        {
            [DebuggerHidden]
            get => MemberTypeIsBoolean ? CliCommandParameterKind.Switch : (KindValue == CliCommandParameterKind.Unspecified ? DefaultKind : KindValue);
            [DebuggerHidden]
            private set => KindValue = value;
        }

        /// <summary>
        /// Gets the name of a <see cref="CliCommand" /> field or property to which the parameter's argument is assigned, or
        /// <see langword="null" /> if the parameter is not matched with a member.
        /// </summary>
        public String MemberName
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the type of a <see cref="CliCommand" /> field or property to which the parameter's argument is assigned, or
        /// <see langword="null" /> if the parameter is not matched with a member.
        /// </summary>
        public Type MemberType
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter is positional and
        /// unnamed.
        /// </summary>
        public String Name
        {
            [DebuggerHidden]
            get => NameReference ?? (MemberName.IsNullOrEmpty() ? DefaultName : MemberName);
            [DebuggerHidden]
            private set => NameReference = value;
        }

        /// <summary>
        /// Gets the zero-based index of the position of the command line parameter, or <see langword="null" /> if the parameter is
        /// named and non-positional.
        /// </summary>
        public Int32? Position
        {
            [DebuggerHidden]
            get => Kind == CliCommandParameterKind.Switch ? DefaultPosition : PositionValue;
            [DebuggerHidden]
            private set => PositionValue = value;
        }

        /// <summary>
        /// Gets a value indicating whether or not <see cref="MemberType" /> is a boolean or nullable boolean type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean MemberTypeIsBoolean => MemberType is null ? false : (MemberType == BooleanType || MemberType == NullableBooleanType);

        /// <summary>
        /// Represents the default abbreviated or alternate alphanumeric name of the command line parameter.
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
        /// Represents the default name of a <see cref="CliCommand" /> field or property to which the parameter's argument is
        /// assigned.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultMemberName = null;

        /// <summary>
        /// Represents the default type of a <see cref="CliCommand" /> field or property to which the parameter's argument is
        /// assigned.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Type DefaultMemberType = null;

        /// <summary>
        /// Represents the default alphanumeric name of the command line parameter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultName = null;

        /// <summary>
        /// Represents the <see cref="Boolean" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type BooleanType = typeof(Boolean);

        /// <summary>
        /// Represents the default zero-based index of the position of the command line parameter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Int32? DefaultPosition = null;

        /// <summary>
        /// Represents the nullable <see cref="Boolean" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type NullableBooleanType = typeof(Boolean?);

        /// <summary>
        /// Represents an abbreviated or alternate alphanumeric name of the command line parameter, or <see langword="null" /> if
        /// the parameter does not have an abbreviated or alternate form.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String AliasReference;

        /// <summary>
        /// Represents a value indicating whether or not the command line parameter is required.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean IsRequiredValue;

        /// <summary>
        /// Represents a value specifying whether the command line parameter is a boolean switch or defines a value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CliCommandParameterKind KindValue;

        /// <summary>
        /// Represents the alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter is
        /// positional and unnamed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String NameReference;

        /// <summary>
        /// Represents the zero-based index of the position of the command line parameter, or <see langword="null" /> if the
        /// parameter is named and non-positional.
        /// </summary>
        private Int32? PositionValue;
    }
}