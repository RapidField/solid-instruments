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
        /// Gets or sets a unique identifier for the message.
        /// </summary>
        public Guid Identifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets instructions and contextual information relating to processing for the current <see cref="IMessageBase" />.
        /// </summary>
        public IMessageProcessingInformation ProcessingInformation
        {
            get;
            set;
        }
    }
}