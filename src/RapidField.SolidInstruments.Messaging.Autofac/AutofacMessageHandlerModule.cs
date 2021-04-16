// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.InversionOfControl.Autofac;
using RapidField.SolidInstruments.Messaging.Autofac.Extensions;
using RapidField.SolidInstruments.Messaging.CommandMessages;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RapidField.SolidInstruments.Messaging.Autofac
{
    /// <summary>
    /// Encapsulates Autofac container configuration for message handlers.
    /// </summary>
    public class AutofacMessageHandlerModule : AutofacDependencyModule, IMessageHandlerModule<ContainerBuilder>
    {
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
        public AutofacMessageHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : this(applicationConfiguration, DefaultRegistersMessageListenerTypesValue, DefaultRegistersMessageTransmitterTypesValue, targetAssembly)
        {
            return;
        }

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
            : this(applicationConfiguration, DefaultRegistersMessageListenerTypesValue, DefaultRegistersMessageTransmitterTypesValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacMessageHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="registersMessageListenerTypes">
        /// A value indicating whether or not the module registers message listener types. The default value is
        /// <see langword="true" />.
        /// </param>
        /// <param name="registersMessageTransmitterTypes">
        /// A value indicating whether or not the module registers message transmitter types. The default value is
        /// <see langword="true" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected AutofacMessageHandlerModule(IConfiguration applicationConfiguration, Boolean registersMessageListenerTypes, Boolean registersMessageTransmitterTypes)
            : base(applicationConfiguration)
        {
            RegistersMessageListenerTypes = registersMessageListenerTypes;
            RegistersMessageTransmitterTypes = registersMessageTransmitterTypes;
            TargetAssembly = null; // This causes the target assembly to be resolved at runtime.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacMessageHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="registersMessageListenerTypes">
        /// A value indicating whether or not the module registers message listener types. The default value is
        /// <see langword="true" />.
        /// </param>
        /// <param name="registersMessageTransmitterTypes">
        /// A value indicating whether or not the module registers message transmitter types. The default value is
        /// <see langword="true" />.
        /// </param>
        /// <param name="targetAssembly">
        /// The assembly from which message types are identified for handler registration.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="targetAssembly" /> is
        /// <see langword="null" />.
        /// </exception>
        protected AutofacMessageHandlerModule(IConfiguration applicationConfiguration, Boolean registersMessageListenerTypes, Boolean registersMessageTransmitterTypes, Assembly targetAssembly)
            : base(applicationConfiguration)
        {
            RegistersMessageListenerTypes = registersMessageListenerTypes;
            RegistersMessageTransmitterTypes = registersMessageTransmitterTypes;
            TargetAssembly = targetAssembly.RejectIf().IsNull(nameof(targetAssembly));
        }

        /// <summary>
        /// Extracts the message type associated with the specified message handler type.
        /// </summary>
        /// <param name="messageHandlerType">
        /// The message handler type to interrogate.
        /// </param>
        /// <returns>
        /// The corresponding message type, or <see cref="Nix.Type" /> if <paramref name="messageHandlerType" /> does not implement
        /// <see cref="IMessageHandler{TMessage}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandlerType" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal static Type GetMessageType(Type messageHandlerType)
        {
            var messageHandlerInterfaceType = messageHandlerType?.GetInterfaces().Where(implementedInterface => MessageHandlerInterfaceType.IsAssignableFrom(implementedInterface) && implementedInterface.GenericTypeArguments.Length == 1).FirstOrDefault();
            return messageHandlerInterfaceType?.GenericTypeArguments.FirstOrDefault() ?? Nix.Type;
        }

        /// <summary>
        /// Extracts the response message type associated with the specified request message type.
        /// </summary>
        /// <param name="requestMessageType">
        /// The request message type to interrogate.
        /// </param>
        /// <returns>
        /// The corresponding response message type, or <see cref="Nix.Type" /> if <paramref name="requestMessageType" /> does not
        /// implement <see cref="IRequestMessage{TResponseMessage}" />.
        /// </returns>
        [DebuggerHidden]
        internal static Type GetResponseMessageType(Type requestMessageType)
        {
            var requestMessageInterfaceType = requestMessageType?.GetInterfaces().Where(implementedInterface => RequestMessageBaseInterfaceType.IsAssignableFrom(implementedInterface) && implementedInterface.GenericTypeArguments.Length == 1).FirstOrDefault();
            return requestMessageInterfaceType?.GenericTypeArguments.FirstOrDefault() ?? Nix.Type;
        }

        /// <summary>
        /// Configures the module.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override void Configure(ContainerBuilder configurator, IConfiguration applicationConfiguration)
        {
            RegisterMessageListenerTypes(configurator);
            RegisterMessageTransmitterTypes(configurator);
        }

        /// <summary>
        /// Registers the specified command message listener types with the specified container configurator.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="messageListenerTypes">
        /// The message handler types to register.
        /// </param>
        /// <param name="registeredTypeNames">
        /// A list of the full names of message handler types that have been registered by the module.
        /// </param>
        [DebuggerHidden]
        private static void RegisterCommandMessageListenerTypes(ContainerBuilder configurator, IEnumerable<Type> messageListenerTypes, IList<String> registeredTypeNames)
        {
            foreach (var messageListenerType in messageListenerTypes)
            {
                var typeName = messageListenerType.ToString();

                if (registeredTypeNames.Contains(typeName))
                {
                    continue;
                }

                var commandMessageType = GetMessageType(messageListenerType);

                if (commandMessageType == Nix.Type)
                {
                    continue;
                }

                configurator.RegisterCommandMessageListener(commandMessageType, messageListenerType);
                registeredTypeNames.Add(typeName);
            }
        }

        /// <summary>
        /// Registers the specified command message transmitter types with the specified container configurator.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="messageTransmitterTypes">
        /// The message handler types to register.
        /// </param>
        /// <param name="registeredTypeNames">
        /// A list of the full names of message handler types that have been registered by the module.
        /// </param>
        [DebuggerHidden]
        private static void RegisterCommandMessageTransmitterTypes(ContainerBuilder configurator, IEnumerable<Type> messageTransmitterTypes, IList<String> registeredTypeNames)
        {
            foreach (var messageTransmitterType in messageTransmitterTypes)
            {
                var typeName = messageTransmitterType.ToString();

                if (registeredTypeNames.Contains(typeName))
                {
                    continue;
                }

                var commandMessageType = GetMessageType(messageTransmitterType);

                if (commandMessageType == Nix.Type)
                {
                    continue;
                }

                configurator.RegisterCommandMessageTransmitter(commandMessageType, messageTransmitterType);
                registeredTypeNames.Add(typeName);
            }
        }

        /// <summary>
        /// Registers the specified custom message listener types with the specified container configurator.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="messageListenerTypes">
        /// The message handler types to register.
        /// </param>
        /// <param name="registeredTypeNames">
        /// A list of the full names of message handler types that have been registered by the module.
        /// </param>
        [DebuggerHidden]
        private static void RegisterCustomMessageListenerTypes(ContainerBuilder configurator, IEnumerable<Type> messageListenerTypes, IList<String> registeredTypeNames)
        {
            foreach (var messageListenerType in messageListenerTypes)
            {
                var typeName = messageListenerType.ToString();

                if (registeredTypeNames.Contains(typeName))
                {
                    continue;
                }

                var customMessageType = GetMessageType(messageListenerType);

                if (customMessageType == Nix.Type)
                {
                    continue;
                }

                configurator.RegisterMessageListener(customMessageType, messageListenerType);
                registeredTypeNames.Add(typeName);
            }
        }

        /// <summary>
        /// Registers the specified custom message transmitter types with the specified container configurator.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="messageTransmitterTypes">
        /// The message handler types to register.
        /// </param>
        /// <param name="registeredTypeNames">
        /// A list of the full names of message handler types that have been registered by the module.
        /// </param>
        [DebuggerHidden]
        private static void RegisterCustomMessageTransmitterTypes(ContainerBuilder configurator, IEnumerable<Type> messageTransmitterTypes, IList<String> registeredTypeNames)
        {
            foreach (var messageTransmitterType in messageTransmitterTypes)
            {
                var typeName = messageTransmitterType.ToString();

                if (registeredTypeNames.Contains(typeName))
                {
                    continue;
                }

                var customMessageType = GetMessageType(messageTransmitterType);

                if (customMessageType == Nix.Type)
                {
                    continue;
                }

                configurator.RegisterMessageTransmitter(customMessageType, messageTransmitterType);
                registeredTypeNames.Add(typeName);
            }
        }

        /// <summary>
        /// Registers the specified event message listener types with the specified container configurator.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="messageListenerTypes">
        /// The message handler types to register.
        /// </param>
        /// <param name="registeredTypeNames">
        /// A list of the full names of message handler types that have been registered by the module.
        /// </param>
        [DebuggerHidden]
        private static void RegisterEventMessageListenerTypes(ContainerBuilder configurator, IEnumerable<Type> messageListenerTypes, IList<String> registeredTypeNames)
        {
            foreach (var messageListenerType in messageListenerTypes)
            {
                var typeName = messageListenerType.ToString();

                if (registeredTypeNames.Contains(typeName))
                {
                    continue;
                }

                var eventMessageType = GetMessageType(messageListenerType);

                if (eventMessageType == Nix.Type)
                {
                    continue;
                }

                configurator.RegisterEventMessageListener(eventMessageType, messageListenerType);
                registeredTypeNames.Add(typeName);
            }
        }

        /// <summary>
        /// Registers the specified event message transmitter types with the specified container configurator.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="messageTransmitterTypes">
        /// The message handler types to register.
        /// </param>
        /// <param name="registeredTypeNames">
        /// A list of the full names of message handler types that have been registered by the module.
        /// </param>
        [DebuggerHidden]
        private static void RegisterEventMessageTransmitterTypes(ContainerBuilder configurator, IEnumerable<Type> messageTransmitterTypes, IList<String> registeredTypeNames)
        {
            foreach (var messageTransmitterType in messageTransmitterTypes)
            {
                var typeName = messageTransmitterType.ToString();

                if (registeredTypeNames.Contains(typeName))
                {
                    continue;
                }

                var eventMessageType = GetMessageType(messageTransmitterType);

                if (eventMessageType == Nix.Type)
                {
                    continue;
                }

                configurator.RegisterEventMessageTransmitter(eventMessageType, messageTransmitterType);
                registeredTypeNames.Add(typeName);
            }
        }

        /// <summary>
        /// Registers the specified request message listener types with the specified container configurator.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="messageListenerTypes">
        /// The message handler types to register.
        /// </param>
        /// <param name="registeredTypeNames">
        /// A list of the full names of message handler types that have been registered by the module.
        /// </param>
        [DebuggerHidden]
        private static void RegisterRequestMessageListenerTypes(ContainerBuilder configurator, IEnumerable<Type> messageListenerTypes, IList<String> registeredTypeNames)
        {
            foreach (var messageListenerType in messageListenerTypes)
            {
                var typeName = messageListenerType.ToString();

                if (registeredTypeNames.Contains(typeName))
                {
                    continue;
                }

                var requestMessageType = GetMessageType(messageListenerType);
                var responseMessageType = GetResponseMessageType(requestMessageType);

                if (requestMessageType == Nix.Type || responseMessageType == Nix.Type)
                {
                    continue;
                }

                configurator.RegisterRequestMessageListener(requestMessageType, responseMessageType, messageListenerType);
                registeredTypeNames.Add(typeName);
            }
        }

        /// <summary>
        /// Registers the specified request message transmitter types with the specified container configurator.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="messageTransmitterTypes">
        /// The message handler types to register.
        /// </param>
        /// <param name="registeredTypeNames">
        /// A list of the full names of message handler types that have been registered by the module.
        /// </param>
        [DebuggerHidden]
        private static void RegisterRequestMessageTransmitterTypes(ContainerBuilder configurator, IEnumerable<Type> messageTransmitterTypes, IList<String> registeredTypeNames)
        {
            foreach (var messageTransmitterType in messageTransmitterTypes)
            {
                var typeName = messageTransmitterType.ToString();

                if (registeredTypeNames.Contains(typeName))
                {
                    continue;
                }

                var requestMessageType = GetMessageType(messageTransmitterType);
                var responseMessageType = GetResponseMessageType(requestMessageType);

                if (requestMessageType == Nix.Type || responseMessageType == Nix.Type)
                {
                    continue;
                }

                configurator.RegisterRequestMessageTransmitter(requestMessageType, responseMessageType, messageTransmitterType);
                registeredTypeNames.Add(typeName);
            }
        }

        /// <summary>
        /// Configures the module for message listener types defined by <see cref="TargetAssembly" />.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        [DebuggerHidden]
        private void RegisterMessageListenerTypes(ContainerBuilder configurator)
        {
            if (RegistersMessageListenerTypes)
            {
                var registeredTypeNames = new List<String>();
                RegisterRequestMessageListenerTypes(configurator, RequestMessageListenerTypes, registeredTypeNames);
                RegisterEventMessageListenerTypes(configurator, EventMessageListenerTypes, registeredTypeNames);
                RegisterCommandMessageListenerTypes(configurator, CommandMessageListenerTypes, registeredTypeNames);
                RegisterCustomMessageListenerTypes(configurator, MessageListenerTypes, registeredTypeNames);
            }
        }

        /// <summary>
        /// Configures the module for message transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        [DebuggerHidden]
        private void RegisterMessageTransmitterTypes(ContainerBuilder configurator)
        {
            if (RegistersMessageTransmitterTypes)
            {
                var registeredTypeNames = new List<String>();
                RegisterRequestMessageTransmitterTypes(configurator, RequestMessageTransmitterTypes, registeredTypeNames);
                RegisterEventMessageTransmitterTypes(configurator, EventMessageTransmitterTypes, registeredTypeNames);
                RegisterCommandMessageTransmitterTypes(configurator, CommandMessageTransmitterTypes, registeredTypeNames);
                RegisterCustomMessageTransmitterTypes(configurator, MessageTransmitterTypes, registeredTypeNames);
            }
        }

        /// <summary>
        /// Gets the collection of non-abstract public command message listener types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> CommandMessageListenerTypes => MessageListenerTypes.Where(type => CommandMessageListenerInterfaceType.IsAssignableFrom(type));

        /// <summary>
        /// Gets the collection of non-abstract public command message transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> CommandMessageTransmitterTypes => MessageTransmitterTypes.Where(type => CommandMessageTransmitterInterfaceType.IsAssignableFrom(type));

        /// <summary>
        /// Gets the collection of non-abstract public event message listener types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> EventMessageListenerTypes => MessageListenerTypes.Where(type => EventMessageListenerInterfaceType.IsAssignableFrom(type));

        /// <summary>
        /// Gets the collection of non-abstract public event message transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> EventMessageTransmitterTypes => MessageTransmitterTypes.Where(type => EventMessageTransmitterInterfaceType.IsAssignableFrom(type));

        /// <summary>
        /// Gets the collection of non-abstract public message handler types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> MessageHandlerTypes => TargetAssembly.GetTypes().Where(type => type.IsPublic && type.IsClass && type.IsAbstract is false && MessageHandlerInterfaceType.IsAssignableFrom(type));

        /// <summary>
        /// Gets the collection of non-abstract public message listener types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> MessageListenerTypes => MessageHandlerTypes.Where(type => MessageListenerInterfaceType.IsAssignableFrom(type));

        /// <summary>
        /// Gets the collection of non-abstract public message transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> MessageTransmitterTypes => MessageHandlerTypes.Where(type => MessageTransmitterInterfaceType.IsAssignableFrom(type));

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="AutofacMessageHandlerModule" /> registers message listener
        /// types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public Boolean RegistersMessageListenerTypes
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="AutofacMessageHandlerModule" /> registers message
        /// transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public Boolean RegistersMessageTransmitterTypes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of non-abstract public request message listener types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> RequestMessageListenerTypes => MessageListenerTypes.Where(type => RequestListenerInterfaceType.IsAssignableFrom(type));

        /// <summary>
        /// Gets the collection of non-abstract public request message transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        public IEnumerable<Type> RequestMessageTransmitterTypes => MessageTransmitterTypes.Where(type => RequestTransmitterInterfaceType.IsAssignableFrom(type));

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
        /// Represents a default value indicating whether or not the current <see cref="AutofacMessageHandlerModule" /> registers
        /// message listener types defined by <see cref="TargetAssembly" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultRegistersMessageListenerTypesValue = true;

        /// <summary>
        /// Represents a default value indicating whether or not the current <see cref="AutofacMessageHandlerModule" /> registers
        /// message transmitter types defined by <see cref="TargetAssembly" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultRegistersMessageTransmitterTypesValue = true;

        /// <summary>
        /// Represents the <see cref="ICommandMessageListener" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandMessageListenerInterfaceType = typeof(ICommandMessageListener);

        /// <summary>
        /// Represents the <see cref="ICommandMessageTransmitter" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandMessageTransmitterInterfaceType = typeof(ICommandMessageTransmitter);

        /// <summary>
        /// Represents the <see cref="IEventMessageListener" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventMessageListenerInterfaceType = typeof(IEventMessageListener);

        /// <summary>
        /// Represents the <see cref="IEventMessageTransmitter" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventMessageTransmitterInterfaceType = typeof(IEventMessageTransmitter);

        /// <summary>
        /// Represents the <see cref="IMessageHandler" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type MessageHandlerInterfaceType = typeof(IMessageHandler);

        /// <summary>
        /// Represents the <see cref="IMessageListener" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type MessageListenerInterfaceType = typeof(IMessageListener);

        /// <summary>
        /// Represents the <see cref="IMessageTransmitter" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type MessageTransmitterInterfaceType = typeof(IMessageTransmitter);

        /// <summary>
        /// Represents the <see cref="IRequestListener" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type RequestListenerInterfaceType = typeof(IRequestListener);

        /// <summary>
        /// Represents the <see cref="IRequestMessageBase" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type RequestMessageBaseInterfaceType = typeof(IRequestMessageBase);

        /// <summary>
        /// Represents the <see cref="IRequestTransmitter" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type RequestTransmitterInterfaceType = typeof(IRequestTransmitter);

        /// <summary>
        /// Represents the assembly from which message types are identified for handler registration.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Assembly TargetAssemblyReference;
    }
}