// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents information about the result of a message processing attempt by a listener.
    /// </summary>
    [DataContract]
    public sealed class MessageProcessingAttemptResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingAttemptResult" /> class.
        /// </summary>
        public MessageProcessingAttemptResult()
            : this(TimeStamp.Current)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingAttemptResult" /> class.
        /// </summary>
        /// <param name="attemptEndTimeStamp">
        /// The date and time when the associated attempt ended, successfully or otherwise. The default value is
        /// <see cref="TimeStamp.Current" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="attemptEndTimeStamp" /> is equal to the default <see cref="DateTime" /> value.
        /// </exception>
        public MessageProcessingAttemptResult(DateTime attemptEndTimeStamp)
            : this(attemptEndTimeStamp, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingAttemptResult" /> class.
        /// </summary>
        /// <param name="attemptEndTimeStamp">
        /// The date and time when the associated attempt ended, successfully or otherwise. The default value is
        /// <see cref="TimeStamp.Current" />.
        /// </param>
        /// <param name="attemptStartTimeStamp">
        /// The date and time when the associated attempt began, or <see langword="null" /> if the start date and time were not
        /// recorded. The default value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="attemptEndTimeStamp" /> is equal to the default <see cref="DateTime" /> value.
        /// </exception>
        public MessageProcessingAttemptResult(DateTime attemptEndTimeStamp, DateTime? attemptStartTimeStamp)
            : this(attemptEndTimeStamp, attemptStartTimeStamp, exceptionStackTrace: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingAttemptResult" /> class.
        /// </summary>
        /// <param name="attemptEndTimeStamp">
        /// The date and time when the associated attempt ended, successfully or otherwise. The default value is
        /// <see cref="TimeStamp.Current" />.
        /// </param>
        /// <param name="attemptStartTimeStamp">
        /// The date and time when the associated attempt began, or <see langword="null" /> if the start date and time were not
        /// recorded. The default value is <see langword="null" />.
        /// </param>
        /// <param name="raisedException">
        /// An exception that was raised during the associated attempt, or <see langword="null" /> if the attempt was successful.
        /// The default value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="attemptEndTimeStamp" /> is equal to the default <see cref="DateTime" /> value.
        /// </exception>
        public MessageProcessingAttemptResult(DateTime attemptEndTimeStamp, DateTime? attemptStartTimeStamp, Exception raisedException)
            : this(attemptEndTimeStamp, attemptStartTimeStamp, raisedException?.StackTrace)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingAttemptResult" /> class.
        /// </summary>
        /// <param name="attemptEndTimeStamp">
        /// The date and time when the associated attempt ended, successfully or otherwise. The default value is
        /// <see cref="TimeStamp.Current" />.
        /// </param>
        /// <param name="attemptStartTimeStamp">
        /// The date and time when the associated attempt began, or <see langword="null" /> if the start date and time were not
        /// recorded. The default value is <see langword="null" />.
        /// </param>
        /// <param name="exceptionStackTrace">
        /// The stack trace for an exception that was raised during processing, or <see langword="null" /> if the associated attempt
        /// was successful. The default value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="attemptEndTimeStamp" /> is equal to the default <see cref="DateTime" /> value.
        /// </exception>
        public MessageProcessingAttemptResult(DateTime attemptEndTimeStamp, DateTime? attemptStartTimeStamp, String exceptionStackTrace)
        {
            AttemptEndTimeStamp = attemptEndTimeStamp.RejectIf().IsEqualToValue(default, nameof(attemptEndTimeStamp));
            AttemptStartTimeStamp = attemptStartTimeStamp;
            ExceptionStackTrace = exceptionStackTrace.IsNullOrEmpty() ? null : exceptionStackTrace;
        }

        /// <summary>
        /// Converts the value of the current <see cref="MessageProcessingAttemptResult" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="MessageProcessingAttemptResult" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(WasSuccessful)}\": {WasSuccessful.ToSerializedString()}, \"{nameof(AttemptEndTimeStamp)}\": \"{AttemptEndTimeStamp.ToSerializedString()}\" }}";

        /// <summary>
        /// Gets or sets the date and time when the associated attempt ended, successfully or otherwise.
        /// </summary>
        [DataMember]
        public DateTime AttemptEndTimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date and time when the associated attempt began, or <see langword="null" /> if the start date and time
        /// were not recorded.
        /// </summary>
        [DataMember]
        public DateTime? AttemptStartTimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stack trace for an exception that was raised during processing, or <see langword="null" /> if the
        /// associated attempt was successful.
        /// </summary>
        [DataMember]
        public String ExceptionStackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the length of time between the start and end of the associated attempt, or <see langword="null" /> if the duration
        /// is unknown.
        /// </summary>
        [IgnoreDataMember]
        public TimeSpan? ProcessingDuration => AttemptStartTimeStamp.HasValue ? (TimeSpan?)(AttemptEndTimeStamp - AttemptStartTimeStamp.Value) : null;

        /// <summary>
        /// Gets a value indicating whether or not the message was successfully processed during the associated attempt.
        /// </summary>
        [IgnoreDataMember]
        public Boolean WasSuccessful => ExceptionStackTrace.IsNullOrEmpty();
    }
}