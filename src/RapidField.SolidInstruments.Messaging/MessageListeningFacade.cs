// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
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
    /// Facilitates implementation-specific listening operations for a message bus.
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
    /// <typeparam name="TTransmittingFacade">
    /// The type of the implementation-specific messaging facade that is used to transmit response messages.
    /// </typeparam>
    /// <remarks>
    /// <see cref="MessageListeningFacade{TSender, TReceiver, TAdaptedMessage, TTransmittingFacade}" /> is the default
    /// implementation of <see cref="IMessageListeningFacade{TSender, TReceiver, TAdaptedMessage, TTransmittingFacade}" />.
    /// </remarks>
    public abstract class MessageListeningFacade<TSender, TReceiver, TAdaptedMessage, TTransmittingFacade> : MessageListeningFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
        where TTransmittingFacade : MessageTransmittingFacade<TSender, TReceiver, TAdaptedMessage>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MessageListeningFacade{TSender, TReceiver, TAdaptedMessage, TTransmittingFacade}" /> class.
        /// </summary>
        /// <param name="transmittingFacade">
        /// An implementation-specific messaging facade that is used to transmit response messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="transmittingFacade" /> is <see langword="null" />.
        /// </exception>
        protected MessageListeningFacade(TTransmittingFacade transmittingFacade)
            : base(transmittingFacade.RejectIf().IsNull(nameof(transmittingFacade)).TargetArgument.ClientFactory, transmittingFacade.MessageAdapter)
        {
            TransmittingFacade = transmittingFacade;
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
        /// <exception cref="MessageListeningException">
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

                    if (TryAddListenedMessageType<TRequestMessage>(controlToken) == false)
                    {
                        // Disallow registration of duplicate request handlers.
                        return;
                    }

                    var requestEntityType = Message.RequestEntityType;
                    var requestReceiveClient = requestEntityType switch
                    {
                        MessagingEntityType.Queue => ClientFactory.GetQueueReceiver<TRequestMessage>(),
                        MessagingEntityType.Topic => ClientFactory.GetTopicReceiver<TRequestMessage>(Identifier),
                        _ => throw new UnsupportedSpecificationException($"The specified messaging entity type, {requestEntityType}, is not supported.")
                    };

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
                                TransmittingFacade.TransmitAsync(responseMessage, null, Message.ResponseEntityType).Wait();
                            }, adaptedRequestMessage, requestMessage, null, requestEntityType).Wait();
                        }
                        catch (Exception exception)
                        {
                            throw new MessageListeningException(typeof(TRequestMessage), exception);
                        }
                    });

                    RegisterMessageHandler(messageHandler, requestReceiveClient, controlToken);
                }
            }
            catch (MessageListeningException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessageListeningException(typeof(TRequestMessage), exception);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessageListeningFacade{TSender, TReceiver, TAdaptedMessage, TTransmittingFacade}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Asynchronously transmits the specified <see cref="ExceptionRaisedEventMessage" /> instance.
        /// </summary>
        /// <param name="exceptionRaisedMessage">
        /// The message to transmit.
        /// </param>
        /// <param name="messagingEntityType">
        /// The messaging type to use when transmitting the message.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected sealed override Task TransmitReceiverExceptionAsync(ExceptionRaisedEventMessage exceptionRaisedMessage, MessagingEntityType messagingEntityType) => TransmittingFacade.TransmitAsync(exceptionRaisedMessage, null, messagingEntityType);

        /// <summary>
        /// Represents an implementation-specific messaging facade that is used to transmit response messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly TTransmittingFacade TransmittingFacade;
    }

    /// <summary>
    /// Facilitates implementation-specific listening operations for a message bus.
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
    /// <see cref="MessageListeningFacade{TSender, TReceiver, TAdaptedMessage}" /> is the default implementation of
    /// <see cref="IMessageListeningFacade{TSender, TReceiver, TAdaptedMessage}" />.
    /// </remarks>
    public abstract class MessageListeningFacade<TSender, TReceiver, TAdaptedMessage> : MessagingFacade<TSender, TReceiver, TAdaptedMessage>, IMessageListeningFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListeningFacade{TSender, TReceiver, TAdaptedMessage}" /> class.
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
        protected MessageListeningFacade(IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage> clientFactory, IMessageAdapter<TAdaptedMessage> messageAdapter)
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
        /// <exception cref="MessageListeningException">
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
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageListeningException">
        /// An exception was raised while attempting to register <paramref name="messageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RegisterQueueMessageHandler<TMessage>(Action<TMessage> messageHandler, IEnumerable<String> pathLabels)
            where TMessage : class, IMessage => RegisterMessageHandler(messageHandler, pathLabels, MessagingEntityType.Queue);

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
        /// <exception cref="MessageListeningException">
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
        /// <exception cref="MessageListeningException">
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
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageListeningException">
        /// An exception was raised while attempting to register <paramref name="messageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RegisterTopicMessageHandler<TMessage>(Action<TMessage> messageHandler, IEnumerable<String> pathLabels)
            where TMessage : class, IMessage => RegisterMessageHandler(messageHandler, pathLabels, MessagingEntityType.Topic);

        /// <summary>
        /// Appends the dead letter queue label to the specified path labels.
        /// </summary>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> if path labels are omitted.
        /// </param>
        /// <returns>
        /// The specified path labels with the dead letter label appended.
        /// </returns>
        [DebuggerHidden]
        internal static IEnumerable<String> AppendDeadLetterQueueLabel(IEnumerable<String> pathLabels)
        {
            if (pathLabels.IsNullOrEmpty())
            {
                return new String[] { DeadLetterQueueLabel };
            }

            return (pathLabels.Count()) switch
            {
                1 => new String[] { pathLabels.ElementAt(0), DeadLetterQueueLabel },
                2 => new String[] { pathLabels.ElementAt(0), pathLabels.ElementAt(1), DeadLetterQueueLabel },
                3 => new String[] { pathLabels.ElementAt(0), pathLabels.ElementAt(1), $"{pathLabels.ElementAt(1)}{MessagingEntityPath.DelimitingCharacterForLabelTokenSubParts}{DeadLetterQueueLabel}" },
                _ => throw new ArgumentException("The specified path label collection contains more than three elements.", nameof(pathLabels)),
            };
        }

        /// <summary>
        /// Asynchronously performs the specified action for the specified message and, upon failure, applies the execution policy.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="adaptedMessage">
        /// The adapted message.
        /// </param>
        /// <param name="message">
        /// A message to handle.
        /// </param>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [DebuggerHidden]
        internal Task HandleMessageAsync<TMessage>(Action<TMessage> messageHandler, TAdaptedMessage adaptedMessage, TMessage message, IEnumerable<String> pathLabels, MessagingEntityType entityType)
            where TMessage : class, IMessageBase
        {
            var attemptStartTimeStamp = TimeStamp.Current;
            Exception raisedException;

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

            return ExecuteFailurePolicyAsync(messageHandler, adaptedMessage, message, pathLabels, entityType, raisedException);
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
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageListeningException">
        /// An exception was raised while attempting to register <paramref name="messageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        internal void RegisterMessageHandler<TMessage>(Action<TMessage> messageHandler, IEnumerable<String> pathLabels, MessagingEntityType entityType)
            where TMessage : class, IMessage
        {
            messageHandler = messageHandler.RejectIf().IsNull(nameof(messageHandler)).TargetArgument;

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (TryAddListenedMessageType<TMessage>(controlToken) == false)
                {
                    if (ResponseMessageInterfaceType.IsAssignableFrom(typeof(TMessage)))
                    {
                        // Disallow registration of duplicate response handlers.
                        return;
                    }
                }

                var receiveClient = entityType switch
                {
                    MessagingEntityType.Queue => ClientFactory.GetQueueReceiver<TMessage>(pathLabels),
                    MessagingEntityType.Topic => ClientFactory.GetTopicReceiver<TMessage>(Identifier, pathLabels),
                    _ => throw new UnsupportedSpecificationException($"The specified messaging entity type, {entityType}, is not supported.")
                };

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

                        HandleMessageAsync(messageHandler, adaptedMessage, message, pathLabels, entityType).Wait();
                    }
                    catch (Exception exception)
                    {
                        throw new MessageListeningException(typeof(TMessage), exception);
                    }
                });

                try
                {
                    RegisterMessageHandler(adaptedMessageHandler, receiveClient, controlToken);
                }
                catch (MessageListeningException)
                {
                    throw;
                }
                catch (ObjectDisposedException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new MessageListeningException(typeof(TMessage), exception);
                }
            }
        }

        /// <summary>
        /// Attempts to add the specified message type to the collection of listened message types.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the listened message.
        /// </typeparam>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified message type was added; <see langword="false" /> if it was already present.
        /// </returns>
        [DebuggerHidden]
        internal Boolean TryAddListenedMessageType<TMessage>(IConcurrencyControlToken controlToken)
            where TMessage : IMessageBase
        {
            var messageType = typeof(TMessage);

            if (ListenedMessageTypeList.Contains(messageType))
            {
                return false;
            }

            ListenedMessageTypeList.Add(messageType);
            return true;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessageListeningFacade{TSender, TReceiver, TAdaptedMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

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
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected abstract void RegisterMessageHandler(Action<TAdaptedMessage> adaptedMessageHandler, TReceiver receiveClient, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Asynchronously sends the specified message to a dead letter queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="adaptedMessage">
        /// The adapted message.
        /// </param>
        /// <param name="message">
        /// The message to route to a dead letter queue.
        /// </param>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task RouteToDeadLetterQueueAsync<TMessage>(TAdaptedMessage adaptedMessage, TMessage message, IEnumerable<String> pathLabels, MessagingEntityType entityType)
            where TMessage : class, IMessageBase;

        /// <summary>
        /// Asynchronously transmits a new <see cref="ExceptionRaisedEventMessage" /> instance for the specified exception.
        /// </summary>
        /// <param name="raisedException">
        /// An exception for which to transmit a new message.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A correlation identifier for the message that could not be processed.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected Task TransmitReceiverExceptionAsync(Exception raisedException, Guid correlationIdentifier)
        {
            var exceptionRaisedEvent = new ExceptionRaisedEvent(ApplicationIdentity, raisedException, ExceptionRaisedMessageEventVerbosity, correlationIdentifier);
            var exceptionRaisedEventMessage = new ExceptionRaisedEventMessage(exceptionRaisedEvent);
            return TransmitReceiverExceptionAsync(exceptionRaisedEventMessage, ExceptionRaisedMessageEntityType);
        }

        /// <summary>
        /// Asynchronously transmits the specified <see cref="ExceptionRaisedEventMessage" /> instance.
        /// </summary>
        /// <param name="exceptionRaisedMessage">
        /// The message to transmit.
        /// </param>
        /// <param name="entityType">
        /// The messaging type to use when transmitting the message.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task TransmitReceiverExceptionAsync(ExceptionRaisedEventMessage exceptionRaisedMessage, MessagingEntityType entityType);

        /// <summary>
        /// Asynchronously executes the failure policy prescribed by the specified message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="adaptedMessage">
        /// The adapted message.
        /// </param>
        /// <param name="message">
        /// A message that could not be processed.
        /// </param>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <param name="raisedException">
        /// An exception that was raised while attempting to process the message.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [DebuggerHidden]
        private Task ExecuteFailurePolicyAsync<TMessage>(Action<TMessage> messageHandler, TAdaptedMessage adaptedMessage, TMessage message, IEnumerable<String> pathLabels, MessagingEntityType entityType, Exception raisedException)
            where TMessage : class, IMessageBase
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

            if (failurePolicy.TransmitExceptionRaisedEventMessage)
            {
                failurePolicyTasks.Add(TransmitReceiverExceptionAsync(raisedException, message.CorrelationIdentifier));
            }

            if (retryPolicy.RetryCount >= attemptCount)
            {
                var baseDelayDurationInSeconds = retryPolicy.BaseDelayDurationInSeconds.AbsoluteValue();
                var calculatedDelayDurationInSeconds = retryPolicy.DurationScale switch
                {
                    MessageListeningRetryDurationScale.Decelerating => (Int32)new FibonacciSequence(0, baseDelayDurationInSeconds).ToArray(attemptCount, 1).First(),
                    MessageListeningRetryDurationScale.Linear => baseDelayDurationInSeconds * processingInformation.AttemptCount,
                    _ => throw new UnsupportedSpecificationException($"The specified duration scale, {retryPolicy.DurationScale}, is not supported.")
                };

                failurePolicyTasks.Add(Task.Delay(TimeSpan.FromSeconds(calculatedDelayDurationInSeconds)).ContinueWith(delayTask =>
                {
                    HandleMessageAsync(messageHandler, adaptedMessage, message, pathLabels, entityType).Wait();
                }));
            }
            else
            {
                var secondaryFailureTask = failurePolicy.SecondaryFailureBehavior switch
                {
                    MessageListeningSecondaryFailureBehavior.Discard => Task.CompletedTask,
                    MessageListeningSecondaryFailureBehavior.RouteToDeadLetterQueue => RouteToDeadLetterQueueAsync(adaptedMessage, message, pathLabels, entityType),
                    _ => throw new UnsupportedSpecificationException($"The specified secondary failure behavior, {failurePolicy.SecondaryFailureBehavior}, is not supported.")
                };
            }

            if (failurePolicyTasks.Any())
            {
                return Task.WhenAll(failurePolicyTasks.ToArray());
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the collection of message types for which the current
        /// <see cref="MessageListeningFacade{TSender, TReceiver, TAdaptedMessage}" /> has one or more registered handlers.
        /// </summary>
        public IEnumerable<Type> ListenedMessageTypes
        {
            get
            {
                foreach (var listenedMessageType in ListenedMessageTypeList)
                {
                    yield return listenedMessageType;
                }
            }
        }

        /// <summary>
        /// Gets the entity type that is used to transmit new instances of <see cref="ExceptionRaisedEventMessage" />.
        /// </summary>
        protected virtual MessagingEntityType ExceptionRaisedMessageEntityType => MessagingEntityType.Topic;

        /// <summary>
        /// Gets the event verbosity level that is used when transmitting new instances of
        /// <see cref="ExceptionRaisedEventMessage" />.
        /// </summary>
        protected virtual EventVerbosity ExceptionRaisedMessageEventVerbosity => EventVerbosity.Normal;

        /// <summary>
        /// Represents the default instructions that guide failure behavior for the listener.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly MessageListeningFailurePolicy DefaultFailurePolicy = MessageListeningFailurePolicy.Default;

        /// <summary>
        /// Represents a label that is appended to dead letter queues.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DeadLetterQueueLabel = "DeadLetter";

        /// <summary>
        /// Represents the <see cref="IResponseMessage" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ResponseMessageInterfaceType = typeof(IResponseMessage);

        /// <summary>
        /// Represents the collection of message types for which the current
        /// <see cref="MessageListeningFacade{TSender, TReceiver, TAdaptedMessage}" /> has one or more registered handlers.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<Type> ListenedMessageTypeList = new List<Type>();
    }
}