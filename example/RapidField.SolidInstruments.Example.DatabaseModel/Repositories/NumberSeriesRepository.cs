﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.Example.DatabaseModel.Entities;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.Example.DatabaseModel.Repositories
{
    /// <summary>
    /// Represents a repository for the <see cref="NumberSeries" /> entity type.
    /// </summary>
    public sealed class NumberSeriesRepository : EntityFrameworkRepository<NumberSeries, ExampleContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberSeriesRepository" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public NumberSeriesRepository(ExampleContext context)
            : base(context)
        {
            return;
        }

        /// <summary>
        /// Finds the <see cref="NumberSeries" /> with the specified identifier.
        /// </summary>
        /// <param name="identifier">
        /// The identifier to find.
        /// </param>
        /// <returns>
        /// The <see cref="NumberSeries" /> with the specified identifier, or <see langword="null" /> if the
        /// <see cref="NumberSeries" /> is not found.
        /// </returns>
        public NumberSeries FindByIdentifier(Guid identifier) => FindWhere(entity => entity.Identifier == identifier).SingleOrDefault();

        /// <summary>
        /// Finds the <see cref="NumberSeries" /> with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name to find.
        /// </param>
        /// <returns>
        /// The <see cref="NumberSeries" /> with the specified name, or <see langword="null" /> if the <see cref="NumberSeries" />
        /// is not found.
        /// </returns>
        public NumberSeries FindByName(String name) => FindWhere(entity => entity.Name == name).SingleOrDefault();
    }
}