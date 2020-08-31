// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using System;
using System.Configuration;
using System.Diagnostics;
using UserModel = RapidField.SolidInstruments.Example.Domain.Models.User.AggregateDataAccessModel;
using UserRoleAssignmentModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.AggregateDataAccessModel;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.AggregateDataAccessModel;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl
{
    /// <summary>
    /// Represents a connection to the AccessControl database.
    /// </summary>
    public sealed class DatabaseContext : ConfiguredContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// The constructor was unable to determine the appropriate connection type by evaluating the connection string.
        /// </exception>
        public DatabaseContext(IConfiguration applicationConfiguration)
            : base(applicationConfiguration, DatabaseName)
        {
            return;
        }

        /// <summary>
        /// Configures the model that is discovered by convention from entity types exposed via <see cref="DbSet{TEntity}" />
        /// properties on the context.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="modelBuilder">
        /// A builder that is used to configure the model.
        /// </param>
        protected override void OnModelCreating(IConfiguration applicationConfiguration, ModelBuilder modelBuilder)
        {
            try
            {
                _ = modelBuilder
                    .Entity<UserModel>(entityType =>
                    {
                        entityType.HasMany(entity => entity.UserRoleAssignments).WithOne(entity => entity.User);
                    })
                    .Entity<UserRoleModel>(entityType =>
                    {
                        return;
                    })
                    .Entity<UserRoleAssignmentModel>(entityType =>
                    {
                        entityType.HasOne(entity => entity.User).WithMany(entity => entity.UserRoleAssignments).HasForeignKey(entity => entity.UserIdentifier);
                        entityType.HasOne(entity => entity.UserRole).WithMany().HasForeignKey(entity => entity.UserRoleIdentifier);
                    });
            }
            finally
            {
                base.OnModelCreating(applicationConfiguration, modelBuilder);
            }
        }

        /// <summary>
        /// Gets or sets a persistent collection of <see cref="UserRoleAssignmentModel" /> records.
        /// </summary>
        public DbSet<UserRoleAssignmentModel> UserRoleAssignments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a persistent collection of <see cref="UserRoleModel" /> records.
        /// </summary>
        public DbSet<UserRoleModel> UserRoles
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a persistent collection of <see cref="UserModel" /> records.
        /// </summary>
        public DbSet<UserModel> Users
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the database name that is used when creating and connecting to the data source.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DatabaseName = "AccessControl";
    }
}