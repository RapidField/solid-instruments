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
    /// Extends the <see cref="ICliTokenDefinition" /> interface with command line features.
    /// </summary>
    internal static class ICliTokenDefinitionExtensions
    {
        /// <summary>
        /// Returns <see langword="true" /> if the current <see cref="ICliTokenDefinition" /> name or alias collides with the
        /// specified <see cref="ICliTokenDefinition" /> names or aliases.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICliTokenDefinition" />.
        /// </param>
        /// <param name="otherTokenDefinitions">
        /// A collection of other token definitions to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="ICliTokenDefinition" /> name or alias collides with the specified
        /// <see cref="ICliTokenDefinition" /> names or aliases, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="otherTokenDefinitions" /> contains one or more null elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="otherTokenDefinitions" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal static Boolean CollidesWith(this ICliTokenDefinition target, IEnumerable<ICliTokenDefinition> otherTokenDefinitions)
        {
            _ = otherTokenDefinitions.RejectIf().IsNull(nameof(otherTokenDefinitions)).OrIf(argument => argument.Any(element => element is null), nameof(otherTokenDefinitions), "The specified token definition collection contains one or more null elements.");

            foreach (var otherTokenDefinition in otherTokenDefinitions)
            {
                if (target.CollidesWith(otherTokenDefinition))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns <see langword="true" /> if the current <see cref="ICliTokenDefinition" /> name or alias collides with the
        /// specified <see cref="ICliTokenDefinition" /> name or alias.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICliTokenDefinition" />.
        /// </param>
        /// <param name="otherTokenDefinition">
        /// Another token definition to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="ICliTokenDefinition" /> name or alias collides with the specified
        /// <see cref="ICliTokenDefinition" /> name or alias, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="otherTokenDefinition" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal static Boolean CollidesWith(this ICliTokenDefinition target, ICliTokenDefinition otherTokenDefinition)
        {
            _ = otherTokenDefinition.RejectIf().IsNull(nameof(otherTokenDefinition));
            var thisAlias = target.Alias?.Trim().ToLower();
            var thisName = target.Name?.Trim().ToLower();
            var otherAlias = otherTokenDefinition.Alias?.Trim().ToLower();
            var otherName = otherTokenDefinition.Name?.Trim().ToLower();
            var tokens = new List<String>();

            if (thisName.IsNullOrEmpty() is false)
            {
                tokens.Add(thisName);
            }

            if (otherName.IsNullOrEmpty() is false)
            {
                tokens.Add(otherName);
            }

            if (thisAlias.IsNullOrEmpty() is false && thisAlias != thisName)
            {
                tokens.Add(thisAlias);
            }

            if (otherAlias.IsNullOrEmpty() is false && otherAlias != otherName)
            {
                tokens.Add(otherAlias);
            }

            var tokenCount = tokens.Count;
            var distinctTokenCount = tokens.Distinct().Count();
            return tokenCount == 0 || distinctTokenCount < tokenCount;
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified, user-provided argument name matches the name or alias for the
        /// current <see cref="ICliTokenDefinition" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICliTokenDefinition" />.
        /// </param>
        /// <param name="argumentName">
        /// A user-provided argument name, or <see langword="null" /> if the argument is unnamed.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="argumentName" /> matches the parameter's name or alias, otherwise
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        internal static Boolean MatchesArgumentName(this ICliTokenDefinition target, String argumentName)
        {
            if (target.Name.IsNullOrEmpty() && target.Alias.IsNullOrEmpty())
            {
                return false;
            }

            var processedArgumentName = argumentName?.Trim().ToLower();

            if (processedArgumentName.IsNullOrEmpty())
            {
                return false;
            }
            else if (target.Name?.Trim().ToLower() == processedArgumentName)
            {
                return true;
            }
            else if (target.Alias?.Trim().ToLower() == processedArgumentName)
            {
                return true;
            }

            return false;
        }
    }
}