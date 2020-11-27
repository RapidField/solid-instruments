// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Caching;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Core.UnitTests.Caching
{
    [TestClass]
    public class InMemoryCacheClientTests
    {
        [TestMethod]
        public void Process_ShouldProduceDesiredResults_ForAggressiveStrategy() => Process_ShouldProduceDesiredResults(InMemoryCachingStrategy.Aggressive, true);

        [TestMethod]
        public void Process_ShouldProduceDesiredResults_ForConservativeStrategy() => Process_ShouldProduceDesiredResults(InMemoryCachingStrategy.Conservative, true);

        [TestMethod]
        public void Process_ShouldProduceDesiredResults_ForModerateStrategy() => Process_ShouldProduceDesiredResults(InMemoryCachingStrategy.Moderate, true);

        [TestMethod]
        public void Process_ShouldProduceDesiredResults_ForNoCachingStrategy() => Process_ShouldProduceDesiredResults(InMemoryCachingStrategy.NoCaching, false);

        private void Process_ShouldProduceDesiredResults(InMemoryCachingStrategy strategy, Boolean cacheShouldBeOperative)
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
            Assert.AreEqual(targetIsOperative, cacheShouldBeOperative);
            Assert.IsFalse(resultsOneAndTwoAreEqual);
            Assert.AreEqual(resultsOneAndThreeAreEqual, cacheShouldBeOperative);
            Assert.IsFalse(resultsOneAndFourAreEqual);
            Assert.IsFalse(resultsTwoAndThreeAreEqual);
            Assert.AreEqual(resultsTwoAndFourAreEqual, cacheShouldBeOperative);
            Assert.IsFalse(resultsThreeAndFourAreEqual);
        }
    }
}