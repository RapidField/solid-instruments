// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents a command to create or update a data access model.
    /// </summary>
    /// <remarks>
    /// <see cref="CreateOrUpdateDataAccessModelCommand{TIdentifier, TDataAccessModel}" /> is the default implementation of
    /// <see cref="ICreateOrUpdateDataAccessModelCommand{TIdentifier, TDataAccessModel}" />.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the associated data access model.
    /// </typeparam>
    [DataContract]
    public class CreateOrUpdateDataAccessModelCommand<TIdentifier, TDataAccessModel> : DataAccessModelCommand<TIdentifier, TDataAccessModel>, ICreateOrUpdateDataAccessModelCommand<TIdentifier, TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateDataAccessModelCommand{TIdentifier, TDataAccessModel}" />
        /// class.
        /// </summary>
        /// <param name="model">
        /// The associated data access model.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        public CreateOrUpdateDataAccessModelCommand(TDataAccessModel model)
            : base(model)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateDataAccessModelCommand{TIdentifier, TDataAccessModel}" />
        /// class.
        /// </summary>
        [DebuggerHidden]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected internal CreateOrUpdateDataAccessModelCommand()
            : base()
        {
            return;
        }
    }
}