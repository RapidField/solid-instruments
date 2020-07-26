// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Represents a configurable dependency resolution system.
    /// </summary>
    public interface IDependencyEngine : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Gets the engine's dependency container.
        /// </summary>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while configuring the container.
        /// </exception>
        public IDependencyContainer Container
        {
            get;
        }

        /// <summary>
        /// Gets the engine's service provider.
        /// </summary>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while configuring the container.
        /// </exception>
        public IServiceProvider Provider
        {
            get;
        }
    }
}