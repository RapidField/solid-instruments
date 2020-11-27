// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command.Configuration
{
    /// <summary>
    /// Represents a command that retrieves a connection string.
    /// </summary>
    [DataContract]
    public sealed class GetConnectionStringCommand : GetConfigurationObjectCommand<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetConnectionStringCommand" /> class.
        /// </summary>
        /// <param name="name">
        /// A textual key for the connection string to retrieve.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal GetConnectionStringCommand(String name)
            : base(name, GetConfigurationObjectCommandTarget.ConnectionString)
        {
            return;
        }

        /// <summary>
        /// Creates and processes a command that retrieves a connection string.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="name">
        /// A textual key for the connection string to retrieve.
        /// </param>
        /// <returns>
        /// The resulting connection string, or <see langword="null" />
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the command.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="mediator" /> is disposed.
        /// </exception>
        public static String Process(ICommandMediator mediator, String name) => mediator.RejectIf().IsNull(nameof(mediator)).TargetArgument.Process(new GetConnectionStringCommand(name));
    }
}