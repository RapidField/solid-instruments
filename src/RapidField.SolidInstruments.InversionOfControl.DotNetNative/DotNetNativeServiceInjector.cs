// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl.DotNetNative
{
    /// <summary>
    /// Facilitates injection of a collection of service descriptors into a <see cref="ServiceCollection" />.
    /// </summary>
    public sealed class DotNetNativeServiceInjector : ServiceInjector<ServiceCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeServiceInjector" /> class.
        /// </summary>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors that are injected to a configurator.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceDescriptors" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal DotNetNativeServiceInjector(IServiceCollection serviceDescriptors)
            : base(serviceDescriptors)
        {
            return;
        }

        /// <summary>
        /// Adds a collection of service descriptors to a <see cref="ServiceCollection" />.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures a container.
        /// </param>
        /// <param name="serviceDescriptors">
        /// a collection of service descriptors that are added to the configurator.
        /// </param>
        protected sealed override void Inject(ServiceCollection configurator, IServiceCollection serviceDescriptors) => configurator.Add(serviceDescriptors);
    }
}