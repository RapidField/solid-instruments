// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.Example.DatabaseModel.Entities;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.Example.DatabaseModel.Repositories
{
    /// <summary>
    /// Represents a repository for the <see cref="Number" /> entity type.
    /// </summary>
    public sealed class NumberRepository : EntityFrameworkRepository<Number, ExampleContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberRepository" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public NumberRepository(ExampleContext context)
            : base(context)
        {
            return;
        }

        /// <summary>
        /// Finds the <see cref="Number" /> with the specified identifier.
        /// </summary>
        /// <param name="identifier">
        /// The identifier to find.
        /// </param>
        /// <returns>
        /// The <see cref="Number" /> with the specified value, or <see langword="null" /> if the <see cref="Number" /> is not found.
        /// </returns>
        public Number FindByIdentifier(Guid identifier) => FindWhere(entity => entity.Identifier == identifier).SingleOrDefault();

        /// <summary>
        /// Finds the <see cref="Number" /> with the specified value.
        /// </summary>
        /// <param name="value">
        /// The value to find.
        /// </param>
        /// <returns>
        /// The <see cref="Number" /> with the specified value, or <see langword="null" /> if the <see cref="Number" /> is not found.
        /// </returns>
        public Number FindByValue(Int64 value) => FindWhere(entity => entity.Value == value).SingleOrDefault();
    }
}