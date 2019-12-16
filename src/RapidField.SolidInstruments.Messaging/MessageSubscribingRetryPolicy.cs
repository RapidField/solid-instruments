﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents instructions that guide retry behavior for the subscriber when processing an <see cref="IMessageBase" />.
    /// </summary>
    [DataContract]
    public sealed class MessageSubscribingRetryPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscribingRetryPolicy" /> class.
        /// </summary>
        public MessageSubscribingRetryPolicy()
            : this(DefaultRetryCount)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscribingRetryPolicy" /> class.
        /// </summary>
        /// <param name="retryCount">
        /// The number of times that subscribers should try to process a failed message before employing secondary failure behavior,
        /// or zero to employ secondary behavior upon first failure. The default value is three.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="retryCount" /> is less than zero.
        /// </exception>
        public MessageSubscribingRetryPolicy(Int32 retryCount)
            : this(retryCount, DefaultBaseDelayDurationInSeconds)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscribingRetryPolicy" /> class.
        /// </summary>
        /// <param name="retryCount">
        /// The number of times that subscribers should try to process a failed message before employing secondary failure behavior,
        /// or zero to employ secondary behavior upon first failure. The default value is three.
        /// </param>
        /// <param name="baseDelayDurationInSeconds">
        /// The number of seconds to wait between retries, or the duration, in seconds, from which to scale non-linearly. The
        /// default value is three.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="baseDelayDurationInSeconds" /> is less than zero -or- <paramref name="retryCount" /> is less than zero.
        /// </exception>
        public MessageSubscribingRetryPolicy(Int32 retryCount, Int32 baseDelayDurationInSeconds)
            : this(retryCount, baseDelayDurationInSeconds, DefaultDurationScale)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscribingRetryPolicy" /> class.
        /// </summary>
        /// <param name="retryCount">
        /// The number of times that subscribers should try to process a failed message before employing secondary failure behavior,
        /// or zero to employ secondary behavior upon first failure. The default value is three.
        /// </param>
        /// <param name="baseDelayDurationInSeconds">
        /// The number of seconds to wait between retries, or the duration, in seconds, from which to scale non-linearly. The
        /// default value is three.
        /// </param>
        /// <param name="durationScale">
        /// The retry duration scaling behavior employed by subscribers in response to message processing failure. The default value
        /// is <see cref="MessageSubscribingRetryDurationScale.Fibonacci" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="baseDelayDurationInSeconds" /> is less than zero -or- <paramref name="durationScale" /> is equal to
        /// <see cref="MessageSubscribingRetryDurationScale.Unspecified" /> -or- <paramref name="retryCount" /> is less than zero.
        /// </exception>
        public MessageSubscribingRetryPolicy(Int32 retryCount, Int32 baseDelayDurationInSeconds, MessageSubscribingRetryDurationScale durationScale)
        {
            BaseDelayDurationInSeconds = baseDelayDurationInSeconds.RejectIf().IsLessThan(0, nameof(baseDelayDurationInSeconds));
            DurationScale = durationScale.RejectIf().IsEqualToValue(MessageSubscribingRetryDurationScale.Unspecified, nameof(durationScale));
            RetryCount = retryCount.RejectIf().IsLessThan(0, nameof(retryCount));
        }

        /// <summary>
        /// Gets or sets the number of seconds to wait between retries, or the duration, in seconds, from which to scale
        /// non-linearly.
        /// </summary>
        [DataMember]
        public Int32 BaseDelayDurationInSeconds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the retry duration scaling behavior employed by subscribers in response to message processing failure.
        /// </summary>
        [DataMember]
        public MessageSubscribingRetryDurationScale DurationScale
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of times that subscribers should try to process a failed message before employing secondary
        /// failure behavior, or zero to employ secondary behavior upon first failure.
        /// </summary>
        [DataMember]
        public Int32 RetryCount
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default retry behavior.
        /// </summary>
        public static readonly MessageSubscribingRetryPolicy Default = new MessageSubscribingRetryPolicy();

        /// <summary>
        /// Represents retry behavior that instructs the subscriber not to retry processing for the message.
        /// </summary>
        public static readonly MessageSubscribingRetryPolicy DoNotRetry = new MessageSubscribingRetryPolicy(0);

        /// <summary>
        /// Represents the default number of seconds to wait between retries, or the duration, in seconds, from which to scale
        /// non-linearly.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DefaultBaseDelayDurationInSeconds = 3;

        /// <summary>
        /// Represents the default retry duration scaling behavior employed by subscribers in response to message processing
        /// failure.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const MessageSubscribingRetryDurationScale DefaultDurationScale = MessageSubscribingRetryDurationScale.Fibonacci;

        /// <summary>
        /// Represents the default number of times that subscribers should try to process a failed message before employing
        /// secondary failure behavior, or zero to employ secondary behavior upon first failure.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DefaultRetryCount = 3;
    }
}