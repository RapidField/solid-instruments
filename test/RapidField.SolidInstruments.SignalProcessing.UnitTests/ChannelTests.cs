// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RapidField.SolidInstruments.SignalProcessing.UnitTests
{
    [TestClass]
    public class ChannelTests
    {
        [TestMethod]
        public void Indexer_ShouldRaiseIndexOutOfRangeException_UsingRaiseExceptionBehavior_UsingOutOfRangeIndex()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var invalidReadBehavior = InvalidReadBehavior.RaiseException;
            var target = new ObjectCollectionChannel<Int32>(collection, invalidReadBehavior);
            var index = 5;

            // Act.
            var action = new Action(() =>
            {
                var result = target[index];
            });

            // Assert.
            action.Should().Throw<IndexOutOfRangeException>();
        }

        [TestMethod]
        public void Indexer_ShouldReturnDefaultValue_ForSilentChannel_UsingValidIndex()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var target = new ObjectCollectionChannel<Int32>(collection);
            var index = 2;
            target.Toggle();

            // Act.
            var result = target[index];

            // Assert.
            result.Should().Be(default);
        }

        [TestMethod]
        public void Indexer_ShouldReturnDefaultValue_UsingReadSilenceBehavior_UsingOutOfRangeIndex()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var invalidReadBehavior = InvalidReadBehavior.ReadSilence;
            var target = new ObjectCollectionChannel<Int32>(collection, invalidReadBehavior);
            var index = 5;

            // Act.
            var result = target[index];

            // Assert.
            result.Should().Be(default);
        }

        [TestMethod]
        public void Indexer_ShouldReturnRequestedValue_ForLiveChannel_UsingValidIndex()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var target = new ObjectCollectionChannel<Int32>(collection);
            var index = 2;

            // Act.
            var result = target[index];

            // Assert.
            result.Should().Be(collection[index]);
        }

        [TestMethod]
        public void OutputType_ShouldReflectChannelOutputType()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var target = new ObjectCollectionChannel<Int32>(collection);

            // Act.
            var result = target.OutputType;

            // Assert.
            result.Should().Be(typeof(Int32));
        }

        [TestMethod]
        public void ReadAsync_ShouldRaiseArgumentOutOfRangeException_ForLiveChannel_WithoutSampleRange_UsingRaiseExceptionBehavior_UsingOutOfRangeIndex()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var invalidReadBehavior = InvalidReadBehavior.RaiseException;
            var target = new ObjectCollectionChannel<Int32>(collection, invalidReadBehavior);
            var index = 5;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    var task = target.ReadAsync(index);
                    task.Wait();
                }
                catch (AggregateException exception)
                {
                    throw exception.Flatten().InnerException;
                }
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ReadAsync_ShouldRaiseArgumentOutOfRangeException_ForLiveChannel_WithSampleRange_UsingRaiseExceptionBehavior_UsingOutOfRangeSample()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var invalidReadBehavior = InvalidReadBehavior.RaiseException;
            var target = new ObjectCollectionChannel<Int32>(collection, invalidReadBehavior);
            var index = 2;
            var lookBehindLength = 1;
            var lookAheadLength = 3;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    var task = target.ReadAsync(index, lookBehindLength, lookAheadLength);
                    task.Wait();
                }
                catch (AggregateException exception)
                {
                    throw exception.Flatten().InnerException;
                }
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ReadAsync_ShouldReturnDefaultValue_ForLiveChannel_WithoutSampleRange_UsingReadSilenceBehavior_UsingOutOfRangeIndex()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var invalidReadBehavior = InvalidReadBehavior.ReadSilence;
            var target = new ObjectCollectionChannel<Int32>(collection, invalidReadBehavior);
            var index = 5;

            // Act.
            var task = target.ReadAsync(index);
            task.Wait();
            var result = task.Result;

            // Assert.
            result.ChannelReadIndex.Should().Be(index);
            result.Value.Should().Be(default);
        }

        [TestMethod]
        public void ReadAsync_ShouldReturnDefaultValue_ForLiveChannel_WithSampleRange_UsingReadSilenceBehavior_UsingOutOfRangeSample()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var invalidReadBehavior = InvalidReadBehavior.ReadSilence;
            var target = new ObjectCollectionChannel<Int32>(collection, invalidReadBehavior);
            var index = 2;
            var lookBehindLength = 1;
            var lookAheadLength = 3;

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
        public void ReadAsync_ShouldReturnDefaultValue_ForSilentChannel_WithoutSampleRange_UsingValidIndex()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var target = new ObjectCollectionChannel<Int32>(collection);
            var index = 2;
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
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var target = new ObjectCollectionChannel<Int32>(collection);
            var index = 2;
            var lookBehindLength = 2;
            var lookAheadLength = 2;
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
        public void ReadAsync_ShouldReturnRequestedValue_ForLiveChannel_WithoutSampleRange_UsingValidIndex()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var target = new ObjectCollectionChannel<Int32>(collection);
            var index = 2;

            // Act.
            var task = target.ReadAsync(index);
            task.Wait();
            var result = task.Result;

            // Assert.
            result.ChannelReadIndex.Should().Be(index);
            result.Value.Should().Be(collection[index]);
        }

        [TestMethod]
        public void ReadAsync_ShouldReturnRequestedValues_ForLiveChannel_WithSampleRange_UsingValidSampleRange()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var target = new ObjectCollectionChannel<Int32>(collection);
            var index = 2;
            var lookBehindLength = 2;
            var lookAheadLength = 2;

            // Act.
            var task = target.ReadAsync(index, lookBehindLength, lookAheadLength);
            task.Wait();
            var result = task.Result;

            // Assert.
            result.UnitOfOutput.ChannelReadIndex.Should().Be(index);
            result.UnitOfOutput.Value.Should().Be(collection[index]);
            result.LookBehindRange.Should().HaveCount(lookBehindLength);
            result.LookBehindRange[0].ChannelReadIndex.Should().Be(0);
            result.LookBehindRange[0].Value.Should().Be(3);
            result.LookBehindRange[1].ChannelReadIndex.Should().Be(1);
            result.LookBehindRange[1].Value.Should().Be(4);
            result.LookAheadRange.Should().HaveCount(lookAheadLength);
            result.LookAheadRange[0].ChannelReadIndex.Should().Be(3);
            result.LookAheadRange[0].Value.Should().Be(6);
            result.LookAheadRange[1].ChannelReadIndex.Should().Be(4);
            result.LookAheadRange[1].Value.Should().Be(7);
        }

        [TestMethod]
        public void ReadSilence_ShouldReturnDefaultValue()
        {
            // Arrange.
            var collection = new Int32[] { 3, 4, 5, 6, 7 };
            var target = new ObjectCollectionChannel<Int32>(collection);
            var index = 2;

            // Act.
            var result = target.ReadSilence(index);

            // Assert.
            result.ChannelReadIndex.Should().Be(index);
            result.Value.Should().Be(default);
        }
    }
}