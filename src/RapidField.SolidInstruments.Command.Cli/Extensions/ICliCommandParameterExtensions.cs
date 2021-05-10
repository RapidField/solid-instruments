// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.Cli.Extensions
{
    /// <summary>
    /// Extends the <see cref="ICliCommandParameter" /> interface with command line features.
    /// </summary>
    internal static class ICliCommandParameterExtensions
    {
        /// <summary>
        /// Returns a value indicating whether or not the specified, user-provided argument name and position matches the name,
        /// alias or position for the current <see cref="ICliCommandParameter" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICliCommandParameter" />.
        /// </param>
        /// <param name="argumentName">
        /// A user-provided argument name, or <see langword="null" /> if the argument is unnamed.
        /// </param>
        /// <param name="argumentPosition">
        /// A user-provided argument position, or <see langword="null" /> if the argument is not positional.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="argumentName" /> matches the parameter's name or alias, or if
        /// <paramref name="argumentPosition" /> matches the parameter's position, otherwise <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        internal static Boolean MatchesArgumentNameOrPosition(this ICliCommandParameter target, String argumentName, Int32? argumentPosition) => target.MatchesArgumentName(argumentName) || target.MatchesArgumentPosition(argumentPosition);

        /// <summary>
        /// Returns a value indicating whether or not the specified, user-provided argument position matches the position for the
        /// current <see cref="ICliCommandParameter" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICliCommandParameter" />.
        /// </param>
        /// <param name="argumentPosition">
        /// A user-provided argument position, or <see langword="null" /> if the argument is not positional.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="argumentPosition" /> matches the parameter's position, otherwise
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        internal static Boolean MatchesArgumentPosition(this ICliCommandParameter target, Int32? argumentPosition)
        {
            if (target.IsPositional)
            {
                var processedArgumentPosition = argumentPosition.HasValue && argumentPosition.Value >= 0 ? argumentPosition : null;
                return target.Position.HasValue && processedArgumentPosition.HasValue ? target.Position.Value == processedArgumentPosition.Value : false;
            }

            return false;
        }
    }
}