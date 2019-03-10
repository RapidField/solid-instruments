// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
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
            : this(DefaultFailurePolicy)
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
            FailurePolicy = failurePolicy.RejectIf().IsNull(nameof(failurePolicy));
            IsSuccessfullyProcessed = false;
            ProcessAttemptCount = 0;
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
        /// Gets or sets a value indicating whether or not the associated message has been processed successfully.
        /// </summary>
        [DataMember]
        public Boolean IsSuccessfullyProcessed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of times that processing has been attempted for the associated message, or zero if processing has
        /// not yet been attempted.
        /// </summary>
        [DataMember]
        public Int32 ProcessAttemptCount
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default instructions that guide failure behavior for the subscriber.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly MessageSubscribingFailurePolicy DefaultFailurePolicy = MessageSubscribingFailurePolicy.Default;
    }
}