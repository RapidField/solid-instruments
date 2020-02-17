// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Service
{
    /// <summary>
    /// Prepares for and performs execution of a service.
    /// </summary>
    public interface IServiceExecutor : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Begins execution of the service and performs the service operations.
        /// </summary>
        /// <exception cref="ServiceExectuionException">
        /// An exception was raised during execution of the service.
        /// </exception>
        void Execute();

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        String ServiceName
        {
            get;
        }
    }
}