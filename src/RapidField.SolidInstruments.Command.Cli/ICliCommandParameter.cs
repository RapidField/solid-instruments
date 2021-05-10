// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents a CLI command parameter.
    /// </summary>
    public interface ICliCommandParameter : ICliTokenDefinition
    {
        /// <summary>
        /// Gets an abbreviated or alternate alphanumeric name of the command line parameter, or <see langword="null" /> if the
        /// parameter does not have an abbreviated or alternate form.
        /// </summary>
        public new String Alias
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
        /// Gets the name of an <see cref="ICliCommand" /> field or property to which the parameter's argument is assigned, or
        /// <see langword="null" /> if the parameter is not matched with a member.
        /// </summary>
        public String MemberName
        {
            get;
        }

        /// <summary>
        /// Gets the type of an <see cref="ICliCommand" /> field or property to which the parameter's argument is assigned, or
        /// <see langword="null" /> if the parameter is not matched with a member.
        /// </summary>
        public Type MemberType
        {
            get;
        }

        /// <summary>
        /// Gets the alphanumeric name of the command line parameter, or <see langword="null" /> if the parameter is positional and
        /// unnamed.
        /// </summary>
        public new String Name
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
        /// Gets a value indicating whether or not the current <see cref="ICliCommandParameter" /> is assigned to an
        /// <see cref="ICliCommand" /> field or property.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Boolean IsAssigned => MemberName.IsNullOrEmpty() is false && MemberType is not null;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ICliCommandParameter" /> contains one or more valid
        /// identifying properties.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Boolean IsIdentified => IsNamed || IsPositional;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ICliCommandParameter" /> contains a valid name or alias.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Boolean IsNamed => Name.IsNullOrEmpty() && Alias.IsNullOrEmpty() ? false : true;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ICliCommandParameter" /> contains a valid position.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Boolean IsPositional => Position.HasValue && Position.Value >= 0;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ICliCommandParameter" /> is both assigned and identified.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Boolean IsValid => IsAssigned && IsIdentified;
    }
}