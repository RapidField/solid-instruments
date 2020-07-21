// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client connection to an <see cref="IMessageTransport" />.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageTransportConnection" /> is the default implementation of <see cref="IMessageTransportConnection" />.
    /// </remarks>
    internal sealed class MessageTransportConnection : Instrument, IMessageTransportConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportConnection" /> class.
        /// </summary>
        /// <param name="transport">
        /// The associated transport.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="transport" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageTransportConnection(IMessageTransport transport)
            : base()
        {
            Handlers = new List<Handler>();
            Identifier = Guid.NewGuid();
            LazyPollTimer = new Lazy<Timer>(InitializePollTimer, LazyThreadSafetyMode.ExecutionAndPublication);
            State = MessageTransportConnectionState.Open;
            TransportReference = transport.RejectIf().IsNull(nameof(transport)).TargetArgument;
        }

        /// <summary>
        /// Closes the current <see cref="MessageTransportConnection" /> as an idempotent operation.
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
                    if (TransportReference.Connections.Any(connection => connection.Identifier == Identifier))
                    {
                        TransportReference.CloseConnection(this);
                    }

                    LazyPollTimer.Dispose();
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
            RejectIfDisposed();

            if (Transport.QueueExists(queuePath))
            {
                Handlers.Add(new Handler(queuePath, MessagingEntityType.Queue, null, handleMessageAction));
                BeginPolling();
                return;
            }

            throw new InvalidOperationException($"Failed to register queue handler. The specified queue, \"{queuePath}\", does not exist.");
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
            RejectIfDisposed();

            if (Transport.SubscriptionExists(topicPath, subscriptionName))
            {
                Handlers.Add(new Handler(topicPath, MessagingEntityType.Topic, subscriptionName, handleMessageAction));
                BeginPolling();
                return;
            }

            throw new InvalidOperationException($"Failed to register subscription handler. The specified subscription, \"{subscriptionName}\", does not exist.");
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageTransportConnection" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        /// <exception cref="MessagingException">
        /// An exception was raised while closing the transport connection.
        /// </exception>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    Close();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Ensures (as an idempotent operation) that the current <see cref="MessageTransportConnection" /> is actively polling the
        /// transport.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The transport connection is in an invalid state.
        /// </exception>
        [DebuggerHidden]
        private void BeginPolling()
        {
            if (PollTimer is null)
            {
                throw new InvalidOperationException("The transport connection is in an invalid state.");
            }
        }

        /// <summary>
        /// Initializes a timer that is used to poll <see cref="Transport" /> for receive operations.
        /// </summary>
        /// <returns>
        /// A timer that is used to poll <see cref="Transport" /> for receive operations.
        /// </returns>
        [DebuggerHidden]
        private Timer InitializePollTimer()
        {
            var timerCallback = new TimerCallback((state) => Poll(state as ICollection<Handler>));
            return new Timer(timerCallback, Handlers, TimeSpan.FromMilliseconds(PollingIntervalInMilliseconds), TimeSpan.FromMilliseconds(PollingIntervalInMilliseconds));
        }

        /// <summary>
        /// Polls <see cref="Transport" /> and performs message handling operations against received messages, if any.
        /// </summary>
        /// <param name="handlers">
        /// A collection of actions that is performed upon message receipt from specific entities.
        /// </param>
        /// <exception cref="MessagingException">
        /// One or more message handling operations failed.
        /// </exception>
        [DebuggerHidden]
        private void Poll(IEnumerable<Handler> handlers)
        {
            if (handlers.Any())
            {
                try
                {
                    using (var pollQueuesTask = PollQueuesAsync(handlers.Where(handler => handler.EntityType == MessagingEntityType.Queue).ToArray()))
                    {
                        using (var pollTopicsTask = PollTopicsAsync(handlers.Where(handler => handler.EntityType == MessagingEntityType.Topic).ToArray()))
                        {
                            Task.WaitAll(pollQueuesTask, pollTopicsTask);
                        }
                    }
                }
                catch (AggregateException exception)
                {
                    throw new MessagingException("One or more exceptions were raised while performing message handling operations. See inner exception(s).", exception);
                }
            }
        }

        /// <summary>
        /// Asynchronously polls the specified queue and performs messaging handling operations against received messages, if any.
        /// </summary>
        /// <param name="queuePath">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="handleMessageActions">
        /// A collection of actions that are performed upon message receipt from the specified queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while performing messaging handling operations.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified queue does not exist.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The transport connection is closed.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        [DebuggerHidden]
        private Task PollQueueAsync(IMessagingEntityPath queuePath, IEnumerable<Action<PrimitiveMessage>> handleMessageActions) => Transport.ReceiveFromQueueAsync(queuePath, MessageReceiptBatchSize).ContinueWith((receiveFromQueueTask) =>
        {
            var messageBatch = receiveFromQueueTask.Result;

            if (messageBatch.Any())
            {
                var handleMessageTasks = new List<Task>();

                foreach (var message in messageBatch)
                {
                    foreach (var handleMessageAction in handleMessageActions)
                    {
                        handleMessageTasks.Add(Task.Factory.StartNew(() =>
                        {
                            handleMessageAction(message);
                        }));
                    }
                }

                Task.WaitAll(handleMessageTasks.ToArray());
            }
        });

        /// <summary>
        /// Asynchronously polls available queues and performs message handling operations against received messages, if any.
        /// </summary>
        /// <param name="queueHandlers">
        /// A collection of actions that are performed upon message receipt from specific queues.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while performing messaging handling operations.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// One or more of the specified queues does not exist.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The transport connection is closed.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        [DebuggerHidden]
        private Task PollQueuesAsync(IEnumerable<Handler> queueHandlers)
        {
            if (queueHandlers.Any())
            {
                var pollTasks = new List<Task>();

                foreach (var handlerGroup in queueHandlers.GroupBy(handler => handler.Path))
                {
                    pollTasks.Add(PollQueueAsync(handlerGroup.Key, handlerGroup.Select(handler => handler.HandleMessageAction)));
                }

                return Task.WhenAll(pollTasks.ToArray());
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Asynchronously polls the specified subscription and performs messaging handling operations against received messages, if
        /// any.
        /// </summary>
        /// <param name="topicPath">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <param name="handleMessageActions">
        /// A collection of actions that are performed upon message receipt from the specified subscription.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while performing messaging handling operations.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified subscription does not exist.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The transport connection is closed.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        [DebuggerHidden]
        private Task PollSubscriptionAsync(IMessagingEntityPath topicPath, String subscriptionName, IEnumerable<Action<PrimitiveMessage>> handleMessageActions) => Transport.ReceiveFromTopicAsync(topicPath, subscriptionName, MessageReceiptBatchSize).ContinueWith((receiveFromTopicTask) =>
        {
            var messageBatch = receiveFromTopicTask.Result;

            if (messageBatch.Any())
            {
                var handleMessageTasks = new List<Task>();

                foreach (var message in messageBatch)
                {
                    foreach (var handleMessageAction in handleMessageActions)
                    {
                        handleMessageTasks.Add(Task.Factory.StartNew(() =>
                        {
                            handleMessageAction(message);
                        }));
                    }
                }

                Task.WaitAll(handleMessageTasks.ToArray());
            }
        });

        /// <summary>
        /// Asynchronously polls the specified topic and performs messaging handling operations against received messages, if any.
        /// </summary>
        /// <param name="topicPath">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="subscriptionHandlers">
        /// A collection of actions that are performed upon message receipt from specific subscriptions.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while performing messaging handling operations.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified topic does not exist.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The transport connection is closed.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        [DebuggerHidden]
        private Task PollTopicAsync(IMessagingEntityPath topicPath, IEnumerable<Handler> subscriptionHandlers)
        {
            var pollTasks = new List<Task>();

            foreach (var handlerGroup in subscriptionHandlers.GroupBy(handler => handler.SubscriptionName))
            {
                pollTasks.Add(PollSubscriptionAsync(topicPath, handlerGroup.Key, handlerGroup.Select(handler => handler.HandleMessageAction)));
            }

            return Task.WhenAll(pollTasks.ToArray());
        }

        /// <summary>
        /// Asynchronously polls available topics and performs message handling operations against received messages, if any.
        /// </summary>
        /// <param name="subscriptionHandlers">
        /// A collection of actions that are performed upon message receipt from specific topics.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while performing messaging handling operations.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// One or more of the specified topics does not exist.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The transport connection is closed.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        [DebuggerHidden]
        private Task PollTopicsAsync(IEnumerable<Handler> subscriptionHandlers)
        {
            if (subscriptionHandlers.Any())
            {
                var pollTasks = new List<Task>();

                foreach (var handlerGroup in subscriptionHandlers.GroupBy(handler => handler.Path))
                {
                    pollTasks.Add(PollTopicAsync(handlerGroup.Key, handlerGroup));
                }

                return Task.WhenAll(pollTasks.ToArray());
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets a value that uniquely identifies the current <see cref="MessageTransportConnection" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the state of the current <see cref="MessageTransportConnection" />.
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
        /// Gets a timer that is used to poll <see cref="Transport" /> for receive operations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Timer PollTimer => LazyPollTimer.Value;

        /// <summary>
        /// Represents the maximum number of messages to dequeue from each entity during a single polling permutation.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MessageReceiptBatchSize = 34;

        /// <summary>
        /// Represents the interval, in milliseconds, at which <see cref="Transport" /> is polled for receive operations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PollingIntervalInMilliseconds = 987;

        /// <summary>
        /// Represents a collection of actions that is performed upon message receipt from specific entities.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ICollection<Handler> Handlers;

        /// <summary>
        /// Represents a lazily-initialized timer that is used to poll <see cref="Transport" /> for receive operations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Timer> LazyPollTimer;

        /// <summary>
        /// Represents the associated <see cref="IMessageTransport" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IMessageTransport TransportReference;

        /// <summary>
        /// Represents an action that is performed upon message receipt from a specific entity.
        /// </summary>
        private sealed class Handler
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            /// <param name="path">
            /// A unique textual path that identifies the associated entity.
            /// </param>
            /// <param name="entityType">
            /// The entity type of the associated entity.
            /// </param>
            /// <param name="subscriptionName">
            /// The unique name of the associated subscription, or <see langword="null" /> if the entity is a queue.
            /// </param>
            /// <param name="handleMessageAction">
            /// An action to perform upon message receipt.
            /// </param>
            /// <exception cref="ArgumentEmptyException">
            /// <paramref name="subscriptionName" /> is empty and <paramref name="entityType" /> is equal to
            /// <see cref="MessagingEntityType.Topic" />.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="handleMessageAction" /> is
            /// <see langword="null" /> -or- <paramref name="subscriptionName" /> is <see langword="null" /> and
            /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Topic" />.
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
            /// </exception>
            [DebuggerHidden]
            internal Handler(IMessagingEntityPath path, MessagingEntityType entityType, String subscriptionName, Action<PrimitiveMessage> handleMessageAction)
            {
                EntityType = entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType));
                HandleMessageAction = handleMessageAction.RejectIf().IsNull(nameof(handleMessageAction));
                Path = path.RejectIf().IsNull(nameof(path)).TargetArgument;
                SubscriptionName = entityType == MessagingEntityType.Queue ? null : subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName));
            }

            /// <summary>
            /// Gets the entity type of the associated <see cref="IMessagingEntity" />.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal MessagingEntityType EntityType
            {
                get;
            }

            /// <summary>
            /// Gets an action to perform upon message receipt.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal Action<PrimitiveMessage> HandleMessageAction
            {
                get;
            }

            /// <summary>
            /// Gets the unique textual path for the messaging entity.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal IMessagingEntityPath Path
            {
                get;
            }

            /// <summary>
            /// Gets the unique name of the associated subscription, or <see langword="null" /> if the entity is a queue.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal String SubscriptionName
            {
                get;
            }
        }
    }
}