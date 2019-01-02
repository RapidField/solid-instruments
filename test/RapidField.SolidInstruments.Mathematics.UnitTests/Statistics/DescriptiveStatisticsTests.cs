// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Statistics;
using RapidField.SolidInstruments.Mathematics.Statistics.Extensions;
using RapidField.SolidInstruments.Serialization;
using System;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Statistics
{
    [TestClass]
    public sealed class DescriptiveStatisticsTests
    {
        [TestMethod]
        public void ShouldBeSerializable_UsingBinaryFormat()
        {
            // Arrange.
            var format = SerializationFormat.Binary;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingJsonFormat()
        {
            // Arrange.
            var format = SerializationFormat.Json;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingXmlFormat()
        {
            // Arrange.
            var format = SerializationFormat.Xml;

            // Assert.
            ShouldBeSerializable(format);
        }

        private static void ShouldBeSerializable(SerializationFormat format)
        {
            // Arrange.
            var series = new Double[] { 0d, 1d, 2d, 3d, 4d };
            var target = series.ComputeDescriptives();
            var serializer = new DynamicSerializer<DescriptiveStatistics>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().NotBeNull();
            deserializedResult.Maximum.Should().Be(target.Maximum);
            deserializedResult.Mean.Should().Be(target.Mean);
            deserializedResult.Median.Should().Be(target.Median);
            deserializedResult.Midrange.Should().Be(target.Midrange);
            deserializedResult.Minimum.Should().Be(target.Minimum);
            deserializedResult.Range.Should().Be(target.Range);
            deserializedResult.Size.Should().Be(target.Size);
            deserializedResult.StandardDeviation.Should().Be(target.StandardDeviation);
            deserializedResult.Sum.Should().Be(target.Sum);
            deserializedResult.Variance.Should().Be(target.Variance);
        }
    }
}