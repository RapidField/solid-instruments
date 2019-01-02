// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents a response message.
    /// </summary>
    /// <remarks>
    /// <see cref="ResponseMessage" /> is the default implementation of <see cref="IResponseMessage" />.
    /// </remarks>
    [DataContract]
    public abstract class ResponseMessage : Message, IResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage" /> class.
        /// </summary>
        protected ResponseMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage" /> class.
        /// </summary>
        /// <param name="requestMessageIdentifier">
        /// The identifier for the associated request message.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="requestMessageIdentifier" /> is equal to <see cref="Guid.Empty" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected ResponseMessage(Guid requestMessageIdentifier, Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            RequestMessageIdentifier = requestMessageIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(requestMessageIdentifier));
        }

        /// <summary>
        /// Gets or sets the identifier for the associated request message.
        /// </summary>
        [DataMember]
        public Guid RequestMessageIdentifier
        {
            get;
            set;
        }
    }
}