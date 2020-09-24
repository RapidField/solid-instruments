// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents a streaming data signal whose underlying source is an object collection.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the channel's output value.
    /// </typeparam>
    public class ObjectCollectionChannel<T> : Channel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCollectionChannel{T}" /> class.
        /// </summary>
        /// <param name="collection">
        /// The collection of objects comprising the signal source.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection" /> is <see langword="null" />.
        /// </exception>
        public ObjectCollectionChannel(T[] collection)
            : this(collection, DefaultInvalidReadBehavior, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCollectionChannel{T}" /> class.
        /// </summary>
        /// <param name="collection">
        /// The collection of objects comprising the signal source.
        /// </param>
        /// <param name="invalidReadBehavior">
        /// The behavior of the channel when an out-of-range read request is made. The default value is
        /// <see cref="InvalidReadBehavior.ReadSilence" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="invalidReadBehavior" /> is equal to <see cref="InvalidReadBehavior.Unspecified" />.
        /// </exception>
        public ObjectCollectionChannel(T[] collection, InvalidReadBehavior invalidReadBehavior)
            : this(collection, invalidReadBehavior, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCollectionChannel{T}" /> class.
        /// </summary>
        /// <param name="collection">
        /// The collection of objects comprising the signal source.
        /// </param>
        /// <param name="name">
        /// The name of the channel, or <see langword="null" /> to use the name of the channel type. The default value is
        /// <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection" /> is <see langword="null" />.
        /// </exception>
        public ObjectCollectionChannel(T[] collection, String name)
            : this(collection, DefaultInvalidReadBehavior, name)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCollectionChannel{T}" /> class.
        /// </summary>
        /// <param name="collection">
        /// The collection of objects comprising the signal source.
        /// </param>
        /// <param name="invalidReadBehavior">
        /// The behavior of the channel when an out-of-range read request is made. The default value is
        /// <see cref="InvalidReadBehavior.ReadSilence" />.
        /// </param>
        /// <param name="name">
        /// The name of the channel, or <see langword="null" /> to use the name of the channel type. The default value is
        /// <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="invalidReadBehavior" /> is equal to <see cref="InvalidReadBehavior.Unspecified" />.
        /// </exception>
        public ObjectCollectionChannel(T[] collection, InvalidReadBehavior invalidReadBehavior, String name)
            : base(invalidReadBehavior, name)
        {
            Collection = collection.RejectIf().IsNull(nameof(collection));
            OutputLength = Collection.Length;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ObjectCollectionChannel{T}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Attempts to read a discrete unit of output from the channel's output stream at the specified index.
        /// </summary>
        /// <param name="index">
        /// A zero-based index within the output stream at which to read.
        /// </param>
        /// <param name="outputValue">
        /// The resulting discrete unit of output.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the read operation was performed, otherwise <see langword="false" />.
        /// </returns>
        protected override sealed Boolean TryRead(Int32 index, out T outputValue)
        {
            outputValue = Collection[index];
            return true;
        }

        /// <summary>
        /// Attempts to read a range of discrete units of output from the channel's output stream at the specified index.
        /// </summary>
        /// <param name="startIndex">
        /// A zero-based index within the output stream at which to being reading.
        /// </param>
        /// <param name="count">
        /// The number of output values to read.
        /// </param>
        /// <param name="outputRange">
        /// An array into which the output range should be filled or copied.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the read operation was performed, otherwise <see langword="false" />.
        /// </returns>
        protected override sealed Boolean TryRead(Int32 startIndex, Int32 count, T[] outputRange)
        {
            for (var i = 0; i < count; i++)
            {
                outputRange[i] = Collection[startIndex + i];
            }

            return true;
        }

        /// <summary>
        /// Gets the number of discrete units in the output stream for the current <see cref="ObjectCollectionChannel{T}" />.
        /// </summary>
        public override sealed Int32 OutputLength
        {
            get;
        }

        /// <summary>
        /// Represents the collection of objects comprising the signal source.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly T[] Collection;
    }
}