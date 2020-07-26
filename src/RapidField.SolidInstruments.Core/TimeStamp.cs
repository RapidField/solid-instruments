// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a configurable utility for capturing the current date and time.
    /// </summary>
    public sealed class TimeStamp : Instrument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeStamp" /> class.
        /// </summary>
        [DebuggerHidden]
        private TimeStamp()
            : base(DefaultConcurrencyControlMode)
        {
            return;
        }

        /// <summary>
        /// Instructs the singleton <see cref="TimeStamp" /> instance to use <see cref="DateTimeKind.Local" /> when producing the
        /// current date and time.
        /// </summary>
        public static void UseLocal() => Instance.UseKind(DateTimeKind.Local);

        /// <summary>
        /// Instructs the singleton <see cref="TimeStamp" /> instance to use <see cref="DateTimeKind.Utc" /> when producing the
        /// current date and time.
        /// </summary>
        public static void UseUtc() => Instance.UseKind(DateTimeKind.Utc);

        /// <summary>
        /// Instructs the current <see cref="TimeStamp" /> to use the specified <see cref="DateTimeKind" /> when producing the
        /// current date and time.
        /// </summary>
        /// <param name="kind">
        /// The <see cref="DateTimeKind" /> to use when producing the current date and time.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="kind" /> is equal to <see cref="DateTimeKind.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal void UseKind(DateTimeKind kind)
        {
            using (var controlToken = StateControl.Enter())
            {
                Kind = kind.RejectIf().IsEqualToValue(DateTimeKind.Unspecified, nameof(kind));
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="TimeStamp" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Finalizes static members of the <see cref="TimeStamp" /> class.
        /// </summary>
        [DebuggerHidden]
        private static void FinalizeStaticMembers() => Instance.Dispose();

        /// <summary>
        /// Produces the current date and time.
        /// </summary>
        /// <returns>
        /// The current date and time.
        /// </returns>
        [DebuggerHidden]
        private DateTime Produce() => Kind switch
        {
            DateTimeKind.Local => DateTime.Now,
            DateTimeKind.Utc => DateTime.UtcNow,
            _ => throw new UnsupportedSpecificationException($"The specified date time kind, {Kind}, is not supported.")
        };

        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        public static DateTime Current => Instance.Produce();

        /// <summary>
        /// Represents a singleton instance of the <see cref="TimeStamp" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly TimeStamp Instance = new TimeStamp();

        /// <summary>
        /// Represents the default concurrency control mode for new <see cref="TimeStamp" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const ConcurrencyControlMode DefaultConcurrencyControlMode = ConcurrencyControlMode.SingleThreadSpinLock;

        /// <summary>
        /// Represents the default <see cref="DateTimeKind" /> for new <see cref="TimeStamp" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const DateTimeKind DefaultKind = DateTimeKind.Utc;

        /// <summary>
        /// Represents a finalizer for static members of the <see cref="TimeStamp" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly StaticMemberFinalizer StaticMemberFinalizer = new StaticMemberFinalizer(FinalizeStaticMembers);

        /// <summary>
        /// Represents the <see cref="DateTimeKind" /> that is used by the current <see cref="TimeStamp" /> when producing the
        /// current date and time.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTimeKind Kind = DefaultKind;
    }
}