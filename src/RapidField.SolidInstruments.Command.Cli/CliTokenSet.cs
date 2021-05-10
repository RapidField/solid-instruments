// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command.Cli.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents a collection of CLI tokens.
    /// </summary>
    /// <remarks>
    /// <see cref="CliTokenSet{TToken}" /> is the default implementation of <see cref="ICliTokenSet{TToken}" />.
    /// </remarks>
    /// <typeparam name="TToken">
    /// The type of the CLI token that is contained by the token set.
    /// </typeparam>
    internal abstract class CliTokenSet<TToken> : ICliTokenSet<TToken>
        where TToken : ICliTokenDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliTokenSet{TToken}" /> class.
        /// </summary>
        protected CliTokenSet()
        {
            return;
        }

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
        public Boolean FindByName(String name, out TToken cliToken)
        {
            var token = FindByName(name, Tokens);

            if (token is null)
            {
                cliToken = default;
                return false;
            }

            cliToken = token;
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ICliTokenDefinition> GetEnumerator()
        {
            lock (SyncRoot)
            {
                foreach (var token in Tokens)
                {
                    yield return token;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Attempts to add the specified CLI token to the current <see cref="CliTokenSet{TToken}" />.
        /// </summary>
        /// <param name="cliToken">
        /// A CLI token to add to the current set.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="cliToken" /> was added successfully, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryAdd(TToken cliToken)
        {
            if (cliToken is null)
            {
                return false;
            }
            else if (PermitsDuplicateTokens is false && cliToken.CollidesWith(this))
            {
                return false;
            }

            lock (SyncRoot)
            {
                Tokens.Add(cliToken);
            }

            return true;
        }

        /// <summary>
        /// Attempts to find and return a CLI token matching the specified name or alias.
        /// </summary>
        /// <param name="name">
        /// The name or alias of the CLI token. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="tokens">
        /// The CLI tokens that are contained by the current set.
        /// </param>
        /// <returns>
        /// A matching CLI token, or <see langword="null" /> if no matching token was found.
        /// </returns>
        protected virtual TToken FindByName(String name, IEnumerable<TToken> tokens)
        {
            foreach (var token in tokens)
            {
                if (token.MatchesArgumentName(name))
                {
                    return token;
                }
            }

            return default;
        }

        /// <summary>
        /// Gets the number of CLI tokens contained by the current <see cref="CliTokenSet{TToken}" />.
        /// </summary>
        public Int32 Count => Tokens.Count;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="CliTokenSet{TToken}" /> permits the addition of tokens
        /// with duplicate names, aliases or positions.
        /// </summary>
        public virtual Boolean PermitsDuplicateTokens => DefaultPermitsDuplicateTokensValue;

        /// <summary>
        /// Represents the CLI tokens that are contained by the current <see cref="CliTokenSet{TToken}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly ICollection<TToken> Tokens = new List<TToken>();

        /// <summary>
        /// Represents the default value indicating whether or not <see cref="CliTokenSet{TToken}" /> instances permit the addition
        /// of tokens with duplicate names, aliases or positions.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultPermitsDuplicateTokensValue = false;

        /// <summary>
        /// Represents an object that is used to synchronize access to the associated resource(s).
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Object SyncRoot = new();
    }
}