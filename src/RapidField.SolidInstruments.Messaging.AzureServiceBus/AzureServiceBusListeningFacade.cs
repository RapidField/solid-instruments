// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AzureServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace RapidField.SolidInstruments.Messaging.AzureServiceBus
{
    /// <summary>
    /// Facilitates listening operations for Azure Service Bus queues.
    /// </summary>
    public sealed class AzureServiceBusListeningFacade : MessageListeningFacade<ISenderClient, IReceiverClient, AzureServiceBusMessage, AzureServiceBusTransmittingFacade>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusListeningFacade" /> class.
        /// </summary>
        /// <param name="transmittingFacade">
        /// An implementation-specific messaging facade that is used to transmit response messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="transmittingFacade" /> is <see langword="null" />.
        /// </exception>
        public AzureServiceBusListeningFacade(AzureServiceBusTransmittingFacade transmittingFacade)
            : base(transmittingFacade)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AzureServiceBusListeningFacade" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Registers the specified message handler with the bus.
        /// </summary>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="receiveClient">
        /// An implementation-specific receive client.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected sealed override void RegisterMessageHandler(Action<AzureServiceBusMessage> messageHandler, IReceiverClient receiveClient, IConcurrencyControlToken controlToken)
        {
            var messageHandlerFunction = new Func<AzureServiceBusMessage, CancellationToken, Task>((message, cancellationToken) =>
            {
                var lockToken = (String)null;

                try
                {
                    lockToken = message.SystemProperties?.LockToken;

                    if (lockToken is null)
                    {
                        throw new MessageListeningException("The message cannot be processed because the lock token is invalid.");
                    }
                    else if (receiveClient.IsClosedOrClosing)
                    {
                        throw new MessageListeningException("The message cannot be processed because the receive client is unavailable.");
                    }

                    return Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            messageHandler(message);
                        }
                        catch (Exception exception)
                        {
                            receiveClient.AbandonAsync(lockToken).ContinueWith(abandonTask => { TransmitReceiverExceptionAsync(exception, ExtractCorrelationIdentifier(message)).Wait(); }).Wait();
                        }
                    }, cancellationToken).ContinueWith(handleMessageTask =>
                    {
                        try
                        {
                            receiveClient.CompleteAsync(lockToken).Wait();
                        }
                        catch (AggregateException exception)
                        {
                            TransmitReceiverExceptionAsync(exception, ExtractCorrelationIdentifier(message)).Wait();
                        }
                    }, TaskContinuationOptions.OnlyOnRanToCompletion);
                }
                catch (MessageListeningException exception)
                {
                    return TransmitReceiverExceptionAsync(exception, ExtractCorrelationIdentifier(message));
                }
            });

            receiveClient.RegisterMessageHandler(messageHandlerFunction, ReceiverOptions);
        }

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
        protected sealed override Task RouteToDeadLetterQueueAsync<TMessage>(AzureServiceBusMessage adaptedMessage, TMessage message, IEnumerable<String> pathLabels, MessagingEntityType entityType)
        {
            var lockToken = adaptedMessage?.SystemProperties.LockToken;

            if (lockToken is null)
            {
                return Task.CompletedTask;
            }

            var receiverClient = entityType switch
            {
                MessagingEntityType.Queue => ClientFactory.GetQueueReceiver<TMessage>(pathLabels),
                MessagingEntityType.Topic => ClientFactory.GetTopicReceiver<TMessage>(Identifier, pathLabels),
                _ => throw new UnsupportedSpecificationException($"The specified messaging entity type, {entityType}, is not supported.")
            };

            var messageType = typeof(TMessage);
            return receiverClient.DeadLetterAsync(lockToken, $"The listener(s) exhausted the primary failure remediation steps for this message. Message type: {messageType.FullName}.");
        }

        /// <summary>
        /// Extracts the correlation identifier from the specified message.
        /// </summary>
        /// <param name="message">
        /// The message to evaluate.
        /// </param>
        /// <returns>
        /// The resulting correlation identifier.
        /// </returns>
        [DebuggerHidden]
        private static Guid ExtractCorrelationIdentifier(AzureServiceBusMessage message)
        {
            if (message?.CorrelationId is not null && Guid.TryParse(message.CorrelationId, out var correlationIdentifier))
            {
                return correlationIdentifier;
            }

            return Guid.NewGuid();
        }

        /// <summary>
        /// Asynchronously handles an exception that was raised by a message receive client.
        /// </summary>
        /// <param name="exceptionReceivedArguments">
        /// Contextual information about the raised exception.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while trying to transmit an <see cref="ExceptionRaisedEventMessage" />.
        /// </exception>
        [DebuggerHidden]
        private static Task HandleReceiverExceptionAsync(ExceptionReceivedEventArgs exceptionReceivedArguments) => Task.CompletedTask; // The message handling function (defined above) handles exceptions internally.

        /// <summary>
        /// Gets options that specify how receive clients handle messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static MessageHandlerOptions ReceiverOptions => new MessageHandlerOptions(HandleReceiverExceptionAsync)
        {
            AutoComplete = false
        };
    }
}