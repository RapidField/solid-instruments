// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemTimeoutException = System.TimeoutException;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents exclusive or semi-exclusive control of a resource or block of code by a single thread.
    /// </summary>
    /// <remarks>
    /// <see cref="ConcurrencyControlToken" /> is the default implementation of <see cref="IConcurrencyControlToken" />.
    /// </remarks>
    public sealed class ConcurrencyControlToken : IConcurrencyControlToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyControlToken" /> class.
        /// </summary>
        /// <param name="context">
        /// The synchronization context for the thread to which exclusive or semi-exclusive control belongs, or
        /// <see langword="null" /> if the context is unspecified.
        /// </param>
        /// <param name="granteeThread">
        /// The thread to which exclusive or semi-exclusive control belongs.
        /// </param>
        /// <param name="control">
        /// The control that issued the token.
        /// </param>
        /// <param name="identifier">
        /// A unique value that identifies the token within the context of the issuing control.
        /// </param>
        /// <param name="expirationThreshold">
        /// The maximum length of time to honor the token, or <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking
        /// is permitted.
        /// </param>
        /// <param name="expirationStopwatch">
        /// A stopwatch that measures the total wait time for the controlled operation, or <see langword="null" /> if indefinite
        /// thread blocking is permitted.
        /// </param>
        [DebuggerHidden]
        internal ConcurrencyControlToken(SynchronizationContext context, Thread granteeThread, IConcurrencyControl control, Int32 identifier, TimeSpan expirationThreshold, Stopwatch expirationStopwatch)
        {
            AttachedTasks = new List<Task>();
            Context = context;
            Control = control;
            ExpirationThreshold = expirationThreshold;
            ExpirationStopwatch = expirationStopwatch;
            GranteeThread = granteeThread;
            Identifier = identifier;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="ConcurrencyControlToken" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(ConcurrencyControlToken a, IConcurrencyControlToken b) => a == b == false;

        /// <summary>
        /// Determines whether or not a specified <see cref="ConcurrencyControlToken" /> instance is less than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(ConcurrencyControlToken a, IConcurrencyControlToken b) => a is null ? b is Object : a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="ConcurrencyControlToken" /> instance is less than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(ConcurrencyControlToken a, IConcurrencyControlToken b) => a is null || a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="ConcurrencyControlToken" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(ConcurrencyControlToken a, IConcurrencyControlToken b)
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
        /// Determines whether or not a specified <see cref="ConcurrencyControlToken" /> instance is greater than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(ConcurrencyControlToken a, IConcurrencyControlToken b) => a is Object && a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="ConcurrencyControlToken" /> instance is greater than or equal to
        /// another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ConcurrencyControlToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(ConcurrencyControlToken a, IConcurrencyControlToken b) => a is null ? b is null : a.CompareTo(b) >= 0;

        /// <summary>
        /// Instructs the current <see cref="ConcurrencyControlToken" /> to wait for the specified task to complete before releasing
        /// control to another thread.
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
        public void AttachTask(Action action) => AttachTask(Task.Factory.StartNew(action.RejectIf().IsNull(nameof(action))));

        /// <summary>
        /// Instructs the current <see cref="ConcurrencyControlToken" /> to wait for the specified task to complete before releasing
        /// control to another thread.
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
        public void AttachTask(Task task)
        {
            if (IsActive)
            {
                AttachedTasks.Add(task.RejectIf().IsNull(nameof(task)));
                return;
            }

            throw new InvalidOperationException("Tasks cannot be attached to the control token because it is no longer active.");
        }

        /// <summary>
        /// Compares the current <see cref="ConcurrencyControlToken" /> to the specified object and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="ConcurrencyControlToken" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(IConcurrencyControlToken other) => Identifier.CompareTo(other.Identifier);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ConcurrencyControl" />.
        /// </summary>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The issuing <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            try
            {
                Release();
            }
            finally
            {
                IsDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Asynchronously releases all resources consumed by the current <see cref="ConcurrencyControlToken" />.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public ValueTask DisposeAsync() => new ValueTask(Task.Factory.StartNew(Dispose));

        /// <summary>
        /// Determines whether or not the current <see cref="ConcurrencyControlToken" /> is equal to the specified
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
            else if (obj is IConcurrencyControlToken token)
            {
                return Equals(token);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="ConcurrencyControlToken" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="ConcurrencyControlToken" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(IConcurrencyControlToken other) => Identifier == other.Identifier;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => Identifier;

        /// <summary>
        /// Interrogates the state of the current <see cref="ConcurrencyControlToken" />.
        /// </summary>
        /// <remarks>
        /// Use this method for graceful handling of operation cancellation and control expiration.
        /// </remarks>
        /// <returns>
        /// <see langword="true" /> if the token is active and not expired, otherwise <see langword="false" />.
        /// </returns>
        public Boolean Poll() => Poll(false);

        /// <summary>
        /// Interrogates the state of the current <see cref="ConcurrencyControlToken" />.
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
        public Boolean Poll(Boolean raiseExceptionIfInactive) => Poll(raiseExceptionIfInactive, false);

        /// <summary>
        /// Interrogates the state of the current <see cref="ConcurrencyControlToken" />.
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
        public Boolean Poll(Boolean raiseExceptionIfInactive, Boolean raiseExceptionIfExpired)
        {
            if (IsActive)
            {
                return IsExpired ? raiseExceptionIfExpired ? throw new SystemTimeoutException("The concurrency control token is expired.") : false : true;
            }

            return raiseExceptionIfInactive ? throw new ConcurrencyControlOperationException("Control has been relinquished to other threads.") : false;
        }

        /// <summary>
        /// Converts the value of the current <see cref="ConcurrencyControlToken" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="ConcurrencyControlToken" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Identifier)}\": {Identifier}, \"{nameof(IsActive)}\": {IsActive.ToSerializedString()} }}";

        /// <summary>
        /// Releases exclusive or semi-exclusive control of the associated resource if <see cref="IsActive" /> is
        /// <see langword="true" />.
        /// </summary>
        /// <remarks>
        /// <see cref="Release" /> is invoked by <see cref="Dispose" />.
        /// </remarks>
        /// <exception cref="AggregateException">
        /// An exception was raised by an attached task.
        /// </exception>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The issuing <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        [DebuggerHidden]
        internal void Release()
        {
            if (Interlocked.Exchange(ref IsActiveValue, IsActiveFalse) == IsActiveTrue)
            {
                try
                {
                    try
                    {
                        if (AttachedTasks.Any())
                        {
                            Task.WaitAll(AttachedTasks.ToArray());
                        }
                    }
                    finally
                    {
                        try
                        {
                            Control.Exit(this);
                        }
                        catch (ConcurrencyControlOperationException)
                        {
                            if (Context != null && Thread.CurrentThread != GranteeThread)
                            {
                                // Attempt to avoid deadlocks in case the IConcurrencyControl implementation is using the wrong
                                // concurrency control primitive, or making some other unforeseen mistake.
                                Context.Post(ExitIssuingControl, this);
                            }
                        }
                    }
                }
                catch (ObjectDisposedException exception)
                {
                    throw new ConcurrencyControlOperationException(exception);
                }
                finally
                {
                    ExpirationStopwatch?.Stop();
                }
            }
        }

        /// <summary>
        /// Informs the issuing control that the associated thread is finished consuming the resource.
        /// </summary>
        /// <param name="state">
        /// The current token to exit.
        /// </param>
        /// <exception cref="ConcurrencyControlOperationException">
        /// The issuing <see cref="IConcurrencyControl" /> is in an invalid state.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The issuing <see cref="IConcurrencyControl" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        private static void ExitIssuingControl(Object state)
        {
            var token = (ConcurrencyControlToken)state;
            token.Control.Exit(token);
        }

        /// <summary>
        /// Gets a unique value that identifies the token within the context of the issuing control.
        /// </summary>
        public Int32 Identifier
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the associated thread currently has control of the resource.
        /// </summary>
        public Boolean IsActive => IsActiveValue == IsActiveTrue;

        /// <summary>
        /// Gets a value indicating whether or not the expiration threshold for the token has been exceeded.
        /// </summary>
        public Boolean IsExpired => (ExpirationStopwatch is null || ExpirationThreshold == Timeout.InfiniteTimeSpan) ? false : (ExpirationThreshold < ExpirationStopwatch.Elapsed);

        /// <summary>
        /// Represents a collection of tasks for which the associated thread will wait before releasing control.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly IList<Task> AttachedTasks;

        /// <summary>
        /// Represents the control that issued the token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly IConcurrencyControl Control;

        /// <summary>
        /// Represents an integer value indicating that the associated thread currently does not have control of the resource.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 IsActiveFalse = 0;

        /// <summary>
        /// Represents an integer value indicating that the associated thread currently has control of the resource.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 IsActiveTrue = 1;

        /// <summary>
        /// Represents the synchronization context for the thread to which exclusive or semi-exclusive control belongs, or
        /// <see langword="null" /> if the context is unspecified.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SynchronizationContext Context;

        /// <summary>
        /// Represents a stopwatch that measures the total wait time for the controlled operation, or <see langword="null" /> if
        /// indefinite thread blocking is permitted.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Stopwatch ExpirationStopwatch;

        /// <summary>
        /// Represents the maximum length of time to honor the token, or <see cref="Timeout.InfiniteTimeSpan" /> if indefinite
        /// thread blocking is permitted.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TimeSpan ExpirationThreshold;

        /// <summary>
        /// Represents the thread to which exclusive or semi-exclusive control belongs.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Thread GranteeThread;

        /// <summary>
        /// Represents an integer value indicating whether or not the associated thread currently has control of the resource.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 IsActiveValue = IsActiveTrue;

        /// <summary>
        /// Represents a value indicating whether or not the current <see cref="ConcurrencyControlToken" /> has been disposed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean IsDisposed = false;
    }
}