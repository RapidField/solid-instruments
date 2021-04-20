// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a composable, configurable utility with disposable resources and exposes a lazily-loaded concurrency control
    /// mechanism.
    /// </summary>
    /// <typeparam name="TConfiguration">
    /// The type of the configuration information for the instrument.
    /// </typeparam>
    public abstract class BuildableInstrument<TConfiguration> : ConfigurableInstrument<TConfiguration>
         where TConfiguration : IInstrumentConfiguration, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildableInstrument{TConfiguration}" /> class.
        /// </summary>
        protected BuildableInstrument()
            : base(true)
        {
            return;
        }

        /// <summary>
        /// Configures the current <see cref="BuildableInstrument{TConfiguration}" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="BuildableInstrument{TConfiguration}" />.
        /// </param>
        protected sealed override void Configure(TConfiguration configuration)
        {
            try
            {
                foreach (var configurationAction in ConfigurationActions)
                {
                    configurationAction(configuration);
                }
            }
            finally
            {
                ConfigurationActions.Clear();
                base.Configure(configuration);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="BuildableInstrument{TConfiguration}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                ConfigurationActions.Clear();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Represents an ordered collection of actions that modify the instrument configuration.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly IList<Action<TConfiguration>> ConfigurationActions = new List<Action<TConfiguration>>();
    }
}