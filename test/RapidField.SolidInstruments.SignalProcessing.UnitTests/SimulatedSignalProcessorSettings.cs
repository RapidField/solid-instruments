// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.SignalProcessing.UnitTests
{
    /// <summary>
    /// Represents a <see cref="SignalProcessorSettings" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedSignalProcessorSettings : SignalProcessorSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedSignalProcessorSettings" /> class.
        /// </summary>
        public SimulatedSignalProcessorSettings()
            : base()
        {
            return;
        }

        /// <summary>
        /// Sets the configuration values for the current <see cref="ISignalProcessor{TOutput, TSettings}" /> to their default
        /// initial state.
        /// </summary>
        /// <remarks>
        /// This method is invoked by the constructor for <see cref="SignalProcessorSettings" /> to initialize settings, as well as
        /// by <see cref="Reset" /> to reset them.
        /// </remarks>
        protected override void Initialize() => Factor = DefaultFactor;

        /// <summary>
        /// Gets or sets a factor by which to multiply the read result.
        /// </summary>
        public Int32 Factor
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default factor by which to multiply the read results.
        /// </summary>
        internal const Int32 DefaultFactor = 3;
    }
}