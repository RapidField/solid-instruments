// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Represents a collection of definitions that a
    /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> uses to publish
    /// heartbeat messages.
    /// </summary>
    public sealed class HeartbeatSchedule : IEnumerable<IHeartbeatScheduleItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatSchedule" /> class.
        /// </summary>
        public HeartbeatSchedule()
        {
            return;
        }

        /// <summary>
        /// Adds the specified heartbeat schedule item to the schedule.
        /// </summary>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is published.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The schedule already contains an item with matching specifications.
        /// </exception>
        public void AddItem(Int32 intervalInSeconds) => AddItem<HeartbeatMessage>(intervalInSeconds);

        /// <summary>
        /// Adds the specified heartbeat schedule item to the schedule.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the associated heartbeat message.
        /// </typeparam>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is published.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The schedule already contains an item with matching specifications.
        /// </exception>
        public void AddItem<TMessage>(Int32 intervalInSeconds)
            where TMessage : HeartbeatMessage, new() => AddItem(new HeartbeatScheduleItem<TMessage>(intervalInSeconds));

        /// <summary>
        /// Adds the specified heartbeat schedule item to the schedule.
        /// </summary>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is published.
        /// </param>
        /// <param name="entityType">
        /// The messaging entity type that is used when publishing the message. The default value is
        /// <see cref="MessagingEntityType.Topic" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero -or- <paramref name="entityType" /> is equal to
        /// <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The schedule already contains an item with matching specifications.
        /// </exception>
        public void AddItem(Int32 intervalInSeconds, MessagingEntityType entityType) => AddItem<HeartbeatMessage>(intervalInSeconds, entityType);

        /// <summary>
        /// Adds the specified heartbeat schedule item to the schedule.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the associated heartbeat message.
        /// </typeparam>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is published.
        /// </param>
        /// <param name="entityType">
        /// The messaging entity type that is used when publishing the message. The default value is
        /// <see cref="MessagingEntityType.Topic" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero -or- <paramref name="entityType" /> is equal to
        /// <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The schedule already contains an item with matching specifications.
        /// </exception>
        public void AddItem<TMessage>(Int32 intervalInSeconds, MessagingEntityType entityType)
            where TMessage : HeartbeatMessage, new() => AddItem(new HeartbeatScheduleItem<TMessage>(intervalInSeconds, entityType));

        /// <summary>
        /// Adds the specified heartbeat schedule item to the schedule.
        /// </summary>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is published.
        /// </param>
        /// <param name="entityType">
        /// The messaging entity type that is used when publishing the message. The default value is
        /// <see cref="MessagingEntityType.Topic" />.
        /// </param>
        /// <param name="label">
        /// The label, if any, that is associated with the message. This argument can be null.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero -or- <paramref name="entityType" /> is equal to
        /// <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The schedule already contains an item with matching specifications.
        /// </exception>
        public void AddItem(Int32 intervalInSeconds, MessagingEntityType entityType, String label) => AddItem<HeartbeatMessage>(intervalInSeconds, entityType, label);

        /// <summary>
        /// Adds the specified heartbeat schedule item to the schedule.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the associated heartbeat message.
        /// </typeparam>
        /// <param name="intervalInSeconds">
        /// The regular interval, in seconds, at which the message is published.
        /// </param>
        /// <param name="entityType">
        /// The messaging entity type that is used when publishing the message. The default value is
        /// <see cref="MessagingEntityType.Topic" />.
        /// </param>
        /// <param name="label">
        /// The label, if any, that is associated with the message. This argument can be null.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="intervalInSeconds" /> is less than or equal to zero -or- <paramref name="entityType" /> is equal to
        /// <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The schedule already contains an item with matching specifications.
        /// </exception>
        public void AddItem<TMessage>(Int32 intervalInSeconds, MessagingEntityType entityType, String label)
            where TMessage : HeartbeatMessage, new() => AddItem(new HeartbeatScheduleItem<TMessage>(intervalInSeconds, entityType, label));

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="HeartbeatSchedule" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="HeartbeatSchedule" />.
        /// </returns>
        public IEnumerator<IHeartbeatScheduleItem> GetEnumerator() => OrderedItems.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="HeartbeatSchedule" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="HeartbeatSchedule" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Adds the specified heartbeat schedule item to the schedule.
        /// </summary>
        /// <param name="item">
        /// An item that specifies how a heartbeat message is published.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The schedule already contains <paramref name="item" />.
        /// </exception>
        [DebuggerHidden]
        private void AddItem(IHeartbeatScheduleItem item)
        {
            item = item.RejectIf().IsNull(nameof(item)).TargetArgument;

            if (Items.Any((element) => element == item))
            {
                throw new InvalidOperationException("The specified item was added twice.");
            }

            Items.Add(item);
        }

        /// <summary>
        /// Gets the ordered collection of schedule items.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IEnumerable<IHeartbeatScheduleItem> OrderedItems => Items.OrderBy((element) => element.IntervalInSeconds).ThenBy((element) => element.Label);

        /// <summary>
        /// Represents the collection of schedule items.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<IHeartbeatScheduleItem> Items = new List<IHeartbeatScheduleItem>();
    }
}