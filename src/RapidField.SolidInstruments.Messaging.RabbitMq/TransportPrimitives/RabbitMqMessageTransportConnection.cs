// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RabbitMQ.Client;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using IRabbitMqChannel = RabbitMQ.Client.IModel;
using IRabbitMqConsumer = RabbitMQ.Client.IBasicConsumer;
using RabbitMqConsumer = RabbitMQ.Client.Events.EventingBasicConsumer;

namespace RapidField.SolidInstruments.Messaging.RabbitMq.TransportPrimitives
{
    /// <summary>
    /// Represents a client connection to a <see cref="RabbitMqMessageTransport" />.
    /// </summary>
    internal sealed class RabbitMqMessageTransportConnection : Instrument, IMessageTransportConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessageTransportConnection" /> class.
        /// </summary>
        /// <param name="transport">
        /// The associated transport.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="transport" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal RabbitMqMessageTransportConnection(RabbitMqMessageTransport transport)
            : base()
        {
            Channels = new List<IRabbitMqChannel>();
            Consumers = new List<IRabbitMqConsumer>();
            Identifier = Guid.NewGuid();
            State = MessageTransportConnectionState.Open;
            TransportReference = transport.RejectIf().IsNull(nameof(transport)).TargetArgument;
        }

        /// <summary>
        /// Closes the current <see cref="RabbitMqMessageTransportConnection" /> as an idempotent operation.
        /// </summary>
        /// <exception cref="MessagingException">
        /// An exception was raised while closing the transport connection.
        /// </exception>
        public void Close()
        {
            if (State == MessageTransportConnectionState.Open)
            {
                State = MessageTransportConnectionState.Closed;

                try
                {
                    try
                    {
                        if (Channels?.Any() ?? false)
                        {
                            foreach (var channel in Channels)
                            {
                                channel?.Dispose();
                            }
                        }
                    }
                    finally
                    {
                        if (TransportReference?.Connections?.Any(connection => connection.Identifier == Identifier) ?? false)
                        {
                            TransportReference?.CloseConnection(this);
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new MessagingException("An exception was raised while closing a transport connection. See inner exception.", exception);
                }
            }
        }

        /// <summary>
        /// Registers the specified message handler for the specified queue.
        /// </summary>
        /// <param name="queuePath">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="handleMessageAction">
        /// An action to perform upon message receipt.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queuePath" /> is <see langword="null" /> -or- <paramref name="handleMessageAction" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified queue does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RegisterQueueHandler(IMessagingEntityPath queuePath, Action<PrimitiveMessage> handleMessageAction)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (Transport.QueueExists(queuePath))
                {
                    _ = handleMessageAction.RejectIf().IsNull(nameof(handleMessageAction));
                    var queueName = queuePath.ToString();
                    var channel = TransportReference.SharedConnection.CreateModel();
                    channel.ConfirmSelect();

                    try
                    {
                        var consumer = new RabbitMqConsumer(channel);
                        consumer.Received += (model, eventArguments) =>
                        {
                            try
                            {
                                var messageBytes = eventArguments.Body.ToArray();
                                var serializer = new DynamicSerializer<PrimitiveMessage>(Transport.MessageBodySerializationFormat);
                                var message = serializer.Deserialize(messageBytes);
                                message.LockToken.DeliveryTag = eventArguments.DeliveryTag;

                                try
                                {
                                    handleMessageAction(message);
                                }
                                catch (Exception exception)
                                {
                                    throw new MessageListeningException($"An exception was raised while processing a message from queue \"{queueName}\". Message identifier: {message?.Identifier.ToSerializedString() ?? "unknown"}.", exception);
                                }
                            }
                            catch (MessageListeningException)
                            {
                                throw;
                            }
                            catch (SerializationException exception)
                            {
                                throw new MessageListeningException($"Failed to deserialize a message from queue \"{queueName}\". Expected format: {Transport.MessageBodySerializationFormat}.", exception);
                            }
                        };

                        try
                        {
                            channel.BasicConsume(queueName, RabbitMqMessageTransport.AutoAcknowledgeIsEnabled, consumer);
                            channel.WaitForConfirmsOrDie();
                            Consumers.Add(consumer);
                        }
                        catch (Exception exception)
                        {
                            throw new MessageListeningException($"An exception was raised while attempting to register a consumer for queue \"{queueName}\".", exception);
                        }

                        Channels.Add(channel);
                    }
                    catch
                    {
                        channel.Dispose();
                        throw;
                    }

                    return;
                }

                throw new InvalidOperationException($"Failed to register queue handler. The specified queue, \"{queuePath}\", does not exist.");
            }
        }

