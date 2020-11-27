// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command.Configuration
{
    /// <summary>
    /// Represents a command that retrieves and deserializes a configuration value.
    /// </summary>
    [DataContract]
    public sealed class GetConfigurationValueCommand : GetConfigurationObjectCommand<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfigurationObjectCommand{TResult}" /> class.
        /// </summary>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal GetConfigurationValueCommand(String key)
            : base(key, GetConfigurationObjectCommandTarget.Value)
        {
            return;
        }

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="Boolean" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Boolean" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static Boolean ProcessBoolean(ICommandMediator mediator, String key) => Boolean.TryParse(ProcessString(mediator, key), out var result) ? result : default;

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="DateTime" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="DateTime" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static DateTime ProcessDateTime(ICommandMediator mediator, String key) => DateTime.TryParse(ProcessString(mediator, key), out var result) ? result : default;

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="Decimal" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Decimal" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static Decimal ProcessDecimal(ICommandMediator mediator, String key) => Decimal.TryParse(ProcessString(mediator, key), out var result) ? result : default;

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="Double" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Double" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static Double ProcessDouble(ICommandMediator mediator, String key) => Double.TryParse(ProcessString(mediator, key), out var result) ? result : default;

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="Guid" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Guid" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static Guid ProcessGuid(ICommandMediator mediator, String key) => Guid.TryParse(ProcessString(mediator, key), out var result) ? result : default;

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="Int16" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Int16" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static Int16 ProcessInt16(ICommandMediator mediator, String key) => Int16.TryParse(ProcessString(mediator, key), out var result) ? result : default;

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="Int32" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Int32" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static Int32 ProcessInt32(ICommandMediator mediator, String key) => Int32.TryParse(ProcessString(mediator, key), out var result) ? result : default;

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="Int64" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Int64" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static Int64 ProcessInt64(ICommandMediator mediator, String key) => Int64.TryParse(ProcessString(mediator, key), out var result) ? result : default;

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="Single" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Single" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static Single ProcessSingle(ICommandMediator mediator, String key) => Single.TryParse(ProcessString(mediator, key), out var result) ? result : default;

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="String" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="String" /> instance, or <see langword="null" /> if the value was not resolved.
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
        public static String ProcessString(ICommandMediator mediator, String key) => mediator.RejectIf().IsNull(nameof(mediator)).TargetArgument.Process(new GetConfigurationValueCommand(key));

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="TimeSpan" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="TimeSpan" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static TimeSpan ProcessTimeSpan(ICommandMediator mediator, String key) => TimeSpan.TryParse(ProcessString(mediator, key), out var result) ? result : default;

        /// <summary>
        /// Creates and processes a command that retrieves and deserializes a <see cref="Uri" /> configuration value.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process the command.
        /// </param>
        /// <param name="key">
        /// A textual key for the configuration value to retrieve.
        /// </param>
        /// <returns>
        /// The resulting <see cref="Uri" /> instance, or <see langword="default" /> if the value was not resolved.
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
        public static Uri ProcessUri(ICommandMediator mediator, String key) => Uri.TryCreate(ProcessString(mediator, key), UriKind.RelativeOrAbsolute, out var result) ? result : default;
    }
}