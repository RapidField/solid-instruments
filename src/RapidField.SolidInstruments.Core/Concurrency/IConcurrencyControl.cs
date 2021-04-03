// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents a concurrency control mechanism.
    /// </summary>
    public interface IConcurrencyControl : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Enqueues the specified operation for controlled execution and returns without waiting.
        /// </summary>
        /// <param name="action">
        /// An action to execute asynchronously.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public void Enqueue(Action action);

        /// <summary>
        /// Enqueues the specified operation for controlled execution and returns without waiting.
        /// </summary>
        /// <param name="task">
        /// A task to execute asynchronously.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        public void Enqueue(Task task);

        /// <summary>
        /// Enqueues the specified operation for controlled execution and returns without waiting.
        /// </summary>
        /// <param name="action">
        /// An action to execute asynchronously.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public void Enqueue(Action<IConcurrencyControlToken> action);

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <param name="action">
        /// An action to execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        public void EnqueueAndWait(Action action);

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <param name="task">
        /// A task to execute.
        /// </param>
        /// <exception cref="AggregateException">
        /// An exception was raised while executing <paramref name="task" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public void EnqueueAndWait(Task task);

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <param name="action">
        /// An action to execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        public void EnqueueAndWait(Action<IConcurrencyControlToken> action);

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="function" />.
        /// </typeparam>
        /// <param name="function">
        /// A function to execute.
        /// </param>
        /// <returns>
        /// The result of <paramref name="function" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        public T EnqueueAndWait<T>(Func<T> function);

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="task" />.
        /// </typeparam>
        /// <param name="task">
        /// A task to execute.
        /// </param>
        /// <returns>
        /// The result of <paramref name="task" />.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while executing <paramref name="task" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public T EnqueueAndWait<T>(Task<T> task);

        /// <summary>
        /// Enqueues the specified operation for controlled execution and waits for it to complete.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="function" />.
        /// </typeparam>
        /// <param name="function">
        /// A function to execute.
        /// </param>
        /// <returns>
        /// The result of <paramref name="function" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        public T EnqueueAndWait<T>(Func<IConcurrencyControlToken, T> function);

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution and returns a task that completes when the
        /// action is finished running.
        /// </summary>
        /// <param name="action">
        /// An action to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        public Task EnqueueAsync(Action action);

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution.
        /// </summary>
        /// <param name="task">
        /// A task to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while executing <paramref name="task" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public Task EnqueueAsync(Task task);

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution and returns a task that completes when the
        /// action is finished running.
        /// </summary>
        /// <param name="action">
        /// An action to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="action" />.
        /// </exception>
        public Task EnqueueAsync(Action<IConcurrencyControlToken> action);

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution and returns a task that completes when the
        /// function is finished running.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="function" />.
        /// </typeparam>
        /// <param name="function">
        /// A function to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the result of <paramref name="function" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        public Task<T> EnqueueAsync<T>(Func<T> function);

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="task" />.
        /// </typeparam>
        /// <param name="task">
        /// A task to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the result of <paramref name="task" />.
        /// </returns>
        /// <exception cref="AggregateException">
        /// An exception was raised while executing <paramref name="task" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public Task<T> EnqueueAsync<T>(Task<T> task);

        /// <summary>
        /// Asynchronously enqueues the specified operation for controlled execution and returns a task that completes when the
        /// function is finished running.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the result that is returned by <paramref name="function" />.
        /// </typeparam>
        /// <param name="function">
        /// A function to execute asynchronously.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the result of <paramref name="function" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="Exception">
        /// An exception was raised while executing <paramref name="function" />.
        /// </exception>
        public Task<T> EnqueueAsync<T>(Func<IConcurrencyControlToken, T> function);

        /// <summary>
        /// Informs the control that a thread is entering a block of code or that it is beginning to consume a resource.
        /// </summary>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The operation timed out or the <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public IConcurrencyControlToken Enter();

        /// <summary>
        /// Informs the control that a thread is exiting a block of code or has finished consuming a resource.
        /// </summary>
        /// <param name="token">
        /// A token issued by the current <see cref="IConcurrencyControl" /> that is no longer in use.
        /// </param>
        /// <exception cref="ConcurrencyControlOperationException">
        /// <paramref name="token" /> was not issued by this control or the <see cref="IConcurrencyControl" /> is in an invalid
        /// state.
        /// </exception>
        public void Exit(IConcurrencyControlToken token);

        /// <summary>
        /// Gets the consumption state of the current <see cref="IConcurrencyControl" />.
        /// </summary>
        public ConcurrencyControlConsumptionState ConsumptionState
        {
            get;
        }
    }
}