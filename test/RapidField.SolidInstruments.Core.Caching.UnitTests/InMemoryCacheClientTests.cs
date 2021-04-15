// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Core.Caching.UnitTests
{
    [TestClass]
    public class InMemoryCacheClientTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForAggressiveStrategy() => FunctionalLifeSpanTest_ShouldProduceDesiredResults(InMemoryCachingStrategy.Aggressive, true);

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForConservativeStrategy() => FunctionalLifeSpanTest_ShouldProduceDesiredResults(InMemoryCachingStrategy.Conservative, true);

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForModerateStrategy() => FunctionalLifeSpanTest_ShouldProduceDesiredResults(InMemoryCachingStrategy.Moderate, true);

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForNoCachingStrategy() => FunctionalLifeSpanTest_ShouldProduceDesiredResults(InMemoryCachingStrategy.NoCaching, false);

        [TestMethod]
        public void Process_ShouldProduceDesiredResults_ForAggressiveStrategy() => Process_ShouldProduceDesiredResults(InMemoryCachingStrategy.Aggressive, true);

        [TestMethod]
        public void Process_ShouldProduceDesiredResults_ForConservativeStrategy() => Process_ShouldProduceDesiredResults(InMemoryCachingStrategy.Conservative, true);

        [TestMethod]
        public void Process_ShouldProduceDesiredResults_ForModerateStrategy() => Process_ShouldProduceDesiredResults(InMemoryCachingStrategy.Moderate, true);

        [TestMethod]
        public void Process_ShouldProduceDesiredResults_ForNoCachingStrategy() => Process_ShouldProduceDesiredResults(InMemoryCachingStrategy.NoCaching, false);

        private static void FunctionalLifeSpanTest_ShouldProduceDesiredResults(InMemoryCachingStrategy strategy, Boolean cacheShouldBeOperative)
        {
            // Arrange.
            var keyOne = "foo";
            var keyTwo = "bar";
            using var randomnessProvider = RandomNumberGenerator.Create();
            var valueOne = SimulatedModel.Random(randomnessProvider);
            var valueTwo = SimulatedModel.Random(randomnessProvider);
            using var target = new InMemoryCacheClient(strategy);

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
            target.Clear();
            var resultNine = target.TryRead<SimulatedModel>(keyTwo, out var resultValueNine);

            if (cacheShouldBeOperative)
            {
                // Assert.
                resultOne.Should().BeTrue();
                resultTwo.Should().BeFalse();
                resultThree.Should().BeFalse();
                resultFour.Should().BeTrue();
                resultFive.Should().BeFalse();
                resultSix.Should().BeTrue();
                resultSeven.Should().BeFalse();
                resultEight.Should().BeTrue();
                resultNine.Should().BeFalse();
                resultValueOne.Should().BeSameAs(valueOne);
                resultValueTwo.Should().BeNull();
                resultValueThree.Should().BeNull();
                resultValueFour.Should().BeSameAs(valueOne);
                resultValueFive.Should().BeNull();
                resultValueSix.Should().BeSameAs(valueTwo);
                resultValueSeven.Should().BeNull();
                resultValueEight.Should().BeSameAs(valueTwo);
                resultValueNine.Should().BeNull();
            }
            else
            {
                // Assert.
                resultOne.Should().BeFalse();
                resultTwo.Should().BeFalse();
                resultThree.Should().BeFalse();
                resultFour.Should().BeFalse();
                resultFive.Should().BeFalse();
                resultSix.Should().BeFalse();
                resultSeven.Should().BeFalse();
                resultEight.Should().BeFalse();
                resultNine.Should().BeFalse();
                resultValueOne.Should().BeNull();
                resultValueTwo.Should().BeNull();
                resultValueThree.Should().BeNull();
                resultValueFour.Should().BeNull();
                resultValueFive.Should().BeNull();
                resultValueSix.Should().BeNull();
                resultValueSeven.Should().BeNull();
                resultValueEight.Should().BeNull();
                resultValueNine.Should().BeNull();
            }
        }

        private static void Process_ShouldProduceDesiredResults(InMemoryCachingStrategy strategy, Boolean cacheShouldBeOperative)
        {
            // Arrange.
            var keyOne = "foo";
            var keyTwo = "bar";
            using var randomnessProvider = RandomNumberGenerator.Create();
            var produceValueFunction = new Func<SimulatedModel>(() => SimulatedModel.Random(randomnessProvider));
            using var target = new InMemoryCacheClient(strategy);

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
            targetIsOperative.Should().Be(cacheShouldBeOperative);
            resultsOneAndTwoAreEqual.Should().BeFalse();
            resultsOneAndThreeAreEqual.Should().Be(cacheShouldBeOperative);
            resultsOneAndFourAreEqual.Should().BeFalse();
            resultsTwoAndThreeAreEqual.Should().BeFalse();
            resultsTwoAndFourAreEqual.Should().Be(cacheShouldBeOperative);
            resultsThreeAndFourAreEqual.Should().BeFalse();
        }
    }
}