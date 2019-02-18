// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Example.ServiceApplication
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
            using (var serviceExecutor = new ExampleMessagingServiceExecutor())
            {
                serviceExecutor.Execute();
            }
        }
    }
}