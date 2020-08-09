// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Encapsulates container configuration for message handlers.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IMessageHandlerModule<TConfigurator> : IMessageHandlerModule
        where TConfigurator : class, new()
    {
    }

    /// <summary>
    /// Encapsulates container configuration for message handlers.
    /// </summary>
    public interface IMessageHandlerModule : IDependencyModule
    {
        /// <summary>
        /// Gets the collection of non-abstract public command message listener types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> CommandMessageListenerTypes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of non-abstract public command message transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> CommandMessageTransmitterTypes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of non-abstract public event message listener types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> EventMessageListenerTypes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of non-abstract public event message transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> EventMessageTransmitterTypes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of non-abstract public message handler types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> MessageHandlerTypes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of non-abstract public message listener types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> MessageListenerTypes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of non-abstract public message transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> MessageTransmitterTypes
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="IMessageHandlerModule" /> registers message listener types
        /// defined by <see cref="TargetAssembly" />.
        /// </summary>
        public Boolean RegistersMessageListenerTypes
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="IMessageHandlerModule" /> registers message transmitter
        /// types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public Boolean RegistersMessageTransmitterTypes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of non-abstract public request message listener types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> RequestMessageListenerTypes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of non-abstract public request message transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> RequestMessageTransmitterTypes
        {
            get;
        }

        /// <summary>
        /// Gets the assembly from which message types are identified for handler registration.
        /// </summary>
        public Assembly TargetAssembly
        {
            get;
        }
    }
}