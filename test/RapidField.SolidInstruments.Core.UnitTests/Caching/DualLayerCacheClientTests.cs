// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Caching;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Core.UnitTests.Caching
{
    [TestClass]
    public class DualLayerCacheClientTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var keyOne = "foo";
            var keyTwo = "bar";
            using var randomnessProvider = RandomNumberGenerator.Create();
            var valueOne = SimulatedModel.Random(randomnessProvider);
            var valueTwo = SimulatedModel.Random(randomnessProvider);
            var remoteCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            using var target = new DualLayerCacheClient(remoteCache);

            // Act.
            target.Write(keyOne, valueOne);
            var resultOne = target.TryRead<SimulatedModel>(keyOne, out var resultValueOne);
            var resultTwo = target.TryRead<String>(keyOne, out var resultValueTwo);
            var resultThree = target.TryRead<SimulatedModel>(keyTwo, out var resultValueThree);
            target.Write(keyTwo, valueTwo);
            var resultFour = target.TryRead<SimulatedModel>(keyOne, out var resultValueFour);
            var resultFive = target.TryRead<String>(keyTwo, out var resultValueFive);
            var resultSix = target.TryRead<SimulatedModel>(keyTwo, out var resultValueSix);
            target.Invalidate(keyOne);
            var resultSeven = target.TryRead<SimulatedModel>(keyOne, out var resultValueSeven);
            var resultEight = target.TryRead<SimulatedModel>(keyTwo, out var resultValueEight);

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
            resultSeven.Should().BeFalse();
            resultEight.Should().BeTrue();
            resultValueOne.Should().Be(valueOne);
            resultValueTwo.Should().BeNull();
            resultValueThree.Should().BeNull();
            resultValueFour.Should().Be(valueOne);
            resultValueFive.Should().BeNull();
            resultValueSix.Should().Be(valueTwo);
            resultValueSeven.Should().BeNull();
            resultValueEight.Should().Be(valueTwo);
        }

        [TestMethod]
        public void Process_ShouldProduceDesiredResults()
        {
            // Arrange.
            var keyOne = "foo";
            var keyTwo = "bar";
            using var randomnessProvider = RandomNumberGenerator.Create();
            var produceValueFunction = new Func<SimulatedModel>(() => SimulatedModel.Random(randomnessProvider));
            var remoteCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            using var target = new DualLayerCacheClient(remoteCache);

            // Act.
            var targetIsOperative = target.IsOperative;
            var resultOne = target.Process(keyOne, produceValueFunction);
            var resultTwo = target.Process(keyTwo, produceValueFunction);
            var resultThree = target.Process(keyOne, produceValueFunction);
            var resultFour = target.Process(keyTwo, produceValueFunction);
            var resultOneHashCode = resultOne.GetImpliedHashCode();
            var resultTwoHashCode = resultTwo.GetImpliedHashCode();
            var resultThreeHashCode = resultThree.GetImpliedHashCode();
            var resultFourHashCode = resultFour.GetImpliedHashCode();
            var resultsOneAndTwoAreEqual = resultOneHashCode == resultTwoHashCode;
            var resultsOneAndThreeAreEqual = resultOneHashCode == resultThreeHashCode;
            var resultsOneAndFourAreEqual = resultOneHashCode == resultFourHashCode;
            var resultsTwoAndThreeAreEqual = resultTwoHashCode == resultThreeHashCode;
            var resultsTwoAndFourAreEqual = resultTwoHashCode == resultFourHashCode;
            var resultsThreeAndFourAreEqual = resultThreeHashCode == resultFourHashCode;

            // Assert.
            targetIsOperative.Should().BeTrue();
            resultsOneAndTwoAreEqual.Should().BeFalse();
            resultsOneAndThreeAreEqual.Should().BeTrue();
            resultsOneAndFourAreEqual.Should().BeFalse();
            resultsTwoAndThreeAreEqual.Should().BeFalse();
            resultsTwoAndFourAreEqual.Should().BeTrue();
            resultsThreeAndFourAreEqual.Should().BeFalse();
        }
    }
}