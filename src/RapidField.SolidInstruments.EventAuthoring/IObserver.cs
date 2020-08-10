// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents an object that responds to events raised by one or more <see cref="IObservationSubject" /> instances.
    /// </summary>
    /// <remarks>
    /// <see cref="IObservationSubject" /> and <see cref="IObserver" /> can be used to implement the observer pattern.
    /// </remarks>
    public interface IObserver
    {
        /// <summary>
        /// Asynchronously notifies the current <see cref="IObserver" /> about the specified event.
        /// </summary>
        /// <typeparam name="TSubject">
        /// The type of the observation subject.
        /// </typeparam>
        /// <typeparam name="TEvent">
        /// The type of the event that is raised.
        /// </typeparam>
        /// <param name="subject">
        /// The observation subject.
        /// </param>
        /// <param name="observation">
        /// The event that was raised.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="subject" /> is <see langword="null" /> -or- <paramref name="observation" /> is <see langword="null" />.
        /// </exception>
        public Task NotifyAsync<TSubject, TEvent>(TSubject subject, TEvent observation)
            where TSubject : IObservationSubject
            where TEvent : IEvent;

        /// <summary>
        /// Adds the current <see cref="IObserver" /> to the specified subject's list of event notification targets.
        /// </summary>
        /// <param name="subject">
        /// The subject to begin observing.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="subject" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="subject" /> is disposed.
        /// </exception>
        public void Observe(IObservationSubject subject);

        /// <summary>
        /// Gets a textual value that uniquely identifies the current <see cref="IObserver" />.
        /// </summary>
        public String ObserverIdentity
        {
            get;
        }
    }
}