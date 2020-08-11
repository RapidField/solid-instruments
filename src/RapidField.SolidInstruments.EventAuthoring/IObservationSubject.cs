// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents an object that is observed by one or more <see cref="IObservationSubject" /> instances.
    /// </summary>
    /// <remarks>
    /// <see cref="IObservationSubject" /> and <see cref="IObserver" /> can be used to implement the observer pattern.
    /// </remarks>
    public interface IObservationSubject : IDisposable
    {
        /// <summary>
        /// Adds the specified observer to a list of event notification targets.
        /// </summary>
        /// <param name="observer">
        /// The observer to add.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="observer" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="observer" /> is already added.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddObserver(IObserver observer);

        /// <summary>
        /// Notifies the subject's observers about the specified event.
        /// </summary>
        /// <typeparam name="TEvent">
        /// The type of the event to raise.
        /// </typeparam>
        /// <param name="observation">
        /// The event to raise.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="observation" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RaiseEvent<TEvent>(TEvent observation)
            where TEvent : IEvent;

        /// <summary>
        /// Removes the specified observer from a list of event notification targets.
        /// </summary>
        /// <param name="observer">
        /// The observer to remove.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="observer" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="observer" /> is not present.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RemoveObserver(IObserver observer);

        /// <summary>
        /// Gets a list of event notification targets for the current <see cref="IObservationSubject" />, which is keyed by observer
        /// identity.
        /// </summary>
        IReadOnlyDictionary<String, IObserver> Observers
        {
            get;
        }
    }
}