// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Tracks a collection of related objects and manages disposal of them.
    /// </summary>
    /// <remarks>
    /// <see cref="ReferenceManager" /> is the default implementation of <see cref="IReferenceManager" />.
    /// </remarks>
    public sealed class ReferenceManager : Instrument, IReferenceManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceManager" /> class.
        /// </summary>
        public ReferenceManager()
            : base()
        {
            return;
        }

        /// <summary>
        /// Instructs the current <see cref="ReferenceManager" /> to manage the specified object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the managed object.
        /// </typeparam>
        /// <param name="reference">
        /// The managed object.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddObject<T>(T reference)
            where T : class
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                var managedReference = new ManagedReference<T>(reference);

                if (References.Contains(managedReference) == false)
                {
                    References.Enqueue(managedReference);
                }
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="ReferenceManager" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="ReferenceManager" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(ObjectCount)}\": {ObjectCount} }}";

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ReferenceManager" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    try
                    {
                        var disposeTasks = new List<Task>();

                        while (References.Count > 0)
                        {
                            IAsyncDisposable reference;

                            using (var controlToken = StateControl.Enter())
                            {
                                reference = References.Dequeue();
                            }

                            disposeTasks.Add(reference?.DisposeAsync().AsTask());
                        }

                        var disposeTaskArray = disposeTasks.Where(task => task is null == false).ToArray();

                        if (disposeTaskArray.Any())
                        {
                            Task.WaitAll(disposeTaskArray);
                        }
                    }
                    finally
                    {
                        References.Clear();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets the number of objects that are managed by the current <see cref="ReferenceManager" />.
        /// </summary>
        public Int32 ObjectCount => References.Count;

        /// <summary>
        /// Represents the objects that are managed by the current <see cref="ReferenceManager" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Queue<IAsyncDisposable> References = new Queue<IAsyncDisposable>();

        /// <summary>
        /// Represents an object that is managed by a <see cref="ReferenceManager" />.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the managed object.
        /// </typeparam>
        private class ManagedReference<T> : IAsyncDisposable, IDisposable, IEquatable<ManagedReference<T>>
            where T : class
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ManagedReference{T}" /> class.
            /// </summary>
            /// <param name="target">
            /// The managed object.
            /// </param>
            [DebuggerHidden]
            internal ManagedReference(T target)
            {
                Target = target;
            }

            /// <summary>
            /// Determines whether or not two specified <see cref="ManagedReference{T}" /> instances are not equal.
            /// </summary>
            /// <param name="a">
            /// The first <see cref="ManagedReference{T}" /> instance to compare.
            /// </param>
            /// <param name="b">
            /// The second <see cref="ManagedReference{T}" /> instance to compare.
            /// </param>
            /// <returns>
            /// A value indicating whether or not the specified instances are not equal.
            /// </returns>
            public static Boolean operator !=(ManagedReference<T> a, ManagedReference<T> b) => (a == b) == false;

            /// <summary>
            /// Determines whether or not two specified <see cref="ManagedReference{T}" /> instances are equal.
            /// </summary>
            /// <param name="a">
            /// The first <see cref="ManagedReference{T}" /> instance to compare.
            /// </param>
            /// <param name="b">
            /// The second <see cref="ManagedReference{T}" /> instance to compare.
            /// </param>
            /// <returns>
            /// A value indicating whether or not the specified instances are equal.
            /// </returns>
            public static Boolean operator ==(ManagedReference<T> a, ManagedReference<T> b)
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
            /// Releases all resources consumed by the current <see cref="ManagedReference{T}" />.
            /// </summary>
            [DebuggerHidden]
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Asynchronously releases all resources consumed by the current <see cref="Instrument" />.
            /// </summary>
            /// <returns>
            /// A task representing the asynchronous operation.
            /// </returns>
            [DebuggerHidden]
            public ValueTask DisposeAsync() => new ValueTask(Task.Factory.StartNew(Dispose));

            /// <summary>
            /// Determines whether or not the current <see cref="ManagedReference{T}" /> is equal to the specified
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
                else if (obj is ManagedReference<T>)
                {
                    return Equals((ManagedReference<T>)obj);
                }

                return false;
            }

            /// <summary>
            /// Determines whether or not two specified <see cref="ManagedReference{T}" /> instances are equal.
            /// </summary>
            /// <param name="other">
            /// The <see cref="ManagedReference{T}" /> to compare to this instance.
            /// </param>
            /// <returns>
            /// A value indicating whether or not the specified instances are equal.
            /// </returns>
            public Boolean Equals(ManagedReference<T> other)
            {
                if (other is null)
                {
                    return false;
                }
                else if (other.Target is null)
                {
                    return false;
                }

                return ReferenceEquals(Target, other.Target);
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            /// A 32-bit signed integer hash code.
            /// </returns>
            public override Int32 GetHashCode() => Target.GetHashCode();

            /// <summary>
            /// Releases all resources consumed by the current <see cref="ManagedReference{T}" />.
            /// </summary>
            /// <param name="disposing">
            /// A value indicating whether or not managed resources should be released.
            /// </param>
            [DebuggerHidden]
            protected void Dispose(Boolean disposing)
            {
                if (disposing)
                {
                    if (Target is null)
                    {
                        return;
                    }

                    try
                    {
                        if (Target is IDisposable disposableTarget)
                        {
                            disposableTarget?.Dispose();
                        }
                        else if (Target is IAsyncDisposable asyncDisposableTarget)
                        {
                            asyncDisposableTarget?.DisposeAsync().AsTask().Wait();
                        }
                    }
                    finally
                    {
                        Target = null;
                    }
                }
            }

            /// <summary>
            /// Represents the managed object.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private T Target;
        }
    }
}