// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification when an <see cref="Exception" /> has been raised.
    /// </summary>
    public interface IExceptionRaisedEventMessage : IExceptionRaisedEventMessage<ExceptionRaisedEvent>
    {
    }

    /// <summary>
    /// Represents a message that provides notification when an <see cref="Exception" /> has been raised.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    public interface IExceptionRaisedEventMessage<TEvent> : IErrorEventMessage<TEvent>
        where TEvent : class, IExceptionRaisedEvent
    {
    }
}