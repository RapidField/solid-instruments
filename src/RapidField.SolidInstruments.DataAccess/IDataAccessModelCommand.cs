// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents a command to perform an action related to a data access model.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the associated data access model.
    /// </typeparam>
    public interface IDataAccessModelCommand<TIdentifier, TDataAccessModel> : IDataAccessCommand, IDataAccessModelCommand<TIdentifier, TDataAccessModel, Nix>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
        /// <summary>
        /// Gets the associated <see cref="IDataAccessModel{TIdentifier}" />.
        /// </summary>
        public TDataAccessModel Model
        {
            get;
        }
    }

    /// <summary>
    /// Represents a command to perform an action related to a data access model.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the associated data access model.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is produced by handling the data access model command.
    /// </typeparam>
    public interface IDataAccessModelCommand<TIdentifier, TDataAccessModel, out TResult> : IDataAccessCommand<TResult>, IDataAccessModelCommand
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
        /// <summary>
        /// Gets a value that uniquely identifies the associated <see cref="IDataAccessModel{TIdentifier}" />.
        /// </summary>
        public TIdentifier ModelIdentifier
        {
            get;
        }
    }

    /// <summary>
    /// Represents a command to perform an action related to a data access model.
    /// </summary>
    public interface IDataAccessModelCommand : ICommandBase
    {
        /// <summary>
        /// Gets the type of the associated data access model.
        /// </summary>
        public Type ModelType
        {
            get;
        }
    }
}