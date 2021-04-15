// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that configures and produces new <typeparamref name="TInstrument" /> instances.
    /// </summary>
    /// <typeparam name="TConfiguration">
    /// The type of the configuration information for the instrument.
    /// </typeparam>
    /// <typeparam name="TInstrument">
    /// The type of the instrument that the builder composes and configures.
    /// </typeparam>
    public interface IInstrumentBuilder<TConfiguration, TInstrument> : IObjectBuilder<TInstrument>, IInstrumentBuilder<TConfiguration>
        where TConfiguration : IInstrumentConfiguration, new()
        where TInstrument : BuildableInstrument<TConfiguration>, new()
    {
    }

    /// <summary>
    /// Represents an object that configures and produces new <see cref="ConfigurableInstrument{TConfiguration}" /> instances.
    /// </summary>
    /// <typeparam name="TConfiguration">
    /// The type of the configuration information for the instrument.
    /// </typeparam>
    public interface IInstrumentBuilder<TConfiguration> : IInstrumentBuilder
        where TConfiguration : IInstrumentConfiguration, new()
    {
        /// <summary>
        /// Gets the type of the configuration information for the instrument.
        /// </summary>
        public Type ConfigurationType
        {
            get;
        }
    }

    /// <summary>
    /// Represents an object that configures and produces new <see cref="IInstrument" /> instances.
    /// </summary>
    public interface IInstrumentBuilder : IObjectBuilder
    {
    }
}