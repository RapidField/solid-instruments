// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Service
{
    /// <summary>
    /// Prepares for and performs execution of a service.
    /// </summary>
    public interface IServiceExecutor : IInstrument
    {
        /// <summary>
        /// Begins execution of the service and performs the service operations.
        /// </summary>
        /// <exception cref="ServiceExectuionException">
        /// An exception was raised during execution of the service.
        /// </exception>
        public void Execute();

        /// <summary>
        /// Begins execution of the service and performs the service operations.
        /// </summary>
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime, if any.
        /// </param>
        /// <exception cref="ServiceExectuionException">
        /// An exception was raised during execution of the service.
        /// </exception>
        public void Execute(String[] commandLineArguments);

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        public String ServiceName
        {
            get;
        }
    }
}