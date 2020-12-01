// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command.Extensions;
using RapidField.SolidInstruments.Messaging.CommandMessages;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Messaging.Extensions
{
    /// <summary>
    /// Extends the <see cref="ICommandMessageRegister" /> interface with general purpose message creation methods.
    /// </summary>
    public static class ICommandMessageRegisterExtensions
    {
        /// <summary>
        /// Creates a new <see cref="TextualCommandMessage" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICommandMessageRegister" />.
        /// </param>
        /// <param name="value">
        /// The textual command value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        /// <returns>
        /// A new <see cref="TextualCommandMessage" />.
        /// </returns>
        public static TextualCommandMessage FromText(this ICommandMessageRegister target, String value) => new(target.Commands.FromText(value));

        /// <summary>
        /// Creates a new <see cref="TextualCommandMessage" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICommandMessageRegister" />.
        /// </param>
        /// <param name="value">
        /// The textual command value.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <returns>
        /// A new <see cref="TextualCommandMessage" />.
        /// </returns>
        public static TextualCommandMessage FromText(this ICommandMessageRegister target, String value, IEnumerable<String> labels) => new(target.Commands.FromText(value, labels));
    }
}