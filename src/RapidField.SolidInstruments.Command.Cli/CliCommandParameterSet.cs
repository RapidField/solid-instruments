// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command.Cli.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents a collection of CLI command parameters.
    /// </summary>
    /// <remarks>
    /// <see cref="CliCommndParameterSet" /> is the default implementation of <see cref="ICliCommandParameterSet" />.
    /// </remarks>
    internal class CliCommndParameterSet : CliTokenSet<ICliCommandParameter>, ICliCommandParameterSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommndParameterSet" /> class.
        /// </summary>
        [DebuggerHidden]
        internal CliCommndParameterSet()
            : base()
        {
            return;
        }

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
        public Boolean FindByPosition(Int32? position, out ICliCommandParameter parameter)
        {
            var token = FindByPosition(position, Tokens);

            if (token is null)
            {
                parameter = default;
                return false;
            }

            parameter = token;
            return true;
        }

        /// <summary>
        /// Attempts to find and return a CLI token matching the specified argument position.
        /// </summary>
        /// <param name="position">
        /// The zero-based index of the position of the command line parameter, or <see langword="null" /> if the parameter is named
        /// and non-positional.
        /// </param>
        /// <param name="tokens">
        /// The CLI tokens that are contained by the current set.
        /// </param>
        /// <returns>
        /// A matching CLI token, or <see langword="null" /> if no matching token was found.
        /// </returns>
        protected virtual ICliCommandParameter FindByPosition(Int32? position, IEnumerable<ICliCommandParameter> tokens)
        {
            foreach (var token in tokens)
            {
                if (token.MatchesArgumentPosition(position))
                {
                    return token;
                }
            }

            return default;
        }
    }
}