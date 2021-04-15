// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a utility that employs the builder pattern to compose and configure a
    /// <see cref="BuildableInstrument{TConfiguration}" />.
    /// </summary>
    /// <remarks>
    /// <see cref="InstrumentBuilder{TConfiguration, TInstrument}" /> is the default implementation of
    /// <see cref="IInstrumentBuilder{TConfiguration, TInstrument}" />.
    /// </remarks>
    /// <typeparam name="TConfiguration">
    /// The type of the configuration information for the instrument.
    /// </typeparam>
    /// <typeparam name="TInstrument">
    /// The type of the instrument that the builder composes and configures.
    /// </typeparam>
    public abstract class InstrumentBuilder<TConfiguration, TInstrument> : ObjectBuilder<TInstrument>, IInstrumentBuilder<TConfiguration, TInstrument>
        where TConfiguration : IInstrumentConfiguration, new()
        where TInstrument : BuildableInstrument<TConfiguration>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstrumentBuilder{TConfiguration, TInstrument}" /> class.
        /// </summary>
        protected InstrumentBuilder()
            : this(DefaultApplicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstrumentBuilder{TConfiguration, TInstrument}" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected InstrumentBuilder(IConfiguration applicationConfiguration)
            : base()
        {
            ApplicationConfiguration = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument;
        }

        /// <summary>
        /// Applies the specified action to the instrument configuration for the current
        /// <see cref="InstrumentBuilder{TConfiguration, TInstrument}" />.
        /// </summary>
        /// <param name="configurationAction">
        /// An action that modifies the instrument configuration.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configurationAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected void Configure(Action<TConfiguration> configurationAction)
        {
            _ = configurationAction.RejectIf().IsNull(nameof(configurationAction));
            WithStateControl(() => ConfigurationActions.Add(configurationAction));
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="InstrumentBuilder{TConfiguration, TInstrument}" />.
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
        /// Produces the configured <typeparamref name="TInstrument" /> instance.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The configured <typeparamref name="TInstrument" /> instance.
        /// </returns>
        protected sealed override TInstrument ToResult(IConcurrencyControlToken controlToken)
        {
            var instrument = new TInstrument
            {
                ApplicationConfiguration = ApplicationConfiguration
            };

            foreach (var configurationAction in ConfigurationActions)
            {
                instrument.ConfigurationActions.Add(configurationAction);
            }

            return instrument;
        }

        /// <summary>
        /// Initializes a default <see cref="IConfiguration" /> instance.
        /// </summary>
        /// <returns>
        /// A default <see cref="IConfiguration" /> instance.
        /// </returns>
        [DebuggerHidden]
        private static IConfiguration InitializeDefaultApplicationConfiguration() => new ConfigurationBuilder().Build();

        /// <summary>
        /// Gets the type of the configuration information for the instrument.
        /// </summary>
        public Type ConfigurationType => ConfigurationTypeValue;

        /// <summary>
        /// Gets a default <see cref="IConfiguration" /> instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static IConfiguration DefaultApplicationConfiguration => LazyDefaultApplicationConfiguration.Value;

        /// <summary>
        /// Represents the type of the configuration information for the instrument.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ConfigurationTypeValue = typeof(TConfiguration);

        /// <summary>
        /// Represents a lazily-initialized default <see cref="IConfiguration" /> instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Lazy<IConfiguration> LazyDefaultApplicationConfiguration = new(InitializeDefaultApplicationConfiguration, LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Represents configuration information for the application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IConfiguration ApplicationConfiguration;

        /// <summary>
        /// Represents an ordered collection of actions that modify the instrument configuration.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<Action<TConfiguration>> ConfigurationActions = new List<Action<TConfiguration>>();
    }
}