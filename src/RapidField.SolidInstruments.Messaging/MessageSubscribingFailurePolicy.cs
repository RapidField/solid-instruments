// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents instructions that guide failure behavior for the subscriber when processing an <see cref="IMessageBase" />.
    /// </summary>
    [DataContract]
    public sealed class MessageSubscribingFailurePolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscribingFailurePolicy" /> class.
        /// </summary>
        public MessageSubscribingFailurePolicy()
            : this(MessageSubscribingRetryPolicy.Default)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscribingFailurePolicy" /> class.
        /// </summary>
        /// <param name="retryPolicy">
        /// Information that defines retry behavior that is employed before employing secondary failure behavior. The default value
        /// is <see cref="MessageSubscribingRetryPolicy.Default" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="retryPolicy" /> is <see langword="null" />.
        /// </exception>
        public MessageSubscribingFailurePolicy(MessageSubscribingRetryPolicy retryPolicy)
            : this(retryPolicy, DefaultSecondaryFailureBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscribingFailurePolicy" /> class.
        /// </summary>
        /// <param name="retryPolicy">
        /// Information that defines retry behavior that is employed before employing secondary failure behavior. The default value
        /// is <see cref="MessageSubscribingRetryPolicy.Default" />.
        /// </param>
        /// <param name="secondaryFailureBehavior">
        /// The failure behavior that is employed after the retry policy is exhausted. The default value is
        /// <see cref="MessageSubscribingSecondaryFailureBehavior.RouteToDeadLetterQueue" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="retryPolicy" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="secondaryFailureBehavior" /> is equal to
        /// <see cref="MessageSubscribingSecondaryFailureBehavior.Unspecified" />.
        /// </exception>
        public MessageSubscribingFailurePolicy(MessageSubscribingRetryPolicy retryPolicy, MessageSubscribingSecondaryFailureBehavior secondaryFailureBehavior)
            : this(retryPolicy, secondaryFailureBehavior, DefaultPublishExceptionRaisedMessage)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscribingFailurePolicy" /> class.
        /// </summary>
        /// <param name="retryPolicy">
        /// Information that defines retry behavior that is employed before employing secondary failure behavior. The default value
        /// is <see cref="MessageSubscribingRetryPolicy.Default" />.
        /// </param>
        /// <param name="secondaryFailureBehavior">
        /// The failure behavior that is employed after the retry policy is exhausted. The default value is
        /// <see cref="MessageSubscribingSecondaryFailureBehavior.RouteToDeadLetterQueue" />.
        /// </param>
        /// <param name="publishExceptionRaisedMessage">
        /// A value indicating whether or not subscribers should publish <see cref="ExceptionRaisedMessage" /> instances when an
        /// exception is raised during message processing. The default value is <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="retryPolicy" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="secondaryFailureBehavior" /> is equal to
        /// <see cref="MessageSubscribingSecondaryFailureBehavior.Unspecified" />.
        /// </exception>
        public MessageSubscribingFailurePolicy(MessageSubscribingRetryPolicy retryPolicy, MessageSubscribingSecondaryFailureBehavior secondaryFailureBehavior, Boolean publishExceptionRaisedMessage)
        {
            PublishExceptionRaisedMessage = publishExceptionRaisedMessage;
            RetryPolicy = retryPolicy.RejectIf().IsNull(nameof(retryPolicy));
            SecondaryFailureBehavior = secondaryFailureBehavior.RejectIf().IsEqualToValue(MessageSubscribingSecondaryFailureBehavior.Unspecified, nameof(secondaryFailureBehavior));
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not subscribers should publish <see cref="ExceptionRaisedMessage" />
        /// instances when an exception is raised during message processing.
        /// </summary>
        [DataMember]
        public Boolean PublishExceptionRaisedMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets information that defines retry behavior that is employed before employing secondary failure behavior.
        /// </summary>
        [DataMember]
        public MessageSubscribingRetryPolicy RetryPolicy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the failure behavior that is employed after the retry policy is exhausted.
        /// </summary>
        [DataMember]
        public MessageSubscribingSecondaryFailureBehavior SecondaryFailureBehavior
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default failure behavior.
        /// </summary>
        public static readonly MessageSubscribingFailurePolicy Default = new MessageSubscribingFailurePolicy();

        /// <summary>
        /// Represents the default value indicating whether or not subscribers should publish <see cref="ExceptionRaisedMessage" />
        /// instances when an exception is raised during message processing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultPublishExceptionRaisedMessage = false;

        /// <summary>
        /// Represents the default failure behavior that is employed after the retry policy is exhausted.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const MessageSubscribingSecondaryFailureBehavior DefaultSecondaryFailureBehavior = MessageSubscribingSecondaryFailureBehavior.RouteToDeadLetterQueue;
    }
}