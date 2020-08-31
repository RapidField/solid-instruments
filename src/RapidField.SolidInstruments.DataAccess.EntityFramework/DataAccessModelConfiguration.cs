// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Encapsulates entity type configuration for an <see cref="IDataAccessModel{TIdentifier}" />.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessModelConfiguration{TIdentifier, TDataAccessModel}" /> is the default implementation of
    /// <see cref="IDataAccessModelConfiguration{TIdentifier, TDataAccessModel}" />.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data access model.
    /// </typeparam>
    public abstract class DataAccessModelConfiguration<TIdentifier, TDataAccessModel> : DataAccessModelConfiguration<TDataAccessModel>, IDataAccessModelConfiguration<TIdentifier, TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessModelConfiguration{TDataAccessModel}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected DataAccessModelConfiguration(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Configures the entity.
        /// </summary>
        /// <param name="builder">
        /// The builder that is used to configure the entity.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected override void Configure(EntityTypeBuilder<TDataAccessModel> builder, IConfiguration applicationConfiguration) => builder.HasKey(model => model.Identifier);

        /// <summary>
        /// Represents the type of the value that uniquely identifies the data access model.
        /// </summary>
        protected static Type DataAccessModelIdentifierType = typeof(TIdentifier);
    }

    /// <summary>
    /// Encapsulates entity type configuration for an <see cref="IDataAccessModel" />.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessModelConfiguration{TDataAccessModel}" /> is the default implementation of
    /// <see cref="IDataAccessModelConfiguration{TDataAccessModel}" />.
    /// </remarks>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data access model.
    /// </typeparam>
    public abstract class DataAccessModelConfiguration<TDataAccessModel> : IDataAccessModelConfiguration<TDataAccessModel>
        where TDataAccessModel : class, IDataAccessModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessModelConfiguration{TDataAccessModel}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected DataAccessModelConfiguration(IConfiguration applicationConfiguration)
        {
            ApplicationConfiguration = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument;
        }

        /// <summary>
        /// Configures the entity.
        /// </summary>
        /// <param name="builder">
        /// The builder that is used to configure the entity.
        /// </param>
        public void Configure(EntityTypeBuilder<TDataAccessModel> builder) => Configure(builder, ApplicationConfiguration);

        /// <summary>
        /// Configures the entity.
        /// </summary>
        /// <param name="builder">
        /// The builder that is used to configure the entity.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected abstract void Configure(EntityTypeBuilder<TDataAccessModel> builder, IConfiguration applicationConfiguration);

        /// <summary>
        /// Represents the type of the data access model.
        /// </summary>
        protected static Type DataAccessModelType = typeof(TDataAccessModel);

        /// <summary>
        /// Represents configuration information for the application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IConfiguration ApplicationConfiguration;
    }
}