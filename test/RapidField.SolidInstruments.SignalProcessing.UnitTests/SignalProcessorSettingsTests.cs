// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidField.SolidInstruments.SignalProcessing.UnitTests
{
    [TestClass]
    public class SignalProcessorSettingsTests
    {
        [TestMethod]
        public void Reset_ShouldReinitializeValues()
        {
            // Arrange.
            var target = new SimulatedSignalProcessorSettings();

            // Assert.
            target.Factor.Should().Be(SimulatedSignalProcessorSettings.DefaultFactor);

            // Act.
            target.Factor = 999;
            target.Reset();

            // Assert.
            target.Factor.Should().Be(SimulatedSignalProcessorSettings.DefaultFactor);
        }
    }
}