// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.InversionOfControl.Autofac.UnitTests
{
    /// <summary>
    /// Represents an <see cref="Instrument" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedInstrument : Instrument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Instrument" /> class.
        /// </summary>
        public SimulatedInstrument()
            : base()
        {
            NullableIntegerValue = null;
        }

        /// <summary>
        /// Sets the value of <see cref="NullableIntegerValue" /> equal to the specified value.
        /// </summary>
        /// <param name="value">
        /// An <see cref="Int32" /> value.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void StoreIntegerValue(Int32 value)
        {
            RejectIfDisposed();
            NullableIntegerValue = value;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedInstrument" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                NullableIntegerValue = null;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets a nullable <see cref="Int32" /> value.
        /// </summary>
        public Int32? NullableIntegerValue
        {
            get;
            private set;
        }
    }
}