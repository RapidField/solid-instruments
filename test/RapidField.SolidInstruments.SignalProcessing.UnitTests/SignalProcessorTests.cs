// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RapidField.SolidInstruments.SignalProcessing.UnitTests
{
    [TestClass]
    public class SignalProcessorTests
    {
        [TestMethod]
        public void ReadAsync_ShouldReturnDefaultValue_ForSilentChannel_WithoutSampleRange_UsingValidIndex()
        {
            // Arrange.
            var channelA = new Int32[] { 1, 3, 5, 7, 9 };
            var channelB = new Int32[] { 2, 4, 6, 8, 10 };
            var factor = 2;
            var target = new SimulatedSignalProcessor(channelA, channelB, new SimulatedSignalProcessorSettings());
            var index = 2;
            target.Settings.Factor = factor;
            target.Toggle();

            // Act.
            var task = target.ReadAsync(index);
            task.Wait();
            var result = task.Result;

            // Assert.
            result.ChannelReadIndex.Should().Be(index);
            result.Value.Should().Be(default);
        }

        [TestMethod]
        public void ReadAsync_ShouldReturnDefaultValues_ForSilentChannel_WithSampleRange_UsingValidSampleRange()
        {
            // Arrange.
            var channelA = new Int32[] { 1, 3, 5, 7, 9 };
            var channelB = new Int32[] { 2, 4, 6, 8, 10 };
            var factor = 2;
            var target = new SimulatedSignalProcessor(channelA, channelB, new SimulatedSignalProcessorSettings());
            var index = 2;
            var lookBehindLength = 2;
            var lookAheadLength = 2;
            target.Settings.Factor = factor;
            target.Toggle();

            // Act.
            var task = target.ReadAsync(index, lookBehindLength, lookAheadLength);
            task.Wait();
            var result = task.Result;

            // Assert.
            result.UnitOfOutput.ChannelReadIndex.Should().Be(index);
            result.UnitOfOutput.Value.Should().Be(default);
            result.LookBehindRange.Should().HaveCount(0);
            result.LookAheadRange.Should().HaveCount(0);
        }

        [TestMethod]
        public void ReadAsync_ShouldReturnProcessedResult_ForLiveChannel_WithoutSampleRange_UsingValidIndex()
        {
            // Arrange.
            var channelA = new Int32[] { 1, 3, 5, 7, 9 };
            var channelB = new Int32[] { 2, 4, 6, 8, 10 };
            var factor = 2;
            var target = new SimulatedSignalProcessor(channelA, channelB, new SimulatedSignalProcessorSettings());
            var index = 2;
            target.Settings.Factor = factor;

            // Act.
            var task = target.ReadAsync(index);
            task.Wait();
            var result = task.Result;

            // Assert.
            result.ChannelReadIndex.Should().Be(index);
            result.Value.Should().Be(22);
        }

        [TestMethod]
        public void ReadAsync_ShouldReturnProcessedResult_ForLiveChannel_WithSampleRange_UsingValidSampleRange()
        {
            // Arrange.
            var channelA = new Int32[] { 1, 3, 5, 7, 9 };
            var channelB = new Int32[] { 2, 4, 6, 8, 10 };
            var factor = 2;
            var target = new SimulatedSignalProcessor(channelA, channelB, new SimulatedSignalProcessorSettings());
            var index = 2;
            var lookBehindLength = 2;
            var lookAheadLength = 2;
            target.Settings.Factor = factor;

            // Act.
            var task = target.ReadAsync(index, lookBehindLength, lookAheadLength);
            task.Wait();
            var result = task.Result;

            // Assert.
            result.UnitOfOutput.ChannelReadIndex.Should().Be(index);
            result.UnitOfOutput.Value.Should().Be(22);
            result.LookBehindRange.Should().HaveCount(lookBehindLength);
            result.LookBehindRange[0].Value.Should().Be(6);
            result.LookBehindRange[1].Value.Should().Be(14);
            result.LookAheadRange.Should().HaveCount(lookAheadLength);
            result.LookAheadRange[0].Value.Should().Be(30);
            result.LookAheadRange[1].Value.Should().Be(38);
        }
    }
}