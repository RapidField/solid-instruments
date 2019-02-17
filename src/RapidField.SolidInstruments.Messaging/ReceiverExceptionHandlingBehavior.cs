// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Messaging.EventMessages;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Defines the behavior of an <see cref="IMessageSubscriptionFacade" /> when handling an exception that is raised by a receiver.
    /// </summary>
    public enum ReceiverExceptionHandlingBehavior : Int32
    {
        /// <summary>
        /// The behavior is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The facade attempts to publish an <see cref="ExceptionRaisedMessage" />.
        /// </summary>
        PublishExceptionRaisedMessage = 1,

        /// <summary>
        /// The handler re-raises the exception for handling at the application level.
        /// </summary>
        Propagate = 2,

        /// <summary>
        /// The handler ignores and suppresses the exception.
        /// </summary>
        Suppress = 3
    }
}