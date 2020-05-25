// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;
using SystemTimeoutException = System.TimeoutException;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents exclusive or semi-exclusive control of a resource or block of code by a single thread.
    /// </summary>
    public interface IConcurrencyControlToken
    {
        /// <summary>
        /// Instructs the current <see cref="IConcurrencyControlToken" /> to wait for the specified task to complete before
        /// releasing control to another thread.
        /// </summary>
        /// <param name="action">
        /// An action to wait upon.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="IsActive" /> is <see langword="false" />.
        /// </exception>
        public void AttachTask(Action action);

        /// <summary>
        /// Instructs the current <see cref="IConcurrencyControlToken" /> to wait for the specified task to complete before
        /// releasing control to another thread.
        /// </summary>
        /// <param name="task">
        /// A task to wait upon.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="task" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="IsActive" /> is <see langword="false" />.
        /// </exception>
        public void AttachTask(Task task);

        /// <summary>
        /// Interrogates the state of the current <see cref="IConcurrencyControlToken" />.
        /// </summary>
        /// <remarks>
        /// Use this method for graceful handling of operation cancellation and control expiration.
        /// </remarks>
        /// <returns>
        /// <see langword="true" /> if the token is active and not expired, otherwise <see langword="false" />.
        /// </returns>
        public Boolean Poll();

        /// <summary>
        /// Interrogates the state of the current <see cref="IConcurrencyControlToken" />.
        /// </summary>
        /// <remarks>
        /// Use this method for graceful handling of operation cancellation and control expiration.
        /// </remarks>
        /// <param name="raiseExceptionIfInactive">
        /// A value specifying whether or not an exception should be raised if the token is inactive. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the token is active and not expired, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ConcurrencyControlOperationException">
        /// <paramref name="raiseExceptionIfInactive" /> is equal to <see langword="true" /> and the token is expired.
        /// </exception>
        public Boolean Poll(Boolean raiseExceptionIfInactive);

        /// <summary>
        /// Interrogates the state of the current <see cref="IConcurrencyControlToken" />.
        /// </summary>
        /// <remarks>
        /// Use this method for graceful handling of operation cancellation and control expiration.
        /// </remarks>
        /// <param name="raiseExceptionIfInactive">
        /// A value specifying whether or not an exception should be raised if the token is inactive. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <param name="raiseExceptionIfExpired">
        /// A value specifying whether or not an exception should be raised if the token is expired. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the token is active and not expired, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ConcurrencyControlOperationException">
        /// <paramref name="raiseExceptionIfInactive" /> is equal to <see langword="true" /> and the token is expired.
        /// </exception>
        /// <exception cref="SystemTimeoutException">
        /// <paramref name="raiseExceptionIfExpired" /> is equal to <see langword="true" /> and the token is inactive.
        /// </exception>
        public Boolean Poll(Boolean raiseExceptionIfInactive, Boolean raiseExceptionIfExpired);

        /// <summary>
        /// Gets a value indicating whether or not the associated thread currently has control of the resource.
        /// </summary>
        public Boolean IsActive
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the expiration threshold for the token has been exceeded.
        /// </summary>
        public Boolean IsExpired
        {
            get;
        }
    }
}