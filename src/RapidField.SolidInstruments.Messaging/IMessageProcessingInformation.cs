// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents instructions and contextual information relating to processing for an <see cref="IMessageBase" />.
    /// </summary>
    public interface IMessageProcessingInformation
    {
        /// <summary>
        /// Gets the number of times that processing has been attempted for the associated message, or zero if processing has not
        /// yet been attempted.
        /// </summary>
        public Int32 AttemptCount
        {
            get;
        }

        /// <summary>
        /// Gets an ordered collection of processing attempt results for the associated message, or an empty collection if
        /// processing has not yet been attempted.
        /// </summary>
        public ICollection<MessageProcessingAttemptResult> AttemptResults
        {
            get;
        }

        /// <summary>
        /// Gets or sets instructions that guide failure behavior for the listener.
        /// </summary>
        public MessageListeningFailurePolicy FailurePolicy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether or not the associated message has been processed successfully.
        /// </summary>
        public Boolean IsSuccessfullyProcessed
        {
            get;
        }
    }
}