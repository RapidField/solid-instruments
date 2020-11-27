// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command that is described by textual information.
    /// </summary>
    [DataContract]
    public sealed class TextualCommandMessage : CommandMessage<TextualCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextualCommandMessage" /> class.
        /// </summary>
        public TextualCommandMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextualCommandMessage" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        public TextualCommandMessage(TextualCommand commandObject)
            : base(commandObject)
        {
            return;
        }
    }
}