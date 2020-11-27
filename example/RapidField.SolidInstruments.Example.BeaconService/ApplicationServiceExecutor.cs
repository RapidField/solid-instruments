﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Messaging.DotNetNative.Service;
using System;

namespace RapidField.SolidInstruments.Example.BeaconService
{
    /// <summary>
    /// Prepares for and performs execution of the beacon service.
    /// </summary>
    public sealed class ApplicationServiceExecutor : DotNetNativeBeaconServiceExecutor<ApplicationDependencyPackage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationServiceExecutor" /> class.
        /// </summary>
        public ApplicationServiceExecutor()
            : base(true, false, false, false, false)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ApplicationServiceExecutor" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// When overridden by a derived class, gets a copyright notice which is written to the console at the start of service
        /// execution.
        /// </summary>
        protected override sealed String CopyrightNotice => "Copyright (c) RapidField LLC. All rights reserved.";

        /// <summary>
        /// When overridden by a derived class, gets a product name associated with the service which is written to the console at
        /// the start of service execution.
        /// </summary>
        protected override sealed String ProductName => "Solid Instruments";
    }
}