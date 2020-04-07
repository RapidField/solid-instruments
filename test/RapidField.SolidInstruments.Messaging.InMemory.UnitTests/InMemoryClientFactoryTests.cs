// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using RapidField.SolidInstruments.Serialization;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests
{
    [TestClass]
    public class InMemoryClientFactoryTests
    {
        [TestMethod]
        public void GetQueuePath_Should()
        {
            // Arrange.
            var serializationFormat = SerializationFormat.CompressedJson;

            using (var transport = new MessageTransport(serializationFormat))
            {
                using (var connection = transport.CreateConnection())
                {
                    using (var target = new InMemoryClientFactory(connection))
                    {
                        // Act.
                    }
                }
            }
        }
    }
}