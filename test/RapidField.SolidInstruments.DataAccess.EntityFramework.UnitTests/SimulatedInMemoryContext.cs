// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.Entities;
using RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.Repositories;
using System;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests
{
    /// <summary>
    /// Represents a connection to a prototypical, in-memory database.
    /// </summary>
    public sealed class SimulatedInMemoryContext : SimulatedContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedInMemoryContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="databaseName">
        /// The name of the backing database, which matches the associated connection string key in
        /// <paramref name="applicationConfiguration" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="databaseName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="databaseName" /> is
        /// <see langword="null" />.
        /// </exception>
        public SimulatedInMemoryContext(IConfiguration applicationConfiguration, String databaseName)
            : base(applicationConfiguration, ContextDatabaseType.InMemory, databaseName.RejectIf().IsNullOrEmpty(nameof(databaseName)))
        {
            return;
        }

        /// <summary>
        /// Populates the current <see cref="SimulatedInMemoryContext" /> with test data.
        /// </summary>
        /// <returns>
        /// The current <see cref="SimulatedInMemoryContext" /> with test data.
        /// </returns>
        [DebuggerHidden]
        internal SimulatedInMemoryContext WithTestData()
        {
            var fibonacciNumberSeriesValues = new Int64[] { 0, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89 };

            using (var numberRepository = new NumberRepository(this))
            {
                for (var i = 0; i <= 100; i++)
                {
                    var number = new Number()
                    {
                        Identifier = Guid.NewGuid(),
                        Value = i
                    };

                    numberRepository.Add(number);
                }

                SaveChanges();

                using (var numberSeriesRepository = new NumberSeriesRepository(this))
                {
                    numberSeriesRepository.Add(NumberSeries.Named.Fibonacci);
                    SaveChanges();

                    using (var numberSeriesNumberRepository = new NumberSeriesNumberRepository(this))
                    {
                        var fibonacciNumbers = numberRepository.FindWhere(entity => fibonacciNumberSeriesValues.Contains(entity.Value));

                        foreach (var fibonacciNumber in fibonacciNumbers)
                        {
                            var fibonacciNumberSeriesNumber = new NumberSeriesNumber()
                            {
                                Identifier = Guid.NewGuid(),
                                NumberIdentifier = fibonacciNumber.Identifier,
                                NumberSeriesIdentifier = NumberSeries.Named.Fibonacci.Identifier
                            };

                            numberSeriesNumberRepository.Add(fibonacciNumberSeriesNumber);
                        }

                        SaveChanges();
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Configures the in-memory database to be used for this context.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="optionsBuilder">
        /// A builder that is used to create or modify options for this context.
        /// </param>
        protected sealed override void OnConfiguringInMemory(IConfiguration applicationConfiguration, InMemoryDbContextOptionsBuilder optionsBuilder) => base.OnConfiguringInMemory(applicationConfiguration, optionsBuilder);

        /// <summary>
        /// Configures the SQL Server database to be used for this context.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="optionsBuilder">
        /// A builder that is used to create or modify options for this context.
        /// </param>
        protected sealed override void OnConfiguringSqlServer(IConfiguration applicationConfiguration, SqlServerDbContextOptionsBuilder optionsBuilder) => throw new InvalidOperationException();
    }
}