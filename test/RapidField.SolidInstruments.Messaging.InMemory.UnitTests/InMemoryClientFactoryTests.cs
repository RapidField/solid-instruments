// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests
{
    [TestClass]
    public class InMemoryClientFactoryTests
    {
        [TestMethod]
        public void GetQueuePath_ShouldProduceDesiredResults_ForPathWithOneLabel()
        {
            // Arrange.
            var entityType = MessagingEntityType.Queue;

            // Act.
            GetPath_ShouldProduceDesiredResults_ForPathWithOneLabel(entityType);
        }

        [TestMethod]
        public void GetQueuePath_ShouldProduceDesiredResults_ForPathWithoutLabels()
        {
            // Arrange.
            var entityType = MessagingEntityType.Queue;

            // Act.
            GetPath_ShouldProduceDesiredResults_ForPathWithoutLabels(entityType);
        }

        [TestMethod]
        public void GetQueuePath_ShouldProduceDesiredResults_ForPathWithThreeLabels()
        {
            // Arrange.
            var entityType = MessagingEntityType.Queue;

            // Act.
            GetPath_ShouldProduceDesiredResults_ForPathWithThreeLabels(entityType);
        }

        [TestMethod]
        public void GetQueuePath_ShouldProduceDesiredResults_ForPathWithTwoLabels()
        {
            // Arrange.
            var entityType = MessagingEntityType.Queue;

            // Act.
            GetPath_ShouldProduceDesiredResults_ForPathWithTwoLabels(entityType);
        }

        [TestMethod]
        public void GetTopicPath_ShouldProduceDesiredResults_ForPathWithOneLabel()
        {
            // Arrange.
            var entityType = MessagingEntityType.Topic;

            // Act.
            GetPath_ShouldProduceDesiredResults_ForPathWithOneLabel(entityType);
        }

        [TestMethod]
        public void GetTopicPath_ShouldProduceDesiredResults_ForPathWithoutLabels()
        {
            // Arrange.
            var entityType = MessagingEntityType.Topic;

            // Act.
            GetPath_ShouldProduceDesiredResults_ForPathWithoutLabels(entityType);
        }

        [TestMethod]
        public void GetTopicPath_ShouldProduceDesiredResults_ForPathWithThreeLabels()
        {
            // Arrange.
            var entityType = MessagingEntityType.Topic;

            // Act.
            GetPath_ShouldProduceDesiredResults_ForPathWithThreeLabels(entityType);
        }

        [TestMethod]
        public void GetTopicPath_ShouldProduceDesiredResults_ForPathWithTwoLabels()
        {
            // Arrange.
            var entityType = MessagingEntityType.Topic;

            // Act.
            GetPath_ShouldProduceDesiredResults_ForPathWithTwoLabels(entityType);
        }

        private static void GetPath_ShouldProduceDesiredResults<TMessage>(MessagingEntityType entityType, String[] pathLabels)
            where TMessage : class, IMessage
        {
            // Arrange.
            var serializationFormat = SerializationFormat.CompressedJson;
            var messageType = typeof(TMessage);
            var messageTypeName = (messageType.GetCustomAttributes(typeof(DataContractAttribute), false).FirstOrDefault() as DataContractAttribute)?.Name ?? messageType.Name;

            using (var transport = new MessageTransport(serializationFormat))
            {
                using (var connection = transport.CreateConnection())
                {
                    using (var target = new InMemoryClientFactory(connection))
                    {
                        // Act.
                        var path = entityType switch
                        {
                            MessagingEntityType.Queue => target.GetQueuePath<TMessage>(pathLabels),
                            MessagingEntityType.Topic => target.GetTopicPath<TMessage>(pathLabels),
                            _ => null
                        };

                        // Assert.
                        path.Should().NotBeNull();
                        path.LabelOne.Should().Be(pathLabels.Length > 0 ? pathLabels[0] : null);
                        path.LabelTwo.Should().Be(pathLabels.Length > 1 ? pathLabels[1] : null);
                        path.LabelThree.Should().Be(pathLabels.Length > 2 ? pathLabels[2] : null);
                        path.MessageType.Should().NotBeNullOrEmpty();
                        path.MessageType.Should().Be(messageTypeName);
                        path.Prefix.Should().BeNull();

                        // Act.
                        var pathString = path.ToString();

                        // Assert.
                        pathString.Should().NotBeNullOrEmpty();
                    }
                }
            }
        }

        private static void GetPath_ShouldProduceDesiredResults_ForPathWithOneLabel(MessagingEntityType entityType)
        {
            // Arrange.
            var pathLabels = new String[] { "foo" };

            // Act.
            GetPath_ShouldProduceDesiredResults<Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage>(entityType, pathLabels);
        }

        private static void GetPath_ShouldProduceDesiredResults_ForPathWithoutLabels(MessagingEntityType entityType)
        {
            // Arrange.
            var pathLabels = Array.Empty<String>();

            // Act.
            GetPath_ShouldProduceDesiredResults<Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage>(entityType, pathLabels);
        }

        private static void GetPath_ShouldProduceDesiredResults_ForPathWithThreeLabels(MessagingEntityType entityType)
        {
            // Arrange.
            var pathLabels = new String[] { "foo", "bar", "baz" };

            // Act.
            GetPath_ShouldProduceDesiredResults<Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage>(entityType, pathLabels);
        }

        private static void GetPath_ShouldProduceDesiredResults_ForPathWithTwoLabels(MessagingEntityType entityType)
        {
            // Arrange.
            var pathLabels = new String[] { "foo", "bar" };

            // Act.
            GetPath_ShouldProduceDesiredResults<Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage>(entityType, pathLabels);
        }
    }
}