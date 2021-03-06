﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about a transaction.
    /// </summary>
    /// <remarks>
    /// <see cref="TransactionEventMessage" /> is the default implementation of <see cref="ITransactionEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class TransactionEventMessage : TransactionEventMessage<TransactionEvent>, ITransactionEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEventMessage" /> class.
        /// </summary>
        public TransactionEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public TransactionEventMessage(TransactionEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification about a transaction.
    /// </summary>
    /// <remarks>
    /// <see cref="TransactionEventMessage{TEvent}" /> is the default implementation of
    /// <see cref="ITransactionEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class TransactionEventMessage<TEvent> : EventMessage<TEvent>, ITransactionEventMessage<TEvent>
        where TEvent : TransactionEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEventMessage{TEvent}" /> class.
        /// </summary>
        protected TransactionEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected TransactionEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}