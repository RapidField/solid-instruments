// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.Messaging.UnitTests
{
    [TestClass]
    public class MessageProcessingInformationTests
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
        public void ShouldBeSerializable_UsingCompressedJsonFormat()
        {
            // Arrange.
            var format = SerializationFormat.CompressedJson;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingCompressedXmlFormat()
        {
            // Arrange.
            var format = SerializationFormat.CompressedXml;

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
            var retryCount = 3;
            var baseDelayDurationInSeconds = 1;
            var durationScale = MessageListeningRetryDurationScale.Decelerating;
            var retryPolicy = new MessageListeningRetryPolicy(retryCount, baseDelayDurationInSeconds, durationScale);
            var secondaryFailureBehavior = MessageListeningSecondaryFailureBehavior.Discard;
            var transmitExceptionRaisedMessage = true;
            var failurePolicy = new MessageListeningFailurePolicy(retryPolicy, secondaryFailureBehavior, transmitExceptionRaisedMessage);
            var target = new MessageProcessingInformation(failurePolicy);
            var attemptEndTimeStamp = DateTime.UtcNow;
            var attemptStartTimeStamp = (DateTime?)null;
            var exceptionStackTrace = "Foo";
            var attemptResult = new MessageProcessingAttemptResult(attemptEndTimeStamp, attemptStartTimeStamp, exceptionStackTrace);
            target.AttemptResults.Add(attemptResult);
            var serializer = new DynamicSerializer<MessageProcessingInformation>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().NotBeNull();
            deserializedResult.AttemptCount.Should().Be(1);
            deserializedResult.AttemptResults.Should().NotBeNull();
            deserializedResult.AttemptResults.Count().Should().Be(1);
            deserializedResult.AttemptResults.Single().AttemptEndTimeStamp.Should().Be(attemptEndTimeStamp);
            deserializedResult.AttemptResults.Single().AttemptStartTimeStamp.Should().Be(attemptStartTimeStamp);
            deserializedResult.AttemptResults.Single().ExceptionStackTrace.Should().Be(exceptionStackTrace);
            deserializedResult.FailurePolicy.Should().NotBeNull();
            deserializedResult.FailurePolicy.TransmitExceptionRaisedEventMessage.Should().Be(transmitExceptionRaisedMessage);
            deserializedResult.FailurePolicy.RetryPolicy.Should().NotBeNull();
            deserializedResult.FailurePolicy.RetryPolicy.BaseDelayDurationInSeconds.Should().Be(baseDelayDurationInSeconds);
            deserializedResult.FailurePolicy.RetryPolicy.DurationScale.Should().Be(durationScale);
            deserializedResult.FailurePolicy.RetryPolicy.RetryCount.Should().Be(retryCount);
            deserializedResult.IsSuccessfullyProcessed.Should().Be(false);
        }
    }
}