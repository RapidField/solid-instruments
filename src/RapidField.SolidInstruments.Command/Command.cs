// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command that emits a result when processed.
    /// </summary>
    /// <remarks>
    /// <see cref="Command{TResult}" /> is the default implementation of <see cref="ICommand{TResult}" />.
    /// </remarks>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted when processing the command.
    /// </typeparam>
    public abstract class Command<TResult> : ICommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command{TResult}" /> class.
        /// </summary>
        protected Command()
        {
            return;
        }

        /// <summary>
        /// Gets the type of the result that is emitted when processing the command.
        /// </summary>
        public Type ResultType => ResultTypeReference;

        /// <summary>
        /// Represents the type of the result that is emitted when processing the command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ResultTypeReference = typeof(TResult);
    }

    /// <summary>
    /// Represents a command.
    /// </summary>
    /// <remarks>
    /// <see cref="Command" /> is the default implementation of <see cref="ICommand" />.
    /// </remarks>
    public abstract class Command : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command" /> class.
        /// </summary>
        protected Command()
        {
            return;
        }

        /// <summary>
        /// Gets the type of the result that is emitted when processing the command.
        /// </summary>
        public Type ResultType => Nix.Type;
    }
}