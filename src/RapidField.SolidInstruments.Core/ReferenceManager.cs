// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
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
            where T : class => AddObject(reference, DefaultStrongReferenceMinimumLifeSpan);

        /// <summary>
        /// Instructs the current <see cref="ReferenceManager" /> to manage the specified object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the managed object.
        /// </typeparam>
        /// <param name="reference">
        /// The managed object.
        /// </param>
        /// <param name="strongReferenceMinimumLifeSpan">
        /// The minimum length of time to preserve a strong reference to the managed object. The default value is 55 seconds. The
        /// observed length of time may be shorter if the reference manager is disposed.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="strongReferenceMinimumLifeSpan" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddObject<T>(T reference, TimeSpan strongReferenceMinimumLifeSpan)
            where T : class
        {
            RejectIfDisposed();

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                Prune();
                References?.Add(new ManagedReference<T>(reference, strongReferenceMinimumLifeSpan));
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
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                while (References?.Any() ?? false)
                {
                    try
                    {
                        var disposeTasks = new List<Task>();
                        var referenceArray = References?.ToArray() ?? Array.Empty<IManagedReference>();

                        foreach (var reference in referenceArray)
                        {
                            disposeTasks.Add(reference?.DisposeAsync().AsTask() ?? Task.CompletedTask);
                        }

                        if (disposeTasks.Any())
                        {
                            Task.WaitAll(disposeTasks.ToArray());
                        }
                    }
                    finally
                    {
                        References?.Clear();
                        References = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Finalizes static members of the <see cref="ReferenceManager" /> class.
        /// </summary>
        [DebuggerHidden]
        private static void FinalizeStaticMembers() => Instance.Dispose();

        /// <summary>
        /// Removes dead references from the current <see cref="ReferenceManager" />.
        /// </summary>
        [DebuggerHidden]
        private void Prune()
        {
            if (References?.Any() ?? false)
            {
                var referenceArray = References?.ToArray() ?? Array.Empty<IManagedReference>();

                foreach (var reference in References)
                {
                    reference?.Poll();
                }

                References = new List<IManagedReference>(References?.Where(reference => reference.IsDead is false) ?? Array.Empty<IManagedReference>());
            }
        }

        /// <summary>
        /// Gets the number of objects that are managed by the current <see cref="ReferenceManager" />.
        /// </summary>
        public Int32 ObjectCount => References?.Count ?? 0;

        /// <summary>
        /// Represents a singleton instance of the <see cref="ReferenceManager" /> class.
        /// </summary>
        public static readonly IReferenceManager Instance = new ReferenceManager();

        /// <summary>
        /// Represents the default minimum length of time to preserve strong references to managed objects.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan DefaultStrongReferenceMinimumLifeSpan = TimeSpan.FromSeconds(55);

        /// <summary>
        /// Represents a finalizer for static members of the <see cref="ReferenceManager" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly StaticMemberFinalizer StaticMemberFinalizer = new StaticMemberFinalizer(FinalizeStaticMembers);

        /// <summary>
        /// Represents the objects that are managed by the current <see cref="ReferenceManager" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IList<IManagedReference> References = new List<IManagedReference>();

        /// <summary>
        /// Represents an object that is managed by a <see cref="ReferenceManager" />.
        /// </summary>
        /// <remarks>
        /// <see cref="ManagedReference{T}" /> is the default implementation of <see cref="IManagedReference" />.
        /// </remarks>
        /// <typeparam name="T">
        /// The type of the managed object.
        /// </typeparam>
        private class ManagedReference<T> : IEquatable<ManagedReference<T>>, IManagedReference
            where T : class
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ManagedReference{T}" /> class.
            /// </summary>
            /// <param name="target">
            /// The managed object.
            /// </param>
            /// <param name="strongReferenceMinimumLifeSpan">
            /// The minimum length of time to preserve a strong reference to the managed object. The observed length of time may be
            /// shorter if the reference manager is disposed.
            /// </param>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="strongReferenceMinimumLifeSpan" /> is less than <see cref="TimeSpan.Zero" />.
            /// </exception>
            [DebuggerHidden]
            internal ManagedReference(T target, TimeSpan strongReferenceMinimumLifeSpan)
            {
                CreationTimeStamp = TimeStamp.Current;
                StrongReferenceMinimumLifeSpan = strongReferenceMinimumLifeSpan.RejectIf().IsLessThan(TimeSpan.Zero, nameof(strongReferenceMinimumLifeSpan));
                Target = target;
            }

            /// <summary>
            /// Finalizes the current <see cref="ManagedReference{T}" />.
            /// </summary>
            [DebuggerHidden]
            ~ManagedReference()
            {
                Dispose(false);
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
            public static Boolean operator !=(ManagedReference<T> a, ManagedReference<T> b) => (a == b) is false;

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
                else if (obj is ManagedReference<T> reference)
                {
                    return Equals(reference);
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

                return ReferenceEquals(Target, other.Target) && CreationTimeStamp == other.CreationTimeStamp;
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            /// A 32-bit signed integer hash code.
            /// </returns>
            public override Int32 GetHashCode() => Target?.GetHashCode() ?? 0;

            /// <summary>
            /// Destroys the strong reference to the managed object if its lifespan is expired.
            /// </summary>
            public void Poll()
            {
                if (StrongReferenceIsExpired)
                {
                    StrongReference = null;
                }
            }

            /// <summary>
            /// Releases all resources consumed by the current <see cref="ManagedReference{T}" />.
            /// </summary>
            /// <param name="disposing">
            /// A value indicating whether or not disposal was invoked by user code.
            /// </param>
            [DebuggerHidden]
            protected void Dispose(Boolean disposing)
            {
                if (IsDead)
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

            /// <summary>
            /// Gets a value indicating whether or not the managed object has live references.
            /// </summary>
            public Boolean IsDead => Target is null;

            /// <summary>
            /// Gets the length of time that the current <see cref="ManagedReference{T}" /> has existed.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private TimeSpan Age => TimeStamp.Current - CreationTimeStamp;

            /// <summary>
            /// Gets a value indicating whether or not the strong reference is expired.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Boolean StrongReferenceIsExpired => StrongReference is null || Age > StrongReferenceMinimumLifeSpan;

            /// <summary>
            /// Gets or sets the managed object.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private T Target
            {
                [DebuggerHidden]
                get => StrongReference is null ? (WeakReference?.Target as T) : StrongReference;
                [DebuggerHidden]
                set
                {
                    StrongReference = value;
                    WeakReference = value is null ? null : new WeakReference(value);
                }
            }

            /// <summary>
            /// Represents the date and time when the current <see cref="ManagedReference{T}" /> was created.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly DateTime CreationTimeStamp;

            /// <summary>
            /// Represents the minimum length of time to preserve a strong reference to the managed object. The observed length of
            /// time may be shorter if the reference manager is disposed.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly TimeSpan StrongReferenceMinimumLifeSpan;

            /// <summary>
            /// Represents a strong reference to the managed object.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private T StrongReference;

            /// <summary>
            /// Represents a weak reference to the managed object.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private WeakReference WeakReference;
        }

        /// <summary>
        /// Represents an object that is managed by a <see cref="ReferenceManager" />.
        /// </summary>
        private interface IManagedReference : IAsyncDisposable, IDisposable
        {
            /// <summary>
            /// Destroys the strong reference to the managed object if its lifespan is expired.
            /// </summary>
            public void Poll();

            /// <summary>
            /// Gets a value indicating whether or not the managed object has live references.
            /// </summary>
            public Boolean IsDead
            {
                get;
            }
        }
    }
}