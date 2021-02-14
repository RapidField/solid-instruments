// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.EventAuthoring.Extensions
{
    /// <summary>
    /// Extends the <see cref="ICommandMediator" /> interface with event handling features.
    /// </summary>
    public static class ICommandMediatorExtensions
    {
        /// <summary>
        /// Processes the specified <see cref="IEvent" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createEventFunction">
        /// A function that returns the event to process.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createEventFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the event.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static void HandleEvent(this ICommandMediator target, Func<IEventRegister, IEvent> createEventFunction)
        {
            try
            {
                _ = target.Process(createEventFunction.RejectIf().IsNull(nameof(createEventFunction)).TargetArgument(EventRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(exception);
            }
        }

        /// <summary>
        /// Processes the specified <see cref="IEvent" />.
        /// </summary>
        /// <typeparam name="TEvent">
        /// The type of the event to process.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createEventFunction">
        /// A function that returns the event to process.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createEventFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the event.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static void HandleEvent<TEvent>(this ICommandMediator target, Func<IEventRegister, TEvent> createEventFunction)
            where TEvent : class, IEvent
        {
            try
            {
                _ = target.Process(createEventFunction.RejectIf().IsNull(nameof(createEventFunction)).TargetArgument(EventRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TEvent), exception);
            }
        }

        /// <summary>
        /// Asynchronously processes the specified <see cref="IEvent" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createEventFunction">
        /// A function that returns the event to process.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createEventFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the event.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static Task HandleEventAsync(this ICommandMediator target, Func<IEventRegister, IEvent> createEventFunction)
        {
            try
            {
                return target.ProcessAsync(createEventFunction.RejectIf().IsNull(nameof(createEventFunction)).TargetArgument(EventRegister.Instance));
            }
            catch (CommandHandlingException exception)
            {
                return Task.FromException(exception);
            }
            catch (ObjectDisposedException exception)
            {
                return Task.FromException(exception);
            }
            catch (Exception exception)
            {
                return Task.FromException(new CommandHandlingException(exception));
            }
        }

        /// <summary>
        /// Asynchronously processes the specified <see cref="IEvent" />.
        /// </summary>
        /// <typeparam name="TEvent">
        /// The type of the event to process.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createEventFunction">
        /// A function that returns the event to process.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createEventFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the event.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static Task HandleEventAsync<TEvent>(this ICommandMediator target, Func<IEventRegister, TEvent> createEventFunction)
            where TEvent : class, IEvent
        {
            try
            {
                return target.ProcessAsync(createEventFunction.RejectIf().IsNull(nameof(createEventFunction)).TargetArgument(EventRegister.Instance));
            }
            catch (CommandHandlingException exception)
            {
                return Task.FromException(exception);
            }
            catch (ObjectDisposedException exception)
            {
                return Task.FromException(exception);
            }
            catch (Exception exception)
            {
                return Task.FromException(new CommandHandlingException(typeof(TEvent), exception));
            }
        }
    }
}