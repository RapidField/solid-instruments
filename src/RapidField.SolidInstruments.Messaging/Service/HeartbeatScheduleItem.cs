// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Specifies a message type, entity type, interval and label for a regularly-transmitted heartbeat message.
    /// </summary>
    /// <remarks>
    /// <see cref="HeartbeatScheduleItem{TMessage}" /> is the default implementation of <see cref="IHeartbeatScheduleItem" />.
    /// </remarks>
    /// <typeparam name="TMessage">
    /// The type of the associated heartbeat message.
    /// </typeparam>
    public sealed class HeartbeatScheduleItem<TMessage> : IHeartbeatScheduleItem
        where TMessage : HeartbeatMessage, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatScheduleItem{TMessage}" /> class.
        /// </summary>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is transmitted.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero.
        /// </exception>
        [DebuggerHidden]
        internal HeartbeatScheduleItem(Int32 intervalInSeconds)
            : this(intervalInSeconds, DefaultEntityType)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatScheduleItem{TMessage}" /> class.
        /// </summary>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is transmitted.
        /// </param>
        /// <param name="entityType">
        /// The messaging entity type that is used when transmitting the message. The default value is
        /// <see cref="MessagingEntityType.Topic" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero -or- <paramref name="entityType" /> is equal to
        /// <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal HeartbeatScheduleItem(Int32 intervalInSeconds, MessagingEntityType entityType)
            : this(intervalInSeconds, entityType, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatScheduleItem{TMessage}" /> class.
        /// </summary>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is transmitted.
        /// </param>
        /// <param name="entityType">
        /// The messaging entity type that is used when transmitting the message. The default value is
        /// <see cref="MessagingEntityType.Topic" />.
        /// </param>
        /// <param name="label">
        /// The label, if any, that is associated with the message. This argument can be null.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero -or- <paramref name="entityType" /> is equal to
        /// <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal HeartbeatScheduleItem(Int32 intervalInSeconds, MessagingEntityType entityType, String label)
            : base()
        {
            EntityType = entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType));
            IntervalInSeconds = intervalInSeconds.RejectIf().IsLessThanOrEqualTo(0, nameof(intervalInSeconds));
            Label = label;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="IHeartbeatScheduleItem" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(HeartbeatScheduleItem<TMessage> a, IHeartbeatScheduleItem b) => (a == b) is false;

        /// <summary>
        /// Determines whether or not a specified <see cref="IHeartbeatScheduleItem" /> instance is less than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(HeartbeatScheduleItem<TMessage> a, IHeartbeatScheduleItem b) => a is null ? b is Object : a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="IHeartbeatScheduleItem" /> instance is less than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(HeartbeatScheduleItem<TMessage> a, IHeartbeatScheduleItem b) => a is null || a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="IHeartbeatScheduleItem" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(HeartbeatScheduleItem<TMessage> a, IHeartbeatScheduleItem b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether or not a specified <see cref="IHeartbeatScheduleItem" /> instance is greater than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(HeartbeatScheduleItem<TMessage> a, IHeartbeatScheduleItem b) => a is Object && a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="IHeartbeatScheduleItem" /> instance is greater than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IHeartbeatScheduleItem" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(HeartbeatScheduleItem<TMessage> a, IHeartbeatScheduleItem b) => a is null ? b is null : a.CompareTo(b) >= 0;

        /// <summary>
        /// Compares the current <see cref="IHeartbeatScheduleItem" /> to the specified object and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IHeartbeatScheduleItem" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(IHeartbeatScheduleItem other)
        {
            var messageTypeComparisonResult = MessageType.FullName.CompareTo(other.MessageType.FullName);

            if (messageTypeComparisonResult != 0)
            {
                return messageTypeComparisonResult;
            }

            var intervalInSecondsComparisonResult = IntervalInSeconds.CompareTo(other.IntervalInSeconds);

            if (intervalInSecondsComparisonResult != 0)
            {
                return intervalInSecondsComparisonResult;
            }

            var entityTypeComparisonResult = EntityType.CompareTo((Int32)other.EntityType);

            if (entityTypeComparisonResult != 0 || Label is null)
            {
                return entityTypeComparisonResult;
            }

            return Label.CompareTo(other.Label);
        }

        /// <summary>
        /// Determines whether or not the current <see cref="HeartbeatScheduleItem{TMessage}" /> is equal to the specified
        /// <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is null)
            {
                return false;
            }
            else if (obj is IHeartbeatScheduleItem item)
            {
                return Equals(item);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="IHeartbeatScheduleItem" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IHeartbeatScheduleItem" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(IHeartbeatScheduleItem other) => MessageType == other.MessageType && IntervalInSeconds == other.IntervalInSeconds && EntityType == other.EntityType && Label == other.Label;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => IntervalInSeconds ^ (Int32)EntityType ^ (Label is null ? 0 : Label.GetHashCode()) ^ MessageType.FullName.GetHashCode();

        /// <summary>
        /// Converts the value of the current <see cref="HeartbeatScheduleItem{TMessage}" /> to its equivalent string
        /// representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="HeartbeatScheduleItem{TMessage}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(EntityType)}\": \"{EntityType}\", {nameof(MessageType)}\": \"{MessageType.FullName}\", {nameof(IntervalInSeconds)}\": {IntervalInSeconds}, \"{nameof(Label)}\": \"{Label}\" }}";

        /// <summary>
        /// Asynchronously transmits a single heartbeat message with characteristics defined by the current
        /// <see cref="IHeartbeatScheduleItem" />.
        /// </summary>
        /// <param name="messageTransmittingFacade">
        /// An appliance that facilitates message transmitting operations.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageTransmittingFacade" /> is null.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit the heartbeat message.
        /// </exception>
        public Task TransmitHeartbeatMessageAsync(IMessageTransmittingFacade messageTransmittingFacade)
        {
            messageTransmittingFacade = messageTransmittingFacade.RejectIf().IsNull(nameof(messageTransmittingFacade)).TargetArgument;

            try
            {
                var message = new TMessage()
                {
                    IntervalInSeconds = IntervalInSeconds,
                    Label = Label
                };

                return EntityType switch
                {
                    MessagingEntityType.Queue => messageTransmittingFacade.TransmitToQueueAsync(message),
                    MessagingEntityType.Topic => messageTransmittingFacade.TransmitToTopicAsync(message),
                    _ => Task.FromException(new UnsupportedSpecificationException($"The specified messaging entity type, {EntityType}, is not supported."))
                };
            }
            catch (MessageTransmissionException exception)
            {
                return Task.FromException(exception);
            }
            catch (Exception exception)
            {
                return Task.FromException(new MessageTransmissionException(typeof(HeartbeatMessage), exception));
            }
        }

        /// <summary>
        /// Gets the messaging entity type that is used when transmitting the message.
        /// </summary>
        public MessagingEntityType EntityType
        {
            get;
        }

        /// <summary>
        /// Gets the regular interval, in seconds, at which the message is transmitted.
        /// </summary>
        public Int32 IntervalInSeconds
        {
            get;
        }

        /// <summary>
        /// Gets the label, if any, that is associated with the message.
        /// </summary>
        public String Label
        {
            get;
        }

        /// <summary>
        /// Gets the type of the associated heartbeat message.
        /// </summary>
        public Type MessageType => typeof(TMessage);

        /// <summary>
        /// Represents the default messaging entity type that is used when transmitting messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const MessagingEntityType DefaultEntityType = MessagingEntityType.Topic;
    }
}