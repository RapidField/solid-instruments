// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Defines the role of an <see cref="IMessageHandler{TMessage, TResult}" />.
    /// </summary>
    public enum MessageHandlerRole : Int32
    {
        /// <summary>
        /// The role is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The handler is a publisher.
        /// </summary>
        Publisher = 1,

        /// <summary>
        /// The handler is a subscriber.
        /// </summary>
        Subscriber = 2
    }
}