// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using System;

namespace RapidField.SolidInstruments.Command.Configuration
{
    /// <summary>
    /// Processes a single <see cref="GetConnectionStringCommand" />.
    /// </summary>
    public sealed class GetConnectionStringCommandHandler : GetConfigurationObjectCommandHandler<GetConnectionStringCommand, String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetConnectionStringCommandHandler" /> class.
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
        public GetConnectionStringCommandHandler(ICommandMediator mediator, IConfiguration applicationConfiguration)
            : base(mediator, applicationConfiguration)
        {
            return;
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
        protected override String ConvertConfigurationObject(Object configurationObject) => configurationObject as String;

        /// <summary>
        /// Releases all resources consumed by the current <see cref="GetConnectionStringCommandHandler" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}