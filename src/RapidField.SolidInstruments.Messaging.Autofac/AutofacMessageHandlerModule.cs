// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.InversionOfControl.Autofac;
using RapidField.SolidInstruments.Messaging.CommandMessages;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RapidField.SolidInstruments.Messaging.Autofac
{
    /// <summary>
    /// Encapsulates Autofac container configuration for message handlers.
    /// </summary>
    public abstract class AutofacMessageHandlerModule : AutofacDependencyModule, IMessageHandlerModule<ContainerBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacMessageHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected AutofacMessageHandlerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            TargetAssembly = null; // This causes the target assembly to be resolved at runtime.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacMessageHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="targetAssembly">
        /// The assembly from which message types are identified for handler registration.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="targetAssembly" /> is
        /// <see langword="null" />.
        /// </exception>
        protected AutofacMessageHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration)
        {
            TargetAssembly = targetAssembly.RejectIf().IsNull(nameof(targetAssembly));
        }

        /// <summary>
        /// Determines whether or not the specified message type is a command message type.
        /// </summary>
        /// <param name="messageType">
        /// The type to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="messageType" /> implements <see cref="ICommandMessage{TCommand}" />,
        /// otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageType" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal Boolean IsCommandMessageType(Type messageType) => CommandMessageInterfaceType.IsAssignableFrom(messageType.RejectIf().IsNull(nameof(messageType)));

        /// <summary>
        /// Determines whether or not the specified message type is an event message type.
        /// </summary>
        /// <param name="messageType">
        /// The type to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="messageType" /> implements <see cref="IEventMessage{TEvent}" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageType" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal Boolean IsEventMessageType(Type messageType) => EventMessageInterfaceType.IsAssignableFrom(messageType.RejectIf().IsNull(nameof(messageType)));

        /// <summary>
        /// Determines whether or not the specified message type is a request message type.
        /// </summary>
        /// <param name="messageType">
        /// The type to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="messageType" /> implements <see cref="IRequestMessage{TResponseMessage}" />,
        /// otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageType" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal Boolean IsRequestMessageType(Type messageType) => RequestMessageInterfaceType.IsAssignableFrom(messageType.RejectIf().IsNull(nameof(messageType)));

        /// <summary>
        /// Configures the module.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected sealed override void Configure(ContainerBuilder configurator, IConfiguration applicationConfiguration)
        {
            foreach (var messageType in MessageTypes)
            {
                ConfigureMessageType(messageType, configurator, applicationConfiguration);
            }
        }

        /// <summary>
        /// Registers one or more handler types for the specified message type.
        /// </summary>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected abstract void ConfigureMessageType(Type messageType, ContainerBuilder configurator, IConfiguration applicationConfiguration);

        /// <summary>
        /// Gets the collection of non-abstract public message types defined by <see cref="TargetAssembly" /> for which handler
        /// types are registered by the current <see cref="IMessageHandlerModule" />.
        /// </summary>
        public IEnumerable<Type> MessageTypes => TargetAssembly.GetTypes().Where(type => type.IsPublic && type.IsClass && type.IsAbstract == false && MessageBaseInterfaceType.IsAssignableFrom(type));

        /// <summary>
        /// Gets the assembly from which message types are identified for handler registration.
        /// </summary>
        public Assembly TargetAssembly
        {
            get
            {
                if (TargetAssemblyReference is null)
                {
                    TargetAssemblyReference = Assembly.GetAssembly(GetType());
                }

                return TargetAssemblyReference;
            }
            private set => TargetAssemblyReference = value;
        }

        /// <summary>
        /// Represents the <see cref="ICommandMessage{TCommand}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandMessageInterfaceType = typeof(ICommandMessage<>);

        /// <summary>
        /// Represents the <see cref="IEventMessage{TEvent}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventMessageInterfaceType = typeof(IEventMessage<>);

        /// <summary>
        /// Represents the <see cref="IMessageBase" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type MessageBaseInterfaceType = typeof(IMessageBase);

        /// <summary>
        /// Represents the <see cref="IRequestMessage{TResponseMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type RequestMessageInterfaceType = typeof(IRequestMessage<>);

        /// <summary>
        /// Represents the assembly from which message types are identified for handler registration.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Assembly TargetAssemblyReference;
    }
}