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
    /// Represents a command to find a data access model by its identifier.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the associated data access model.
    /// </typeparam>
    [DataContract]
    public class FindDataAccessModelByIdentifierCommand<TIdentifier, TDataAccessModel> : DataAccessModelCommand<TIdentifier, TDataAccessModel, TDataAccessModel>, IFindDataAccessModelByIdentifierCommand<TIdentifier, TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindDataAccessModelByIdentifierCommand{TIdentifier, TDataAccessModel}" />
        /// class.
        /// </summary>
        /// <param name="modelIdentifier">
        /// A value that uniquely identifies the associated data access model.
        /// </param>
        public FindDataAccessModelByIdentifierCommand(TIdentifier modelIdentifier)
            : base(modelIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDataAccessModelByIdentifierCommand{TIdentifier, TDataAccessModel}" />
        /// class.
        /// </summary>
        [DebuggerHidden]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected internal FindDataAccessModelByIdentifierCommand()
            : base()
        {
            return;
        }
    }
}