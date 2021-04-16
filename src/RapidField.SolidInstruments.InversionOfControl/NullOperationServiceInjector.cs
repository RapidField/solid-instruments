// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Represents a <see cref="ServiceInjector{TConfigurator}" /> that performs a null operation.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures a container.
    /// </typeparam>
    internal sealed class NullOperationServiceInjector<TConfigurator> : ServiceInjector<TConfigurator>
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullOperationServiceInjector{TConfigurator}" /> class.
        /// </summary>
        public NullOperationServiceInjector()
            : base(EmptyServiceCollection)
        {
            return;
        }

        /// <summary>
        /// Adds a collection of service descriptors to a <see cref="DependencyContainer{TContainer, TConfigurator}" />
        /// configurator.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures a container.
        /// </param>
        /// <param name="serviceDescriptors">
        /// a collection of service descriptors that are added to the configurator.
        /// </param>
        [DebuggerHidden]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected sealed override void Inject(TConfigurator configurator, IServiceCollection serviceDescriptors)
        {
            return;
        }

        /// <summary>
        /// Represents an empty service descriptor collection.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly IServiceCollection EmptyServiceCollection = new ServiceCollection();
    }
}