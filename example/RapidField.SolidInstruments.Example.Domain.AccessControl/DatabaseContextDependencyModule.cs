// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.DataAccess.DotNetNative.Ef;
using RapidField.SolidInstruments.DataAccess.DotNetNative.Extensions;
using RapidField.SolidInstruments.Example.Domain.AccessControl.Repositories;
using System;
using UserModel = RapidField.SolidInstruments.Example.Domain.Models.User.AggregateDataAccessModel;
using UserRoleAssignmentModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.AggregateDataAccessModel;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.AggregateDataAccessModel;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl
{
    /// <summary>
    /// Encapsulates container configuration for the AccessControl database connection and related data access dependencies.
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

        /// <summary>
        /// Registers additional components.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected override void RegisterCustomComponents(ServiceCollection configurator, IConfiguration applicationConfiguration) => _ = configurator
            .AddStandardDataAccessModelCommandHandlers<Guid, UserModel, UserRepository>()
            .AddStandardDataAccessModelCommandHandlers<Guid, UserRoleAssignmentModel, UserRoleAssignmentRepository>()
            .AddStandardDataAccessModelCommandHandlers<Guid, UserRoleModel, UserRoleRepository>();
    }
}