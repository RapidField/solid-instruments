// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents a message that serves as a response to an <see cref="IMessage{TResponseMessage}" />.
    /// </summary>
    public interface IResponseMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the identifier for the associated request message.
        /// </summary>
        Guid RequestMessageIdentifier
        {
            get;
            set;
        }
    }
}