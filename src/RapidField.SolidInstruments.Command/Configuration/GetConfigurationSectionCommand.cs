// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command.Configuration
{
    /// <summary>
    /// Represents a command that retrieves and deserializes a configuration section.
    /// </summary>
    [DataContract]
    public sealed class GetConfigurationSectionCommand : GetConfigurationObjectCommand<IConfigurationSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfigurationSectionCommand" /> class.
        /// </summary>
        /// <param name="key">
        /// A textual key for the configuration section to retrieve.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal GetConfigurationSectionCommand(String key)
            : base(key, GetConfigurationObjectCommandTarget.Section)
        {
            return;
        }

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a configuration section.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the configuration section.
        /// </typeparam>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration section to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <typeparamref name="T" /> instance, or <see langword="null" /> if the section was not resolved.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the command.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="mediator" /> is disposed.
        /// </exception>
        public static T Process<T>(ICommandMediator mediator, String key)
            where T : class => mediator.RejectIf().IsNull(nameof(mediator)).TargetArgument.Process(new GetConfigurationSectionCommand(key)).Get<T>();
    }
}