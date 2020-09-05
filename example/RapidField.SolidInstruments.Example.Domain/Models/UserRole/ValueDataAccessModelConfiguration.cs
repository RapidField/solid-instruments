// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using System;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRole
{
    /// <summary>
    /// Encapsulates entity type configuration for the <see cref="ValueDataAccessModel" /> class.
    /// </summary>
    public sealed class ValueDataAccessModelConfiguration : DataAccessModelConfiguration<ValueDataAccessModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueDataAccessModelConfiguration" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public ValueDataAccessModelConfiguration(IConfiguration applicationConfiguration)
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
        protected override void Configure(EntityTypeBuilder<ValueDataAccessModel> entityType, IConfiguration applicationConfiguration)
        {
            return;
        }
    }
}