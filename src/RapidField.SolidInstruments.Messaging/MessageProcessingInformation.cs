// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents instructions and contextual information relating to processing for an <see cref="IMessageBase" />.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageProcessingInformation" /> is the default implementation of <see cref="IMessageProcessingInformation" />.
    /// </remarks>
    [DataContract]
    public sealed class MessageProcessingInformation : IMessageProcessingInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingInformation" /> class.
        /// </summary>
        public MessageProcessingInformation()
            : this(MessageListeningFailurePolicy.Default)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingInformation" /> class.
        /// </summary>
        /// <param name="failurePolicy">
        /// Instructions that guide failure behavior for the listener. The default value is
        /// <see cref="MessageListeningFailurePolicy.Default" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="failurePolicy" /> is <see langword="null" />.
        /// </exception>
        public MessageProcessingInformation(MessageListeningFailurePolicy failurePolicy)
        {
            AttemptResults = new List<MessageProcessingAttemptResult>();
            FailurePolicy = failurePolicy.RejectIf().IsNull(nameof(failurePolicy));
        }

        /// <summary>
        /// Converts the value of the current <see cref="MessageProcessingInformation" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="MessageProcessingInformation" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(IsSuccessfullyProcessed)}\": {IsSuccessfullyProcessed.ToSerializedString()}, \"{nameof(AttemptCount)}\": {AttemptCount} }}";

        /// <summary>
        /// Gets the number of times that processing has been attempted for the associated message, or zero if processing has not
        /// yet been attempted.
        /// </summary>
        [IgnoreDataMember]
        public Int32 AttemptCount => AttemptResults.Count;

        /// <summary>
        /// Gets or sets an ordered collection of processing attempt results for the associated message, or an empty collection if
        /// processing has not yet been attempted.
        /// </summary>
        [DataMember]
        public List<MessageProcessingAttemptResult> AttemptResults
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets instructions that guide failure behavior for the listener.
        /// </summary>
        [DataMember]
        public MessageListeningFailurePolicy FailurePolicy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether or not the associated message has been processed successfully.
        /// </summary>
        [IgnoreDataMember]
        public Boolean IsSuccessfullyProcessed => AttemptResults.Any() ? AttemptResults.Last().WasSuccessful : false;
    }
}