        /// <summary>
        /// Registers the specified message handler for the specified topic subscription.
        /// </summary>
        /// <param name="topicPath">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <param name="handleMessageAction">
        /// An action to perform upon message receipt.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="topicPath" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" /> -or- <paramref name="handleMessageAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified subscription does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RegisterSubscriptionHandler(IMessagingEntityPath topicPath, String subscriptionName, Action<PrimitiveMessage> handleMessageAction)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (Transport.SubscriptionExists(topicPath, subscriptionName))
                {
                    _ = handleMessageAction.RejectIf().IsNull(nameof(handleMessageAction));
                    var topicName = topicPath.ToString();
                    var channel = TransportReference.SharedConnection.CreateModel();
                    channel.ConfirmSelect();

                    try
                    {
                        var consumer = new RabbitMqConsumer(channel);
                        consumer.Received += (model, eventArguments) =>
                        {
                            try
                            {
                                var messageBytes = eventArguments.Body.ToArray();
                                var serializer = new DynamicSerializer<PrimitiveMessage>(Transport.MessageBodySerializationFormat);
                                var message = serializer.Deserialize(messageBytes);
                                message.LockToken.DeliveryTag = eventArguments.DeliveryTag;

                                try
                                {
                                    handleMessageAction(message);
                                }
                                catch (Exception exception)
                                {
                                    throw new MessageListeningException($"An exception was raised while processing a message from topic \"{topicName}\" for subscription \"{subscriptionName}\". Message identifier: {message?.Identifier.ToSerializedString() ?? "unknown"}.", exception);
                                }
                            }
                            catch (MessageListeningException)
                            {
                                throw;
                            }
                            catch (SerializationException exception)
                            {
                                throw new MessageListeningException($"Failed to deserialize a message from topic \"{topicName}\" for subscription \"{subscriptionName}\". Expected format: {Transport.MessageBodySerializationFormat}.", exception);
                            }
                        };

                        try
                        {
                            channel.BasicConsume(subscriptionName, RabbitMqMessageTransport.AutoAcknowledgeIsEnabled, consumer);
                            channel.WaitForConfirmsOrDie();
                            Consumers.Add(consumer);
                        }
                        catch (Exception exception)
                        {
                            throw new MessageListeningException($"An exception was raised while attempting to register a consumer for topic \"{topicName}\" with subscription name \"{subscriptionName}\".", exception);
                        }

                        Channels.Add(channel);
                    }
                    catch
                    {
                        channel.Dispose();
                        throw;
                    }

                    return;
                }

                throw new InvalidOperationException($"Failed to register subscription handler. The specified subscription, \"{subscriptionName}\", does not exist.");
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="RabbitMqMessageTransportConnection" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        /// <exception cref="MessagingException">
        /// An exception was raised while closing the transport connection.
        /// </exception>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                Close();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets a value that uniquely identifies the current <see cref="RabbitMqMessageTransportConnection" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the state of the current <see cref="RabbitMqMessageTransportConnection" />.
        /// </summary>
        public MessageTransportConnectionState State
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the associated <see cref="IMessageTransport" />.
        /// </summary>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The connection is closed.
        /// </exception>
        public IMessageTransport Transport => State == MessageTransportConnectionState.Open ? TransportReference : throw new MessageTransportConnectionClosedException($"Connection {Identifier.ToSerializedString()} is closed.");

        /// <summary>
        /// Represents a collection of channels that are registered with the current
        /// <see cref="RabbitMqMessageTransportConnection" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<IRabbitMqChannel> Channels;

        /// <summary>
        /// Represents a collection of consumers that are registered with the current
        /// <see cref="RabbitMqMessageTransportConnection" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<IRabbitMqConsumer> Consumers;

        /// <summary>
        /// Represents the associated <see cref="RabbitMqMessageTransport" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly RabbitMqMessageTransport TransportReference;
    }
}