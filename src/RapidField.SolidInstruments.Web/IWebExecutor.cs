// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Web
{
    /// <summary>
    /// Prepares for and performs execution of a web application.
    /// </summary>
    public interface IWebExecutor : IInstrument
    {
        /// <summary>
        /// Begins execution of the web application.
        /// </summary>
        /// <exception cref="WebExectuionException">
        /// An exception was raised during execution of the web application.
        /// </exception>
        public void Execute();

        /// <summary>
        /// Begins execution of the web application.
        /// </summary>
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime, if any.
        /// </param>
        /// <exception cref="WebExectuionException">
        /// An exception was raised during execution of the web application.
        /// </exception>
        public void Execute(String[] commandLineArguments);

        /// <summary>
        /// Gets the name of the web application.
        /// </summary>
        public String ApplicationName
        {
            get;
        }
    }
}