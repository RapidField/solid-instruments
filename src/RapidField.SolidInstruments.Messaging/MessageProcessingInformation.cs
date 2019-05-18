// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents instructions and contextual information relating to processing for an <see cref="IMessageBase" />.
    /// </summary>
    [DataContract]
    public sealed class MessageProcessingInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingInformation" /> class.
        /// </summary>
        public MessageProcessingInformation()
            : this(MessageSubscribingFailurePolicy.Default)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingInformation" /> class.
        /// </summary>
        /// <param name="failurePolicy">
        /// Instructions that guide failure behavior for the subscriber. The default value is
        /// <see cref="MessageSubscribingFailurePolicy.Default" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="failurePolicy" /> is <see langword="null" />.
        /// </exception>
        public MessageProcessingInformation(MessageSubscribingFailurePolicy failurePolicy)
        {
            AttemptResults = new Collection<MessageProcessingAttemptResult>();
            FailurePolicy = failurePolicy.RejectIf().IsNull(nameof(failurePolicy));
        }

        /// <summary>
        /// Gets the number of times that processing has been attempted for the associated message, or zero if processing has not yet
        /// been attempted.
        /// </summary>
        [IgnoreDataMember]
        public Int32 AttemptCount => AttemptResults.Count();

        /// <summary>
        /// Gets an ordered collection of processing attempt results for the associated message, or an empty collection if processing
        /// has not yet been attempted.
        /// </summary>
        [DataMember]
        public Collection<MessageProcessingAttemptResult> AttemptResults
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets instructions that guide failure behavior for the subscriber.
        /// </summary>
        [DataMember]
        public MessageSubscribingFailurePolicy FailurePolicy
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