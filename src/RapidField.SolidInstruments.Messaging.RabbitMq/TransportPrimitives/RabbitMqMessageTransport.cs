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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IRabbitMqConnection = RabbitMQ.Client.IConnection;
using IRabbitMqConnectionFactory = RabbitMQ.Client.IConnectionFactory;
using RabbitMqConnectionFactory = RabbitMQ.Client.ConnectionFactory;

namespace RapidField.SolidInstruments.Messaging.RabbitMq.TransportPrimitives
{
    /// <summary>
    /// Supports message exchange for RabbitMQ queues and topics.
    /// </summary>
    internal sealed class RabbitMqMessageTransport : Instrument, IMessageTransport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessageTransport" /> class.
        /// </summary>
        [DebuggerHidden]
        internal RabbitMqMessageTransport()
            : this(DefaultConnectionHostName, null, null, null, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessageTransport" /> class.
        /// </summary>
        /// <param name="connectionUri">
        /// The connection URI for the target RabbitMQ instance. The default value is "amqp://guest:guest@localhost:5672".
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="connectionUri" /> is not valid for AMQP connections.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connectionUri" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal RabbitMqMessageTransport(Uri connectionUri)
            : this(connectionUri, PrimitiveMessage.DefaultBodySerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessageTransport" /> class.
        /// </summary>
        /// <param name="connectionUri">
        /// The connection URI for the target RabbitMQ instance. The default value is "amqp://guest:guest@localhost:5672".
        /// </param>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="connectionUri" /> is not valid for AMQP connections.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connectionUri" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageBodySerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal RabbitMqMessageTransport(Uri connectionUri, SerializationFormat messageBodySerializationFormat)
            : this(CreateConnectionFactory(connectionUri), messageBodySerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessageTransport" /> class.
        /// </summary>
        /// <param name="hostName">
        /// The name of the host to connect to. The default value is "localhost".
        /// </param>
        /// <param name="portNumber">
        /// The port number to connect to, or <see langword="null" /> to use the default port number (5672).
        /// </param>
        /// <param name="virtualHost">
        /// The name of the virtual host to connect to, or <see langword="null" /> to omit a virtual host.
        /// </param>
        /// <param name="userName">
        /// The user name for the connection, or <see langword="null" /> to use the default user name ("guest").
        /// </param>
        /// <param name="password">
        /// The password for the connection, or <see langword="null" /> to use the default password ("guest").
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="hostName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hostName" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal RabbitMqMessageTransport(String hostName, Int32? portNumber, String virtualHost, String userName, String password)
            : this(hostName, portNumber, virtualHost, userName, password, PrimitiveMessage.DefaultBodySerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessageTransport" /> class.
        /// </summary>
        /// <param name="hostName">
        /// The name of the host to connect to. The default value is "localhost".
        /// </param>
        /// <param name="portNumber">
        /// The port number to connect to, or <see langword="null" /> to use the default port number (5672).
        /// </param>
        /// <param name="virtualHost">
        /// The name of the virtual host to connect to, or <see langword="null" /> to omit a virtual host.
        /// </param>
        /// <param name="userName">
        /// The user name for the connection, or <see langword="null" /> to use the default user name ("guest").
        /// </param>
        /// <param name="password">
        /// The password for the connection, or <see langword="null" /> to use the default password ("guest").
        /// </param>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="hostName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hostName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageBodySerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal RabbitMqMessageTransport(String hostName, Int32? portNumber, String virtualHost, String userName, String password, SerializationFormat messageBodySerializationFormat)
            : this(CreateConnectionFactory(hostName, portNumber, virtualHost, userName, password), messageBodySerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessageTransport" /> class.
        /// </summary>
        /// <param name="connectionFactory">
        /// A factory that contains connection information for the target RabbitMQ instance.
        /// </param>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connectionFactory" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageBodySerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private RabbitMqMessageTransport(IRabbitMqConnectionFactory connectionFactory, SerializationFormat messageBodySerializationFormat)
            : base(Core.Concurrency.ConcurrencyControlMode.ProcessorCountSemaphore)
        {
            connectionFactory.RequestedHeartbeat = HeartbeatFrequency;
            connectionFactory.UseBackgroundThreadsForIO = true;
            SharedConnection = connectionFactory.RejectIf().IsNull(nameof(connectionFactory)).TargetArgument.CreateConnection();
            MessageBodySerializationFormat = messageBodySerializationFormat.RejectIf().IsEqualToValue(SerializationFormat.Unspecified, nameof(messageBodySerializationFormat));
        }

        /// <summary>
        /// Closes the specified connection as an idempotent operation.
        /// </summary>
        /// <param name="connection">
        /// The connection to close.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        public void CloseConnection(IMessageTransportConnection connection)
        {
            if (ConnectionDictionary.ContainsKey(connection.RejectIf().IsNull(nameof(connection)).TargetArgument.Identifier))
            {
                if (ConnectionDictionary.TryRemove(connection.Identifier, out _))
                {
                    connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Asynchronously notifies the specified queue that a locked message was not processed and can be made available for
        /// processing by other consumers.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was not processed.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message -or- <paramref name="path" /> does not
        /// reference an existing queue.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task ConveyFailureToQueueAsync(MessageLockToken lockToken, IMessagingEntityPath path)
        {
            RejectIfDisposed();

            return AutoAcknowledgeIsEnabled ? Task.CompletedTask : Task.Factory.StartNew(() =>
            {
                try
                {
                    WithStateControl(() =>
                    {
                        using (var channel = SharedConnection.CreateModel())
                        {
                            channel.ConfirmSelect();
                            channel.BasicReject(lockToken.DeliveryTag, true);
                            channel.WaitForConfirmsOrDie();
                        }
                    });
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException($"Failed to convey failure. The specified queue, \"{path}\", does not exist or the RabbitMQ connection is unavailable.", exception);
                }
            });
        }

        /// <summary>
        /// Asynchronously notifies the specified subscription that a locked message was not processed and can be made available for
        /// processing by other consumers.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was not processed.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message -or- <paramref name="path" /> does not
        /// reference an existing topic.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task ConveyFailureToSubscriptionAsync(MessageLockToken lockToken, IMessagingEntityPath path)
        {
            RejectIfDisposed();

            return AutoAcknowledgeIsEnabled ? Task.CompletedTask : Task.Factory.StartNew(() =>
            {
                try
                {
                    WithStateControl(() =>
                    {
                        using (var channel = SharedConnection.CreateModel())
                        {
                            channel.ConfirmSelect();
                            channel.BasicReject(lockToken.DeliveryTag, true);
                            channel.WaitForConfirmsOrDie();
                        }
                    });
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException($"Failed to convey failure. The specified topic, \"{path}\", does not exist or the RabbitMQ connection is unavailable.", exception);
                }
            });
        }

        /// <summary>
        /// Asynchronously notifies the specified queue that a locked message was processed successfully and can be destroyed
        /// permanently.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was processed successfully.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message -or- <paramref name="path" /> does not
        /// reference an existing queue.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task ConveySuccessToQueueAsync(MessageLockToken lockToken, IMessagingEntityPath path)
        {
            RejectIfDisposed();

            return AutoAcknowledgeIsEnabled ? Task.CompletedTask : Task.Factory.StartNew(() =>
            {
                try
                {
                    WithStateControl(() =>
                    {
                        using (var channel = SharedConnection.CreateModel())
                        {
                            channel.ConfirmSelect();
                            channel.BasicAck(lockToken.DeliveryTag, false);
                            channel.WaitForConfirmsOrDie();
                        }
                    });
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException($"Failed to convey success. The specified queue, \"{path}\", does not exist or the RabbitMQ connection is unavailable.", exception);
                }
            });
        }

        /// <summary>
        /// Asynchronously notifies the specified subscription that a locked message was processed successfully and can be destroyed
        /// permanently.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was processed successfully.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message -or- <paramref name="path" /> does not
        /// reference an existing topic.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task ConveySuccessToSubscriptionAsync(MessageLockToken lockToken, IMessagingEntityPath path)
        {
            RejectIfDisposed();

            return AutoAcknowledgeIsEnabled ? Task.CompletedTask : Task.Factory.StartNew(() =>
            {
                try
                {
                    WithStateControl(() =>
                    {
                        using (var channel = SharedConnection.CreateModel())
                        {
                            channel.ConfirmSelect();
                            channel.BasicAck(lockToken.DeliveryTag, false);
                            channel.WaitForConfirmsOrDie();
                        }
                    });
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException($"Failed to convey success. The specified topic, \"{path}\", does not exist or the RabbitMQ connection is unavailable.", exception);
                }
            });
        }

        /// <summary>
        /// Opens and returns a new <see cref="IMessageTransportConnection" /> to the current
        /// <see cref="RabbitMqMessageTransport" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="IMessageTransportConnection" /> to the current <see cref="RabbitMqMessageTransport" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IMessageTransportConnection CreateConnection()
        {
            RejectIfDisposed();
            var connection = new RabbitMqMessageTransportConnection(this);

            if (ConnectionDictionary.TryAdd(connection.Identifier, connection))
            {
                return connection;
            }

            return connection;
        }

        /// <summary>
        /// Asynchronously creates a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateQueueAsync(IMessagingEntityPath path) => CreateQueueAsync(path, MessagingEntity.DefaultMessageLockExpirationThreshold);

        /// <summary>
        /// Asynchronously creates a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageLockExpirationThreshold" /> is less than eight seconds.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateQueueAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold) => CreateQueueAsync(path, messageLockExpirationThreshold, MessagingEntity.DefaultEnqueueTimeoutThreshold);

        /// <summary>
        /// Asynchronously creates a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <param name="enqueueTimeoutThreshold">
        /// The maximum length of time to wait for a message to be enqueued before raising an exception. The default value is eight
        /// seconds.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageLockExpirationThreshold" /> is less than eight seconds -or-
        /// <paramref name="enqueueTimeoutThreshold" /> is less than two seconds.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateQueueAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryCreateQueue(path.RejectIf().IsNull(nameof(path)).TargetArgument, messageLockExpirationThreshold.RejectIf().IsLessThan(MessagingEntity.MessageLockExpirationThresholdFloor, nameof(messageLockExpirationThreshold)), enqueueTimeoutThreshold.RejectIf().IsLessThan(MessagingEntity.EnqueueTimeoutThresholdFloor, nameof(enqueueTimeoutThreshold))))
            {
                return;
            }

            throw new InvalidOperationException($"The specified queue, \"{path}\", already exists");
        });

        /// <summary>
        /// Asynchronously creates a new subscription.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified subscription already exists.
        /// </exception>
        public Task CreateSubscriptionAsync(IMessagingEntityPath path, String subscriptionName) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryCreateSubscription(path.RejectIf().IsNull(nameof(path)).TargetArgument, subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName)).TargetArgument))
            {
                return;
            }

            throw new InvalidOperationException($"The specified subscription, \"{subscriptionName}\", already exists");
        });

        /// <summary>
        /// Asynchronously creates a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateTopicAsync(IMessagingEntityPath path) => CreateTopicAsync(path, MessagingEntity.DefaultMessageLockExpirationThreshold);

        /// <summary>
        /// Asynchronously creates a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageLockExpirationThreshold" /> is less than eight seconds.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateTopicAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold) => CreateTopicAsync(path, messageLockExpirationThreshold, MessagingEntity.DefaultEnqueueTimeoutThreshold);

        /// <summary>
        /// Asynchronously creates a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <param name="enqueueTimeoutThreshold">
        /// The maximum length of time to wait for a message to be enqueued before raising an exception. The default value is eight
        /// seconds.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageLockExpirationThreshold" /> is less than eight seconds -or-
        /// <paramref name="enqueueTimeoutThreshold" /> is less than two seconds.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateTopicAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryCreateTopic(path.RejectIf().IsNull(nameof(path)).TargetArgument, messageLockExpirationThreshold.RejectIf().IsLessThan(MessagingEntity.MessageLockExpirationThresholdFloor, nameof(messageLockExpirationThreshold)), enqueueTimeoutThreshold.RejectIf().IsLessThan(MessagingEntity.EnqueueTimeoutThresholdFloor, nameof(enqueueTimeoutThreshold))))
            {
                return;
            }

            throw new InvalidOperationException($"Failed to create topic. The specified topic, \"{path}\", already exists");
        });

        /// <summary>
        /// Asynchronously destroys the specified queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified queue does not exist.
        /// </exception>
        public Task DestroyQueueAsync(IMessagingEntityPath path) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryDestroyQueue(path.RejectIf().IsNull(nameof(path)).TargetArgument))
            {
                return;
            }

            throw new InvalidOperationException($"Failed to destroy queue. The specified queue, \"{path}\", does not exist or the RabbitMQ connection is unavailable.");
        });

        /// <summary>
        /// Asynchronously destroys the specified subscription.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified subscription does not exist.
        /// </exception>
        public Task DestroySubscriptionAsync(IMessagingEntityPath path, String subscriptionName) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryDestroySubscription(path.RejectIf().IsNull(nameof(path)).TargetArgument, subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName)).TargetArgument))
            {
                return;
            }

            throw new InvalidOperationException($"Failed to destroy subscription. The specified subscription, \"{subscriptionName}\", does not exist or the RabbitMQ connection is unavailable.");
        });

        /// <summary>
        /// Asynchronously destroys the specified topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified topic does not exist.
        /// </exception>
        public Task DestroyTopicAsync(IMessagingEntityPath path) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryDestroyTopic(path.RejectIf().IsNull(nameof(path)).TargetArgument))
            {
                return;
            }

            throw new InvalidOperationException($"Failed to destroy topic. The specified topic, \"{path}\", does not exist.");
        });

        /// <summary>
        /// Returns a value indicating whether or not the specified queue exists.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue exists, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean QueueExists(IMessagingEntityPath path)
        {
            RejectIfDisposed();
            DeclareAndBindQueue(path);
            return true;
        }

        /// <summary>
        /// Asynchronously requests the specified number of messages from the specified queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="count">
        /// The maximum number of messages to read from the queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the dequeued messages, if any.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified queue does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task<IEnumerable<PrimitiveMessage>> ReceiveFromQueueAsync(IMessagingEntityPath path, Int32 count) => Task.FromException<IEnumerable<PrimitiveMessage>>(new NotImplementedException($"The RabbitMQ implementation does not use this abstraction."));

        /// <summary>
        /// Asynchronously requests the specified number of messages from the specified topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <param name="count">
        /// The maximum number of messages to read from the topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the dequeued messages, if any.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified topic does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task<IEnumerable<PrimitiveMessage>> ReceiveFromTopicAsync(IMessagingEntityPath path, String subscriptionName, Int32 count) => Task.FromException<IEnumerable<PrimitiveMessage>>(new NotImplementedException($"The RabbitMQ implementation does not use this abstraction."));

        /// <summary>
        /// Asynchronously sends the specified message to the specified queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="message">
        /// The message to send.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified queue does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task SendToQueueAsync(IMessagingEntityPath path, PrimitiveMessage message)
        {
            var queueName = path.RejectIf().IsNull(nameof(path)).TargetArgument.ToString();
            var exchangeName = $"{EntityPathExchangePrefix}{DelimitingCharacterForExchangePrefix}{queueName}";
            _ = message.RejectIf().IsNull(nameof(message));

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var serializer = new DynamicSerializer<PrimitiveMessage>(MessageBodySerializationFormat);
                    var body = new ReadOnlyMemory<Byte>(serializer.Serialize(message));
                    WithStateControl(() =>
                    {
                        using (var channel = SharedConnection.CreateModel())
                        {
                            var basicProperties = channel.CreateBasicProperties();
                            basicProperties.Persistent = true;
                            channel.ConfirmSelect();
                            channel.BasicPublish(exchangeName, queueName, true, basicProperties, body);
                            channel.WaitForConfirmsOrDie();
                        }
                    });
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException($"Failed to send message. The specified queue, \"{path}\", does not exist or the RabbitMQ connection is unavailable.", exception);
                }
            });
        }

        /// <summary>
        /// Asynchronously sends the specified message to the specified topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="message">
        /// The message to send.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified topic does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task SendToTopicAsync(IMessagingEntityPath path, PrimitiveMessage message)
        {
            var topicName = path.RejectIf().IsNull(nameof(path)).TargetArgument.ToString();
            var exchangeName = $"{EntityPathExchangePrefix}{DelimitingCharacterForExchangePrefix}{topicName}";
            _ = message.RejectIf().IsNull(nameof(message));

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var serializer = new DynamicSerializer<PrimitiveMessage>(MessageBodySerializationFormat);
                    var body = new ReadOnlyMemory<Byte>(serializer.Serialize(message));
                    WithStateControl(() =>
                    {
                        using (var channel = SharedConnection.CreateModel())
                        {
                            var basicProperties = channel.CreateBasicProperties();
                            basicProperties.Persistent = true;
                            channel.ConfirmSelect();
                            channel.BasicPublish(exchangeName, topicName, true, basicProperties, body);
                            channel.WaitForConfirmsOrDie();
                        }
                    });
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException($"Failed to send message. The specified topic, \"{path}\", does not exist or the RabbitMQ connection is unavailable.", exception);
                }
            });
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified subscription exists.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the subscription exists, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean SubscriptionExists(IMessagingEntityPath path, String subscriptionName)
        {
            RejectIfDisposed();

            if (TopicExists(path))
            {
                DeclareAndBindSubscription(path, subscriptionName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified topic exists.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic exists, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean TopicExists(IMessagingEntityPath path)
        {
            RejectIfDisposed();
            DeclareTopic(path);
            return true;
        }

        /// <summary>
        /// Attempts to create a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateQueue(IMessagingEntityPath path) => TryCreateQueue(path, MessagingEntity.DefaultMessageLockExpirationThreshold);

        /// <summary>
        /// Attempts to create a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateQueue(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold) => TryCreateQueue(path, messageLockExpirationThreshold, MessagingEntity.DefaultEnqueueTimeoutThreshold);

        /// <summary>
        /// Attempts to create a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <param name="enqueueTimeoutThreshold">
        /// The maximum length of time to wait for a message to be enqueued before raising an exception. The default value is eight
        /// seconds.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateQueue(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold)
        {
            if (IsDisposedOrDisposing || path is null || messageLockExpirationThreshold < MessagingEntity.MessageLockExpirationThresholdFloor || enqueueTimeoutThreshold < MessagingEntity.EnqueueTimeoutThresholdFloor)
            {
                return false;
            }

            return QueueExists(path);
        }

        /// <summary>
        /// Attempts to create a new subscription.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the subscription was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateSubscription(IMessagingEntityPath path, String subscriptionName)
        {
            if (IsDisposedOrDisposing || path is null || subscriptionName is null)
            {
                return false;
            }

            return SubscriptionExists(path, subscriptionName);
        }

        /// <summary>
        /// Attempts to create a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateTopic(IMessagingEntityPath path) => TryCreateTopic(path, MessagingEntity.DefaultMessageLockExpirationThreshold);

        /// <summary>
        /// Attempts to create a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateTopic(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold) => TryCreateTopic(path, messageLockExpirationThreshold, MessagingEntity.DefaultEnqueueTimeoutThreshold);

        /// <summary>
        /// Attempts to create a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <param name="enqueueTimeoutThreshold">
        /// The maximum length of time to wait for a message to be enqueued before raising an exception. The default value is eight
        /// seconds.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateTopic(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold)
        {
            if (IsDisposedOrDisposing || path is null || messageLockExpirationThreshold < MessagingEntity.MessageLockExpirationThresholdFloor || enqueueTimeoutThreshold < MessagingEntity.EnqueueTimeoutThresholdFloor)
            {
                return false;
            }

            return TopicExists(path);
        }

        /// <summary>
        /// Attempts to destroy the specified queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue was successfully destroyed, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryDestroyQueue(IMessagingEntityPath path)
        {
            if (IsDisposedOrDisposing || path is null)
            {
                return false;
            }

            var queueName = path.ToString();
            var exchangeName = $"{EntityPathExchangePrefix}{DelimitingCharacterForExchangePrefix}{queueName}";
            WithStateControl(() =>
            {
                using (var channel = SharedConnection.CreateModel())
                {
                    channel.ConfirmSelect();
                    channel.QueueUnbind(queueName, exchangeName, queueName);
                    channel.QueueDelete(queueName);
                    channel.ExchangeDelete(exchangeName);
                    channel.WaitForConfirmsOrDie();
                }
            });

            return true;
        }

        /// <summary>
        /// Attempts to destroy the specified subscription.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// **
        /// <returns>
        /// <see langword="true" /> if the subscription was successfully destroyed, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryDestroySubscription(IMessagingEntityPath path, String subscriptionName)
        {
            if (IsDisposedOrDisposing || path is null || subscriptionName.IsNullOrEmpty())
            {
                return false;
            }

            var topicName = path.ToString();
            var queueName = subscriptionName;
            var exchangeName = $"{EntityPathExchangePrefix}{DelimitingCharacterForExchangePrefix}{topicName}";
            WithStateControl(() =>
            {
                using (var channel = SharedConnection.CreateModel())
                {
                    channel.ConfirmSelect();
                    channel.QueueUnbind(queueName, exchangeName, queueName);
                    channel.QueueDelete(queueName);
                    channel.WaitForConfirmsOrDie();
                }
            });

            return true;
        }

        /// <summary>
        /// Attempts to destroy the specified topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic was successfully destroyed, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryDestroyTopic(IMessagingEntityPath path)
        {
            if (IsDisposedOrDisposing || path is null)
            {
                return false;
            }

            var topicName = path.ToString();
            var exchangeName = $"{EntityPathExchangePrefix}{DelimitingCharacterForExchangePrefix}{topicName}";
            WithStateControl(() =>
            {
                using (var channel = SharedConnection.CreateModel())
                {
                    channel.ConfirmSelect();
                    channel.ExchangeDelete(exchangeName);
                    channel.WaitForConfirmsOrDie();
                }
            });

            return true;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="RabbitMqMessageTransport" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                while (ConnectionCount > 0)
                {
                    DestroyAllConnections();
                }

                SharedConnection?.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Creates and hydrates a connection factory using the specified connection information.
        /// </summary>
        /// <param name="connectionUri">
        /// The connection URI for the target RabbitMQ instance.
        /// </param>
        /// <returns>
        /// The resulting connection factory.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="connectionUri" /> is not valid for AMQP connections.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connectionUri" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static IRabbitMqConnectionFactory CreateConnectionFactory(Uri connectionUri) => new RabbitMqConnectionFactory()
        {
            Uri = connectionUri.RejectIf().IsNull(nameof(connectionUri)).OrIf(argument => argument.Scheme != AmqpUriScheme, nameof(connectionUri), "The specified connection URI is not valid for AMQP connections.")
        };

        /// <summary>
        /// Creates and hydrates a connection factory using the specified connection information.
        /// </summary>
        /// <param name="hostName">
        /// The name of the host to connect to.
        /// </param>
        /// <param name="portNumber">
        /// The port number to connect to, or <see langword="null" /> to use the default port number (5672).
        /// </param>
        /// <param name="virtualHost">
        /// The name of the virtual host to connect to, or <see langword="null" /> to omit a virtual host.
        /// </param>
        /// <param name="userName">
        /// The user name for the connection, or <see langword="null" /> to use the default user name ("guest").
        /// </param>
        /// <param name="password">
        /// The password for the connection, or <see langword="null" /> to use the default password ("guest").
        /// </param>
        /// <returns>
        /// The resulting connection factory.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="hostName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hostName" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static IRabbitMqConnectionFactory CreateConnectionFactory(String hostName, Int32? portNumber, String virtualHost, String userName, String password) => new RabbitMqConnectionFactory()
        {
            HostName = hostName.RejectIf().IsNullOrEmpty(nameof(hostName)),
            Password = password.IsNullOrEmpty() ? DefaultConnectionPassword : password,
            Port = portNumber ?? DefaultConnectionPortNumber,
            UserName = userName.IsNullOrEmpty() ? DefaultConnectionUserName : userName,
            VirtualHost = virtualHost.IsNullOrEmpty() ? null : virtualHost
        };

        /// <summary>
        /// Declares a direct exchange-queue pair and binds them using specified queue path as an idempotent operation.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessagingException">
        /// An exception was raised while attempting to declare or bind the queue.
        /// </exception>
        [DebuggerHidden]
        private void DeclareAndBindQueue(IMessagingEntityPath path)
        {
            var queueName = path.RejectIf().IsNull(nameof(path)).TargetArgument.ToString();
            var exchangeName = $"{EntityPathExchangePrefix}{DelimitingCharacterForExchangePrefix}{queueName}";

            try
            {
                WithStateControl(() =>
                {
                    var channel = SharedConnection.CreateModel();

                    try
                    {
                        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false);
                        channel.QueueDeclare(queueName, true, false, false);
                        channel.QueueBind(queueName, exchangeName, queueName);
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(ChannelKeepAliveDuration);
                            channel.Dispose();
                        });
                    }
                    catch
                    {
                        channel.Dispose();
                        throw;
                    }
                });
            }
            catch (Exception exception)
            {
                throw new MessagingException("Failed to declare and bind the specified queue. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Declares a subscription queue and binds it to the appropriate exchange using specified path and subscription name as an
        /// idempotent operation.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="MessagingException">
        /// An exception was raised while attempting to declare or bind the subscription.
        /// </exception>
        [DebuggerHidden]
        private void DeclareAndBindSubscription(IMessagingEntityPath path, String subscriptionName)
        {
            var topicName = path.RejectIf().IsNull(nameof(path)).TargetArgument.ToString();
            var queueName = subscriptionName.RejectIf().IsNullOrEmpty(nameof(path));
            var exchangeName = $"{EntityPathExchangePrefix}{DelimitingCharacterForExchangePrefix}{topicName}";

            try
            {
                WithStateControl(() =>
                {
                    var channel = SharedConnection.CreateModel();

                    try
                    {
                        channel.QueueDeclare(queueName, true, true, true);
                        channel.QueueBind(queueName, exchangeName, topicName);
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(ChannelKeepAliveDuration);
                            channel.Dispose();
                        });
                    }
                    catch
                    {
                        channel.Dispose();
                        throw;
                    }
                });
            }
            catch (Exception exception)
            {
                throw new MessagingException("Failed to declare and bind the specified subscription. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Declares a fanout exchange for the specified topic path as an idempotent operation.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessagingException">
        /// An exception was raised while attempting to declare the exchange.
        /// </exception>
        [DebuggerHidden]
        private void DeclareTopic(IMessagingEntityPath path)
        {
            var topicName = path.RejectIf().IsNull(nameof(path)).TargetArgument.ToString();
            var exchangeName = $"{EntityPathExchangePrefix}{DelimitingCharacterForExchangePrefix}{topicName}";

            try
            {
                WithStateControl(() =>
                {
                    var channel = SharedConnection.CreateModel();

                    try
                    {
                        channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true, false);
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(ChannelKeepAliveDuration);
                            channel.Dispose();
                        });
                    }
                    catch
                    {
                        channel.Dispose();
                        throw;
                    }
                });
            }
            catch (Exception exception)
            {
                throw new MessagingException("Failed to declare the specified topic. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Closes and disposes of all connections to the current <see cref="RabbitMqMessageTransport" />.
        /// </summary>
        [DebuggerHidden]
        private void DestroyAllConnections()
        {
            var destroyConnectionTasks = new List<Task>();

            while (ConnectionDictionary?.Any() ?? false)
            {
                var connectionIdentifier = ConnectionDictionary?.Keys.FirstOrDefault() ?? default;

                if (connectionIdentifier == default)
                {
                    continue;
                }
                else if (ConnectionDictionary.TryRemove(connectionIdentifier, out var connection))
                {
                    destroyConnectionTasks.Add(Task.Factory.StartNew(() =>
                    {
                        connection.Dispose();
                    }));
                }
            }

            Task.WaitAll(destroyConnectionTasks.ToArray());
        }

        /// <summary>
        /// Gets the number of active connections to the current <see cref="RabbitMqMessageTransport" />.
        /// </summary>
        public Int32 ConnectionCount => Connections.Count();

        /// <summary>
        /// Gets a collection of active connections to the current <see cref="RabbitMqMessageTransport" />.
        /// </summary>
        public IEnumerable<IMessageTransportConnection> Connections => ConnectionDictionary.Values;

        /// <summary>
        /// Gets the format that is used to serialize enqueued message bodies.
        /// </summary>
        public SerializationFormat MessageBodySerializationFormat
        {
            get;
        }

        /// <summary>
        /// Represents a value indicating whether or not automatic acknowledgment is enabled.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Boolean AutoAcknowledgeIsEnabled = true;

        /// <summary>
        /// Represents the delimiting character that follows the exchange prefix token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Char DelimitingCharacterForExchangePrefix = '-';

        /// <summary>
        /// Represents the entity path prefix for exchanges.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String EntityPathExchangePrefix = "Exc";

        /// <summary>
        /// Represents the entity path prefix for queues.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String EntityPathQueuePrefix = "Que";

        /// <summary>
        /// Represents the entity path prefix for topics.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String EntityPathTopicPrefix = "Top";

        /// <summary>
        /// Represents the name prefix for subscriptions.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String SubscriptionNamePrefix = "Sub";

        /// <summary>
        /// Represents the shared connection to the target RabbitMQ instance through which all other connections communicate.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly IRabbitMqConnection SharedConnection;

        /// <summary>
        /// Represents the valid URI scheme for AMQP connections.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String AmqpUriScheme = "amqp";

        /// <summary>
        /// Represents the default host name for new connections.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultConnectionHostName = "localhost";

        /// <summary>
        /// Represents the default password for new connections.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultConnectionPassword = "guest";

        /// <summary>
        /// Represents the default port number for new connections.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DefaultConnectionPortNumber = 5672;

        /// <summary>
        /// Represents the default user name for new connections.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultConnectionUserName = "guest";

        /// <summary>
        /// Represents the length of time to keep channels alive after issuing non-publish calls.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan ChannelKeepAliveDuration = TimeSpan.FromMilliseconds(21);

        /// <summary>
        /// Represents the polling frequency used by RabbitMQ connections.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan HeartbeatFrequency = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Represents a collection of active connections to the current <see cref="RabbitMqMessageTransport" />, which are keyed by
        /// identifier.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<Guid, IMessageTransportConnection> ConnectionDictionary = new();
    }
}