// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents a message that requests a response of a specified type.
    /// </summary>
    /// <remarks>
    /// <see cref="RequestMessage{TResponseMessage}" /> is the default implementation of
    /// <see cref="IRequestMessage{TResponseMessage}" />.
    /// </remarks>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is associated with the request.
    /// </typeparam>
    [DataContract]
    public abstract class RequestMessage<TResponseMessage> : Message<TResponseMessage>, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessage{TResponseMessage}" /> class.
        /// </summary>
        protected RequestMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessage{TResponseMessage}" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected RequestMessage(Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            return;
        }
    }
}