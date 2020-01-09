// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.TextEncoding;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates implementation-specific operations for a message bus.
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
    /// <see cref="MessagingFacade{TSender, TReceiver, TAdaptedMessage}" /> is the default implementation of
    /// <see cref="IMessagingFacade{TSender, TReceiver, TAdaptedMessage}" />.
    /// </remarks>
    public abstract class MessagingFacade<TSender, TReceiver, TAdaptedMessage> : Instrument, IMessagingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingFacade{TSender, TReceiver, TAdaptedMessage}" /> class.
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
        protected MessagingFacade(IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage> clientFactory, IMessageAdapter<TAdaptedMessage> messageAdapter)
            : base(ConcurrencyControlMode.SingleThreadLock, Timeout.InfiniteTimeSpan)
        {
            ClientFactory = clientFactory.RejectIf().IsNull(nameof(clientFactory)).TargetArgument;
            LazyApplicationIdentity = new Lazy<String>(InitializeApplicationIdentity, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyIdentifier = new Lazy<String>(InitializeIdentifier, LazyThreadSafetyMode.ExecutionAndPublication);
            MessageAdapter = messageAdapter.RejectIf().IsNull(nameof(messageAdapter)).TargetArgument;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessagingFacade{TSender, TReceiver, TAdaptedMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Initializes the name or value that uniquely identifies the application in which the current
        /// <see cref="MessagingFacade{TSender, TReceiver, TAdaptedMessage}" /> is running.
        /// </summary>
        /// <returns>
        /// A unique textual identifier.
        /// </returns>
        protected virtual String InitializeApplicationIdentity()
        {
            var process = Process.GetCurrentProcess();
            return $"{process.MachineName}_{process.ProcessName}_{process.Id.ToString()}";
        }

        /// <summary>
        /// Initializes the unique textual identifier for the current
        /// <see cref="MessagingFacade{TSender, TReceiver, TAdaptedMessage}" />.
        /// </summary>
        /// <returns>
        /// A unique textual identifier.
        /// </returns>
        protected virtual String InitializeIdentifier() => new String(new ZBase32Encoding().GetChars(Guid.NewGuid().ToByteArray().ComputeThirtyTwoBitHash().ToByteArray()));

        /// <summary>
        /// Gets the name or value that uniquely identifies the application in which the current
        /// <see cref="MessagingFacade{TSender, TReceiver, TAdaptedMessage}" /> is running.
        /// </summary>
        public String ApplicationIdentity => LazyApplicationIdentity.Value;

        /// <summary>
        /// Gets the unique identifier for the current <see cref="MessagingFacade{TSender, TReceiver, TAdaptedMessage}" />.
        /// </summary>
        public String Identifier => LazyIdentifier.Value;

        /// <summary>
        /// Represents an appliance that creates and manages implementation-specific messaging clients.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage> ClientFactory;

        /// <summary>
        /// Represents an appliance that facilitates implementation-specific message conversion.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly IMessageAdapter<TAdaptedMessage> MessageAdapter;

        /// <summary>
        /// Represents the lazily-initialized name or value that uniquely identifies the application in which the current
        /// <see cref="MessagingFacade{TSender, TReceiver, TAdaptedMessage}" /> is running.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<String> LazyApplicationIdentity;

        /// <summary>
        /// Represents the lazily-initialized unique identifier for the current
        /// <see cref="MessagingFacade{TSender, TReceiver, TAdaptedMessage}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<String> LazyIdentifier;
    }
}