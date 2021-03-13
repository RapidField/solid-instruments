// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    [TestClass]
    public class NumberTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var cases = NumericTestPairList.Instance;
            var caseCount = cases.Count;

            for (var i = 0; i < caseCount; i++)
            {
                var target = cases[i];

                for (var j = 0; j < caseCount; j++)
                {
                    // Assert.
                    var subject = cases[j];
                    target.VerifyRelativeStateConsistency(subject);
                    target.VerifyArithmeticOperationalCorrectness(subject);
                }
            }
        }
    }
}