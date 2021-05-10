// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.Cli.Extensions
{
    /// <summary>
    /// Extends the <see cref="ICliCommand" /> interface with command line features.
    /// </summary>
    internal static class ICliCommandExtensions
    {
        /// <summary>
        /// Converts the current <see cref="ICliCommand" /> to the specified concrete implementation.
        /// </summary>
        /// <typeparam name="TCliCommand">
        /// The type of the resulting <see cref="ICliCommand" />.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICliCommand" />.
        /// </param>
        /// <returns>
        /// The resulting command.
        /// </returns>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while initializing the command or processing the command line arguments.
        /// </exception>
        [DebuggerHidden]
        internal static TCliCommand As<TCliCommand>(this ICliCommand target)
            where TCliCommand : class, ICliCommand, new()
        {
            try
            {
                var command = new TCliCommand();

                foreach (var argument in target.Arguments)
                {
                    command.Arguments.Add(argument);
                }

                command.Alias = target.Alias;
                command.CorrelationIdentifier = target.CorrelationIdentifier;
                command.Name = target.Name;
                command.Hydrate();
                return command;
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TCliCommand), exception);
            }
        }
    }
}