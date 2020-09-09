﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.Service
{
    /// <summary>
    /// Houses the entry point for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Begins execution of the application.
        /// </summary>
        /// <param name="args">
        /// Command line arguments that are provided at runtime.
        /// </param>
        public static void Main(String[] args)
        {
            using var serviceExecutor = new ApplicationServiceExecutor(ServiceName);
            serviceExecutor.Execute(args);
        }

        /// <summary>
        /// Represents the name of the service that is hosted by this application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly String ServiceName = $"{nameof(AccessControl)} Service";
    }
}