// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents instructions that guide failure behavior for the listener when processing an <see cref="IMessageBase" />.
    /// </summary>
    [DataContract]
    public sealed class MessageListeningFailurePolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListeningFailurePolicy" /> class.
        /// </summary>
        public MessageListeningFailurePolicy()
            : this(MessageListeningRetryPolicy.Default)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListeningFailurePolicy" /> class.
        /// </summary>
        /// <param name="retryPolicy">
        /// Information that defines retry behavior that is employed before employing secondary failure behavior. The default value
        /// is <see cref="MessageListeningRetryPolicy.Default" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="retryPolicy" /> is <see langword="null" />.
        /// </exception>
        public MessageListeningFailurePolicy(MessageListeningRetryPolicy retryPolicy)
            : this(retryPolicy, DefaultSecondaryFailureBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListeningFailurePolicy" /> class.
        /// </summary>
        /// <param name="retryPolicy">
        /// Information that defines retry behavior that is employed before employing secondary failure behavior. The default value
        /// is <see cref="MessageListeningRetryPolicy.Default" />.
        /// </param>
        /// <param name="secondaryFailureBehavior">
        /// The failure behavior that is employed after the retry policy is exhausted. The default value is
        /// <see cref="MessageListeningSecondaryFailureBehavior.RouteToDeadLetterQueue" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="retryPolicy" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="secondaryFailureBehavior" /> is equal to
        /// <see cref="MessageListeningSecondaryFailureBehavior.Unspecified" />.
        /// </exception>
        public MessageListeningFailurePolicy(MessageListeningRetryPolicy retryPolicy, MessageListeningSecondaryFailureBehavior secondaryFailureBehavior)
            : this(retryPolicy, secondaryFailureBehavior, DefaultTransmitExceptionRaisedEventMessage)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListeningFailurePolicy" /> class.
        /// </summary>
        /// <param name="retryPolicy">
        /// Information that defines retry behavior that is employed before employing secondary failure behavior. The default value
        /// is <see cref="MessageListeningRetryPolicy.Default" />.
        /// </param>
        /// <param name="secondaryFailureBehavior">
        /// The failure behavior that is employed after the retry policy is exhausted. The default value is
        /// <see cref="MessageListeningSecondaryFailureBehavior.RouteToDeadLetterQueue" />.
        /// </param>
        /// <param name="transmitExceptionRaisedEventMessage">
        /// A value indicating whether or not listeners should transmit <see cref="ExceptionRaisedEventMessage" /> instances when an
        /// exception is raised during message processing. The default value is <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="retryPolicy" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="secondaryFailureBehavior" /> is equal to
        /// <see cref="MessageListeningSecondaryFailureBehavior.Unspecified" />.
        /// </exception>
        public MessageListeningFailurePolicy(MessageListeningRetryPolicy retryPolicy, MessageListeningSecondaryFailureBehavior secondaryFailureBehavior, Boolean transmitExceptionRaisedEventMessage)
        {
            RetryPolicy = retryPolicy.RejectIf().IsNull(nameof(retryPolicy));
            SecondaryFailureBehavior = secondaryFailureBehavior.RejectIf().IsEqualToValue(MessageListeningSecondaryFailureBehavior.Unspecified, nameof(secondaryFailureBehavior));
            TransmitExceptionRaisedEventMessage = transmitExceptionRaisedEventMessage;
        }

        /// <summary>
        /// Converts the value of the current <see cref="MessageListeningFailurePolicy" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="MessageListeningFailurePolicy" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(TransmitExceptionRaisedEventMessage)}\": {TransmitExceptionRaisedEventMessage.ToSerializedString()}, \"{nameof(SecondaryFailureBehavior)}\": \"{SecondaryFailureBehavior}\" }}";

        /// <summary>
        /// Gets or sets information that defines retry behavior that is employed before employing secondary failure behavior.
        /// </summary>
        [DataMember]
        public MessageListeningRetryPolicy RetryPolicy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the failure behavior that is employed after the retry policy is exhausted.
        /// </summary>
        [DataMember]
        public MessageListeningSecondaryFailureBehavior SecondaryFailureBehavior
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not listeners should transmit <see cref="ExceptionRaisedEventMessage" />
        /// instances when an exception is raised during message processing.
        /// </summary>
        [DataMember]
        public Boolean TransmitExceptionRaisedEventMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default failure behavior.
        /// </summary>
        public static readonly MessageListeningFailurePolicy Default = new MessageListeningFailurePolicy();

        /// <summary>
        /// Represents the default failure behavior that is employed after the retry policy is exhausted.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const MessageListeningSecondaryFailureBehavior DefaultSecondaryFailureBehavior = MessageListeningSecondaryFailureBehavior.RouteToDeadLetterQueue;

        /// <summary>
        /// Represents the default value indicating whether or not listeners should transmit
        /// <see cref="ExceptionRaisedEventMessage" /> instances when an exception is raised during message processing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultTransmitExceptionRaisedEventMessage = false;
    }
}