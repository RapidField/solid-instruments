// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.ObjectComposition;
using System;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Encapsulates creation of new <see cref="IDurableMessageQueue" /> instances that map to database entities.
    /// </summary>
    /// <remarks>
    /// <see cref="DurableMessageQueueFactory" /> is the default implementation of <see cref="IDurableMessageQueueFactory" />.
    /// </remarks>
    public abstract class DurableMessageQueueFactory : ObjectFactory<IDurableMessageQueue>, IDurableMessageQueueFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueueFactory" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected DurableMessageQueueFactory(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DurableMessageQueueFactory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}