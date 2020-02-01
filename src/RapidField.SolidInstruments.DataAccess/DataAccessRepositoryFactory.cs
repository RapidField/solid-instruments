// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.ObjectComposition;
using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Encapsulates creation of new <see cref="IDataAccessRepository" /> instances that map to database entities.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessRepositoryFactory" /> is the default implementation of <see cref="IDataAccessRepositoryFactory" />.
    /// </remarks>
    public abstract class DataAccessRepositoryFactory : ObjectFactory<IDataAccessRepository>, IDataAccessRepositoryFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessRepositoryFactory" /> class.
        /// </summary>
        protected DataAccessRepositoryFactory()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessRepositoryFactory" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected DataAccessRepositoryFactory(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DataAccessRepositoryFactory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}