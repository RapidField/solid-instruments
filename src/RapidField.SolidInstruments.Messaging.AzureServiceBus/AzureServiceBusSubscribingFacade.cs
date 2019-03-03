// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AzureServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace RapidField.SolidInstruments.Messaging.AzureServiceBus
{
    /// <summary>
    /// Facilitates subscribing operations for Azure Service Bus queues.
    /// </summary>
    public sealed class AzureServiceBusSubscribingFacade : MessageSubscribingFacade<ISenderClient, IReceiverClient, AzureServiceBusMessage, AzureServiceBusPublishingFacade>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusSubscribingFacade" /> class.
        /// </summary>
        /// <param name="publishingFacade">
        /// An implementation-specific messaging facade that is used to publish response messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="publishingFacade" /> is <see langword="null" />.
        /// </exception>
        public AzureServiceBusSubscribingFacade(AzureServiceBusPublishingFacade publishingFacade)
            : base(publishingFacade)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusSubscribingFacade" /> class.
        /// </summary>
        /// <param name="publishingFacade">
        /// An implementation-specific messaging facade that is used to publish response messages.
        /// </param>
        /// <param name="exceptionHandlingBehavior">
        /// The behavior of the facade when handling an exception that is raised by a receiver. The default value is
        /// <see cref="ReceiverExceptionHandlingBehavior.PublishExceptionRaisedMessage" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="publishingFacade" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="exceptionHandlingBehavior" /> is equal to <see cref="ReceiverExceptionHandlingBehavior.Unspecified" />.
        /// </exception>
        public AzureServiceBusSubscribingFacade(AzureServiceBusPublishingFacade publishingFacade, ReceiverExceptionHandlingBehavior exceptionHandlingBehavior)
            : base(publishingFacade, exceptionHandlingBehavior)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AzureServiceBusSubscribingFacade" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
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
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected sealed override void RegisterMessageHandler(Action<AzureServiceBusMessage> messageHandler, IReceiverClient receiveClient, ConcurrencyControlToken controlToken)
        {
            var messageHandlerFunction = new Func<AzureServiceBusMessage, CancellationToken, Task>((message, cancellationToken) =>
            {
                var lockToken = message.SystemProperties?.LockToken;

                if (lockToken is null)
                {
                    throw new MessageSubscribingException("The message cannot be processed because the lock token is invalid.");
                }
                else if (receiveClient.IsClosedOrClosing)
                {
                    throw new MessageSubscribingException("The message cannot be processed because the receive client is unavailable.");
                }

                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    messageHandler(message);
                    return receiveClient.CompleteAsync(lockToken);
                }
                catch
                {
                    receiveClient.AbandonAsync(lockToken).Wait();
                    throw;
                }
            });

            receiveClient.RegisterMessageHandler(messageHandlerFunction, ReceiverOptions);
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
        /// An exception was raised while trying to publish an <see cref="ExceptionRaisedMessage" />.
        /// </exception>
        [DebuggerHidden]
        private Task HandleReceiverExceptionAsync(ExceptionReceivedEventArgs exceptionReceivedArguments) => HandleReceiverExceptionAsync(exceptionReceivedArguments.Exception);

        /// <summary>
        /// Gets options that specify how receive clients handle messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MessageHandlerOptions ReceiverOptions => new MessageHandlerOptions(HandleReceiverExceptionAsync)
        {
            AutoComplete = false
        };
    }
}