// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.DataAccess;
using RapidField.SolidInstruments.ObjectComposition;
using RapidField.SolidInstruments.Prototype.DatabaseModel.Commands;
using RapidField.SolidInstruments.Prototype.DatabaseModel.Entities;
using RapidField.SolidInstruments.Prototype.DatabaseModel.Repositories;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.Prototype.DatabaseModel.CommandHandlers
{
    /// <summary>
    /// Processes a command that adds a Fibonacci number to the Prototype database.
    /// </summary>
    public sealed class AddFibonacciNumberCommandHandler : PrototypeCommandHandler<AddFibonacciNumberCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddFibonacciNumberCommandHandler" /> class.
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
        public AddFibonacciNumberCommandHandler(ICommandMediator mediator, PrototypeRepositoryFactory repositoryFactory)
            : base(mediator, repositoryFactory)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AddFibonacciNumberCommandHandler" />.
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
        /// <param name="transaction">
        /// A transaction that is used to process the command.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected override void Process(AddFibonacciNumberCommand command, IFactoryProducedInstanceGroup repositories, IDataAccessTransaction transaction, ConcurrencyControlToken controlToken)
        {
            var fibonacciNumberSeries = NumberSeries.Named.Fibonacci;
            var numberRepository = repositories.Get<NumberRepository>();
            var number = numberRepository.FindByValue(command.NumberValue);

            if (number is null)
            {
                number = new Number()
                {
                    Identifier = Guid.NewGuid(),
                    Value = command.NumberValue
                };

                numberRepository.Add(number);
            }

            var numberSeriesNumberRespository = repositories.Get<NumberSeriesNumberRepository>();
            var numberSeriesNumber = numberSeriesNumberRespository.FindWhere(entity => entity.Number.Value == number.Value && entity.NumberSeriesIdentifier == fibonacciNumberSeries.Identifier).SingleOrDefault();

            if (numberSeriesNumber is null)
            {
                numberSeriesNumber = new NumberSeriesNumber()
                {
                    Identifier = Guid.NewGuid(),
                    Number = number,
                    NumberIdentifier = number.Identifier,
                    NumberSeriesIdentifier = fibonacciNumberSeries.Identifier
                };

                numberSeriesNumberRespository.Add(numberSeriesNumber);
            }
        }
    }
}