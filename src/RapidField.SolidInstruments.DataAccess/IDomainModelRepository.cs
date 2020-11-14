// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Performs data access operations for a specified domain model type.
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
    public interface IDomainModelRepository<TIdentifier, TDataAccessModel, TDomainModel> : IDataAccessModelRepository<TIdentifier, TDataAccessModel>, IReadOnlyDomainModelRepository<TIdentifier, TDataAccessModel, TDomainModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDomainModel : class, IDomainModel<TIdentifier>, new()
        where TDataAccessModel : class, IDataAccessModel<TIdentifier, TDomainModel>, new()
    {
        /// <summary>
        /// Adds the specified domain model to the current
        /// <see cref="IDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </summary>
        /// <param name="domainModel">
        /// The domain model to add.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The state of <paramref name="domainModel" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModel" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddDomainModel(TDomainModel domainModel) => Add(ConvertToDataAccessModel(domainModel));

        /// <summary>
        /// Adds the specified domain models to the current
        /// <see cref="IDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </summary>
        /// <param name="domainModels">
        /// The domain models to add.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="domainModels" /> contains one or more null references -or- the state of one or more models is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddDomainModelRange(IEnumerable<TDomainModel> domainModels) => AddRange(domainModels.RejectIf().IsNull(nameof(domainModels)).TargetArgument.Select(domainModel => ConvertToDataAccessModel(domainModel)));

        /// <summary>
        /// Updates the specified domain model in the current
        /// <see cref="IDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />, or adds it if it doesn't exist.
        /// </summary>
        /// <param name="domainModel">
        /// The domain model to add or update.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The state of <paramref name="domainModel" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModel" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdateDomainModel(TDomainModel domainModel) => AddOrUpdate(ConvertToDataAccessModel(domainModel));

        /// <summary>
        /// Updates the specified domain models in the current
        /// <see cref="IDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />, or adds them if they don't exist.
        /// </summary>
        /// <param name="domainModels">
        /// The domain models to add or update.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="domainModels" /> contains one or more null references -or- the state of one or more models is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdateDomainModelRange(IEnumerable<TDomainModel> domainModels) => AddOrUpdateRange(domainModels.RejectIf().IsNull(nameof(domainModels)).TargetArgument.Select(domainModel => ConvertToDataAccessModel(domainModel)));

        /// <summary>
        /// Removes the specified domain model from the current
        /// <see cref="IDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </summary>
        /// <param name="domainModel">
        /// The domain model to remove.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The state of <paramref name="domainModel" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModel" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RemoveDomainModel(TDomainModel domainModel) => Remove(ConvertToDataAccessModel(domainModel));

        /// <summary>
        /// Removes the specified domain models from the current
        /// <see cref="IDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </summary>
        /// <param name="domainModels">
        /// The domain models to remove.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="domainModels" /> contains one or more null references -or- the state of one or more models is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RemoveDomainModelRange(IEnumerable<TDomainModel> domainModels) => RemoveRange(domainModels.RejectIf().IsNull(nameof(domainModels)).TargetArgument.Select(domainModel => ConvertToDataAccessModel(domainModel)));

        /// <summary>
        /// Updates the specified domain model in the current
        /// <see cref="IDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </summary>
        /// <param name="domainModel">
        /// The domain model to update.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The state of <paramref name="domainModel" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModel" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void UpdateDomainModel(TDomainModel domainModel) => Update(ConvertToDataAccessModel(domainModel));

        /// <summary>
        /// Updates the specified domain models in the current
        /// <see cref="IDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </summary>
        /// <param name="domainModels">
        /// The domain models to update.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="domainModels" /> contains one or more null references -or- the state of one or more models is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void UpdateDomainModelRange(IEnumerable<TDomainModel> domainModels) => UpdateRange(domainModels.RejectIf().IsNull(nameof(domainModels)).TargetArgument.Select(domainModel => ConvertToDataAccessModel(domainModel)));

        /// <summary>
        /// Converts the specified domain model to its data access model equivalent.
        /// </summary>
        /// <param name="domainModel">
        /// The domain model to convert.
        /// </param>
        /// <returns>
        /// The resulting data access model.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The state of <paramref name="domainModel" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModel" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static TDataAccessModel ConvertToDataAccessModel(TDomainModel domainModel) => ConvertToDataAccessModel<TIdentifier, TDataAccessModel, TDomainModel>(domainModel);
    }

    /// <summary>
    /// Performs data access operations for a specified domain model type.
    /// </summary>
    public interface IDomainModelRepository : IDataAccessModelRepository
    {
        /// <summary>
        /// Converts the specified domain model to its data access model equivalent.
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
        /// <param name="domainModel">
        /// The domain model to convert.
        /// </param>
        /// <returns>
        /// The resulting data access model.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The state of <paramref name="domainModel" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModel" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal static TDataAccessModel ConvertToDataAccessModel<TIdentifier, TDataAccessModel, TDomainModel>(TDomainModel domainModel)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDomainModel : class, IDomainModel<TIdentifier>, new()
            where TDataAccessModel : class, IDataAccessModel<TIdentifier, TDomainModel>, new()
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

            return dataAccessModel;
        }

        /// <summary>
        /// Converts the specified data access model to its domain model equivalent.
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
        internal static TDomainModel ConvertToDomainModel<TIdentifier, TDataAccessModel, TDomainModel>(TDataAccessModel dataAccessModel)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDomainModel : class, IDomainModel<TIdentifier>, new()
            where TDataAccessModel : class, IDataAccessModel<TIdentifier, TDomainModel>, new()
        {
            try
            {
                return dataAccessModel?.ToDomainModel();
            }
            catch (TypeInitializationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                var domainModelType = typeof(TDomainModel);
                throw new TypeInitializationException(domainModelType.FullName, exception);
            }
        }
    }
}