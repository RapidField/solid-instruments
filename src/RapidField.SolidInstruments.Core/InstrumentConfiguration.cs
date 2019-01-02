// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents configuration information for a <see cref="ConfigurableInstrument{TConfiguration}" /> instance.
    /// </summary>
    public abstract class InstrumentConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstrumentConfiguration" /> class.
        /// </summary>
        protected InstrumentConfiguration()
        {
            Application = null;
            StateControlMode = Instrument.DefaultStateControlMode;
            StateControlTimeoutThreshold = Instrument.DefaultStateControlTimeoutThreshold;
        }

        /// <summary>
        /// Gets configuration information for the application.
        /// </summary>
        public IConfiguration Application
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the concurrency control mode that is used to manage state for the associated
        /// <see cref="ConfigurableInstrument{TConfiguration}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ConcurrencyControlMode StateControlMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum length of time that the instrument's state control may block a thread before raising an
        /// exception, or <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking is permitted.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public TimeSpan StateControlTimeoutThreshold
        {
            get;
            set;
        }
    }
}