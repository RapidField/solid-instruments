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
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected sealed override void RegisterMessageHandler(Action<AzureServiceBusMessage> messageHandler, IReceiverClient receiveClient, ConcurrencyControlToken controlToken)
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

                    cancellationToken.ThrowIfCancellationRequested();
                    messageHandler(message);
                    return receiveClient.CompleteAsync(lockToken);
                }
                catch
                {
                    return receiveClient.AbandonAsync(lockToken);
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
        /// An exception was raised while trying to transmit an <see cref="ExceptionRaisedEventMessage" />.
        /// </exception>
        [DebuggerHidden]
        private static Task HandleReceiverExceptionAsync(ExceptionReceivedEventArgs exceptionReceivedArguments) => Task.CompletedTask; // TODO Handle receiver exceptions.

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