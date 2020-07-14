// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Example.DatabaseModel.Commands;
using RapidField.SolidInstruments.Example.DatabaseModel.Entities;
using RapidField.SolidInstruments.Example.DatabaseModel.Repositories;
using RapidField.SolidInstruments.ObjectComposition;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Example.DatabaseModel.CommandHandlers
{
    /// <summary>
    /// Processes a command that gets all of the Fibonacci number values from the Example database.
    /// </summary>
    public sealed class GetFibonacciNumberValuesCommandHandler : ExampleCommandHandler<GetFibonacciNumberValuesCommand, IEnumerable<Int64>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFibonacciNumberValuesCommandHandler" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="repositoryFactory">
        /// The factory that produces data access repositories for the handler.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="repositoryFactory" /> is
        /// <see langword="null" />.
        /// </exception>
        public GetFibonacciNumberValuesCommandHandler(ICommandMediator mediator, ExampleRepositoryFactory repositoryFactory)
            : base(mediator, repositoryFactory)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="GetFibonacciNumberValuesCommandHandler" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <param name="repositories">
        /// An object that provides access to data access repositories.
        /// </param>
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        protected sealed override IEnumerable<Int64> Process(GetFibonacciNumberValuesCommand command, IFactoryProducedInstanceGroup repositories)
        {
            var numberSeriesNumberRepository = repositories.Get<NumberSeriesNumberRepository>();
            return numberSeriesNumberRepository.FindWhere(entity => entity.NumberSeries.Identifier == NumberSeries.Named.Fibonacci.Identifier).Select(entity => entity.Number.Value).ToList();
        }
    }
}