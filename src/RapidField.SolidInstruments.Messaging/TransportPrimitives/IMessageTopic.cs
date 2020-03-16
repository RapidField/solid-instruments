// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a message topic.
    /// </summary>
    public interface IMessageTopic : IMessagingEntity
    {
        /// <summary>
        /// Asynchronously creates a new subscription to the current <see cref="IMessageTopic" />.
        /// </summary>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="subscriptionName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// A subscription with the specified name already exists.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task CreateSubscriptionAsync(String subscriptionName);

        /// <summary>
        /// Asynchronously and non-destructively returns the next available messages from the current <see cref="IMessageTopic" />,
        /// if any, up to the specified maximum count.
        /// </summary>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <param name="count">
        /// The maximum number of messages to read from the topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the next available messages from the topic, or an empty
        /// collection if no messages are available.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="subscriptionName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        Task<IEnumerable<PrimitiveMessage>> DequeueAsync(String subscriptionName, Int32 count);

        /// <summary>
        /// Asynchronously destroys the specified subscription to the current <see cref="IMessageTopic" />.
        /// </summary>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="subscriptionName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// A subscription with the specified name does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task DestroySubscriptionAsync(String subscriptionName);

        /// <summary>
        /// Attempts to create a new subscription to the current <see cref="IMessageTopic" />.
        /// </summary>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the subscription was successfully created, otherwise <see langword="false" />.
        /// </returns>
        Boolean TryCreateSubscription(String subscriptionName);

        /// <summary>
        /// Attempts to destroy the specified subscription to the current <see cref="IMessageTopic" />.
        /// </summary>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the subscription was successfully destroyed, otherwise <see langword="false" />.
        /// </returns>
        Boolean TryDestroySubscription(String subscriptionName);

        /// <summary>
        /// Gets the number of subscriptions to the current <see cref="IMessageTopic" />.
        /// </summary>
        Int32 SubscriptionCount
        {
            get;
        }

        /// <summary>
        /// Gets the unique names of the subscriptions to the current <see cref="IMessageTopic" />.
        /// </summary>
        IEnumerable<String> SubscriptionNames
        {
            get;
        }
    }
}