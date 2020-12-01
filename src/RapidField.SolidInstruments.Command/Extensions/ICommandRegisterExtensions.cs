// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command.Configuration;
using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Command.Extensions
{
    /// <summary>
    /// Extends the <see cref="ICommandRegister" /> interface with general purpose command creation methods.
    /// </summary>
    public static class ICommandRegisterExtensions
    {
        /// <summary>
        /// Creates a new <see cref="TextualCommand" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="value">
        /// The textual command value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        /// <returns>
        /// A new <see cref="TextualCommand" />.
        /// </returns>
        public static TextualCommand FromText(this ICommandRegister target, String value) => new(value);

        /// <summary>
        /// Creates a new <see cref="TextualCommand" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
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
        /// A new <see cref="TextualCommand" />.
        /// </returns>
        public static TextualCommand FromText(this ICommandRegister target, String value, IEnumerable<String> labels) => new(value, labels);

        /// <summary>
        /// Creates a new <see cref="GetConfigurationSectionCommand" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration section to retrieve.
        /// </param>
        /// <returns>
        /// A new <see cref="GetConfigurationSectionCommand" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        public static GetConfigurationSectionCommand GetConfigurationSection(this ICommandRegister target, String key) => new(key);

        /// <summary>
        /// Creates a new <see cref="GetConfigurationValueCommand" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration section to retrieve.
        /// </param>
        /// <returns>
        /// A new <see cref="GetConfigurationValueCommand" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        public static GetConfigurationValueCommand GetConfigurationValue(this ICommandRegister target, String key) => new(key);

        /// <summary>
        /// Creates a new <see cref="GetConnectionStringCommand" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="name">
        /// A textual key for the connection string to retrieve.
        /// </param>
        /// <returns>
        /// A new <see cref="GetConnectionStringCommand" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        public static GetConnectionStringCommand GetConnectionString(this ICommandRegister target, String name) => new(name);
    }
}