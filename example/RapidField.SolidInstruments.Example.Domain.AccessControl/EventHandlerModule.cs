// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.EventAuthoring.DotNetNative;
using System;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl
{
    /// <summary>
    /// Encapsulates container configuration for access control domain event handlers.
    /// </summary>
    public sealed class EventHandlerModule : DotNetNativeEventHandlerModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public EventHandlerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }
    }
}