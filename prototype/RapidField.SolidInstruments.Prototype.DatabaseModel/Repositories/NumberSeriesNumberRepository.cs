// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.Prototype.DatabaseModel.Entities;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.Prototype.DatabaseModel.Repositories
{
    /// <summary>
    /// Represents a repository for the <see cref="NumberSeriesNumber" /> entity type.
    /// </summary>
    public sealed class NumberSeriesNumberRepository : EntityFrameworkRepository<NumberSeriesNumber, PrototypeContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberSeriesNumberRepository" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public NumberSeriesNumberRepository(PrototypeContext context)
            : base(context)
        {
            return;
        }

        /// <summary>
        /// Finds the <see cref="NumberSeriesNumber" /> with the specified identifier.
        /// </summary>
        /// <param name="identifier">
        /// The identifier to find.
        /// </param>
        /// <returns>
        /// The <see cref="NumberSeriesNumber" /> with the specified value, or <see langword="null" /> if the
        /// <see cref="NumberSeriesNumber" /> is not found.
        /// </returns>
        public NumberSeriesNumber FindByIdentifier(Guid identifier) => FindWhere(entity => entity.Identifier == identifier).SingleOrDefault();
    }
}