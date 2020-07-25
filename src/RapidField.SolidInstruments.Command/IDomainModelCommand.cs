// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command to perform an action related to an object that models a domain construct.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    public interface IDomainModelCommand<TModel> : IDomainModelCommand
        where TModel : class, IDomainModel
    {
        /// <summary>
        /// Gets the desired state of the associated domain model.
        /// </summary>
        public TModel Model
        {
            get;
        }
    }

    /// <summary>
    /// Represents a command to perform an action related to an object that models a domain construct.
    /// </summary>
    public interface IDomainModelCommand : IDomainCommand
    {
        /// <summary>
        /// Gets a classification that describes the effect of a the current <see cref="IDomainModelCommand" /> upon the associated
        /// model.
        /// </summary>
        public DomainModelCommandClassification Classification
        {
            get;
        }

        /// <summary>
        /// Gets the type of the associated domain model.
        /// </summary>
        public Type ModelType
        {
            get;
        }
    }
}