// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a lazily-configurable utility with disposable resources and exposes a lazily-loaded concurrency control
    /// mechanism.
    /// </summary>
    /// <typeparam name="TConfiguration">
    /// The type of the configuration information for the instrument.
    /// </typeparam>
    public abstract class ConfigurableInstrument<TConfiguration> : Instrument
        where TConfiguration : IInstrumentConfiguration, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurableInstrument{TConfiguration}" /> class.
        /// </summary>
        /// <param name="isBuildable">
        /// A value indicating whether or not the instrument is a buildable instrument. The default value is
        /// <see langword="false" />.
        /// </param>
        [DebuggerHidden]
        internal ConfigurableInstrument(Boolean isBuildable)
            : this(isBuildable ? null : DefaultApplicationConfiguration, isBuildable)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurableInstrument{TConfiguration}" /> class.
        /// </summary>
        protected ConfigurableInstrument()
            : this(DefaultApplicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurableInstrument{TConfiguration}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected ConfigurableInstrument(IConfiguration applicationConfiguration)
            : this(applicationConfiguration, DefaultIsBuildableValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurableInstrument{TConfiguration}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="isBuildable">
        /// A value indicating whether or not the instrument is a buildable instrument. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="isBuildable" /> is <see langword="false" /> and <paramref name="applicationConfiguration" /> is
        /// <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private ConfigurableInstrument(IConfiguration applicationConfiguration, Boolean isBuildable)
            : base()
        {
            ApplicationConfiguration = isBuildable ? null : applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument;
            IsBuildable = isBuildable;
            LazyConfiguration = new(Configure, LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>
        /// Initializes a concurrency control mechanism that is used to manage state for the current <see cref="Instrument" />.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="SemaphoreSlim" /> class.
        /// </returns>
        /// <exception cref="ObjectConfigurationException">
        /// An exception was raised during configuration of the instrument.
        /// </exception>
        [DebuggerHidden]
        internal sealed override IConcurrencyControl InitializeStateControl()
        {
            StateControlMode = Configuration.StateControlMode;
            StateControlTimeoutThreshold = Configuration.StateControlTimeoutThreshold;
            return base.InitializeStateControl();
        }

        /// <summary>
        /// Configures the current <see cref="ConfigurableInstrument{TConfiguration}" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="ConfigurableInstrument{TConfiguration}" />.
        /// </param>
        protected virtual void Configure(TConfiguration configuration)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ConfigurableInstrument{TConfiguration}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Initializes a default <see cref="IConfiguration" /> instance.
        /// </summary>
        /// <returns>
        /// A default <see cref="IConfiguration" /> instance.
        /// </returns>
        [DebuggerHidden]
        private static IConfiguration InitializeDefaultApplicationConfiguration() => new ConfigurationBuilder().Build();

        /// <summary>
        /// Configures the current <see cref="ConfigurableInstrument{TConfiguration}" />.
        /// </summary>
        /// <returns>
        /// Configuration information for the current <see cref="ConfigurableInstrument{TConfiguration}" />.
        /// </returns>
        /// <exception cref="ObjectConfigurationException">
        /// An exception was raised during configuration of the instrument.
        /// </exception>
        [DebuggerHidden]
        private TConfiguration Configure()
        {
            try
            {
                var configuration = new TConfiguration
                {
                    Application = ApplicationConfiguration
                };

                Configure(configuration);
                return configuration;
            }
            catch (Exception exception)
            {
                throw new ObjectConfigurationException(GetType(), exception);
            }
        }

        /// <summary>
        /// Gets a default <see cref="IConfiguration" /> instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static IConfiguration DefaultApplicationConfiguration => LazyDefaultApplicationConfiguration.Value;

        /// <summary>
        /// Gets configuration information for the current <see cref="ConfigurableInstrument{TConfiguration}" />.
        /// </summary>
        /// <exception cref="ObjectConfigurationException">
        /// An exception was raised during configuration of the instrument.
        /// </exception>
        protected TConfiguration Configuration => LazyConfiguration.Value;

        /// <summary>
        /// Represents a value indicating whether or not the current instrument is a buildable instrument.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly Boolean IsBuildable;

        /// <summary>
        /// Represents configuration information for the application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal IConfiguration ApplicationConfiguration;

        /// <summary>
        /// Represents the default value indicating whether or not the current instrument is a buildable instrument.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultIsBuildableValue = false;

        /// <summary>
        /// Represents a lazily-initialized default <see cref="IConfiguration" /> instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Lazy<IConfiguration> LazyDefaultApplicationConfiguration = new(InitializeDefaultApplicationConfiguration, LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Represents lazily-initialized configuration information for the current
        /// <see cref="ConfigurableInstrument{TConfiguration}" />.
        /// </summary>
        /// <exception cref="ObjectConfigurationException">
        /// An exception was raised during configuration of the instrument.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<TConfiguration> LazyConfiguration;
    }
}