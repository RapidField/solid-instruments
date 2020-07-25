// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents an object that models a data access entity.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    public interface IDataAccessModel<TIdentifier, TDomainModel> : IDataAccessModel<TIdentifier>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDomainModel : class, IDomainModel<TIdentifier>
    {
        /// <summary>
        /// Copies the state of the specified domain model to the current
        /// <see cref="IDataAccessModel{TIdentifier, TDomainModel}" />.
        /// </summary>
        /// <param name="domainModel">
        /// A domain model from which to hydrate the state of the data access model.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The state of <paramref name="domainModel" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModel" /> is <see langword="null" />.
        /// </exception>
        public void HydrateFromDomainModel(TDomainModel domainModel);

        /// <summary>
        /// Converts the current <see cref="IDataAccessModel{TIdentifier, TDomainModel}" /> to its equivalent domain model
        /// representation.
        /// </summary>
        /// <returns>
        /// An <see cref="IDomainModel{TIdentifier}" /> that is equivalent to the current data access model.
        /// </returns>
        /// <exception cref="TypeInitializationException">
        /// An exception was raised while converting the data access model to its equivalent domain model.
        /// </exception>
        public TDomainModel ToDomainModel();
    }

    /// <summary>
    /// Represents an object that models a data access entity.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    public interface IDataAccessModel<TIdentifier> : IDataAccessModel
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
    {
        /// <summary>
        /// Gets a value that uniquely identifies the current <see cref="IDataAccessModel{TIdentifier}" />.
        /// </summary>
        public TIdentifier Identifier
        {
            get;
        }
    }

    /// <summary>
    /// Represents an object that models a data access entity.
    /// </summary>
    public interface IDataAccessModel
    {
    }
}