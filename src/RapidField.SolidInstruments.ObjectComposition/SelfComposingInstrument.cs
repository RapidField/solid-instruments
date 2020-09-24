// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents an instrument that configures and lazily composes its own dependencies.
    /// </summary>
    public abstract class SelfComposingInstrument : Instrument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelfComposingInstrument" /> class.
        /// </summary>
        protected SelfComposingInstrument()
            : base()
        {
            LazyContainer = new Lazy<IObjectContainer>(InitializeContainer, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfComposingInstrument" /> class.
        /// </summary>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.SingleThreadLock" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" />.
        /// </exception>
        protected SelfComposingInstrument(ConcurrencyControlMode stateControlMode)
            : base(stateControlMode)
        {
            LazyContainer = new Lazy<IObjectContainer>(InitializeContainer, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfComposingInstrument" /> class.
        /// </summary>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.SingleThreadLock" />.
        /// </param>
        /// <param name="stateControlTimeoutThreshold">
        /// The maximum length of time that the instrument's state control may block a thread before raising an exception, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking is permitted. The default value is
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" /> -or-
        /// <paramref name="stateControlTimeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> and is not equal
        /// to <see cref="Timeout.InfiniteTimeSpan" />.
        /// </exception>
        protected SelfComposingInstrument(ConcurrencyControlMode stateControlMode, TimeSpan stateControlTimeoutThreshold)
            : base(stateControlMode, stateControlTimeoutThreshold)
        {
            LazyContainer = new Lazy<IObjectContainer>(InitializeContainer, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Configures and finalizes the object container that produces and manages object instances for the current
        /// <see cref="SelfComposingInstrument" />.
        /// </summary>
        /// <param name="containerBuilder">
        /// A builder that is used to configure and finalize the container.
        /// </param>
        /// <returns>
        /// An object container that produces and manages object instances for the current <see cref="SelfComposingInstrument" />.
        /// </returns>
        protected abstract IObjectContainer BuildContainer(ObjectContainerBuilder containerBuilder);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SelfComposingInstrument" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                LazyContainer?.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Initializes a lazily-initializes object container that produces and manages object instances for the current
        /// <see cref="SelfComposingInstrument" />.
        /// </summary>
        /// <returns>
        /// A lazily-initializes object container that produces and manages object instances for the current
        /// <see cref="SelfComposingInstrument" />.
        /// </returns>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised while configuring or finalizing the container.
        /// </exception>
        [DebuggerHidden]
        private IObjectContainer InitializeContainer()
        {
            using (var containerBuilder = new ObjectContainerBuilder())
            {
                try
                {
                    return BuildContainer(containerBuilder);
                }
                catch (ObjectBuilderException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new ObjectBuilderException(containerBuilder.GetType(), exception);
                }
            }
        }

        /// <summary>
        /// Gets an object container that produces and manages object instances for the current
        /// <see cref="SelfComposingInstrument" />.
        /// </summary>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised while configuring or finalizing the container.
        /// </exception>
        protected IObjectContainer Container => LazyContainer.Value;

        /// <summary>
        /// Represents a lazily-initializes object container that produces and manages object instances for the current
        /// <see cref="SelfComposingInstrument" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IObjectContainer> LazyContainer;
    }
}