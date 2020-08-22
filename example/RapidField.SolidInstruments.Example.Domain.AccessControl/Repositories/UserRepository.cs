// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using DataAccessModel = RapidField.SolidInstruments.Example.Domain.Models.User.AggregateDataAccessModel;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.Repositories
{
    using BaseRepository = Domain.Repositories.User.AggregateModelRepository<DatabaseContext>;

    /// <summary>
    /// Performs data access operations for the <see cref="DataAccessModel" /> type.
    /// </summary>
    public sealed class UserRepository : BaseRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public UserRepository(DatabaseContext context)
            : base(context)
        {
            return;
        }
    }
}