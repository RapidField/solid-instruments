// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.Mathematics.Extensions;
using RapidField.SolidInstruments.Mathematics.Sequences;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates implementation-specific subscribing operations for a message bus.
    /// </summary>
    /// <typeparam name="TSender">
    /// The type of the implementation-specific send client.
    /// </typeparam>
    /// <typeparam name="TReceiver">
    /// The type of the implementation-specific receive client.
    /// </typeparam>
    /// <typeparam name="TAdaptedMessage">
    /// The type of implementation-specific adapted messages.
    /// </typeparam>
    /// <typeparam name="TPublishingFacade">
    /// The type of the implementation-specific messaging facade that is used to publish response messages.
    /// </typeparam>
    /// <remarks>
    /// <see cref="MessageSubscribingFacade{TSender, TReceiver, TAdaptedMessage, TPublishingFacade}" /> is the default implementation
    /// of <see cref="IMessageSubscribingFacade{TSender, TReceiver, TAdaptedMessage, TPublishingFacade}" />.
    /// </remarks>
    public abstract class MessageSubscribingFacade<TSender, TReceiver, TAdaptedMessage, TPublishingFacade> : MessageSubscribingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
        where TPublishingFacade : MessagePublishingFacade<TSender, TReceiver, TAdaptedMessage>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MessageSubscribingFacade{TSender, TReceiver, TAdaptedMessage, TPublishingFacade}" /> class.
        /// </summary>
        /// <param name="publishingFacade">
        /// An implementation-specific messaging facade that is used to publish response messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="publishingFacade" /> is <see langword="null" />.
        /// </exception>
        protected MessageSubscribingFacade(TPublishingFacade publishingFacade)
            : base(publishingFacade.RejectIf().IsNull(nameof(publishingFacade)).TargetArgument.ClientFactory, publishingFacade.MessageAdapter)
        {
            PublishingFacade = publishingFacade;
        }

        /// <summary>
        /// Registers the specified request message handler with the bus.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessageHandler">
        /// A function that handles a request message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while attempting to register <paramref name="requestMessageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public sealed override void RegisterRequestMessageHandler<TRequestMessage, TResponseMessage>(Func<TRequestMessage, TResponseMessage> requestMessageHandler)
        {
            requestMessageHandler = requestMessageHandler.RejectIf().IsNull(nameof(requestMessageHandler)).TargetArgument;

            try
            {
                using (var controlToken = StateControl.Enter())
                {
                    RejectIfDisposed();

                    if (TryAddSubscribedMessageType<TRequestMessage>(controlToken) == false)
                    {
                        // Disallow registration of duplicate request handlers.
                        return;
                    }

                    var requestEntityType = Message.RequestEntityType;
                    var requestReceiveClient = default(TReceiver);

                    switch (requestEntityType)
                    {
                        case MessagingEntityType.Queue:

                            requestReceiveClient = ClientFactory.GetQueueReceiver<TRequestMessage>();
                            break;

                        case MessagingEntityType.Topic:

                            requestReceiveClient = ClientFactory.GetTopicReceiver<TRequestMessage>(Identifier);
                            break;

                        default:

                            throw new InvalidOperationException($"The specified messaging entity type, {requestEntityType}, is not supported.");
                    }

                    var messageHandler = new Action<TAdaptedMessage>((adaptedRequestMessage) =>
                    {
                        try
                        {
                            RejectIfDisposed();
                            var requestMessage = MessageAdapter.ConvertReverse<TRequestMessage>(adaptedRequestMessage);

                            if (requestMessage.ProcessingInformation is null)
                            {
                                requestMessage.ProcessingInformation = new MessageProcessingInformation(DefaultFailurePolicy);
                            }
                            else if (requestMessage.ProcessingInformation.FailurePolicy is null)
                            {
                                requestMessage.ProcessingInformation.FailurePolicy = DefaultFailurePolicy;
                            }

                            HandleMessageAsync((message) =>
                            {
                                var responseMessage = requestMessageHandler(message);
                                PublishingFacade.PublishAsync(responseMessage, null, Message.ResponseEntityType).Wait();
                            }, requestMessage).Wait();
                        }
                        catch (Exception exception)
                        {
                            throw new MessageSubscribingException(typeof(TRequestMessage), exception);
                        }
                    });

                    RegisterMessageHandler(messageHandler, requestReceiveClient, controlToken);
                }
            }
            catch (MessageSubscribingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessageSubscribingException(typeof(TRequestMessage), exception);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessageSubscribingFacade{TSender, TReceiver, TAdaptedMessage, TPublishingFacade}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Asynchronously publishes the specified <see cref="ExceptionRaisedMessage" /> instance.
        /// </summary>
        /// <param name="exceptionRaisedMessage">
        /// The message to publish.
        /// </param>
        /// <param name="messagingEntityType">
        /// The messaging type to use when publishing the message.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected sealed override Task PublishReceiverExceptionAsync(ExceptionRaisedMessage exceptionRaisedMessage, MessagingEntityType messagingEntityType) => PublishingFacade.PublishAsync(exceptionRaisedMessage, null, messagingEntityType);

        /// <summary>
        /// Represents an implementation-specific messaging facade that is used to publish response messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly TPublishingFacade PublishingFacade;
    }

    /// <summary>
    /// Facilitates implementation-specific subscribing operations for a message bus.
    /// </summary>
    /// <typeparam name="TSender">
    /// The type of the implementation-specific send client.
    /// </typeparam>
    /// <typeparam name="TReceiver">
    /// The type of the implementation-specific receive client.
    /// </typeparam>
    /// <typeparam name="TAdaptedMessage">
    /// The type of implementation-specific adapted messages.
    /// </typeparam>
    /// <remarks>
    /// <see cref="MessageSubscribingFacade{TSender, TReceiver, TAdaptedMessage}" /> is the default implementation of
    /// <see cref="IMessageSubscribingFacade{TSender, TReceiver, TAdaptedMessage}" />.
    /// </remarks>
    public abstract class MessageSubscribingFacade<TSender, TReceiver, TAdaptedMessage> : MessagingFacade<TSender, TReceiver, TAdaptedMessage>, IMessageSubscribingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscribingFacade{TSender, TReceiver, TAdaptedMessage}" /> class.
        /// </summary>
        /// <param name="clientFactory">
        /// An appliance that creates manages implementation-specific messaging clients.
        /// </param>
        /// <param name="messageAdapter">
        /// An appliance that facilitates implementation-specific message conversion.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="clientFactory" /> is <see langword="null" /> -or- <paramref name="messageAdapter" /> is
        /// <see langword="null" />.
        /// </exception>
        protected MessageSubscribingFacade(IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage> clientFactory, IMessageAdapter<TAdaptedMessage> messageAdapter)
            : base(clientFactory, messageAdapter)
        {
            return;
        }

        /// <summary>
        /// Registers the specified queue message handler with the bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while attempting to register <paramref name="messageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RegisterQueueMessageHandler<TMessage>(Action<TMessage> messageHandler)
            where TMessage : class, IMessage => RegisterQueueMessageHandler(messageHandler, null);

        /// <summary>
        /// Registers the specified queue message handler with the bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while attempting to register <paramref name="messageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RegisterQueueMessageHandler<TMessage>(Action<TMessage> messageHandler, IEnumerable<String> pathTokens)
            where TMessage : class, IMessage => RegisterMessageHandler(messageHandler, pathTokens, MessagingEntityType.Queue);

        /// <summary>
        /// Registers the specified request message handler with the bus.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessageHandler">
        /// A function that handles a request message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while attempting to register <paramref name="requestMessageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public abstract void RegisterRequestMessageHandler<TRequestMessage, TResponseMessage>(Func<TRequestMessage, TResponseMessage> requestMessageHandler)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage;

        /// <summary>
        /// Registers the specified topic message handler with the bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while attempting to register <paramref name="messageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RegisterTopicMessageHandler<TMessage>(Action<TMessage> messageHandler)
            where TMessage : class, IMessage => RegisterTopicMessageHandler(messageHandler, null);

        /// <summary>
        /// Registers the specified topic message handler with the bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while attempting to register <paramref name="messageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RegisterTopicMessageHandler<TMessage>(Action<TMessage> messageHandler, IEnumerable<String> pathTokens)
            where TMessage : class, IMessage => RegisterMessageHandler(messageHandler, pathTokens, MessagingEntityType.Topic);

        /// <summary>
        /// Asynchronously performs the specified action for the specified message and, upon failure, applies the execution policy.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="message">
        /// A message to handle.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [DebuggerHidden]
        internal Task HandleMessageAsync<TMessage>(Action<TMessage> messageHandler, TMessage message)
            where TMessage : IMessageBase
        {
            var attemptStartTimeStamp = TimeStamp.Current;
            var raisedException = (Exception)null;

            try
            {
                messageHandler(message);
                message.ProcessingInformation.AttemptResults.Add(new MessageProcessingAttemptResult(TimeStamp.Current, attemptStartTimeStamp));
                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                message.ProcessingInformation.AttemptResults.Add(new MessageProcessingAttemptResult(TimeStamp.Current, attemptStartTimeStamp, exception));
                raisedException = exception;
            }

            return ExecuteFailurePolicyAsync(messageHandler, message, raisedException);
        }

        /// <summary>
        /// Registers the specified message handler with the bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscribingException">
        /// An exception was raised while attempting to register <paramref name="messageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        internal void RegisterMessageHandler<TMessage>(Action<TMessage> messageHandler, IEnumerable<String> pathTokens, MessagingEntityType entityType)
            where TMessage : class, IMessage
        {
            messageHandler = messageHandler.RejectIf().IsNull(nameof(messageHandler)).TargetArgument;

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (TryAddSubscribedMessageType<TMessage>(controlToken) == false)
                {
                    if (ResponseMessageInterfaceType.IsAssignableFrom(typeof(TMessage)))
                    {
                        // Disallow registration of duplicate response handlers.
                        return;
                    }
                }

                var receiveClient = default(TReceiver);

                switch (entityType)
                {
                    case MessagingEntityType.Queue:

                        receiveClient = ClientFactory.GetQueueReceiver<TMessage>(pathTokens);
                        break;

                    case MessagingEntityType.Topic:

                        receiveClient = ClientFactory.GetTopicReceiver<TMessage>(Identifier, pathTokens);
                        break;

                    default:

                        throw new InvalidOperationException($"The specified messaging entity type, {entityType}, is not supported.");
                }

                var adaptedMessageHandler = new Action<TAdaptedMessage>((adaptedMessage) =>
                {
                    try
                    {
                        RejectIfDisposed();
                        var message = MessageAdapter.ConvertReverse<TMessage>(adaptedMessage);

                        if (message.ProcessingInformation is null)
                        {
                            message.ProcessingInformation = new MessageProcessingInformation(DefaultFailurePolicy);
                        }
                        else if (message.ProcessingInformation.FailurePolicy is null)
                        {
                            message.ProcessingInformation.FailurePolicy = DefaultFailurePolicy;
                        }

                        HandleMessageAsync(messageHandler, message).Wait();
                    }
                    catch (Exception exception)
                    {
                        throw new MessageSubscribingException(typeof(TMessage), exception);
                    }
                });

                try
                {
                    RegisterMessageHandler(adaptedMessageHandler, receiveClient, controlToken);
                }
                catch (MessageSubscribingException)
                {
                    throw;
                }
                catch (ObjectDisposedException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new MessageSubscribingException(typeof(TMessage), exception);
                }
            }
        }

        /// <summary>
        /// Attempts to add the specified message type to the collection of subscribed message types.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the subscribed message.
        /// </typeparam>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified message type was added; <see langword="false" /> if it was already present.
        /// </returns>
        [DebuggerHidden]
        internal Boolean TryAddSubscribedMessageType<TMessage>(ConcurrencyControlToken controlToken)
            where TMessage : IMessageBase
        {
            var messageType = typeof(TMessage);

            if (SubscribedMessageTypeList.Contains(messageType))
            {
                return false;
            }

            SubscribedMessageTypeList.Add(messageType);
            return true;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessageSubscribingFacade{TSender, TReceiver, TAdaptedMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Asynchronously publishes the specified <see cref="ExceptionRaisedMessage" /> instance.
        /// </summary>
        /// <param name="exceptionRaisedMessage">
        /// The message to publish.
        /// </param>
        /// <param name="messagingEntityType">
        /// The messaging type to use when publishing the message.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task PublishReceiverExceptionAsync(ExceptionRaisedMessage exceptionRaisedMessage, MessagingEntityType messagingEntityType);

        /// <summary>
        /// Registers the specified message handler with the bus.
        /// </summary>
        /// <param name="adaptedMessageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="receiveClient">
        /// An implementation-specific receive client.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected abstract void RegisterMessageHandler(Action<TAdaptedMessage> adaptedMessageHandler, TReceiver receiveClient, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Asynchronously executes the failure policy prescribed by the specified message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="message">
        /// A message that could not be processed.
        /// </param>
        /// <param name="raisedException">
        /// An exception that was raised while attempting to process the message.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [DebuggerHidden]
        private Task ExecuteFailurePolicyAsync<TMessage>(Action<TMessage> messageHandler, TMessage message, Exception raisedException)
            where TMessage : IMessageBase
        {
            var processingInformation = message?.ProcessingInformation;

            if (processingInformation is null)
            {
                return Task.CompletedTask;
            }

            var attemptCount = processingInformation.AttemptCount.AbsoluteValue();
            var failurePolicy = processingInformation?.FailurePolicy ?? DefaultFailurePolicy;
            var retryPolicy = failurePolicy?.RetryPolicy ?? DefaultFailurePolicy.RetryPolicy;
            var failurePolicyTasks = new List<Task>();

            if (failurePolicy.PublishExceptionRaisedMessage)
            {
                failurePolicyTasks.Add(PublishReceiverExceptionAsync(raisedException, message.CorrelationIdentifier));
            }

            if (retryPolicy.RetryCount >= attemptCount)
            {
                var baseDelayDurationInSeconds = retryPolicy.BaseDelayDurationInSeconds.AbsoluteValue();
                var calculatedDelayDurationInSeconds = default(Int32);

                switch (retryPolicy.DurationScale)
                {
                    case MessageSubscribingRetryDurationScale.Fibonacci:

                        calculatedDelayDurationInSeconds = (Int32)new FibonacciSequence(0, baseDelayDurationInSeconds).ToArray(attemptCount, 1).First();
                        break;

                    case MessageSubscribingRetryDurationScale.Linear:

                        calculatedDelayDurationInSeconds = baseDelayDurationInSeconds * processingInformation.AttemptCount;
                        break;

                    default:

                        throw new InvalidOperationException($"The specified duration scale, {retryPolicy.DurationScale}, is not supported.");
                }

                failurePolicyTasks.Add(Task.Delay(TimeSpan.FromSeconds(calculatedDelayDurationInSeconds)));
                failurePolicyTasks.Add(HandleMessageAsync(messageHandler, message));
            }
            else
            {
                switch (failurePolicy.SecondaryFailureBehavior)
                {
                    case MessageSubscribingSecondaryFailureBehavior.Discard:

                        // TODO
                        break;

                    case MessageSubscribingSecondaryFailureBehavior.RouteToDeadLetterQueue:

                        // TODO
                        break;

                    default:

                        throw new InvalidOperationException($"The specified secondary failure behavior, {failurePolicy.SecondaryFailureBehavior}, is not supported.");
                }
            }

            if (failurePolicyTasks.Any())
            {
                return Task.WhenAll(failurePolicyTasks.ToArray());
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Asynchronously publishes a new <see cref="ExceptionRaisedMessage" /> instance for the specified exception.
        /// </summary>
        /// <param name="raisedException">
        /// An exception for which to publish a new message.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A correlation identifier for the message that could not be processed.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [DebuggerHidden]
        private Task PublishReceiverExceptionAsync(Exception raisedException, Guid correlationIdentifier)
        {
            var exceptionRaisedMessage = new ExceptionRaisedMessage(raisedException, ExceptionRaisedMessageEventVerbosity, correlationIdentifier);
            return PublishReceiverExceptionAsync(exceptionRaisedMessage, ExceptionRaisedMessageEntityType);
        }

        /// <summary>
        /// Gets the collection of message types for which the current
        /// <see cref="MessageSubscribingFacade{TSender, TReceiver, TAdaptedMessage}" /> has one or more registered handlers.
        /// </summary>
        public IEnumerable<Type> SubscribedMessageTypes => SubscribedMessageTypeList.ToArray();

        /// <summary>
        /// Gets the entity type that is used to publish new instances of <see cref="ExceptionRaisedMessage" />.
        /// </summary>
        protected virtual MessagingEntityType ExceptionRaisedMessageEntityType => MessagingEntityType.Topic;

        /// <summary>
        /// Gets the event verbosity level that is used when publishing new instances of <see cref="ExceptionRaisedMessage" />.
        /// </summary>
        protected virtual ApplicationEventVerbosity ExceptionRaisedMessageEventVerbosity => ApplicationEventVerbosity.Normal;

        /// <summary>
        /// Represents the default instructions that guide failure behavior for the subscriber.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly MessageSubscribingFailurePolicy DefaultFailurePolicy = MessageSubscribingFailurePolicy.Default;

        /// <summary>
        /// Represents the <see cref="IResponseMessage" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ResponseMessageInterfaceType = typeof(IResponseMessage);

        /// <summary>
        /// Represents the collection of message types for which the current
        /// <see cref="MessageSubscribingFacade{TSender, TReceiver, TAdaptedMessage}" /> has one or more registered handlers.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<Type> SubscribedMessageTypeList = new List<Type>();
    }
}