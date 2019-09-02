// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents an operation performed against an <see cref="IDurableMessageQueue" />.
    /// </summary>
    public interface IDurableMessageQueueOperation
    {
        /// <summary>
        /// Gets the date and time when the operation was recorded.
        /// </summary>
        DateTime TimeStamp
        {
            get;
        }
    }
}