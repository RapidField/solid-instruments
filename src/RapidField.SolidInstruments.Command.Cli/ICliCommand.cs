// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents a command that is described by textual command line arguments.
    /// </summary>
    public interface ICliCommand : ICommand, ICliCommandDefinition
    {
        /// <summary>
        /// Interrogates <see cref="Arguments" /> and hydrates the derived properties of the current <see cref="ICliCommand" />.
        /// </summary>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the command line arguments.
        /// </exception>
        public void Hydrate();

        /// <summary>
        /// Gets or sets a collection of textual command line arguments constituting the current <see cref="ICliCommand" />.
        /// </summary>
        public ICollection<String> Arguments
        {
            get;
            set;
        }
    }
}