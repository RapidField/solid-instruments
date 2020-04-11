// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Specifies a message type, entity type, interval and label for a regularly-transmitted heartbeat message.
    /// </summary>
    public interface IHeartbeatScheduleItem : IComparable<IHeartbeatScheduleItem>, IEquatable<IHeartbeatScheduleItem>
    {
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
        public Task TransmitHeartbeatMessageAsync(IMessageTransmittingFacade messageTransmittingFacade);

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
        public Type MessageType
        {
            get;
        }
    }
}