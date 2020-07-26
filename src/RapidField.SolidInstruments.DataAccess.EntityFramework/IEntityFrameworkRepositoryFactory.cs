// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using System;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Encapsulates creation of new Entity Framework <see cref="IDataAccessRepository" /> instances that map to database entities.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the database session that is used by the produced repositories.
    /// </typeparam>
    public interface IEntityFrameworkRepositoryFactory<TContext> : IEntityFrameworkRepositoryFactory
        where TContext : DbContext
    {
        /// <summary>
        /// Gets the database session that is used by the produced repositories.
        /// </summary>
        public TContext Context
        {
            get;
        }
    }

    /// <summary>
    /// Encapsulates creation of new Entity Framework <see cref="IDataAccessRepository" /> instances that map to database entities.
    /// </summary>
    public interface IEntityFrameworkRepositoryFactory : IDataAccessRepositoryFactory
    {
        /// <summary>
        /// Gets the type of the database session that is used by the produced repositories.
        /// </summary>
        public Type ContextType
        {
            get;
        }
    }
}