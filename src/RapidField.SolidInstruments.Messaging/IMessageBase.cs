// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents a message.
    /// </summary>
    public interface IMessageBase : ICommandBase
    {
        /// <summary>
        /// Gets or sets a unique identifier that is assigned to related messages.
        /// </summary>
        Guid CorrelationIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a unique identifier for the message.
        /// </summary>
        Guid Identifier
        {
            get;
            set;
        }
    }
}