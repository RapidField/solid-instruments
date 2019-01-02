// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Specifies a logical mode for concurrency control.
    /// </summary>
    public enum ConcurrencyControlMode : Int32
    {
        /// <summary>
        /// The concurrency control mode is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// A lock is used to restrict access the target resource(s) by a maximum of one thread concurrently. This mode is suitable
        /// for most purposes.
        /// </summary>
        SingleThreadLock = 1,

        /// <summary>
        /// A spin lock is used to restrict access the target resource(s) by a maximum of one thread concurrently. This mode is
        /// suitable for fast operations.
        /// </summary>
        SingleThreadSpinLock = 2,

        /// <summary>
        /// A semaphore is used to restrict access the target resource(s) by a maximum of two threads concurrently.
        /// </summary>
        DuplexSemaphore = 3,

        /// <summary>
        /// A semaphore is used to restrict access the target resource(s) by a number of threads at a time equal to
        /// <see cref="Environment.ProcessorCount" />.
        /// </summary>
        ProcessorCountSemaphore = 4,

        /// <summary>
        /// No concurrency control is applied.
        /// </summary>
        Unconstrained = 5
    }
}