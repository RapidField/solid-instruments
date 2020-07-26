// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about a transaction event.
    /// </summary>
    public interface ITransactionEventMessage : ITransactionEventMessage<TransactionEvent>
    {
    }

    /// <summary>
    /// Represents a message that provides notification about a transaction event.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    public interface ITransactionEventMessage<TEvent> : IEventMessage<TEvent>
        where TEvent : class, ITransactionEvent
    {
    }
}