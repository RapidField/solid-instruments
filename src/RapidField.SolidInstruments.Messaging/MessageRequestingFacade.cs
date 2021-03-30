// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates implementation-specific requesting operations for a message bus.
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
    /// The type of the implementation-specific messaging facade that is used to transmit request and response messages.
    /// </typeparam>
    /// <typeparam name="TListeningFacade">
    /// The type of the implementation-specific messaging facade that listens for request messages.
    /// </typeparam>
    /// <remarks>
    /// <see cref="MessageRequestingFacade{TSender, TReceiver, TAdaptedMessage, TTransmittingFacade, TListeningFacade}" /> is the
    /// default implementation of
    /// <see cref="IMessageRequestingFacade{TSender, TReceiver, TAdaptedMessage, TTransmittingFacade, TListeningFacade}" />.
    /// </remarks>
    public abstract class MessageRequestingFacade<TSender, TReceiver, TAdaptedMessage, TTransmittingFacade, TListeningFacade> : MessageRequestingFacade<TSender, TReceiver, TAdaptedMessage>, IMessageRequestingFacade<TSender, TReceiver, TAdaptedMessage, TTransmittingFacade, TListeningFacade>
        where TAdaptedMessage : class
        where TTransmittingFacade : MessageTransmittingFacade<TSender, TReceiver, TAdaptedMessage>
        where TListeningFacade : MessageListeningFacade<TSender, TReceiver, TAdaptedMessage, TTransmittingFacade>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestingFacade{TSender, TReceiver, TAdaptedMessage}" /> class.
        /// </summary>
        /// <param name="listeningFacade">
        /// An implementation-specific messaging facade that listens for request messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="listeningFacade" /> is <see langword="null" />.
        /// </exception>
        protected MessageRequestingFacade(TListeningFacade listeningFacade)
            : base(listeningFacade.RejectIf().IsNull(nameof(listeningFacade)).TargetArgument.ClientFactory, listeningFacade.MessageAdapter)
        {
            TransmittingFacade = listeningFacade.TransmittingFacade;
            ListeningFacade = listeningFacade;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessageRequestingFacade{TSender, TReceiver, TAdaptedMessage, TTransmittingFacade, TListeningFacade}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Registers the specified response handler with a bus.
        /// </summary>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="responseHandler">
        /// An action that handles the response.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected sealed override void RegisterResponseHandler<TResponseMessage>(Action<TResponseMessage> responseHandler, IConcurrencyControlToken controlToken) => ListeningFacade.RegisterTopicMessageHandler(responseHandler);

        /// <summary>
        /// Asynchronously transmits the specified request message to a bus.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessage">
        /// The request message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected sealed override Task TransmitRequestMessageAsync<TRequestMessage, TResponseMessage>(TRequestMessage requestMessage) => TransmittingFacade.TransmitAsync(requestMessage, null, Message.RequestEntityType);

        /// <summary>
        /// Represents an implementation-specific messaging facade that listens for request messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TListeningFacade ListeningFacade;

        /// <summary>
        /// Represents an implementation-specific messaging facade that is used to transmit request and response messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TTransmittingFacade TransmittingFacade;
    }

    /// <summary>
    /// Facilitates implementation-specific requesting operations for a message bus.
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
    /// <see cref="MessageRequestingFacade{TSender, TReceiver, TAdaptedMessage}" /> is the default implementation of
    /// <see cref="IMessageRequestingFacade{TSender, TReceiver, TAdaptedMessage}" />.
    /// </remarks>
    public abstract class MessageRequestingFacade<TSender, TReceiver, TAdaptedMessage> : MessagingFacade<TSender, TReceiver, TAdaptedMessage>, IMessageRequestingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestingFacade{TSender, TReceiver, TAdaptedMessage}" /> class.
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
        protected MessageRequestingFacade(IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage> clientFactory, IMessageAdapter<TAdaptedMessage> messageAdapter)
            : base(clientFactory, messageAdapter)
        {
            return;
        }

        /// <summary>
        /// Asynchronously transmits the specified request message to a bus and waits for the correlated response message.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessage">
        /// The request message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the correlated response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessage" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageRequestingException">
        /// An exception was raised while attempting to process <paramref name="requestMessage" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task<TResponseMessage> RequestAsync<TRequestMessage, TResponseMessage>(TRequestMessage requestMessage)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
        {
            var requestMessageIdentifier = requestMessage.RejectIf().IsNull(nameof(requestMessage)).TargetArgument.Identifier;

            using (var controlToken = StateControl.Enter())
            {
                if (TryAddOutstandingRequest<TRequestMessage, TResponseMessage>(requestMessage, requestMessageIdentifier) is false)
                {
                    return Task.FromException<TResponseMessage>(new InvalidOperationException("The request was not processed because it is a duplicate."));
                }

                try
                {
                    RegisterResponseHandler<TResponseMessage>(HandleResponseMessage, controlToken);
                }
                catch (Exception exception)
                {
                    return Task.FromException<TResponseMessage>(new MessageRequestingException(typeof(TRequestMessage), exception));
                }
            }

            return TransmitRequestMessageAsync<TRequestMessage, TResponseMessage>(requestMessage).ContinueWith(transmitTask =>
            {
                RejectIfDisposed();

                try
                {
                    if (WaitForResponse(requestMessageIdentifier) is not TResponseMessage responseMessage)
                    {
                        throw new MessageListeningException($"The response message is not a valid {typeof(TResponseMessage)}.");
                    }

                    return responseMessage;
                }
                catch (MessageRequestingException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new MessageRequestingException(typeof(TRequestMessage), exception);
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessageRequestingFacade{TSender, TReceiver, TAdaptedMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Registers the specified response handler with a bus.
        /// </summary>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="responseHandler">
        /// An action that handles the response.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected abstract void RegisterResponseHandler<TResponseMessage>(Action<TResponseMessage> responseHandler, IConcurrencyControlToken controlToken)
            where TResponseMessage : class, IResponseMessage;

        /// <summary>
        /// Asynchronously transmits the specified request message to a bus.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessage">
        /// The request message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task TransmitRequestMessageAsync<TRequestMessage, TResponseMessage>(TRequestMessage requestMessage)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage;

        /// <summary>
        /// Handles the specified response message.
        /// </summary>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="responseMessage">
        /// The response message to handle.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="responseMessage" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private void HandleResponseMessage<TResponseMessage>(TResponseMessage responseMessage)
            where TResponseMessage : class, IResponseMessage => TryAddUnprocessedResponse(responseMessage.RejectIf().IsNull(nameof(responseMessage)).TargetArgument);

        /// <summary>
        /// Attempts to add the specified request to a list of outstanding requests as a thread safe operation.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessage">
        /// The outstanding request message.
        /// </param>
        /// <param name="requestMessageIdentifier">
        /// An identifier for the outstanding request message.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the request was added, otherwise <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        private Boolean TryAddOutstandingRequest<TRequestMessage, TResponseMessage>(TRequestMessage requestMessage, Guid requestMessageIdentifier)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage => OutstandingRequests.TryAdd(requestMessageIdentifier, requestMessage);

        /// <summary>
        /// Attempts to add the specified response to a list of unprocessed responses as a thread safe operation.
        /// </summary>
        /// <param name="responseMessage">
        /// The unprocessed response message.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the response was added, otherwise <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        private Boolean TryAddUnprocessedResponse(IResponseMessage responseMessage)
        {
            if (OutstandingRequests.TryRemove(responseMessage.RequestMessageIdentifier, out _))
            {
                if (UnprocessedResponses.TryAdd(responseMessage.RequestMessageIdentifier, responseMessage))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Waits for a response message matching the specified request to populate the client manager's response dictionary.
        /// </summary>
        /// <param name="requestMessageIdentifier">
        /// An identifier for the associated request message.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the response message.
        /// </returns>
        /// <exception cref="TimeoutException">
        /// The timeout threshold duration was exceeded while waiting for a response.
        /// </exception>
        [DebuggerHidden]
        private IResponseMessage WaitForResponse(Guid requestMessageIdentifier) => WaitForResponse(requestMessageIdentifier, DefaultWaitForResponseTimeoutThreshold);

        /// <summary>
        /// Waits for a response message matching the specified request to populate the client manager's response dictionary.
        /// </summary>
        /// <param name="requestMessageIdentifier">
        /// An identifier for the associated request message.
        /// </param>
        /// <param name="timeoutThreshold">
        /// The maximum length of time to wait for the response before raising an exception, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> to specify an infinite duration. The default value is one minute.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the response message.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> -and-
        /// <paramref name="timeoutThreshold" /> is not equal to <see cref="Timeout.InfiniteTimeSpan" />.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The timeout threshold duration was exceeded while waiting for a response.
        /// </exception>
        [DebuggerHidden]
        private IResponseMessage WaitForResponse(Guid requestMessageIdentifier, TimeSpan timeoutThreshold)
        {
            var stopwatch = new Stopwatch();

            if (timeoutThreshold.RejectIf(argument => argument <= TimeSpan.Zero && argument != Timeout.InfiniteTimeSpan, nameof(timeoutThreshold)) != Timeout.InfiniteTimeSpan)
            {
                stopwatch.Start();
            }

            while (stopwatch.Elapsed < timeoutThreshold)
            {
                Thread.Sleep(ResponseMessagePollingInterval);

                if (UnprocessedResponses.TryRemove(requestMessageIdentifier, out var responseMessage))
                {
                    return responseMessage;
                }
            }

            throw new TimeoutException($"The timeout threshold duration was exceeded while waiting for a response to request message {requestMessageIdentifier.ToSerializedString()}.");
        }

        /// <summary>
        /// Represents a collection of outstanding requests that key pending response messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConcurrentDictionary<Guid, IMessageBase> OutstandingRequests => LazyOutstandingRequests.Value;

        /// <summary>
        /// Gets a collection of unprocessed response messages that are keyed by request message identifier.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConcurrentDictionary<Guid, IResponseMessage> UnprocessedResponses => LazyUnprocessedResponses.Value;

        /// <summary>
        /// Represents the default maximum length of time to wait for the response before raising an exception.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan DefaultWaitForResponseTimeoutThreshold = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Represents the polling interval that is used when waiting for a response message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan ResponseMessagePollingInterval = TimeSpan.FromMilliseconds(3);

        /// <summary>
        /// Represents a lazily-initialized collection of outstanding requests that key pending response messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<ConcurrentDictionary<Guid, IMessageBase>> LazyOutstandingRequests = new(() => new(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Represents a lazily-initialized collection of unprocessed response messages that are keyed by request message
        /// identifier.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<ConcurrentDictionary<Guid, IResponseMessage>> LazyUnprocessedResponses = new(() => new(), LazyThreadSafetyMode.ExecutionAndPublication);
    }
}