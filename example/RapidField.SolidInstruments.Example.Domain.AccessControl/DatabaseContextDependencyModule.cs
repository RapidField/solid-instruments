// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.DataAccess.DotNetNative.Ef;
using System;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl
{
    /// <summary>
    /// Encapsulates native .NET container configuration for the AccessControl database connection and related data access
    /// dependencies.
    /// </summary>
    public sealed class DatabaseContextDependencyModule : DotNetNativeEntityFrameworkDataStoreDependencyModule<DatabaseContext, DatabaseContextRepositoryFactory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContextDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public DatabaseContextDependencyModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration, DatabaseContext.DatabaseName)
        {
            return;
        }
    }
}