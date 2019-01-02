// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Prototype.DatabaseModel;
using RapidField.SolidInstruments.Prototype.DatabaseModel.Repositories;
using RapidField.SolidInstruments.TextEncoding;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests
{
    [TestClass]
    public class EntityFrameworkRepositoryTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var databaseName = EnhancedReadabilityGuid.New().ToString();
            var configuration = new ConfigurationBuilder().Build();
            var fibonacciNumberSeriesName = "Fibonacci";
            var fibonacciNumberSeriesValues = new Int64[] { 0, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89 };

            using (var context = new PrototypeInMemoryContext(configuration, databaseName).WithTestData())
            {
                using (var numberSeriesRepository = new NumberSeriesRepository(context))
                {
                    // Act.
                    var fibonacciNumberSeries = numberSeriesRepository.FindByName(fibonacciNumberSeriesName);

                    // Assert.
                    fibonacciNumberSeries.Should().NotBeNull();
                    fibonacciNumberSeries.Name.Should().Be(fibonacciNumberSeriesName);

                    using (var numberSeriesNumberRepository = new NumberSeriesNumberRepository(context))
                    {
                        // Act.
                        var fibonacciNumberSeriesNumbers = numberSeriesNumberRepository.FindWhere(entity => entity.NumberSeriesIdentifier == fibonacciNumberSeries.Identifier);
                        var fibonacciNumberIdentifiers = fibonacciNumberSeriesNumbers.Select(entity => entity.NumberIdentifier);

                        // Assert.
                        fibonacciNumberSeriesNumbers.Should().NotBeNull();
                        fibonacciNumberSeriesNumbers.Count().Should().Be(fibonacciNumberSeriesValues.Length);

                        using (var numberRepository = new NumberRepository(context))
                        {
                            // Act.
                            var fibonacciNumbers = numberRepository.FindWhere(entity => fibonacciNumberIdentifiers.Contains(entity.Identifier)).OrderBy(entity => entity.Value);

                            // Assert.
                            fibonacciNumbers.Select(entity => entity.Value).Should().BeEquivalentTo(fibonacciNumberSeriesValues);
                        }
                    }
                }
            }
        }
    }
}