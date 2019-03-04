// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

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
    public interface IMessagingFacade<TSender, TReceiver, TAdaptedMessage> : IMessagingFacade<TAdaptedMessage>
        where TAdaptedMessage : class
    {
    }

    /// <summary>
    /// Facilitates implementation-specific operations for a message bus.
    /// </summary>
    /// <typeparam name="TAdaptedMessage">
    /// The type of implementation-specific adapted messages.
    /// </typeparam>
    public interface IMessagingFacade<TAdaptedMessage> : IMessagingFacade
        where TAdaptedMessage : class
    {
    }

    /// <summary>
    /// Facilitates implementation-specific operations for a message bus.
    /// </summary>
    public interface IMessagingFacade : IDisposable
    {
        /// <summary>
        /// Gets the unique textual identifier for the current <see cref="IMessagingFacade" />.
        /// </summary>
        String Identifier
        {
            get;
        }
    }
}