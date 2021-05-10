// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents a collection of CLI command parameters.
    /// </summary>
    internal interface ICliCommandParameterSet : ICliTokenSet<ICliCommandParameter>
    {
        /// <summary>
        /// Attempts to find and return a CLI token matching the specified argument position.
        /// </summary>
        /// <param name="position">
        /// The zero-based index of the position of the command line parameter, or <see langword="null" /> if the parameter is named
        /// and non-positional.
        /// </param>
        /// <param name="parameter">
        /// A matching CLI command parameter, or <see langword="null" /> if no matching parameter was found.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if a matching CLI command parameter was found, otherwise <see langword="false" />.
        /// </returns>
        public Boolean FindByPosition(Int32? position, out ICliCommandParameter parameter);
    }
}