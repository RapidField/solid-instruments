// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents identifying information about a CLI command.
    /// </summary>
    public interface ICliCommandDefinition : ICliTokenDefinition
    {
        /// <summary>
        /// Gets or sets an abbreviated or alternate alphanumeric name of the CLI command, or <see langword="null" /> if the command
        /// does not have an abbreviated form.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The specified value contains non-alphanumeric characters.
        /// </exception>
        public new String Alias
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether or not the CLI command's identity is the default identity.
        /// </summary>
        public Boolean IsDefaultDefinition => Alias == CliCommandAttribute.DefaultAlias && Name == CliCommandAttribute.DefaultName;

        /// <summary>
        /// Gets or sets the alphanumeric name of the CLI command, or <see langword="null" /> if the parameter is positional and
        /// unnamed.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The specified value contains non-alphanumeric characters.
        /// </exception>
        public new String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the type of the associated <see cref="ICliCommand" />, or <see langword="null" /> if the command definition is not
        /// matched with a command type.
        /// </summary>
        public Type Type
        {
            get;
        }
    }
}