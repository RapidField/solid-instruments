// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace RapidField.SolidInstruments.Messaging.DotNetNative
{
    /// <summary>
    /// Encapsulates native .NET container configuration for message listeners.
    /// </summary>
    public class DotNetNativeMessageListenerModule : DotNetNativeMessageHandlerModule, IMessageListenerModule<ServiceCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeMessageListenerModule" /> class.
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
        public DotNetNativeMessageListenerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, targetAssembly)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeMessageListenerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected DotNetNativeMessageListenerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
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
        protected sealed override void ConfigureMessageType(Type messageType, ServiceCollection configurator, IConfiguration applicationConfiguration)
        {
            if (IsRequestMessageType(messageType))
            {
                // TODO: Register request listener.
            }
            else if (IsEventMessageType(messageType))
            {
                // TODO: Register event listener.
            }
            else if (IsCommandMessageType(messageType))
            {
                // TODO: Register command listener.
            }
            else
            {
                // TODO: Register message listener.
            }
        }
    }
}