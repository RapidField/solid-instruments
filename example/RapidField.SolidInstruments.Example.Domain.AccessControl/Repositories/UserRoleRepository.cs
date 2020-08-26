// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using DataAccessModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.AggregateDataAccessModel;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.Repositories
{
    using BaseRepository = Domain.Repositories.UserRole.AggregateModelRepository<DatabaseContext>;

    /// <summary>
    /// Performs data access operations for the <see cref="DataAccessModel" /> type.
    /// </summary>
    public sealed class UserRoleRepository : BaseRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleRepository" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public UserRoleRepository(DatabaseContext context)
            : base(context)
        {
            return;
        }
    }
}