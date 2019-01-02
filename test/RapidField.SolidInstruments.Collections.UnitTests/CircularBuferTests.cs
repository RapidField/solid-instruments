// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RapidField.SolidInstruments.Collections.UnitTests
{
    [TestClass]
    public class CircularBufferTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var capacity = 4;
            var target = new CircularBuffer<Int32>(capacity);

            // Assert.
            target.Length.Should().Be(0, "because the initial state of the buffer is empty");
            target.Capacity.Should().Be(capacity);

            // Act.
            target.Write(0);

            // Assert.
            target.Read().Should().Be(0);

            // Act.
            target.Write(1);

            // Assert.
            target.Read().Should().Be(1);

            // Act.
            target.Write(2);

            // Assert.
            target.Read().Should().Be(2);

            // Act.
            target.Write(3);

            // Assert.
            target.Read().Should().Be(3);

            // Act.
            target.Write(0);

            // Assert.
            target.Read().Should().Be(0);

            // Act.
            target.Write(0);
            target.Write(1);

            // Assert.
            target.Length.Should().Be(2);

            // Act.
            target.Write(2);
            target.Write(3);

            // Assert.
            target.Read().Should().Be(0);
            target.Read().Should().Be(1);
            target.Read().Should().Be(2);
            target.Length.Should().Be(1);

            // Act.
            target.Write(4);
            target.Write(5);

            // Assert.
            target.Capacity.Should().Be(capacity);

            // Act.
            target.Write(6);

            // Assert.
            target.Read().Should().Be(3);
            target.Length.Should().Be(3);
            target.Read().Should().Be(4);
            target.Read().Should().Be(5);
            target.Read().Should().Be(6);
            target.Length.Should().Be(0);
        }

        [TestMethod]
        public void Read_ShouldRaiseInvalidOperationException_ForEmptyBuffer()
        {
            // Arrange.
            var capacity = 4;
            var target = new CircularBuffer<Int32>(capacity);
            var result = default(Int32);
            target.Write(3);

            // Act.
            var action = new Action(() =>
            {
                result = target.Read();
                result = target.Read();
            });

            // Assert.
            action.Should().Throw<InvalidOperationException>("because the buffer is empty after the first read");
            result.Should().Be(3, "because the first read operation is valid");
        }

        [TestMethod]
        public void Write_ShouldNotRaiseException_ForPermittedOverwrite()
        {
            // Arrange.
            var capacity = 4;
            var target = new CircularBuffer<Int32>(capacity);
            target.Write(1, false);
            target.Write(2, false);
            target.Write(3, false);
            target.Write(4, false);

            // Act.
            var action = new Action(() =>
            {
                target.Write(5);
            });

            // Assert.
            action.Should().NotThrow("because overwrite is permitted");
            target.Read().Should().Be(5, "because the head value is overwritten");
        }

        [TestMethod]
        public void Write_ShouldRaiseInvalidOperationException_ForUnpermittedOverwrite()
        {
            // Arrange.
            var capacity = 4;
            var target = new CircularBuffer<Int32>(capacity);
            target.Write(1, false);
            target.Write(2, false);
            target.Write(3, false);
            target.Write(4, false);

            // Act.
            var action = new Action(() =>
            {
                target.Write(5, false);
            });

            // Assert.
            action.Should().Throw<InvalidOperationException>("because the buffer is full and overwrite is not permitted");
        }
    }
}