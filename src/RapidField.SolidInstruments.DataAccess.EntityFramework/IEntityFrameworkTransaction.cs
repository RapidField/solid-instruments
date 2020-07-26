// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Fulfills the unit of work pattern for Entity Framework data access operations.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the database session for the transaction.
    /// </typeparam>
    public interface IEntityFrameworkTransaction<TContext> : IEntityFrameworkTransaction
        where TContext : DbContext
    {
    }

    /// <summary>
    /// Fulfills the unit of work pattern for Entity Framework data access operations.
    /// </summary>
    public interface IEntityFrameworkTransaction : IDataAccessTransaction
    {
    }
}