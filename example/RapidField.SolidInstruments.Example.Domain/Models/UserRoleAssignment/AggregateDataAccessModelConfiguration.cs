// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using System;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Encapsulates entity type configuration for the <see cref="AggregateDataAccessModel" /> class.
    /// </summary>
    public sealed class AggregateDataAccessModelConfiguration : DataAccessModelConfiguration<AggregateDataAccessModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateDataAccessModelConfiguration" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public AggregateDataAccessModelConfiguration(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Configures the entity.
        /// </summary>
        /// <param name="entityType">
        /// The builder that is used to configure the entity.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected override void Configure(EntityTypeBuilder<AggregateDataAccessModel> entityType, IConfiguration applicationConfiguration)
        {
            _ = entityType.HasOne(entity => entity.User).WithMany(entity => entity.UserRoleAssignments).HasForeignKey(entity => entity.UserIdentifier);
            _ = entityType.HasOne(entity => entity.UserRole).WithMany().HasForeignKey(entity => entity.UserRoleIdentifier);
        }
    }
}