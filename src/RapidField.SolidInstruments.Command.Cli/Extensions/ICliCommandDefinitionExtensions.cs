// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Command.Cli.Extensions
{
    /// <summary>
    /// Extends the <see cref="ICliCommandDefinition" /> interface with command line features.
    /// </summary>
    internal static class ICliCommandDefinitionExtensions
    {
        /// <summary>
        /// Returns <see langword="true" /> if the current <see cref="ICliCommandDefinition" /> matches the specified command line
        /// arguments.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICliCommandDefinition" />.
        /// </param>
        /// <param name="arguments">
        /// The command line arguments to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="ICliCommandDefinition" /> matches <paramref name="arguments" />,
        /// otherwise <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        internal static Boolean MatchesArguments(this ICliCommandDefinition target, IEnumerable<String> arguments)
        {
            if (target.IsDefaultDefinition)
            {
                return true;
            }
            else if (arguments.IsNullOrEmpty())
            {
                return false;
            }

            var leadingArgument = arguments.First()?.Trim().ToLower();
            return leadingArgument.IsNullOrEmpty() ? false : leadingArgument == target.Name?.ToLower() || leadingArgument == target.Alias?.ToLower();
        }
    }
}