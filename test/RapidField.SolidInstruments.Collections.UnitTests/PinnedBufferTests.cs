// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Collections.UnitTests
{
    [TestClass]
    public class PinnedBufferTests
    {
        [TestMethod]
        public void Constructor_ShouldRaiseArgumentOutOfRangeException_ForNegativeLengthArgument()
        {
            // Arrange.
            var length = -1;

            // Act.
            var action = new Action(() =>
            {
                using (var target = new PinnedBuffer(length))
                {
                    return;
                }
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentOutOfRangeException_ForZeroLengthArgument()
        {
            // Arrange.
            var length = 0;

            // Act.
            var action = new Action(() =>
            {
                using (var target = new PinnedBuffer(length))
                {
                    return;
                }
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_UsingFieldConstructor()
        {
            // Arrange.
            var length = 8;
            var field = new Byte[length];

            using (var target = new PinnedBuffer(field, true))
            {
                // Assert.
                ReferenceEquals(field, target).Should().BeFalse();
                target.Length.Should().Be(length);
                field.Should().NotBeNull();
                field.Length.Should().Be(length);
                field.Should().OnlyContain(value => value == 0x00);

                // Act.
                target[5] = 0x4a;
                target[3] = 0xf6;

                // Assert.
                field[1].Should().Be(0x00);
                field[3].Should().Be(0xf6);
                field[5].Should().Be(0x4a);
            }

            // Assert.
            field.Should().OnlyContain(value => value == 0x00);
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_UsingLengthConstructor()
        {
            // Arrange.
            var length = 8;
            var field = (Byte[])null;

            using (var target = new PinnedBuffer(length, true))
            {
                // Arrange.
                field = target;

                // Assert.
                ReferenceEquals(field, target).Should().BeFalse();
                target.Length.Should().Be(length);
                field.Should().NotBeNull();
                field.Length.Should().Be(length);
                field.Should().OnlyContain(value => value == 0x00);

                // Act.
                target[5] = 0x4a;
                target[3] = 0xf6;

                // Assert.
                field[1].Should().Be(0x00);
                field[3].Should().Be(0xf6);
                field[5].Should().Be(0x4a);
            }

            // Assert.
            field.Should().OnlyContain(value => value == 0x00);
        }

        [TestMethod]
        public void LengthInBytes_ShouldProduceDesiredResults()
        {
            // Arrange.
            var length = 10;
            var field = new Int32[length];

            using (var target = new PinnedBuffer<Int32>(field, false))
            {
                // Assert.
                target.LengthInBytes.Should().Be(length * 4);
            }
        }

        [TestMethod]
        public void OverwriteWithZeros_ShouldProduceDesiredResults_UsingFieldConstructor()
        {
            // Arrange.
            var length = 8;
            var field = new Byte[length];

            using (var target = new PinnedBuffer(field, false))
            {
                // Arrange.
                target[0] = 0x01;
                target[2] = 0x01;
                target[4] = 0x01;
                target[6] = 0x01;

                // Assert.
                field[0].Should().Be(0x01);
                field[2].Should().Be(0x01);
                field[4].Should().Be(0x01);
                field[6].Should().Be(0x01);

                // Act.
                target.OverwriteWithZeros();

                // Assert.
                target.Should().OnlyContain(value => value == 0x00);
                field.Should().OnlyContain(value => value == 0x00);
            }
        }

        [TestMethod]
        public void OverwriteWithZeros_ShouldProduceDesiredResults_UsingLengthConstructor()
        {
            // Arrange.
            var length = 8;

            using (var target = new PinnedBuffer(length, false))
            {
                // Arrange.
                target[0] = 0x01;
                target[2] = 0x01;
                target[4] = 0x01;
                target[6] = 0x01;

                // Act.
                target.OverwriteWithZeros();

                // Assert.
                target.Should().OnlyContain(value => value == 0x00);
            }
        }
    }
}