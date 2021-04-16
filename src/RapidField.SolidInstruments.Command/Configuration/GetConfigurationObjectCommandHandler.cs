// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.Configuration
{
    /// <summary>
    /// Processes a single <see cref="IGetConfigurationObjectCommand{TResult}" />.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the configuration object that is produced by interrogating the specified configuration key and target.
    /// </typeparam>
    public abstract class GetConfigurationObjectCommandHandler<TCommand, TResult> : CommandHandler<TCommand, TResult>
        where TCommand : class, IGetConfigurationObjectCommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfigurationObjectCommandHandler{TCommand, TResult}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="applicationConfiguration" /> is
        /// <see langword="null" />.
        /// </exception>
        protected GetConfigurationObjectCommandHandler(ICommandMediator mediator, IConfiguration applicationConfiguration)
            : base(mediator)
        {
            ApplicationConfiguration = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument;
        }

        /// <summary>
        /// Converts the specified configuration object to the appropriate result type.
        /// </summary>
        /// <param name="configurationObject">
        /// The configuration value or section to convert.
        /// </param>
        /// <returns>
        /// The converted result object.
        /// </returns>
        protected abstract TResult ConvertConfigurationObject(Object configurationObject);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="GetConfigurationObjectCommandHandler{TCommand, TResult}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <note type="note">
        /// Do not process <paramref name="command" /> using <paramref name="mediator" />, as doing so will generally result in
        /// infinite-looping; <paramref name="mediator" /> is exposed to this method to facilitate sub-command processing.
        /// </note>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands. Do not process <paramref name="command" /> using
        /// <paramref name="mediator" />, as doing so will generally result in infinite-looping.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override TResult Process(TCommand command, ICommandMediator mediator, IConcurrencyControlToken controlToken) => command.Target switch
        {
            GetConfigurationObjectCommandTarget.ConnectionString => ConvertConfigurationObject(ApplicationConfiguration.GetConnectionString(command.Key)),
            GetConfigurationObjectCommandTarget.Section => ConvertConfigurationObject(ApplicationConfiguration.GetSection(command.Key)),
            GetConfigurationObjectCommandTarget.Value => ConvertConfigurationObject(ApplicationConfiguration.GetValue<String>(command.Key)),
            _ => throw new UnsupportedSpecificationException($"The specified configuration command target, {command.Target}, is not supported.")
        };

        /// <summary>
        /// Represents configuration information for the application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IConfiguration ApplicationConfiguration;
    }
}