// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Performs read-only data access operations for a specified domain model type.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data model.
    /// </typeparam>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    public interface IReadOnlyDomainModelRepository<TIdentifier, TDataAccessModel, TDomainModel> : IDomainModelRepository, IReadOnlyDataAccessModelRepository<TIdentifier, TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDomainModel : class, IDomainModel<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier, TDomainModel>, new()
    {
        /// <summary>
        /// Determines whether or not the specified domain model exists in the current
        /// <see cref="IReadOnlyDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </summary>
        /// <param name="domainModel">
        /// The domain model to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified domain model exists in the current
        /// <see cref="IReadOnlyDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The state of <paramref name="domainModel" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModel" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean ContainsDomainModel(TDomainModel domainModel)
        {
            _ = domainModel.RejectIf().IsNull(nameof(domainModel));
            var dataAccessModel = new TDataAccessModel();

            try
            {
                dataAccessModel.HydrateFromDomainModel(domainModel);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new ArgumentException("The state of the specified domain model is invalid.", nameof(domainModel), exception);
            }

            return Contains(dataAccessModel);
        }

        /// <summary>
        /// Returns the domain model matching the specified identifier from the current
        /// <see cref="IReadOnlyDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />, or <see langword="null" />
        /// if no model matching <paramref name="identifier" /> is found.
        /// </summary>
        /// <param name="identifier">
        /// The unique primary identifier for the domain model.
        /// </param>
        /// <returns>
        /// The domain model matching the specified identifier within the current
        /// <see cref="IReadOnlyDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />, or <see langword="null" />
        /// if no model matching <paramref name="identifier" /> is found.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TypeInitializationException">
        /// An exception was raised while converting the data access model to its equivalent domain model.
        /// </exception>
        public TDomainModel FindDomainModelByIdentifier(TIdentifier identifier) => ConvertToDomainModel(FindByIdentifier(identifier));

        /// <summary>
        /// Returns all domain models matching the specified predicate from the current
        /// <see cref="IReadOnlyDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// All domain models matching the specified predicate within the current
        /// <see cref="IReadOnlyDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TypeInitializationException">
        /// An exception was raised while converting one or more data access models to their equivalent domain model(s).
        /// </exception>
        public IQueryable<TDomainModel> FindDomainModelsWhere(Expression<Func<TDataAccessModel, Boolean>> predicate) => FindWhere(predicate).Select(dataAccesModel => ConvertToDomainModel(dataAccesModel));

        /// <summary>
        /// Converts the specified data access model to its domain model equivalent.
        /// </summary>
        /// <param name="dataAccessModel">
        /// The data access model to convert.
        /// </param>
        /// <returns>
        /// The resulting domain model.
        /// </returns>
        /// <exception cref="TypeInitializationException">
        /// An exception was raised while converting the data access model to its equivalent domain model.
        /// </exception>
        [DebuggerHidden]
        private TDomainModel ConvertToDomainModel(TDataAccessModel dataAccessModel) => ConvertToDomainModel<TIdentifier, TDataAccessModel, TDomainModel>(dataAccessModel);

        /// <summary>
        /// Gets the domain model type of the current
        /// <see cref="IReadOnlyDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </summary>
        public Type DomainModelType => typeof(TDomainModel);
    }
}