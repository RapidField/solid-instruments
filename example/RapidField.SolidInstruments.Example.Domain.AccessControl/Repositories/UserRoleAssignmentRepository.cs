// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using DataAccessModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.AggregateDataAccessModel;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.Repositories
{
    using BaseRepository = Domain.Repositories.UserRoleAssignment.AggregateModelRepository<DatabaseContext>;

    /// <summary>
    /// Performs data access operations for the <see cref="DataAccessModel" /> type.
    /// </summary>
    public sealed class UserRoleAssignmentRepository : BaseRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleAssignmentRepository" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public UserRoleAssignmentRepository(DatabaseContext context)
            : base(context)
        {
            return;
        }
    }
}