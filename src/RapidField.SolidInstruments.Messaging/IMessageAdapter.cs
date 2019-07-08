// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Serialization;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates conversion of general-format messages to and from implementation-specific messages.
    /// </summary>
    /// <typeparam name="TAdaptedMessage">
    /// The type of the implementation-specific message.
    /// </typeparam>
    public interface IMessageAdapter<TAdaptedMessage>
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Converts the specified general-format message to an implementation-specific message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="message">
        /// A general-format message to convert.
        /// </param>
        /// <returns>
        /// The implementation-specific message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        TAdaptedMessage ConvertForward<TMessage>(TMessage message)
            where TMessage : class, IMessageBase;

        /// <summary>
        /// Converts the specified implementation-specific message to a general-format message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="message">
        /// An implementation-specific message to convert.
        /// </param>
        /// <returns>
        /// The general-format message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        TMessage ConvertReverse<TMessage>(TAdaptedMessage message)
            where TMessage : class, IMessageBase;

        /// <summary>
        /// Gets the type of implementation-specific adapted messages.
        /// </summary>
        Type AdaptedMessageType
        {
            get;
        }

        /// <summary>
        /// Gets the format that is used to serialize and deserialize messages.
        /// </summary>
        SerializationFormat MessageSerializationFormat
        {
            get;
        }
    }
}