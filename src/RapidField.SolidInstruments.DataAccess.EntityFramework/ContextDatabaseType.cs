// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Specifies a database type for an <see cref="ConfiguredContext" /> instance.
    /// </summary>
    public enum ContextDatabaseType : Int32
    {
        /// <summary>
        /// The database type is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The context is backed by a Cosmos DB database.
        /// </summary>
        Cosmos = 1,

        /// <summary>
        /// The context is backed by an in-memory database.
        /// </summary>
        InMemory = 2,

        /// <summary>
        /// The context is backed by a SQL Server database.
        /// </summary>
        SqlServer = 3
    }
}