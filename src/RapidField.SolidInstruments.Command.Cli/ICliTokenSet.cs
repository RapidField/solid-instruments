// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents a collection of CLI tokens.
    /// </summary>
    /// <typeparam name="TToken">
    /// The type of the CLI token that is contained by the token set.
    /// </typeparam>
    internal interface ICliTokenSet<TToken> : ICliTokenSet
        where TToken : ICliTokenDefinition
    {
        /// <summary>
        /// Attempts to find and return a CLI token matching the specified name or alias.
        /// </summary>
        /// <param name="name">
        /// The name or alias of the CLI token. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="cliToken">
        /// A matching CLI token, or <see langword="null" /> if no matching token was found.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if a matching CLI token was found, otherwise <see langword="false" />.
        /// </returns>
        public Boolean FindByName(String name, out TToken cliToken);

        /// <summary>
        /// Attempts to add the specified CLI token to the current <see cref="ICliTokenSet{TToken}" />.
        /// </summary>
        /// <param name="cliToken">
        /// A CLI token to add to the current set.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="cliToken" /> was added successfully, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryAdd(TToken cliToken);
    }

    /// <summary>
    /// Represents a collection of CLI tokens.
    /// </summary>
    internal interface ICliTokenSet : IEnumerable<ICliTokenDefinition>
    {
        /// <summary>
        /// Gets the number of CLI tokens contained by the current <see cref="ICliTokenSet" />.
        /// </summary>
        public Int32 Count
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ICliTokenSet" /> permits the addition of tokens with
        /// duplicate names, aliases or positions.
        /// </summary>
        public Boolean PermitsDuplicateTokens
        {
            get;
        }
    }
}