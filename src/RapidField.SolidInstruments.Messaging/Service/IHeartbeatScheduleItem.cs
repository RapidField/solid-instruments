// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Specifies a message type, entity type, interval and label for a regularly-published heartbeat message.
    /// </summary>
    public interface IHeartbeatScheduleItem : IComparable<IHeartbeatScheduleItem>, IEquatable<IHeartbeatScheduleItem>
    {
        /// <summary>
        /// Asynchronously publishes a single heartbeat message with characteristics defined by the current
        /// <see cref="IHeartbeatScheduleItem" />.
        /// </summary>
        /// <param name="messagePublishingFacade">
        /// An appliance that facilitates message publishing operations.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messagePublishingFacade" /> is null.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while attempting to publish the heartbeat message.
        /// </exception>
        Task PublishHeartbeatMessageAsync(IMessagePublishingFacade messagePublishingFacade);

        /// <summary>
        /// Gets the messaging entity type that is used when publishing the message.
        /// </summary>
        MessagingEntityType EntityType
        {
            get;
        }

        /// <summary>
        /// Gets the regular interval, in seconds, at which the message is published.
        /// </summary>
        Int32 IntervalInSeconds
        {
            get;
        }

        /// <summary>
        /// Gets the label, if any, that is associated with the message.
        /// </summary>
        String Label
        {
            get;
        }

        /// <summary>
        /// Gets the type of the associated heartbeat message.
        /// </summary>
        Type MessageType
        {
            get;
        }
    }
}