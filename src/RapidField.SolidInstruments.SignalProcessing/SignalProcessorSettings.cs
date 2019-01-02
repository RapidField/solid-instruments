// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents operational settings for an <see cref="ISignalProcessor{TOutput, TSettings}" />.
    /// </summary>
    public abstract class SignalProcessorSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignalProcessorSettings" /> class.
        /// </summary>
        protected SignalProcessorSettings()
        {
            Initialize();
        }

        /// <summary>
        /// Resets the configuration values for the current <see cref="ISignalProcessor{TOutput, TSettings}" /> to their default
        /// initial state.
        /// </summary>
        public void Reset() => Initialize();

        /// <summary>
        /// Sets the configuration values for the current <see cref="ISignalProcessor{TOutput, TSettings}" /> to their default
        /// initial state.
        /// </summary>
        /// <remarks>
        /// This method is invoked by the constructor for <see cref="SignalProcessorSettings" /> to initialize settings, as well as
        /// by <see cref="Reset" /> to reset them.
        /// </remarks>
        protected abstract void Initialize();
    }
}