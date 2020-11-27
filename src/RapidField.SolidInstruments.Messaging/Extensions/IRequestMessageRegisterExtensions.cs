// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Messaging.Service;
using System;

namespace RapidField.SolidInstruments.Messaging.Extensions
{
    /// <summary>
    /// Extends the <see cref="IRequestMessageRegister" /> interface with general purpose message creation methods.
    /// </summary>
    public static class IRequestMessageRegisterExtensions
    {
        /// <summary>
        /// Creates a new <see cref="PingRequestMessage" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IRequestMessageRegister" />.
        /// </param>
        /// <returns>
        /// A new <see cref="PingRequestMessage" />.
        /// </returns>
        public static PingRequestMessage Ping(this IRequestMessageRegister target) => target.Ping(Guid.NewGuid());

        /// <summary>
        /// Creates a new <see cref="PingRequestMessage" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IRequestMessageRegister" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <returns>
        /// A new <see cref="PingRequestMessage" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public static PingRequestMessage Ping(this IRequestMessageRegister target, Guid correlationIdentifier) => new PingRequestMessage(correlationIdentifier);
    }
}