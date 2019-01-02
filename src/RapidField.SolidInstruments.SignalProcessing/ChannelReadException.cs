// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents an exception that is raised while attempting to perform a read operation against an <see cref="IChannel{T}" />.
    /// </summary>
    public class ChannelReadException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelReadException" /> class.
        /// </summary>
        public ChannelReadException()
            : this(message: null, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelReadException" /> class.
        /// </summary>
        /// <param name="channel">
        /// The channel that could not be read.
        /// </param>
        public ChannelReadException(IChannel channel)
            : this($"The channel, {channel.Name}, was unavailable.")
        {
            ChannelName = channel.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelReadException" /> class.
        /// </summary>
        /// <param name="channel">
        /// The channel that could not be read.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ChannelReadException(IChannel channel, Exception innerException)
            : this($"An exception was raised while attempting to read the channel {channel.Name}.", innerException)
        {
            ChannelName = channel.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelReadException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ChannelReadException(String message)
            : this(message, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelReadException" /> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ChannelReadException(Exception innerException)
            : this(message: null, innerException: innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelReadException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ChannelReadException(String message, Exception innerException)
            : base(message.IsNullOrEmpty() ? "An exception was raised while attempting to read a channel." : message, innerException)
        {
            ChannelName = null;
        }

        /// <summary>
        /// Gets or sets the name of the channel that could not be read; or <see langword="null" /> if the name of the channel is
        /// unknown.
        /// </summary>
        public String ChannelName
        {
            get;
            set;
        }
    }
}